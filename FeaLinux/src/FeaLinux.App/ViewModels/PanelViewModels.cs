using CommunityToolkit.Mvvm.ComponentModel;
using FeaLinux.App.Services;
using FeaLinux.Core.Models;

namespace FeaLinux.App.ViewModels;

// ── Model Tree ────────────────────────────────────────────────────────────
public partial class ModelTreeViewModel : ObservableObject
{
    public StructuralModel Model { get; }
    public ModelTreeViewModel(StructuralModel model) => Model = model;
}

// ── Properties Panel ─────────────────────────────────────────────────────
public partial class PropertiesViewModel : ObservableObject
{
    private readonly SelectionService _sel;

    [ObservableProperty] private string _headerText = "No Selection";
    [ObservableProperty] private object? _selectedItem;

    public PropertiesViewModel(SelectionService selection)
    {
        _sel = selection;
        _sel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(SelectionService.SelectedObject))
                UpdateFromSelection();
        };
    }

    private void UpdateFromSelection()
    {
        SelectedItem = _sel.SelectedObject;
        HeaderText = _sel.SelectedObject switch
        {
            Node n    => $"Node N{n.Id}",
            Member m  => $"Member M{m.Id}",
            Plate p   => $"Plate P{p.Id}",
            Support s => $"Support at N{s.Node.Id}",
            null      => "No Selection",
            _         => "Object"
        };
    }

    public void ShowNode(Node n)     { SelectedItem = n;    HeaderText = $"Node N{n.Id}"; }
    public void ShowMember(Member m) { SelectedItem = m;    HeaderText = $"Member M{m.Id} — L={m.Length:F3} m"; }
    public void Clear()              { SelectedItem = null; HeaderText = "No Selection"; }
}

// ── Viewport ─────────────────────────────────────────────────────────────
public partial class ViewportViewModel : ObservableObject
{
    public StructuralModel Model { get; }
    public SelectionService Selection { get; }
    public MainViewModel Main { get; }

    public ViewportViewModel(StructuralModel model, SelectionService selection, MainViewModel main)
    { Model = model; Selection = selection; Main = main; }
}
