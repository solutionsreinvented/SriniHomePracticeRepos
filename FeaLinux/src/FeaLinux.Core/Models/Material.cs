using CommunityToolkit.Mvvm.ComponentModel;
using FeaLinux.Core.Enums;

namespace FeaLinux.Core.Models;

public partial class Material : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private MaterialType _type;
    [ObservableProperty] private double _elasticModulus;      // E  (kN/m²)
    [ObservableProperty] private double _shearModulus;        // G  (kN/m²)
    [ObservableProperty] private double _poissonRatio;        // ν
    [ObservableProperty] private double _density;             // ρ  (kg/m³)
    [ObservableProperty] private double _yieldStrength;       // fy (kN/m²)
    [ObservableProperty] private double _ultimateStrength;    // fu (kN/m²)
    [ObservableProperty] private double _thermalExpansion;    // α  (1/°C)

    // ── Standard material presets ─────────────────────────────────────────
    public static Material SteelIS2062_E250 => new()
    {
        Id = 1, Name = "Steel IS 2062 E250", Type = MaterialType.Steel,
        ElasticModulus = 2e8, ShearModulus = 7.69e7, PoissonRatio = 0.3,
        Density = 7850, YieldStrength = 2.5e5, UltimateStrength = 4.1e5, ThermalExpansion = 12e-6
    };
    public static Material SteelA36 => new()
    {
        Id = 2, Name = "Steel ASTM A36", Type = MaterialType.Steel,
        ElasticModulus = 2e8, ShearModulus = 7.69e7, PoissonRatio = 0.3,
        Density = 7850, YieldStrength = 2.5e5, UltimateStrength = 4.0e5, ThermalExpansion = 12e-6
    };
    public static Material ConcreteM25 => new()
    {
        Id = 3, Name = "Concrete M25", Type = MaterialType.Concrete,
        ElasticModulus = 25e6, ShearModulus = 10.4e6, PoissonRatio = 0.2,
        Density = 2500, YieldStrength = 25e3, UltimateStrength = 25e3, ThermalExpansion = 10e-6
    };
    public static Material ConcreteM30 => new()
    {
        Id = 4, Name = "Concrete M30", Type = MaterialType.Concrete,
        ElasticModulus = 27.4e6, ShearModulus = 11.4e6, PoissonRatio = 0.2,
        Density = 2500, YieldStrength = 30e3, UltimateStrength = 30e3, ThermalExpansion = 10e-6
    };
    public static IEnumerable<Material> DefaultLibrary => [SteelIS2062_E250, SteelA36, ConcreteM25, ConcreteM30];

    public override string ToString() => $"{Name} (E={ElasticModulus / 1e6:F0} GPa)";
}
