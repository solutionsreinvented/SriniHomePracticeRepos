using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FeaLinux.App.Services;
using FeaLinux.App.ViewModels;
using FeaLinux.Core.Enums;
using FeaLinux.Core.Models;

namespace FeaLinux.App.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly StructuralModel _model;
    private readonly SelectionService _selection;
    private readonly AnalysisService _analysisService;
    private readonly CommandHistory _history;
    private readonly ProjectService _projectService;

    // ── Sub-ViewModels (set by MainWindow ctor) ────────────────────────────
    public ModelTreeViewModel?  ModelTreeVM  { get; set; }
    public PropertiesViewModel? PropertiesVM { get; set; }
    public ViewportViewModel?   ViewportVM   { get; set; }

    // ── Observable Properties ──────────────────────────────────────────────
    [ObservableProperty] private string _title = "FeaLinux — Structural Analysis & Design";
    [ObservableProperty] private string _statusMessage = "Ready";
    [ObservableProperty] private string _coordinateDisplay = "X: 0.000   Y: 0.000   Z: 0.000";
    [ObservableProperty] private string _unitDisplay = "kN · m · °C";
    [ObservableProperty] private SelectionMode _activeMode = SelectionMode.Select;
    [ObservableProperty] private bool _isAnalysisRunning;
    [ObservableProperty] private bool _showDeformedShape;
    [ObservableProperty] private bool _showLoads = true;
    [ObservableProperty] private bool _showSupports = true;
    [ObservableProperty] private bool _showNodeLabels = true;
    [ObservableProperty] private bool _showMemberLabels;
    [ObservableProperty] private RenderMode _renderMode = RenderMode.Solid;
    [ObservableProperty] private double _deformationScale = 100.0;

    // ── Input Bar properties ──────────────────────────────────────────────
    [ObservableProperty] private string _inputX = "0";
    [ObservableProperty] private string _inputY = "0";
    [ObservableProperty] private string _inputZ = "0";
    [ObservableProperty] private string _inputBarPrompt = "";
    [ObservableProperty] private string _memberStatus = "";

    public bool IsInputBarVisible => ActiveMode is SelectionMode.AddNode or SelectionMode.AddMember;
    public bool IsAddNodeMode     => ActiveMode == SelectionMode.AddNode;
    public bool IsAddMemberMode   => ActiveMode == SelectionMode.AddMember;

    // Member-build state
    private Node? _memberStart;

    // ── Expose model to XAML ───────────────────────────────────────────────
    public StructuralModel Model     => _model;
    public SelectionService Selection => _selection;

    // ── Scene refresh event (subscribed by MainWindow) ─────────────────────
    public event Action? SceneRefreshRequested;

    // ── Constructor ────────────────────────────────────────────────────────
    public MainViewModel(StructuralModel model, SelectionService selection,
        AnalysisService analysisService, CommandHistory history, ProjectService projectService)
    {
        _model = model; _selection = selection;
        _analysisService = analysisService; _history = history; _projectService = projectService;

        _analysisService.ProgressChanged += msg => StatusMessage = msg;
        _analysisService.AnalysisCompleted += ok =>
        {
            IsAnalysisRunning = false;
            if (ok)
            {
                StatusMessage = "✔ Analysis completed successfully. Deformed shape shown.";
                ShowDeformedShape = true;
            }
            else
            {
                StatusMessage = "✘ Analysis failed — check output panel.";
            }
            SceneRefreshRequested?.Invoke();
        };

        LoadDemoPortalFrame();
    }

    // ── Active-mode changes notify input bar ──────────────────────────────
    partial void OnActiveModeChanged(SelectionMode value)
    {
        OnPropertyChanged(nameof(IsInputBarVisible));
        OnPropertyChanged(nameof(IsAddNodeMode));
        OnPropertyChanged(nameof(IsAddMemberMode));
        _memberStart = null;
        InputBarPrompt = value switch
        {
            SelectionMode.AddNode   => "ADD NODE — enter coordinates:",
            SelectionMode.AddMember => "ADD MEMBER — enter start-node coordinates:",
            _                       => ""
        };
        MemberStatus = "";
    }

    // ── Mode Commands ──────────────────────────────────────────────────────
    [RelayCommand] void SetModeSelect()    => ActiveMode = SelectionMode.Select;
    [RelayCommand] void SetModeAddNode()   => ActiveMode = SelectionMode.AddNode;
    [RelayCommand] void SetModeAddMember() => ActiveMode = SelectionMode.AddMember;
    [RelayCommand] void SetModeAddPlate()  => ActiveMode = SelectionMode.AddPlate;
    [RelayCommand] void SetModeAddSupport()=> ActiveMode = SelectionMode.AddSupport;
    [RelayCommand] void SetModeAddLoad()   => ActiveMode = SelectionMode.AddLoad;

    // ── Input Bar Commands ─────────────────────────────────────────────────
    [RelayCommand]
    void ConfirmAddNode()
    {
        if (!TryParseCoords(out double x, out double y, out double z)) return;
        var node = _model.AddNode(x, y, z);
        StatusMessage = $"Node N{node.Id} added at ({x:F3}, {y:F3}, {z:F3})";
        InputX = "0"; InputY = "0"; InputZ = "0";
        SceneRefreshRequested?.Invoke();
    }

    [RelayCommand]
    void ConfirmMemberStart()
    {
        if (!TryParseCoords(out double x, out double y, out double z)) return;
        // Find or create node at coords
        _memberStart = _model.Nodes.FirstOrDefault(n =>
            Math.Abs(n.X - x) < 0.001 && Math.Abs(n.Y - y) < 0.001 && Math.Abs(n.Z - z) < 0.001)
            ?? _model.AddNode(x, y, z);
        MemberStatus = $"Start: N{_memberStart.Id} — now enter end coordinates and click Add End";
        InputBarPrompt = "ADD MEMBER — enter end-node coordinates:";
        SceneRefreshRequested?.Invoke();
    }

    // Called from viewport when user clicks a node in AddMember mode
    public void HandleMemberNodeClick(Node node)
    {
        if (_memberStart == null)
        {
            _memberStart = node;
            MemberStatus = $"Start: N{node.Id} — click end node";
            InputBarPrompt = "ADD MEMBER — click the end node:";
        }
        else if (_memberStart.Id != node.Id)
        {
            var sec = _model.Sections.FirstOrDefault();
            var mat = _model.Materials.FirstOrDefault();
            if (sec == null || mat == null) { StatusMessage = "⚠ Assign a section and material first."; return; }
            var m = _model.AddMember(_memberStart, node, sec, mat);
            StatusMessage = $"Member M{m.Id} added: N{_memberStart.Id}→N{node.Id}";
            _memberStart = null;
            MemberStatus = "Click next start node or press Esc";
            SceneRefreshRequested?.Invoke();
        }
    }

    // Support placement from viewport click
    public void HandleSupportClick(Node node)
    {
        _model.AddSupport(node, SupportType.Fixed);
        StatusMessage = $"Fixed support added at N{node.Id}";
        SceneRefreshRequested?.Invoke();
    }

    private bool TryParseCoords(out double x, out double y, out double z)
    {
        x = y = z = 0;
        if (!double.TryParse(InputX, out x)) { StatusMessage = "⚠ Invalid X coordinate."; return false; }
        if (!double.TryParse(InputY, out y)) { StatusMessage = "⚠ Invalid Y coordinate."; return false; }
        if (!double.TryParse(InputZ, out z)) { StatusMessage = "⚠ Invalid Z coordinate."; return false; }
        return true;
    }

    // ── File Commands ──────────────────────────────────────────────────────
    [RelayCommand] async Task NewProject()
    {
        await _projectService.NewProjectAsync();
        StatusMessage = "New project created.";
        SceneRefreshRequested?.Invoke();
    }
    [RelayCommand] void Undo() { _history.Undo(); StatusMessage = "Undo"; }
    [RelayCommand] void Redo() { _history.Redo(); StatusMessage = "Redo"; }

    // ── Analysis ───────────────────────────────────────────────────────────
    [RelayCommand]
    async Task RunAnalysis()
    {
        var (ok, err) = new Engine.AnalysisEngine(_model).ValidateModel();
        if (!ok) { StatusMessage = $"⚠ {err}"; return; }
        IsAnalysisRunning = true;
        StatusMessage = "Running analysis…";
        await _analysisService.RunAsync();
    }

    // ── View Commands ──────────────────────────────────────────────────────
    [RelayCommand] void ToggleDeformedShape()
    {
        ShowDeformedShape = !ShowDeformedShape;
        SceneRefreshRequested?.Invoke();
    }

    public void UpdateCoordinateDisplay(double x, double y, double z)
        => CoordinateDisplay = $"X: {x:F3}   Y: {y:F3}   Z: {z:F3}";

    // ── Demo Portal Frame ──────────────────────────────────────────────────
    private void LoadDemoPortalFrame()
    {
        try
        {
            _model.LoadDefaultLibraries();
            var steel   = _model.Materials.First(m => m.Type == MaterialType.Steel);
            var ismb300 = _model.Sections.First(s => s.Name.Contains("300"));
            var ismb200 = _model.Sections.First(s => s.Name.Contains("200"));

            // 2-bay portal frame: 6 m span, 4 m height
            var n1 = _model.AddNode(0,  0, 0);
            var n2 = _model.AddNode(6,  0, 0);
            var n3 = _model.AddNode(12, 0, 0);
            var n4 = _model.AddNode(0,  4, 0);
            var n5 = _model.AddNode(6,  4, 0);
            var n6 = _model.AddNode(12, 4, 0);

            _model.AddMember(n1, n4, ismb300, steel, MemberType.Column);
            _model.AddMember(n2, n5, ismb300, steel, MemberType.Column);
            _model.AddMember(n3, n6, ismb300, steel, MemberType.Column);
            _model.AddMember(n4, n5, ismb200, steel, MemberType.Beam);
            _model.AddMember(n5, n6, ismb200, steel, MemberType.Beam);

            _model.AddSupport(n1, SupportType.Fixed);
            _model.AddSupport(n2, SupportType.Fixed);
            _model.AddSupport(n3, SupportType.Fixed);

            var ll = _model.LoadCases.First(lc => lc.Type == LoadCaseType.Live);
            _model.AddMemberLoad(ll, _model.Members[3], -20, -20, LoadDirection.GlobalY);
            _model.AddMemberLoad(ll, _model.Members[4], -20, -20, LoadDirection.GlobalY);
            _model.AddNodeLoad(ll, n5, 10, 0, 0);

            _model.IsModified = false;
            StatusMessage = "Demo portal frame loaded — press F5 or Run Analysis to solve.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Demo load error: {ex.Message}";
        }
    }
}
