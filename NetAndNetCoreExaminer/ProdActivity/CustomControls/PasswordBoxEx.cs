using System.Windows;
using System.Windows.Controls;

namespace ProdActivity.CustomControls
{
    public class PasswordBoxEx : Control
    {
        static PasswordBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PasswordBoxEx), new PropertyMetadata());
        }
    }
}
