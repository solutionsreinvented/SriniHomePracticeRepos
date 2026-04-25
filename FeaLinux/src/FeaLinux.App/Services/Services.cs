using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using FeaLinux.Core.Models;

namespace FeaLinux.App.Services;

public partial class SelectionService : ObservableObject
{
    [ObservableProperty] private Node? _selectedNode;
    [ObservableProperty] private Member? _selectedMember;
    [ObservableProperty] private Plate? _selectedPlate;
    [ObservableProperty] private object? _selectedObject;

    public ObservableCollection<object> MultiSelection { get; } = [];

    public void SelectNode(Node? node)
    {
        ClearAll();
        SelectedNode = node;
        SelectedObject = node;
        if (node != null) { node.IsSelected = true; MultiSelection.Add(node); }
    }
    public void SelectMember(Member? m)
    {
        ClearAll();
        SelectedMember = m;
        SelectedObject = m;
        if (m != null) { m.IsSelected = true; MultiSelection.Add(m); }
    }
    public void ClearAll()
    {
        if (SelectedNode != null) SelectedNode.IsSelected = false;
        if (SelectedMember != null) SelectedMember.IsSelected = false;
        SelectedNode = null; SelectedMember = null; SelectedPlate = null;
        SelectedObject = null; MultiSelection.Clear();
    }
}

public class ProjectService
{
    private readonly StructuralModel _model;
    public ProjectService(StructuralModel model) => _model = model;

    public async Task NewProjectAsync()
    {
        _model.Clear();
        _model.LoadDefaultLibraries();
        _model.ProjectName = "Untitled Project";
        await Task.CompletedTask;
    }

    public async Task SaveAsync(string path)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(new
        {
            ProjectName = _model.ProjectName,
            NodeCount = _model.Nodes.Count,
            MemberCount = _model.Members.Count
        }, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(path, json);
        _model.ProjectPath = path;
        _model.IsModified = false;
    }
}

public class CommandHistory
{
    private readonly Stack<IUndoableCommand> _undoStack = new();
    private readonly Stack<IUndoableCommand> _redoStack = new();

    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;

    public void Execute(IUndoableCommand cmd) { cmd.Execute(); _undoStack.Push(cmd); _redoStack.Clear(); }
    public void Undo() { if (!CanUndo) return; var cmd = _undoStack.Pop(); cmd.Undo(); _redoStack.Push(cmd); }
    public void Redo() { if (!CanRedo) return; var cmd = _redoStack.Pop(); cmd.Execute(); _undoStack.Push(cmd); }
}

public interface IUndoableCommand { void Execute(); void Undo(); }

public class AnalysisService
{
    private readonly Engine.AnalysisEngine _engine;
    public event Action<string>? ProgressChanged;
    public event Action<bool>? AnalysisCompleted;

    public AnalysisService(Engine.AnalysisEngine engine) => _engine = engine;

    public async Task RunAsync(CancellationToken ct = default)
    {
        var progress = new Progress<string>(msg => ProgressChanged?.Invoke(msg));
        bool ok = await _engine.RunAllLoadCasesAsync(progress, ct);
        AnalysisCompleted?.Invoke(ok);
    }
}
