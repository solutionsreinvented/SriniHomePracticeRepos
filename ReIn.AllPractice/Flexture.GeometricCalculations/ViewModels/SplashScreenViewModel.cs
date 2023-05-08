using System;
using System.Timers;
using System.Windows.Threading;


namespace Flexture.GeometricCalculations.ViewModels
{
    public class SplashScreenViewModel
    {
        public SplashScreenViewModel(Timer timer)
        {
            Timer = timer;
        }

        public Timer Timer { get; set; }

        public string RemainingTime => $"{Timer.Interval/1000}s Remaining";
    }
}
