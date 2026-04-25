namespace FeaLinux.Core.Geometry;

/// <summary>
/// 3D point in FEA coordinate system: X=right, Y=up, Z=toward viewer
/// </summary>
public struct FEPoint3D : IEquatable<FEPoint3D>
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public FEPoint3D(double x, double y, double z) { X = x; Y = y; Z = z; }

    public static FEPoint3D Origin => new(0, 0, 0);

    public double DistanceTo(FEPoint3D other)
    {
        double dx = X - other.X, dy = Y - other.Y, dz = Z - other.Z;
        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    public FEVector3D VectorTo(FEPoint3D other) => new(other.X - X, other.Y - Y, other.Z - Z);

    public static FEPoint3D operator +(FEPoint3D p, FEVector3D v) => new(p.X + v.X, p.Y + v.Y, p.Z + v.Z);
    public static FEVector3D operator -(FEPoint3D a, FEPoint3D b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public bool Equals(FEPoint3D other) => Math.Abs(X - other.X) < 1e-9 && Math.Abs(Y - other.Y) < 1e-9 && Math.Abs(Z - other.Z) < 1e-9;
    public override bool Equals(object? obj) => obj is FEPoint3D p && Equals(p);
    public override int GetHashCode() => HashCode.Combine(Math.Round(X, 6), Math.Round(Y, 6), Math.Round(Z, 6));
    public override string ToString() => $"({X:F3}, {Y:F3}, {Z:F3})";
}

public struct FEVector3D
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public FEVector3D(double x, double y, double z) { X = x; Y = y; Z = z; }

    public double Magnitude => Math.Sqrt(X * X + Y * Y + Z * Z);

    public FEVector3D Normalized()
    {
        double mag = Magnitude;
        if (mag < 1e-12) return new(0, 0, 0);
        return new(X / mag, Y / mag, Z / mag);
    }

    public static FEVector3D Cross(FEVector3D a, FEVector3D b) =>
        new(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);

    public static double Dot(FEVector3D a, FEVector3D b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

    public static FEVector3D operator *(FEVector3D v, double s) => new(v.X * s, v.Y * s, v.Z * s);
    public static FEVector3D operator +(FEVector3D a, FEVector3D b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static FEVector3D UnitY => new(0, 1, 0);
    public static FEVector3D UnitX => new(1, 0, 0);
    public static FEVector3D UnitZ => new(0, 0, 1);

    public override string ToString() => $"[{X:F4}, {Y:F4}, {Z:F4}]";
}
