using System;
using System.Collections.Generic;
using System.Text;

namespace AllCorePracticeApp.ViewModels
{
    internal class MainViewModel
    {
        public BaseViewModel CurrentViewModel { get; set; } = new SNiPViewModel();

    }
}
