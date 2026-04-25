using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FeaLinux.Core.Enums;

namespace FeaLinux.Core.Models;

public partial class StructuralModel : ObservableObject
{
    [ObservableProperty] private string _projectName = "Untitled Project";
    [ObservableProperty] private string _projectPath = string.Empty;
    [ObservableProperty] private bool _isModified;
    [ObservableProperty] private UnitSystem _unitSystem = UnitSystem.SI;
    [ObservableProperty] private AnalysisStatus _analysisStatus = AnalysisStatus.NotAnalyzed;

    // ── Collections ───────────────────────────────────────────────────────
    public ObservableCollection<Node> Nodes { get; } = [];
    public ObservableCollection<Member> Members { get; } = [];
    public ObservableCollection<Plate> Plates { get; } = [];
    public ObservableCollection<Section> Sections { get; } = [];
    public ObservableCollection<Material> Materials { get; } = [];
    public ObservableCollection<Support> Supports { get; } = [];
    public ObservableCollection<LoadCase> LoadCases { get; } = [];
    public ObservableCollection<LoadCombination> LoadCombinations { get; } = [];

    // ── Analysis results (populated after solver runs) ────────────────────
    /// <summary>Populated by FeaLinux.Engine after a successful solve. Typed as object to avoid circular dependency.</summary>
    public object? Results { get; set; }

    // ── ID counters ───────────────────────────────────────────────────────
    private int _nextNodeId = 1;
    private int _nextMemberId = 1;
    private int _nextPlateId = 1;
    private int _nextSupportId = 1;
    private int _nextLoadCaseId = 1;
    private int _nextLoadCombinationId = 1;

    // ── Factory helpers ───────────────────────────────────────────────────
    public Node AddNode(double x, double y, double z, string label = "")
    {
        var node = new Node(_nextNodeId++, x, y, z, label);
        Nodes.Add(node); IsModified = true; return node;
    }

    public Member AddMember(Node start, Node end, Section section, Material material,
                            MemberType type = MemberType.Beam)
    {
        var member = new Member(_nextMemberId++, start, end, section, material, type);
        Members.Add(member); IsModified = true; return member;
    }

    public Support AddSupport(Node node, SupportType supportType)
    {
        Support s = supportType switch
        {
            SupportType.Fixed  => Support.Fixed(_nextSupportId++, node),
            SupportType.Pinned => Support.Pinned(_nextSupportId++, node),
            _                  => Support.Pinned(_nextSupportId++, node)
        };
        Supports.Add(s); IsModified = true; return s;
    }

    public LoadCase AddLoadCase(string name, LoadCaseType type, double swFactor = 0)
    {
        var lc = new LoadCase(_nextLoadCaseId++, name, type, swFactor);
        LoadCases.Add(lc); IsModified = true; return lc;
    }

    public void AddNodeLoad(LoadCase lc, Node node, double fx, double fy, double fz,
                            double mx = 0, double my = 0, double mz = 0)
    {
        var load = new NodeLoad { Node = node, Fx = fx, Fy = fy, Fz = fz, Mx = mx, My = my, Mz = mz, LoadCase = lc };
        lc.Loads.Add(load); IsModified = true;
    }

    public void AddMemberLoad(LoadCase lc, Member member, double w1, double w2 = double.NaN,
                              LoadDirection dir = LoadDirection.GlobalY, MemberLoadType loadType = MemberLoadType.Uniform)
    {
        if (double.IsNaN(w2)) w2 = w1;
        var load = new MemberLoad { Member = member, W1 = w1, W2 = w2, Direction = dir, LoadType = loadType, LoadCase = lc };
        lc.Loads.Add(load); IsModified = true;
    }

    // ── Statistics ────────────────────────────────────────────────────────
    public int TotalDof => Nodes.Count * 6;
    public int RestrainedDof => Supports.SelectMany(s => s.GetRestrainedDofs()).Distinct().Count();
    public int FreeDof => TotalDof - RestrainedDof;

    // ── Default library initialisation ────────────────────────────────────
    public void LoadDefaultLibraries()
    {
        foreach (var mat in Material.DefaultLibrary) Materials.Add(mat);
        foreach (var sec in Section.DefaultLibrary) Sections.Add(sec);
        var dl = AddLoadCase("Dead Load", LoadCaseType.Dead, 1.0);
        var ll = AddLoadCase("Live Load", LoadCaseType.Live, 0);
        foreach (var combo in LoadCombination.GenerateIS875Combinations(dl, ll))
            LoadCombinations.Add(combo);
    }

    public void Clear()
    {
        Nodes.Clear(); Members.Clear(); Plates.Clear();
        Supports.Clear(); LoadCases.Clear(); LoadCombinations.Clear();
        Results = null; IsModified = false; AnalysisStatus = AnalysisStatus.NotAnalyzed;
        _nextNodeId = _nextMemberId = _nextPlateId = _nextSupportId = _nextLoadCaseId = _nextLoadCombinationId = 1;
    }
}
