using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmDialogs.Interfaces
{
    public interface IDialogService
    {
        void Register<TViewModel, TView>() where TViewModel : IDialogRequestClose 
                                           where TView : IDialog;

        bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose;
    }
}
