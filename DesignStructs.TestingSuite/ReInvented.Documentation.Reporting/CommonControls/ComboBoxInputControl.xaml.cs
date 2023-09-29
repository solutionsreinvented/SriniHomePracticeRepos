using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace ReInvented.Documentation.Reporting.Controls
{
    /// <summary>
    /// Interaction logic for ComboBoxInputControl.xaml
    /// </summary>
    public partial class ComboBoxInputControl : UserControl
    {
        public ComboBoxInputControl()
        {
            InitializeComponent();
        }

        #region CLR Wrappers

        public string Label { get => (string)GetValue(LabelProperty); set => SetValue(LabelProperty, value); }

        public IEnumerable ItemsSource { get => (IEnumerable)GetValue(ItemsSourceProperty); set => SetValue(ItemsSourceProperty, value); }

        public object SelectedItem { get => GetValue(SelectedItemProperty); set => SetValue(SelectedItemProperty, value); }

        public GridLength LabelColumnWidth { get => (GridLength)GetValue(LabelColumnWidthProperty); set => SetValue(LabelColumnWidthProperty, value); }

        public GridLength ComboBoxColumnWidth { get => (GridLength)GetValue(ComboBoxColumnWidthProperty); set => SetValue(ComboBoxColumnWidthProperty, value); }

        #endregion

        #region Dependency Properties

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(ComboBoxInputControl), new PropertyMetadata(string.Empty));

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ComboBoxInputControl), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(ComboBoxInputControl), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for LabelColumnWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelColumnWidthProperty =
            DependencyProperty.Register("LabelColumnWidth", typeof(GridLength), typeof(ComboBoxInputControl), new PropertyMetadata(GridLength.Auto));

        // Using a DependencyProperty as the backing store for ComboBoxColumnWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ComboBoxColumnWidthProperty =
            DependencyProperty.Register("ComboBoxColumnWidth", typeof(GridLength), typeof(ComboBoxInputControl), new PropertyMetadata(GridLength.Auto));

        #endregion

    }
}
