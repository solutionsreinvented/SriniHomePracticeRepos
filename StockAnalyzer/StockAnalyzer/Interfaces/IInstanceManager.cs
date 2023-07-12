using System.Windows;

namespace StockAnalyzer.Interfaces
{
    public interface IInstanceManager
    {
        Application Current { get; }

        void Start();
    }
}