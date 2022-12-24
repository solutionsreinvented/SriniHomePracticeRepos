using ReIn.MvvmDialogs.DialogWindows;

namespace ReIn.MvvmDialogs
{
    public interface IDialogService
    {
        void ShowDialog();
    }
    public class DialogService : IDialogService
    {
        public void ShowDialog()
        {
            var dialog = new DialogWindow();
            dialog.ShowDialog();
        }
    }
}
