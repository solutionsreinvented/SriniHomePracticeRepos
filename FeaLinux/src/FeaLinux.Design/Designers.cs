using FeaLinux.Core.Models;
using FeaLinux.Engine.Results;

namespace FeaLinux.Design;

public enum DesignStatus { Pass, Fail, NotChecked }

public class DesignResult
{
    public int MemberId { get; set; }
    public string Code { get; set; } = string.Empty;
    public DesignStatus Status { get; set; }
    public double UtilizationRatio { get; set; }  // <= 1.0 = pass
    public string GoverningCheck { get; set; } = string.Empty;
    public List<string> CheckDetails { get; } = [];
}

// ── IS 800:2007 Steel Design (Limit State) ────────────────────────────────
public class IS800Designer
{
    private readonly StructuralModel _model;
    private readonly AnalysisResults _results;
    private const double _gammaM0 = 1.10;
    private const double _gammaM1 = 1.10;

    public IS800Designer(StructuralModel model, AnalysisResults results)
    { _model = model; _results = results; }

    public List<DesignResult> DesignAll()
    {
        var results = new List<DesignResult>();
        foreach (var member in _model.Members.Where(m => m.Material.Type == Core.Enums.MaterialType.Steel))
        {
            if (!_results.MemberResults.TryGetValue(member.Id, out var mr)) continue;
            results.Add(DesignMember(member, mr));
        }
        return results;
    }

    private DesignResult DesignMember(Member member, MemberResult mr)
    {
        var result = new DesignResult { MemberId = member.Id, Code = "IS 800:2007" };
        var sec = member.Section; var mat = member.Material;
        double fy = mat.YieldStrength; double E = mat.ElasticModulus;

        // Tension capacity (Cl. 6.2)
        double Tdg = sec.Area * fy / _gammaM0;
        double Nf = Math.Abs(mr.AxialStart); // Tension force
        double tensionUR = Nf > 0 ? Nf / Tdg : 0;
        result.CheckDetails.Add($"Tension: Nf={Nf:F1} kN, Tdg={Tdg:F1} kN, UR={tensionUR:F3}");

        // Compression capacity (Cl. 7) - simplified
        double Leff = member.Length;
        double lambda_e = Leff / (Math.Sqrt(sec.Iyy / sec.Area));
        double fcc = Math.PI * Math.PI * E / (lambda_e * lambda_e);
        double chi = 1.0 / Math.Min(1.0, Math.Sqrt(1 + Math.Pow(fy / fcc, 0.5)));
        double Pd = chi * sec.Area * fy / _gammaM0;
        double Nc = Math.Abs(mr.AxialEnd);
        double compUR = Nc > 0 ? Nc / Pd : 0;
        result.CheckDetails.Add($"Compression: Nc={Nc:F1} kN, Pd={Pd:F1} kN, UR={compUR:F3}");

        // Bending capacity (Cl. 8.2)
        double Md = sec.Zxx * fy / _gammaM0;
        double M = mr.MaxMomentZ;
        double bendUR = Md > 0 ? M / Md : 0;
        result.CheckDetails.Add($"Bending: M={M:F1} kN·m, Md={Md:F1} kN·m, UR={bendUR:F3}");

        result.UtilizationRatio = Math.Max(tensionUR, Math.Max(compUR, bendUR));
        result.Status = result.UtilizationRatio <= 1.0 ? DesignStatus.Pass : DesignStatus.Fail;
        result.GoverningCheck = bendUR >= compUR && bendUR >= tensionUR ? "Bending" :
                                compUR >= tensionUR ? "Compression" : "Tension";
        return result;
    }
}

// ── IS 456:2000 RC Design (simplified) ──────────────────────────────────────
public class IS456Designer
{
    private readonly StructuralModel _model;
    private readonly AnalysisResults _results;

    public IS456Designer(StructuralModel model, AnalysisResults results)
    { _model = model; _results = results; }

    public List<DesignResult> DesignAll()
    {
        var results = new List<DesignResult>();
        foreach (var member in _model.Members.Where(m => m.Material.Type == Core.Enums.MaterialType.Concrete))
        {
            if (!_results.MemberResults.TryGetValue(member.Id, out var mr)) continue;
            results.Add(DesignMember(member, mr));
        }
        return results;
    }

    private DesignResult DesignMember(Member member, MemberResult mr)
    {
        var result = new DesignResult { MemberId = member.Id, Code = "IS 456:2000" };
        var sec = member.Section; double fck = member.Material.YieldStrength;

        // Flexural capacity (simplified Xu,max method)
        double d = sec.Height - 0.05; // effective depth
        double b = sec.Width;
        double Mulim = 0.138 * fck * b * d * d / 1e3;  // kN·m (approximate)
        double Mu = mr.MaxMomentZ;
        double flexUR = Mulim > 0 ? Mu / Mulim : 0;
        result.CheckDetails.Add($"Flexure: Mu={Mu:F1} kN·m, Mulim={Mulim:F1} kN·m, UR={flexUR:F3}");

        // Shear capacity
        double Vc = 0.34 * Math.Sqrt(fck) * b * d / 1e3; // kN (concrete shear)
        double Vu = Math.Abs(mr.ShearY_Start);
        double shearUR = Vc > 0 ? Vu / Vc : 0;
        result.CheckDetails.Add($"Shear: Vu={Vu:F1} kN, Vc={Vc:F1} kN, UR={shearUR:F3}");

        result.UtilizationRatio = Math.Max(flexUR, shearUR);
        result.Status = result.UtilizationRatio <= 1.0 ? DesignStatus.Pass : DesignStatus.Fail;
        result.GoverningCheck = flexUR >= shearUR ? "Flexure" : "Shear";
        return result;
    }
}
