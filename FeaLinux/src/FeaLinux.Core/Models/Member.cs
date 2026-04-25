using CommunityToolkit.Mvvm.ComponentModel;
using FeaLinux.Core.Enums;

namespace FeaLinux.Core.Models;

public partial class Member : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private Node _startNode = null!;
    [ObservableProperty] private Node _endNode = null!;
    [ObservableProperty] private Section _section = null!;
    [ObservableProperty] private Material _material = null!;
    [ObservableProperty] private MemberType _memberType;
    [ObservableProperty] private MemberReleaseType _releaseType;
    [ObservableProperty] private string _label = string.Empty;
    [ObservableProperty] private bool _isSelected;
    [ObservableProperty] private double _betaAngle;  // Cross-section rotation angle (degrees)

    // End releases (moment releases as 6-element boolean arrays)
    public bool[] StartReleases { get; set; } = new bool[6]; // Tx,Ty,Tz,Rx,Ry,Rz
    public bool[] EndReleases { get; set; } = new bool[6];

    public Member() { }
    public Member(int id, Node start, Node end, Section section, Material material,
                  MemberType type = MemberType.Beam)
    {
        _id = id; _startNode = start; _endNode = end;
        _section = section; _material = material; _memberType = type;
        _label = $"M{id}";
    }

    public double Length => StartNode?.DistanceTo(EndNode) ?? 0;

    /// <summary>Unit vector along member axis (local x-axis)</summary>
    public Geometry.FEVector3D LocalX =>
        StartNode.Position.VectorTo(EndNode.Position).Normalized();

    public override string ToString() => $"Member {Id}: N{StartNode?.Id}→N{EndNode?.Id} L={Length:F3}m [{MemberType}]";
}
