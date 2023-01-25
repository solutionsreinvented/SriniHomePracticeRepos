
using TwistGate.ObjectConstruction.Enums;

namespace TwistGate.ObjectConstruction.Interfaces
{
    public interface ILoadCase
    {
        int Id { get; }

        string Title { get; }

        LoadCaseType Type { get; }

    }
}
