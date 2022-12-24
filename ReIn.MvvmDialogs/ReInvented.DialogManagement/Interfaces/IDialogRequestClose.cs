using ReInvented.DialogManagement.EventsData;
using System;

namespace ReInvented.DialogManagement.Interfaces
{
    public interface IDialogRequestClose
    {
        event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
    }
}
