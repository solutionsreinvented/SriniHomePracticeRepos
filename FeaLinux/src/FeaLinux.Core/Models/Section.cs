using CommunityToolkit.Mvvm.ComponentModel;
using FeaLinux.Core.Enums;

namespace FeaLinux.Core.Models;

public partial class Section : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private SectionType _type;

    // ── I / Channel / T section dimensions (m) ───────────────────────────
    [ObservableProperty] private double _depth;          // h
    [ObservableProperty] private double _flangeWidth;    // bf
    [ObservableProperty] private double _flangeThickness;// tf
    [ObservableProperty] private double _webThickness;   // tw

    // ── Rectangular / Circular ────────────────────────────────────────────
    [ObservableProperty] private double _width;   // b
    [ObservableProperty] private double _height;  // d
    [ObservableProperty] private double _diameter;// D

    // ── Directly assigned (for custom sections) ───────────────────────────
    [ObservableProperty] private double _area;
    [ObservableProperty] private double _ixx;   // Major axis moment of inertia
    [ObservableProperty] private double _iyy;   // Minor axis moment of inertia
    [ObservableProperty] private double _j;     // Torsional constant
    [ObservableProperty] private double _zxx;   // Elastic section modulus major
    [ObservableProperty] private double _zyy;   // Elastic section modulus minor
    [ObservableProperty] private double _zpx;   // Plastic section modulus major

    public void ComputeProperties()
    {
        switch (Type)
        {
            case SectionType.ISection:
                var tf = FlangeThickness; var tw = WebThickness;
                var bf = FlangeWidth; var h = Depth;
                var hw = h - 2 * tf;
                Area = 2 * bf * tf + hw * tw;
                Ixx = (bf * Math.Pow(h, 3) - (bf - tw) * Math.Pow(hw, 3)) / 12.0;
                Iyy = (2 * tf * Math.Pow(bf, 3) + hw * Math.Pow(tw, 3)) / 12.0;
                J = (2 * bf * Math.Pow(tf, 3) + hw * Math.Pow(tw, 3)) / 3.0;
                Zxx = Ixx / (h / 2);
                Zyy = Iyy / (bf / 2);
                Zpx = tw * hw * hw / 4 + bf * tf * (h - tf);
                break;
            case SectionType.Rectangular:
                Area = Width * Height;
                Ixx = Width * Math.Pow(Height, 3) / 12.0;
                Iyy = Height * Math.Pow(Width, 3) / 12.0;
                J = ComputeRectangularJ(Width, Height);
                Zxx = Ixx / (Height / 2); Zyy = Iyy / (Width / 2);
                Zpx = Width * Height * Height / 4;
                break;
            case SectionType.Circular:
            case SectionType.Pipe:
                Area = Math.PI * Math.Pow(Diameter / 2, 2);
                Ixx = Iyy = Math.PI * Math.Pow(Diameter, 4) / 64.0;
                J = Math.PI * Math.Pow(Diameter, 4) / 32.0;
                Zxx = Zyy = Ixx / (Diameter / 2);
                Zpx = Math.Pow(Diameter, 3) / 6.0;
                break;
        }
    }

    private static double ComputeRectangularJ(double b, double d)
    {
        if (b > d) (b, d) = (d, b);
        return b * b * b * d * (1.0 / 3.0 - 0.21 * b / d * (1 - Math.Pow(b, 4) / (12 * Math.Pow(d, 4))));
    }

    // ── Standard section presets ──────────────────────────────────────────
    public static Section ISMB200 => new Section()
    {
        Id = 1, Name = "ISMB 200", Type = SectionType.ISection,
        Depth = 0.200, FlangeWidth = 0.100, FlangeThickness = 0.0102, WebThickness = 0.0057
    }.WithComputed();
    public static Section ISMB300 => new Section()
    {
        Id = 2, Name = "ISMB 300", Type = SectionType.ISection,
        Depth = 0.300, FlangeWidth = 0.140, FlangeThickness = 0.0126, WebThickness = 0.0074
    }.WithComputed();
    public static Section ISMB400 => new Section()
    {
        Id = 3, Name = "ISMB 400", Type = SectionType.ISection,
        Depth = 0.400, FlangeWidth = 0.140, FlangeThickness = 0.0160, WebThickness = 0.0086
    }.WithComputed();
    public static Section Rect300x450 => new Section()
    {
        Id = 4, Name = "Rect 300×450", Type = SectionType.Rectangular,
        Width = 0.300, Height = 0.450
    }.WithComputed();
    public static IEnumerable<Section> DefaultLibrary => [ISMB200, ISMB300, ISMB400, Rect300x450];

    public override string ToString() => $"{Name} (A={Area * 1e4:F1} cm²)";
}

internal static class SectionExtensions
{
    public static Section WithComputed(this Section s) { s.ComputeProperties(); return s; }
}
