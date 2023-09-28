using System.Windows;
using System.Windows.Controls;

using ReInvented.Documentation.Reporting.Models;

namespace ReInvented.Documentation.Reporting.Controls
{
    /// <summary>
    /// Interaction logic for ScrutinizerControl.xaml
    /// </summary>
    public partial class ScrutinizerControl : UserControl
    {
        public ScrutinizerControl()
        {
            InitializeComponent();
        }

        #region CLR Wrappers

        public object Header { get => GetValue(HeaderProperty); set => SetValue(HeaderProperty, value); }

        public Scrutinizer Scrutinizer { get => (Scrutinizer)GetValue(ScrutinizerProperty); set => SetValue(ScrutinizerProperty, value); }

        #endregion

        #region Dependency Properties

        // Using a DependencyProperty as the backing store for Scrutinizer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrutinizerProperty =
            DependencyProperty.Register("Scrutinizer", typeof(Scrutinizer), typeof(ScrutinizerControl), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(ScrutinizerControl), new PropertyMetadata(null));

        #endregion

    }
}
