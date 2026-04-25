using FeaLinux.Core.Models;
using FeaLinux.Engine.Results;

namespace FeaLinux.Engine;

/// <summary>Orchestrates analysis: validates model, runs solver, stores results.</summary>
public class AnalysisEngine
{
    private readonly StructuralModel _model;

    public AnalysisEngine(StructuralModel model) => _model = model;

    public (bool ok, string error) ValidateModel()
    {
        if (_model.Nodes.Count < 2) return (false, "At least 2 nodes required.");
        if (_model.Members.Count == 0) return (false, "At least 1 member required.");
        if (_model.Supports.Count == 0) return (false, "At least 1 support required.");
        if (_model.LoadCases.Count == 0) return (false, "At least 1 load case required.");
        if (_model.Members.Any(m => m.Section == null)) return (false, "All members must have a section assigned.");
        if (_model.Members.Any(m => m.Material == null)) return (false, "All members must have a material assigned.");
        if (_model.Members.Any(m => m.Length < 1e-6)) return (false, "One or more members have zero length.");
        return (true, string.Empty);
    }

    public async Task<bool> RunAllLoadCasesAsync(IProgress<string>? progress = null,
        CancellationToken ct = default)
    {
        var (ok, err) = ValidateModel();
        if (!ok) { progress?.Report($"ERROR: {err}"); return false; }

        _model.AnalysisStatus = Core.Enums.AnalysisStatus.Running;
        bool allOk = true;
        AnalysisResults? lastResult = null;

        foreach (var lc in _model.LoadCases.Where(l => l.IsActive))
        {
            ct.ThrowIfCancellationRequested();
            progress?.Report($"Solving LC{lc.Id}: {lc.Name}...");
            var solver = new Solvers.LinearStaticSolver(_model);
            var result = await solver.SolveAsync(lc, progress, ct);
            if (!result.Success) { allOk = false; progress?.Report($"FAILED: {result.ErrorMessage}"); }
            else _model.Results = result;   // Store latest result (TODO: multi-LC result storage)
        }

        _model.AnalysisStatus = allOk ? Core.Enums.AnalysisStatus.Completed : Core.Enums.AnalysisStatus.Failed;
        return allOk;
    }
}
