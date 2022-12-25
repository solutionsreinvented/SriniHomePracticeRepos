using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmDialogs.Interfaces
{
    public interface IDialogRequestClose
    {
        event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
    }
}
