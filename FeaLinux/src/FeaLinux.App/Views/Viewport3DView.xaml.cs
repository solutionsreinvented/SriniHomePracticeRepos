using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using FeaLinux.App.ViewModels;
using FeaLinux.Core.Enums;
using FeaLinux.Core.Models;
using HelixToolkit.Wpf;
using FEASelectionMode = FeaLinux.Core.Enums.SelectionMode;

namespace FeaLinux.App.Views;

public partial class Viewport3DView : UserControl
{
    private MainViewModel? _main;
    private ViewportViewModel? _vm;

    // Visual dictionaries for hit testing
    private readonly Dictionary<int, SphereVisual3D>   _nodeVisuals   = [];
    private readonly Dictionary<int, PipeVisual3D>     _memberVisuals = [];
    private readonly Dictionary<int, BoxVisual3D>      _supportVisuals = [];
    private readonly Dictionary<int, BillboardTextVisual3D> _labelVisuals = [];

    // Standard engineering colors
    private static readonly Color NodeColor     = Color.FromRgb(0xD4, 0x86, 0x0A); // amber
    private static readonly Color MemberColor   = Color.FromRgb(0x14, 0x64, 0xC0); // blue
    private static readonly Color ColumnColor   = Color.FromRgb(0x2E, 0x86, 0xDE); // lighter blue
    private static readonly Color BraceColor    = Color.FromRgb(0x8E, 0x44, 0xAD); // purple
    private static readonly Color SelectedColor = Color.FromRgb(0xFF, 0x6B, 0x35); // orange
    private static readonly Color SupportColor  = Color.FromRgb(0xC0, 0x39, 0x2B); // red
    private static readonly Color LoadColor     = Color.FromRgb(0x27, 0xAE, 0x60); // green

    public Viewport3DView()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is not ViewportViewModel vm) return;
        _vm   = vm;
        _main = vm.Main;

        // Collection changes → rebuild
        _vm.Model.Nodes.CollectionChanged    += (_, _) => Dispatcher.Invoke(RebuildScene);
        _vm.Model.Members.CollectionChanged  += (_, _) => Dispatcher.Invoke(RebuildScene);
        _vm.Model.Supports.CollectionChanged += (_, _) => Dispatcher.Invoke(RebuildScene);
        _vm.Model.LoadCases.CollectionChanged += (_, _) => Dispatcher.Invoke(RebuildScene);

        // Display toggle changes → rebuild
        _main.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName is nameof(MainViewModel.ShowLoads)
                or nameof(MainViewModel.ShowSupports)
                or nameof(MainViewModel.ShowNodeLabels)
                or nameof(MainViewModel.ShowDeformedShape)
                or nameof(MainViewModel.DeformationScale))
                Dispatcher.Invoke(RebuildScene);

            if (args.PropertyName == nameof(MainViewModel.IsAnalysisRunning))
                Dispatcher.Invoke(() => AnalysisOverlay.Visibility =
                    _main.IsAnalysisRunning ? Visibility.Visible : Visibility.Collapsed);

            if (args.PropertyName == nameof(MainViewModel.ActiveMode))
                Dispatcher.Invoke(() => ModeLabel.Text = _main.ActiveMode.ToString().ToUpper());
        };

        RebuildScene();
        SetPerspectiveView();
    }

    // ══════════════════════════════════════════════════════════════════════
    // SCENE REBUILD
    // ══════════════════════════════════════════════════════════════════════
    public void RebuildScene()
    {
        StructureRoot.Children.Clear();
        _nodeVisuals.Clear(); _memberVisuals.Clear();
        _supportVisuals.Clear(); _labelVisuals.Clear();

        if (_vm == null) return;

        DrawMembers();
        DrawNodes();
        if (_main!.ShowSupports)  DrawSupports();
        if (_main.ShowLoads)      DrawLoads();
        if (_main.ShowNodeLabels) DrawNodeLabels();
        if (_main.ShowDeformedShape && _vm.Model.Results is Engine.Results.AnalysisResults r)
            DrawDeformedShape(r);
    }

    // ── Members ────────────────────────────────────────────────────────────
    private void DrawMembers()
    {
        foreach (var m in _vm!.Model.Members)
        {
            var p0 = Pt(m.StartNode); var p1 = Pt(m.EndNode);
            var col = m.IsSelected ? SelectedColor : m.MemberType switch
            {
                MemberType.Column => ColumnColor,
                MemberType.Brace  => BraceColor,
                _                 => MemberColor
            };
            double dia = m.MemberType == MemberType.Column ? 0.22 : 0.16;
            var pipe = new PipeVisual3D
            {
                Point1 = p0, Point2 = p1, Diameter = dia, ThetaDiv = 10,
                Fill = new SolidColorBrush(col)
            };
            StructureRoot.Children.Add(pipe);
            _memberVisuals[m.Id] = pipe;
        }
    }

    // ── Nodes ──────────────────────────────────────────────────────────────
    private void DrawNodes()
    {
        foreach (var n in _vm!.Model.Nodes)
        {
            var sphere = new SphereVisual3D
            {
                Center = Pt(n), Radius = 0.14,
                PhiDiv = 12, ThetaDiv = 12,
                Fill = new SolidColorBrush(n.IsSelected ? SelectedColor : NodeColor)
            };
            StructureRoot.Children.Add(sphere);
            _nodeVisuals[n.Id] = sphere;
        }
    }

    // ── Node Labels ────────────────────────────────────────────────────────
    private void DrawNodeLabels()
    {
        foreach (var n in _vm!.Model.Nodes)
        {
            var lbl = new BillboardTextVisual3D
            {
                Text     = $"N{n.Id}",
                Position = new Point3D(n.X, n.Y + 0.28, n.Z),
                Foreground = new SolidColorBrush(Color.FromRgb(0x1C, 0x23, 0x33)),
                FontSize = 12, FontWeight = FontWeights.SemiBold
            };
            StructureRoot.Children.Add(lbl);
            _labelVisuals[n.Id] = lbl;
        }
    }

    // ── Supports ───────────────────────────────────────────────────────────
    private void DrawSupports()
    {
        foreach (var sup in _vm!.Model.Supports)
        {
            var pos = Pt(sup.Node);
            // Base plate
            var plate = new BoxVisual3D
            {
                Center = new Point3D(pos.X, pos.Y - 0.12, pos.Z),
                Width = 0.6, Height = 0.06, Length = 0.6,
                Fill = new SolidColorBrush(SupportColor)
            };
            StructureRoot.Children.Add(plate);
            _supportVisuals[sup.Id] = plate;

            // Ground hatch lines
            for (int i = -2; i <= 2; i++)
            {
                var line = new LinesVisual3D
                {
                    Color = SupportColor, Thickness = 1.5,
                    Points = new Point3DCollection {
                        new(pos.X + i * 0.12 - 0.1, pos.Y - 0.16, pos.Z),
                        new(pos.X + i * 0.12 + 0.1, pos.Y - 0.24, pos.Z)
                    }
                };
                StructureRoot.Children.Add(line);
            }
        }
    }

    // ── Loads ──────────────────────────────────────────────────────────────
    private void DrawLoads()
    {
        foreach (var lc in _vm!.Model.LoadCases)
        {
            foreach (var load in lc.Loads.OfType<NodeLoad>())
            {
                if (load.Resultant < 1e-6) continue;
                var pos = Pt(load.Node);
                var dir = new Vector3D(load.Fx, load.Fy, load.Fz);
                dir.Normalize();
                var arrowStart = pos + dir * -1.8;
                var arrow = new ArrowVisual3D
                {
                    Point1 = arrowStart, Point2 = pos,
                    Diameter = 0.09, Fill = new SolidColorBrush(LoadColor)
                };
                StructureRoot.Children.Add(arrow);
                StructureRoot.Children.Add(new BillboardTextVisual3D
                {
                    Text = $"{load.Resultant:F1} kN", Position = arrowStart,
                    Foreground = new SolidColorBrush(LoadColor), FontSize = 10
                });
            }
            foreach (var load in lc.Loads.OfType<MemberLoad>())
                DrawMemberLoadArrows(load);
        }
    }

    private void DrawMemberLoadArrows(MemberLoad load)
    {
        var m = load.Member;
        for (int i = 0; i <= 5; i++)
        {
            double t = (double)i / 5;
            double x = m.StartNode.X + (m.EndNode.X - m.StartNode.X) * t;
            double y = m.StartNode.Y + (m.EndNode.Y - m.StartNode.Y) * t;
            double z = m.StartNode.Z + (m.EndNode.Z - m.StartNode.Z) * t;
            double w = load.W1 + (load.W2 - load.W1) * t;
            double len = Math.Abs(w) / 40.0;
            if (len < 0.04) continue;
            StructureRoot.Children.Add(new ArrowVisual3D
            {
                Point1 = new Point3D(x, y + len, z),
                Point2 = new Point3D(x, y, z),
                Diameter = 0.04, Fill = new SolidColorBrush(LoadColor)
            });
        }
    }

    // ── Deformed Shape ─────────────────────────────────────────────────────
    private void DrawDeformedShape(Engine.Results.AnalysisResults results)
    {
        double scale = _main!.DeformationScale;
        foreach (var m in _vm!.Model.Members)
        {
            if (!results.NodeResults.TryGetValue(m.StartNode.Id, out var sn)) continue;
            if (!results.NodeResults.TryGetValue(m.EndNode.Id,   out var en)) continue;
            var p0 = new Point3D(m.StartNode.X + sn.Ux * scale,
                                 m.StartNode.Y + sn.Uy * scale,
                                 m.StartNode.Z + sn.Uz * scale);
            var p1 = new Point3D(m.EndNode.X   + en.Ux * scale,
                                 m.EndNode.Y   + en.Uy * scale,
                                 m.EndNode.Z   + en.Uz * scale);
            StructureRoot.Children.Add(new LinesVisual3D
            {
                Color = Color.FromRgb(0xFF, 0x6B, 0x35),
                Thickness = 2.5,
                Points = new Point3DCollection { p0, p1 }
            });
        }
    }

    // ══════════════════════════════════════════════════════════════════════
    // MOUSE INTERACTION
    // ══════════════════════════════════════════════════════════════════════
    private void Viewport_MouseMove(object sender, MouseEventArgs e)
    {
        if (_main == null) return;
        var pt = Viewport.FindNearestPoint(e.GetPosition(Viewport));
        if (pt.HasValue) _main.UpdateCoordinateDisplay(pt.Value.X, pt.Value.Y, pt.Value.Z);
    }

    private void Viewport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (_main == null) return;
        var mousePos = e.GetPosition(Viewport);
        var mode = _main.ActiveMode;

        if (mode == FEASelectionMode.Select)
        {
            HitTest(mousePos);
        }
        else if (mode == FEASelectionMode.AddNode)
        {
            // Place node at grid-snapped ground plane
            var worldPt = PickGroundPlane(mousePos);
            if (worldPt.HasValue)
            {
                _main.InputX = $"{worldPt.Value.X:F2}";
                _main.InputY = $"{worldPt.Value.Y:F2}";
                _main.InputZ = $"{worldPt.Value.Z:F2}";
                _main.ConfirmAddNodeCommand.Execute(null);
            }
        }
        else if (mode == FEASelectionMode.AddMember)
        {
            // Click a node sphere to start/end member
            var hitNode = HitTestNode(mousePos);
            if (hitNode != null) _main.HandleMemberNodeClick(hitNode);
        }
        else if (mode == FEASelectionMode.AddSupport)
        {
            var hitNode = HitTestNode(mousePos);
            if (hitNode != null) _main.HandleSupportClick(hitNode);
        }
    }

    private void Viewport_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        => _main?.SetModeSelectCommand.Execute(null);

    // ── Hit testing ────────────────────────────────────────────────────────
    private void HitTest(Point mousePos)
    {
        if (_vm == null) return;
        var hits = Viewport.Viewport.FindHits(mousePos);
        _vm.Selection.ClearAll();

        foreach (var hit in hits)
        {
            // Node?
            foreach (var (id, vis) in _nodeVisuals)
            {
                if (hit.Visual != vis) continue;
                var node = _vm.Model.Nodes.FirstOrDefault(n => n.Id == id);
                if (node == null) continue;
                _vm.Selection.SelectNode(node);
                _main?.PropertiesVM?.ShowNode(node);
                RebuildScene(); return;
            }
            // Member?
            foreach (var (id, vis) in _memberVisuals)
            {
                if (hit.Visual != vis) continue;
                var member = _vm.Model.Members.FirstOrDefault(m => m.Id == id);
                if (member == null) continue;
                _vm.Selection.SelectMember(member);
                _main?.PropertiesVM?.ShowMember(member);
                RebuildScene(); return;
            }
            // Support?
            foreach (var (id, vis) in _supportVisuals)
            {
                if (hit.Visual != vis) continue;
                var sup = _vm.Model.Supports.FirstOrDefault(s => s.Id == id);
                if (sup != null) _vm.Selection.SelectNode(sup.Node);
                RebuildScene(); return;
            }
        }
        _main?.PropertiesVM?.Clear();
        RebuildScene();
    }

    private Node? HitTestNode(Point mousePos)
    {
        if (_vm == null) return null;
        var hits = Viewport.Viewport.FindHits(mousePos);
        foreach (var hit in hits)
            foreach (var (id, vis) in _nodeVisuals)
                if (hit.Visual == vis)
                    return _vm.Model.Nodes.FirstOrDefault(n => n.Id == id);
        return null;
    }

    // Raycast to Y=0 plane
    private Point3D? PickGroundPlane(Point mousePos)
    {
        var ray = Viewport.Viewport.GetRay(mousePos);
        if (ray.Direction.Y == 0) return null;
        double t = -ray.Origin.Y / ray.Direction.Y;
        if (t < 0) return null;
        var pt = ray.Origin + t * ray.Direction;
        // Grid snap to 0.5 m
        return new Point3D(Math.Round(pt.X * 2) / 2, 0, Math.Round(pt.Z * 2) / 2);
    }

    // ══════════════════════════════════════════════════════════════════════
    // CAMERA PRESETS — Y-up, X right, Z toward viewer
    // ══════════════════════════════════════════════════════════════════════
    public void SetPerspectiveView()
    {
        var b = GetBounds();
        double d = b.Size * 2.2;
        Viewport.Camera.Position     = new Point3D(b.Cx + d * 0.7, b.Cy + d * 0.6, b.Cz + d * 0.7);
        Viewport.Camera.LookDirection = new Vector3D(-0.7, -0.6, -0.7);
        Viewport.Camera.UpDirection  = new Vector3D(0, 1, 0);
    }

    public void SetTopView()
    {
        var b = GetBounds();
        Viewport.Camera.Position     = new Point3D(b.Cx, b.Cy + b.Size * 3, b.Cz);
        Viewport.Camera.LookDirection = new Vector3D(0, -1, 0);
        Viewport.Camera.UpDirection  = new Vector3D(0, 0, -1);
    }

    public void SetFrontView()
    {
        var b = GetBounds();
        Viewport.Camera.Position     = new Point3D(b.Cx, b.Cy, b.Cz + b.Size * 3);
        Viewport.Camera.LookDirection = new Vector3D(0, 0, -1);
        Viewport.Camera.UpDirection  = new Vector3D(0, 1, 0);
    }

    public void SetRightView()
    {
        var b = GetBounds();
        Viewport.Camera.Position     = new Point3D(b.Cx + b.Size * 3, b.Cy, b.Cz);
        Viewport.Camera.LookDirection = new Vector3D(-1, 0, 0);
        Viewport.Camera.UpDirection  = new Vector3D(0, 1, 0);
    }

    public void FitAll() => Viewport.ZoomExtents(500);

    private (double Cx, double Cy, double Cz, double Size) GetBounds()
    {
        if (_vm == null || !_vm.Model.Nodes.Any()) return (6, 2, 0, 14);
        var ns = _vm.Model.Nodes;
        double minX = ns.Min(n => n.X), maxX = ns.Max(n => n.X);
        double minY = ns.Min(n => n.Y), maxY = ns.Max(n => n.Y);
        double minZ = ns.Min(n => n.Z), maxZ = ns.Max(n => n.Z);
        double size = Math.Max(1, Math.Max(maxX - minX, Math.Max(maxY - minY, maxZ - minZ)));
        return ((minX + maxX) / 2, (minY + maxY) / 2, (minZ + maxZ) / 2, size);
    }

    private static Point3D Pt(Node n) => new(n.X, n.Y, n.Z);
}
