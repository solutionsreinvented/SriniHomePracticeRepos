using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using FeaLinux.Core.Models;
using FeaLinux.Engine.Elements;
using FeaLinux.Engine.Results;

namespace FeaLinux.Engine.Solvers;

/// <summary>
/// Linear Static FEA Solver.
/// Assembles global stiffness matrix, applies BCs, solves [K]{u}={F}.
/// </summary>
public class LinearStaticSolver
{
    private readonly StructuralModel _model;

    public LinearStaticSolver(StructuralModel model) => _model = model;

    public async Task<AnalysisResults> SolveAsync(LoadCase loadCase,
        IProgress<string>? progress = null, CancellationToken ct = default)
    {
        var result = new AnalysisResults { LoadCaseId = loadCase.Id };
        var sw = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            progress?.Report("Numbering DOFs...");
            int n = _model.TotalDof;
            if (n == 0) throw new InvalidOperationException("Model has no nodes.");

            // ── 1. Assemble global stiffness matrix (sparse via triplets) ─
            progress?.Report("Assembling stiffness matrix...");
            var triplets = new List<(int row, int col, double val)>();

            foreach (var member in _model.Members)
            {
                ct.ThrowIfCancellationRequested();
                var elem = new BeamElement3D(member);
                var Kg = elem.GlobalStiffnessMatrix();
                var dofs = elem.GlobalDofIndices();

                for (int i = 0; i < 12; i++)
                    for (int j = 0; j < 12; j++)
                        if (Math.Abs(Kg[i, j]) > 1e-15)
                            triplets.Add((dofs[i], dofs[j], Kg[i, j]));
            }

            var K = SparseMatrix.OfIndexed(n, n, triplets.Select(t => (t.row, t.col, t.val)));

            // ── 2. Assemble load vector ───────────────────────────────────
            progress?.Report("Assembling load vector...");
            var F = Vector<double>.Build.Dense(n);

            // Self-weight
            if (loadCase.SelfWeightFactor != 0)
                ApplySelfWeight(F, loadCase.SelfWeightFactor);

            // Node loads
            foreach (var load in loadCase.Loads.OfType<NodeLoad>())
            {
                var dofs = load.Node.GetDofIndices();
                F[dofs[0]] += load.Fx;
                F[dofs[1]] += load.Fy;
                F[dofs[2]] += load.Fz;
                F[dofs[3]] += load.Mx;
                F[dofs[4]] += load.My;
                F[dofs[5]] += load.Mz;
            }

            // Member distributed loads → fixed-end forces
            foreach (var load in loadCase.Loads.OfType<MemberLoad>())
                ApplyMemberLoad(F, load);

            // ── 3. Apply boundary conditions (penalty / elimination) ──────
            progress?.Report("Applying boundary conditions...");
            var restrainedDofs = new HashSet<int>(
                _model.Supports.SelectMany(s => s.GetRestrainedDofs()));

            // Store reaction DOFs, then zero rows/cols and set diagonal = 1
            foreach (int dof in restrainedDofs)
            {
                for (int j = 0; j < n; j++) { K[dof, j] = 0; K[j, dof] = 0; }
                K[dof, dof] = 1.0;
                F[dof] = 0.0;
            }

            // ── 4. Solve ──────────────────────────────────────────────────
            progress?.Report("Solving linear system...");
            await Task.Run(() =>
            {
                var u = K.Solve(F);
                ExtractResults(u, result, restrainedDofs);
            }, ct);

            // ── 5. Recover member end forces ──────────────────────────────
            progress?.Report("Recovering member forces...");
            RecoverMemberForces(result);

            result.Success = true;
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = ex.Message;
        }

        sw.Stop();
        result.SolveTime = sw.Elapsed;
        progress?.Report($"Done in {result.SolveTime.TotalSeconds:F2}s");
        return result;
    }

    private void ApplySelfWeight(Vector<double> F, double factor)
    {
        const double g = 9.81;  // m/s²
        foreach (var member in _model.Members)
        {
            double w = member.Section.Area * member.Material.Density * g * factor / 1000.0; // kN/m
            double L = member.Length;
            double totalLoad = w * L;
            var dofs = new BeamElement3D(member).GlobalDofIndices();
            // Half load to each node in global Y direction
            F[dofs[1]] -= totalLoad / 2;
            F[dofs[7]] -= totalLoad / 2;
        }
    }

    private static void ApplyMemberLoad(Vector<double> F, MemberLoad load)
    {
        double L = load.Member.Length;
        double w = (load.W1 + load.W2) / 2.0;  // Average (simplified)
        var elem = new BeamElement3D(load.Member);
        var dofs = elem.GlobalDofIndices();

        // Fixed-end forces for UDL in local y (simplified, direction mapping TODO)
        double R = w * L / 2;
        double M = w * L * L / 12.0;
        int yDof = load.Direction == Core.Enums.LoadDirection.GlobalY ? 1 : 2;
        int mDof = load.Direction == Core.Enums.LoadDirection.GlobalY ? 5 : 4;

        F[dofs[yDof]] += R;
        F[dofs[yDof + 6]] += R;
        F[dofs[mDof]] += M;
        F[dofs[mDof + 6]] -= M;
    }

    private void ExtractResults(Vector<double> u, AnalysisResults result, HashSet<int> restrained)
    {
        foreach (var node in _model.Nodes)
        {
            var dofs = node.GetDofIndices();
            result.NodeResults[node.Id] = new NodeResult
            {
                NodeId = node.Id,
                Ux = u[dofs[0]], Uy = u[dofs[1]], Uz = u[dofs[2]],
                Rx = u[dofs[3]], Ry = u[dofs[4]], Rz = u[dofs[5]]
            };
        }

        foreach (var support in _model.Supports)
        {
            var dofs = support.Node.GetDofIndices();
            result.Reactions[support.Node.Id] = [
                support.Tx ? -u[dofs[0]] : 0,
                support.Ty ? -u[dofs[1]] : 0,
                support.Tz ? -u[dofs[2]] : 0,
                support.Rx ? -u[dofs[3]] : 0,
                support.Ry ? -u[dofs[4]] : 0,
                support.Rz ? -u[dofs[5]] : 0
            ];
        }
    }

    private void RecoverMemberForces(AnalysisResults result)
    {
        foreach (var member in _model.Members)
        {
            if (!result.NodeResults.TryGetValue(member.StartNode.Id, out var sn)) continue;
            if (!result.NodeResults.TryGetValue(member.EndNode.Id, out var en)) continue;

            var elem = new BeamElement3D(member);
            var T = elem.TransformationMatrix();
            var Kl = elem.LocalStiffnessMatrix();

            // Global displacement vector for this element
            var ug = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.DenseOfArray(
            [
                sn.Ux, sn.Uy, sn.Uz, sn.Rx, sn.Ry, sn.Rz,
                en.Ux, en.Uy, en.Uz, en.Rx, en.Ry, en.Rz
            ]);
            var ul = T * ug;           // Local displacements
            var fl = Kl * ul;          // Local forces

            result.MemberResults[member.Id] = new MemberResult
            {
                MemberId = member.Id,
                LocalForces = fl.ToArray()
            };
        }
    }
}
