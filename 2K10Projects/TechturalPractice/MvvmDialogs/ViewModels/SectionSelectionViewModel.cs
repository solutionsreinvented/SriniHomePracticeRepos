using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmDialogs.Interfaces;

namespace MvvmDialogs.ViewModels
{
    public class SectionSelectionViewModel : IDialogRequestClose
    {

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        public SectionSelectionViewModel()
        {
            CloseRequested += OnCloseRequested;
        }

        void OnCloseRequested(object sender, DialogCloseRequestedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
