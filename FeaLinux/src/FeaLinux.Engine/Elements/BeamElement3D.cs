using MathNet.Numerics.LinearAlgebra;
using FeaLinux.Core.Models;
using FeaLinux.Core.Geometry;

namespace FeaLinux.Engine.Elements;

/// <summary>
/// 3D Euler-Bernoulli Beam Element with 12 DOF (6 per node).
/// DOF order per node: [Tx, Ty, Tz, Rx, Ry, Rz]
/// Local axes: x = along member, y = major-axis bending, z = minor-axis bending
/// </summary>
public class BeamElement3D
{
    public Member Member { get; }
    private readonly double _E, _G, _A, _Ixx, _Iyy, _J, _L;

    public BeamElement3D(Member member)
    {
        Member = member;
        _E   = member.Material.ElasticModulus;   // kN/m²
        _G   = member.Material.ShearModulus;
        _A   = member.Section.Area;
        _Ixx = member.Section.Ixx;               // Major axis (bending in local y plane)
        _Iyy = member.Section.Iyy;               // Minor axis
        _J   = member.Section.J;
        _L   = member.Length;
    }

    /// <summary>12×12 local stiffness matrix in local member coordinates.</summary>
    public Matrix<double> LocalStiffnessMatrix()
    {
        double L = _L, L2 = L * L, L3 = L * L * L;
        double EAL  = _E * _A / L;
        double GJL  = _G * _J / L;
        double EIy  = _E * _Ixx;   // bending about local-y (strong axis)
        double EIz  = _E * _Iyy;   // bending about local-z (weak axis)

        var k = Matrix<double>.Build.Dense(12, 12);

        // Axial (DOF 0 and 6 — local x translation)
        k[0, 0] = k[6, 6] = EAL;
        k[0, 6] = k[6, 0] = -EAL;

        // Torsion (DOF 3 and 9 — local x rotation)
        k[3, 3] = k[9, 9] = GJL;
        k[3, 9] = k[9, 3] = -GJL;

        // Bending in local x-y plane (strong axis): DOF 1(Ty), 5(Rz), 7(Ty'), 11(Rz')
        double a = 12 * EIy / L3, b = 6 * EIy / L2;
        double c = 4 * EIy / L,   d = 2 * EIy / L;
        k[1, 1] = k[7, 7] = a;  k[1, 7] = k[7, 1] = -a;
        k[1, 5] = k[5, 1] = b;  k[1, 11] = k[11, 1] = b;
        k[7, 5] = k[5, 7] = -b; k[7, 11] = k[11, 7] = -b;
        k[5, 5] = k[11, 11] = c; k[5, 11] = k[11, 5] = d;

        // Bending in local x-z plane (weak axis): DOF 2(Tz), 4(Ry), 8(Tz'), 10(Ry')
        double a2 = 12 * EIz / L3, b2 = 6 * EIz / L2;
        double c2 = 4 * EIz / L,   d2 = 2 * EIz / L;
        k[2, 2] = k[8, 8] = a2;  k[2, 8] = k[8, 2] = -a2;
        k[2, 4] = k[4, 2] = -b2; k[2, 10] = k[10, 2] = -b2;
        k[8, 4] = k[4, 8] = b2;  k[8, 10] = k[10, 8] = b2;
        k[4, 4] = k[10, 10] = c2; k[4, 10] = k[10, 4] = d2;

        return k;
    }

    /// <summary>12×12 transformation matrix from local to global coordinates.</summary>
    public Matrix<double> TransformationMatrix()
    {
        // Local x-axis = member direction
        var lx = Member.LocalX;

        // Local y-axis: perpendicular to x, preferring global Y unless member is vertical
        FEVector3D refVec;
        if (Math.Abs(lx.X) < 1e-6 && Math.Abs(lx.Z) < 1e-6)
            refVec = FEVector3D.UnitZ;   // vertical member — use Z as reference
        else
            refVec = FEVector3D.UnitY;

        var ly = FEVector3D.Cross(FEVector3D.Cross(lx, refVec), lx).Normalized();
        var lz = FEVector3D.Cross(lx, ly).Normalized();

        // Apply beta angle rotation if needed
        if (Math.Abs(Member.BetaAngle) > 1e-6)
        {
            double beta = Member.BetaAngle * Math.PI / 180.0;
            var ly2 = new FEVector3D(
                ly.X * Math.Cos(beta) + lz.X * Math.Sin(beta),
                ly.Y * Math.Cos(beta) + lz.Y * Math.Sin(beta),
                ly.Z * Math.Cos(beta) + lz.Z * Math.Sin(beta));
            lz = FEVector3D.Cross(lx, ly2).Normalized();
            ly = ly2;
        }

        // 3×3 direction cosine matrix
        var r = Matrix<double>.Build.Dense(3, 3);
        r[0, 0] = lx.X; r[0, 1] = lx.Y; r[0, 2] = lx.Z;
        r[1, 0] = ly.X; r[1, 1] = ly.Y; r[1, 2] = ly.Z;
        r[2, 0] = lz.X; r[2, 1] = lz.Y; r[2, 2] = lz.Z;

        // Block-diagonal 12×12 transformation matrix
        var T = Matrix<double>.Build.Dense(12, 12);
        for (int block = 0; block < 4; block++)
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    T[block * 3 + i, block * 3 + j] = r[i, j];

        return T;
    }

    /// <summary>12×12 global stiffness matrix contribution.</summary>
    public Matrix<double> GlobalStiffnessMatrix()
    {
        var T = TransformationMatrix();
        var Kl = LocalStiffnessMatrix();
        return T.Transpose() * Kl * T;
    }

    /// <summary>Global DOF indices for this element's 12 DOFs.</summary>
    public int[] GlobalDofIndices()
    {
        var sn = Member.StartNode.GetDofIndices();
        var en = Member.EndNode.GetDofIndices();
        return [.. sn, .. en];
    }
}
