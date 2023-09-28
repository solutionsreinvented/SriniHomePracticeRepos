using System.Windows;
using System.Windows.Controls;

namespace ReInvented.Documentation.Reporting.Controls
{
    /// <summary>
    /// Interaction logic for HorizontalTextBoxInputControl.xaml
    /// </summary>
    public partial class HorizontalTextBoxInputControl : UserControl
    {
        public HorizontalTextBoxInputControl()
        {
            InitializeComponent();
        }

        #region CLR Wrappers

        public string Label { get => (string)GetValue(LabelProperty); set => SetValue(LabelProperty, value); }

        public object Value { get => GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

        public GridLength LabelColumnWidth { get => (GridLength)GetValue(LabelColumnWidthProperty); set => SetValue(LabelColumnWidthProperty, value); }

        public GridLength TextBoxColumnWidth { get => (GridLength)GetValue(TextBoxColumnWidthProperty); set => SetValue(TextBoxColumnWidthProperty, value); }

        #endregion

        #region Dependency Properties

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(HorizontalTextBoxInputControl), new PropertyMetadata(string.Empty));

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(HorizontalTextBoxInputControl), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for LabelColumnWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelColumnWidthProperty =
            DependencyProperty.Register("LabelColumnWidth", typeof(GridLength), typeof(HorizontalTextBoxInputControl), new PropertyMetadata(GridLength.Auto));

        // Using a DependencyProperty as the backing store for TextBoxColumnWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextBoxColumnWidthProperty =
            DependencyProperty.Register("TextBoxColumnWidth", typeof(GridLength), typeof(HorizontalTextBoxInputControl), new PropertyMetadata(GridLength.Auto));


        #endregion

    }
}
