namespace FeaLinux.Engine.Results;

public class AnalysisResults
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public TimeSpan SolveTime { get; set; }
    public Dictionary<int, NodeResult> NodeResults { get; } = [];
    public Dictionary<int, MemberResult> MemberResults { get; } = [];
    public Dictionary<int, double[]> Reactions { get; } = [];   // NodeId → [Rx,Ry,Rz,Mx,My,Mz]
    public int LoadCaseId { get; set; }
}

public class NodeResult
{
    public int NodeId { get; set; }
    public double Ux { get; set; }   // Displacement X (m)
    public double Uy { get; set; }   // Displacement Y (m)
    public double Uz { get; set; }   // Displacement Z (m)
    public double Rx { get; set; }   // Rotation X (rad)
    public double Ry { get; set; }   // Rotation Y (rad)
    public double Rz { get; set; }   // Rotation Z (rad)

    public double TotalDisplacement => Math.Sqrt(Ux * Ux + Uy * Uy + Uz * Uz);
    public double[] AsArray() => [Ux, Uy, Uz, Rx, Ry, Rz];
}

public class MemberResult
{
    public int MemberId { get; set; }
    /// <summary>Start-end forces in LOCAL member coordinates [Fx1,Fy1,Fz1,Mx1,My1,Mz1, Fx2,Fy2,Fz2,Mx2,My2,Mz2]</summary>
    public double[] LocalForces { get; set; } = new double[12];

    public double AxialStart    => LocalForces[0];
    public double ShearY_Start  => LocalForces[1];
    public double ShearZ_Start  => LocalForces[2];
    public double TorsionStart  => LocalForces[3];
    public double MomentY_Start => LocalForces[4];
    public double MomentZ_Start => LocalForces[5];
    public double AxialEnd      => LocalForces[6];
    public double ShearY_End    => LocalForces[7];
    public double ShearZ_End    => LocalForces[8];
    public double TorsionEnd    => LocalForces[9];
    public double MomentY_End  => LocalForces[10];
    public double MomentZ_End  => LocalForces[11];

    public double MaxAxial => Math.Max(Math.Abs(AxialStart), Math.Abs(AxialEnd));
    public double MaxMomentZ => Math.Max(Math.Abs(MomentZ_Start), Math.Abs(MomentZ_End));

    /// <summary>Generate BMD/SFD values at n points along the member.</summary>
    public (double[] Positions, double[] Values) GetDiagram(Core.Enums.DiagramType type, int n = 20)
    {
        var pos = Enumerable.Range(0, n + 1).Select(i => (double)i / n).ToArray();
        var vals = pos.Select(x => type switch
        {
            Core.Enums.DiagramType.Axial         => AxialStart + (AxialEnd - AxialStart) * x,
            Core.Enums.DiagramType.ShearFy       => ShearY_Start + (ShearY_End - ShearY_Start) * x,
            Core.Enums.DiagramType.BendingMomentZ=> MomentZ_Start + (MomentZ_End - MomentZ_Start) * x,
            Core.Enums.DiagramType.BendingMomentY=> MomentY_Start + (MomentY_End - MomentY_Start) * x,
            Core.Enums.DiagramType.Torsion       => TorsionStart,
            _ => 0.0
        }).ToArray();
        return (pos, vals);
    }
}
