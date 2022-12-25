using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MvvmDialogs.Interfaces
{
    public interface IDialog
    {
        object DataContext { get; set; }
        Window Owner { get; set; }
        bool? DialogResult { get; set; }

        void Close();
        bool? ShowDialog();
    }
}
