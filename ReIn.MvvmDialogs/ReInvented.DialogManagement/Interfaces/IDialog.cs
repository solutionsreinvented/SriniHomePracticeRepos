using System.Windows;

namespace ReInvented.DialogManagement.Interfaces
{
    public interface IDialog
    {
        object DataContext { get; set; }

        bool? DialogResult { get; set; }

        Window Owner { get; set; }

        void Close();

        bool? ShowDialog();
    }
}
