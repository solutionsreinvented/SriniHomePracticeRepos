using System.Windows;
using System.Windows.Controls;

namespace ReInvented.CustomControls.Modals
{
    public class Modal : ContentControl
    {
        static Modal()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Modal), new FrameworkPropertyMetadata(typeof(Modal)));
        }
    }
}
