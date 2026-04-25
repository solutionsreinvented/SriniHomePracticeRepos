using CommunityToolkit.Mvvm.ComponentModel;
using FeaLinux.Core.Enums;

namespace FeaLinux.Core.Models;

public abstract partial class Load : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private LoadCase _loadCase = null!;
    [ObservableProperty] private LoadType _type;
    [ObservableProperty] private string _description = string.Empty;
}

// ── Point / Nodal Load ────────────────────────────────────────────────────
public partial class NodeLoad : Load
{
    [ObservableProperty] private Node _node = null!;
    [ObservableProperty] private double _fx;  // kN
    [ObservableProperty] private double _fy;  // kN
    [ObservableProperty] private double _fz;  // kN
    [ObservableProperty] private double _mx;  // kN·m
    [ObservableProperty] private double _my;  // kN·m
    [ObservableProperty] private double _mz;  // kN·m
    public double Resultant => Math.Sqrt(Fx * Fx + Fy * Fy + Fz * Fz);
    public override string ToString() => $"NodeLoad @ N{Node?.Id}: Fx={Fx} Fy={Fy} Fz={Fz} kN";
}

// ── Member Distributed Load ───────────────────────────────────────────────
public partial class MemberLoad : Load
{
    [ObservableProperty] private Member _member = null!;
    [ObservableProperty] private MemberLoadType _loadType;
    [ObservableProperty] private LoadDirection _direction;
    [ObservableProperty] private double _w1;   // Start intensity (kN/m)
    [ObservableProperty] private double _w2;   // End intensity (kN/m)
    [ObservableProperty] private double _startDist;  // Relative start distance (0–1)
    [ObservableProperty] private double _endDist = 1.0;
    public override string ToString() => $"MemberLoad @ M{Member?.Id}: {W1}–{W2} kN/m ({Direction})";
}

// ── Area / Pressure Load on Plate ─────────────────────────────────────────
public partial class AreaLoad : Load
{
    [ObservableProperty] private Plate _plate = null!;
    [ObservableProperty] private double _pressure;   // kN/m²
    [ObservableProperty] private LoadDirection _direction;
    public override string ToString() => $"AreaLoad @ Plate{Plate?.Id}: {Pressure} kN/m²";
}

// ── Self-Weight ────────────────────────────────────────────────────────────
public partial class SelfWeightLoad : Load
{
    [ObservableProperty] private double _factor = 1.0;  // usually 1.0 (gravity dir = -Y)
    public override string ToString() => $"Self-Weight factor={Factor}";
}

// ── Temperature Load ───────────────────────────────────────────────────────
public partial class TemperatureLoad : Load
{
    [ObservableProperty] private Member? _member;
    [ObservableProperty] private double _deltaT;   // Temperature change (°C)
    [ObservableProperty] private double _topFibreT;
    [ObservableProperty] private double _bottomFibreT;
}
