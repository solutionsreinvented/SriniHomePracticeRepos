using CommunityToolkit.Mvvm.ComponentModel;
using FeaLinux.Core.Enums;

namespace FeaLinux.Core.Models;

public partial class Support : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private Node _node = null!;
    [ObservableProperty] private bool _tx = true;   // Restrain translation X
    [ObservableProperty] private bool _ty = true;   // Restrain translation Y
    [ObservableProperty] private bool _tz = true;   // Restrain translation Z
    [ObservableProperty] private bool _rx;           // Restrain rotation X
    [ObservableProperty] private bool _ry;           // Restrain rotation Y
    [ObservableProperty] private bool _rz;           // Restrain rotation Z
    [ObservableProperty] private double _springKx;  // Spring stiffness X (kN/m)
    [ObservableProperty] private double _springKy;
    [ObservableProperty] private double _springKz;
    [ObservableProperty] private SupportType _supportType;

    public bool IsFixed => Tx && Ty && Tz && Rx && Ry && Rz;
    public bool IsPinned => Tx && Ty && Tz && !Rx && !Ry && !Rz;

    /// <summary>Returns list of global DOF indices that are restrained.</summary>
    public List<int> GetRestrainedDofs()
    {
        var dofs = Node.GetDofIndices();
        var restrained = new List<int>();
        if (Tx) restrained.Add(dofs[0]);
        if (Ty) restrained.Add(dofs[1]);
        if (Tz) restrained.Add(dofs[2]);
        if (Rx) restrained.Add(dofs[3]);
        if (Ry) restrained.Add(dofs[4]);
        if (Rz) restrained.Add(dofs[5]);
        return restrained;
    }

    public static Support Fixed(int id, Node node) =>
        new() { Id = id, Node = node, Tx=true, Ty=true, Tz=true, Rx=true, Ry=true, Rz=true, SupportType=SupportType.Fixed };
    public static Support Pinned(int id, Node node) =>
        new() { Id = id, Node = node, Tx=true, Ty=true, Tz=true, Rx=false, Ry=false, Rz=false, SupportType=SupportType.Pinned };
    public static Support RollerY(int id, Node node) =>
        new() { Id = id, Node = node, Tx=false, Ty=true, Tz=false, Rx=false, Ry=false, Rz=false, SupportType=SupportType.RollerY };

    public override string ToString() => $"Support {Id} @ Node {Node?.Id} [{(Tx?"TX ":"")}{(Ty?"TY ":"")}{(Tz?"TZ ":"")}{(Rx?"RX ":"")}{(Ry?"RY ":"")}{(Rz?"RZ":"")}]";
}
