using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmDialogs.Interfaces
{
    public class DialogCloseRequestedEventArgs : EventArgs
    {
        public DialogCloseRequestedEventArgs(bool? dialogResult)
        {
            DialogResult = dialogResult;
        }

        public bool? DialogResult { get; private set; }
    }
}
