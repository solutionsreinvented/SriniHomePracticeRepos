using CommunityToolkit.Mvvm.ComponentModel;
using FeaLinux.Core.Enums;

namespace FeaLinux.Core.Models;

public partial class Plate : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private List<Node> _nodes = [];  // 3 or 4 nodes
    [ObservableProperty] private Section _section = null!;
    [ObservableProperty] private Material _material = null!;
    [ObservableProperty] private PlateType _plateType = PlateType.Shell;
    [ObservableProperty] private double _thickness;  // m
    [ObservableProperty] private string _label = string.Empty;
    [ObservableProperty] private bool _isSelected;

    public Plate() { }
    public Plate(int id, IEnumerable<Node> nodes, double thickness, Material material)
    {
        _id = id; _nodes = nodes.ToList(); _thickness = thickness;
        _material = material; _label = $"P{id}";
        _section = new Section { Name = $"Slab {thickness * 1000:F0}mm", Type = SectionType.Rectangular, Thickness = thickness };
    }

    public int NodeCount => Nodes.Count;
    public bool IsQuad => NodeCount == 4;
    public bool IsTriangle => NodeCount == 3;

    public Geometry.FEPoint3D Centroid
    {
        get
        {
            if (!Nodes.Any()) return Geometry.FEPoint3D.Origin;
            return new(Nodes.Average(n => n.X), Nodes.Average(n => n.Y), Nodes.Average(n => n.Z));
        }
    }

    public double Area
    {
        get
        {
            if (NodeCount < 3) return 0;
            // Shoelace/cross-product method for polygon area
            double area = 0;
            for (int i = 0; i < NodeCount; i++)
            {
                var a = Nodes[i].Position;
                var b = Nodes[(i + 1) % NodeCount].Position;
                var c = Nodes[(i + 2) % NodeCount].Position;
                var ab = b - a; var ac = c - a;
                area += Geometry.FEVector3D.Cross(ab, ac).Magnitude;
            }
            return area / 2.0;
        }
    }
    public override string ToString() => $"Plate {Id} ({NodeCount}-node, t={Thickness * 1000:F0}mm)";
}

// Extension to Section to hold plate thickness
public partial class Section
{
    public double Thickness { get; set; }
}
