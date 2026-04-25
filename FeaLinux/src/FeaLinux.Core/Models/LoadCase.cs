using CommunityToolkit.Mvvm.ComponentModel;
using FeaLinux.Core.Enums;

namespace FeaLinux.Core.Models;

public partial class LoadCase : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private LoadCaseType _type;
    [ObservableProperty] private double _selfWeightFactor;
    [ObservableProperty] private bool _isActive = true;

    public List<Load> Loads { get; init; } = [];

    public LoadCase() { }
    public LoadCase(int id, string name, LoadCaseType type, double selfWeightFactor = 0)
    {
        _id = id; _name = name; _type = type; _selfWeightFactor = selfWeightFactor;
    }

    public static LoadCase DeadLoad(int id) => new(id, "Dead Load", LoadCaseType.Dead, 1.0);
    public static LoadCase LiveLoad(int id) => new(id, "Live Load", LoadCaseType.Live, 0);
    public static LoadCase WindX(int id) => new(id, "Wind X", LoadCaseType.WindX, 0);
    public static LoadCase WindY(int id) => new(id, "Wind Y", LoadCaseType.WindY, 0);
    public static LoadCase SeismicX(int id) => new(id, "Seismic X", LoadCaseType.SeismicX, 0);
    public static LoadCase SeismicY(int id) => new(id, "Seismic Y", LoadCaseType.SeismicY, 0);

    public override string ToString() => $"LC{Id}: {Name}";
}

public partial class LoadCombination : ObservableObject
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private LoadCombinationType _type;
    [ObservableProperty] private LoadCombinationMethod _method;

    public Dictionary<LoadCase, double> Factors { get; init; } = [];

    public LoadCombination() { }
    public LoadCombination(int id, string name, LoadCombinationType type)
    {
        _id = id; _name = name; _type = type;
    }

    public void AddFactor(LoadCase lc, double factor) => Factors[lc] = factor;

    public override string ToString() => $"COMB{Id}: {Name}";

    // ── IS 875 standard combinations ──────────────────────────────────────
    public static List<LoadCombination> GenerateIS875Combinations(LoadCase? dl, LoadCase? ll,
        LoadCase? windX = null, LoadCase? windY = null, LoadCase? seismicX = null, LoadCase? seismicY = null)
    {
        int id = 1;
        var combos = new List<LoadCombination>();

        void Add(string name, LoadCombinationType type, params (LoadCase? lc, double f)[] pairs)
        {
            var combo = new LoadCombination(id++, name, type);
            foreach (var (lc, f) in pairs)
                if (lc != null) combo.Factors[lc] = f;
            combos.Add(combo);
        }

        if (dl != null && ll != null)
        {
            Add("1.5(DL+LL)", LoadCombinationType.Ultimate, (dl, 1.5), (ll, 1.5));
            Add("DL+LL", LoadCombinationType.Serviceability, (dl, 1.0), (ll, 1.0));
        }
        if (dl != null && windX != null)
            Add("1.5(DL+WLx)", LoadCombinationType.Ultimate, (dl, 1.5), (windX, 1.5));
        if (dl != null && ll != null && windX != null)
            Add("1.2(DL+LL+WLx)", LoadCombinationType.Ultimate, (dl, 1.2), (ll, 1.2), (windX, 1.2));
        if (dl != null && seismicX != null)
            Add("1.5(DL+EQx)", LoadCombinationType.Seismic, (dl, 1.5), (seismicX, 1.5));
        if (dl != null && ll != null && seismicX != null)
            Add("1.2(DL+LL+EQx)", LoadCombinationType.Seismic, (dl, 1.2), (ll, 1.2), (seismicX, 1.2));

        return combos;
    }
}
