using CommunityToolkit.Mvvm.ComponentModel;
using FeaLinux.Core.Geometry;

namespace FeaLinux.Core.Models;

public partial class Node : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private double _x;
    [ObservableProperty] private double _y;
    [ObservableProperty] private double _z;
    [ObservableProperty] private string _label = string.Empty;
    [ObservableProperty] private bool _isSelected;

    public Node() { }
    public Node(int id, double x, double y, double z, string label = "")
    {
        _id = id; _x = x; _y = y; _z = z;
        _label = string.IsNullOrEmpty(label) ? $"N{id}" : label;
    }

    public FEPoint3D Position => new(X, Y, Z);

    /// <summary>Global DOF indices: [Tx, Ty, Tz, Rx, Ry, Rz] = [6*Id, 6*Id+1, ... 6*Id+5]</summary>
    public int[] GetDofIndices() => Enumerable.Range(6 * (Id - 1), 6).ToArray();

    public double DistanceTo(Node other) => Position.DistanceTo(other.Position);

    public override string ToString() => $"Node {Id} {Position}";
}
