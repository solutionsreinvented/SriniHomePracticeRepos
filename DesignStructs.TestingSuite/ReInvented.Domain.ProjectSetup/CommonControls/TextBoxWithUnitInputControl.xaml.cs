using System.Windows;
using System.Windows.Controls;

namespace ReInvented.Domain.ProjectSetup.Controls
{
    /// <summary>
    /// Interaction logic for TextBoxWithUnitInputControl.xaml
    /// </summary>
    public partial class TextBoxWithUnitInputControl : UserControl
    {
        #region Constructor

        public TextBoxWithUnitInputControl()
        {
            InitializeComponent();
        } 

        #endregion

        #region CLR Wrappers

        public string Label { get => (string)GetValue(LabelProperty); set => SetValue(LabelProperty, value); }

        public object Value { get => (object)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

        public object Unit { get => (object)GetValue(UnitProperty); set => SetValue(UnitProperty, value); }

        public GridLength LabelColumnWidth { get => (GridLength)GetValue(LabelColumnWidthProperty); set => SetValue(LabelColumnWidthProperty, value); }

        public GridLength ValueColumnWidth { get => (GridLength)GetValue(ValueColumnWidthProperty); set => SetValue(ValueColumnWidthProperty, value); }

        public GridLength UnitColumnWidth { get => (GridLength)GetValue(UnitColumnWidthProperty); set => SetValue(UnitColumnWidthProperty, value); } 

        #endregion

        #region Dependency Properties

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(TextBoxWithUnitInputControl), new PropertyMetadata(string.Empty));

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(TextBoxWithUnitInputControl), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for LabelColumnWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelColumnWidthProperty =
            DependencyProperty.Register("LabelColumnWidth", typeof(GridLength), typeof(TextBoxWithUnitInputControl), new PropertyMetadata(GridLength.Auto));

        // Using a DependencyProperty as the backing store for ValueColumnWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueColumnWidthProperty =
            DependencyProperty.Register("ValueColumnWidth", typeof(GridLength), typeof(TextBoxWithUnitInputControl), new PropertyMetadata(GridLength.Auto));

        // Using a DependencyProperty as the backing store for UnitColumnWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnitColumnWidthProperty =
            DependencyProperty.Register("UnitColumnWidth", typeof(GridLength), typeof(TextBoxWithUnitInputControl), new PropertyMetadata(GridLength.Auto));

        // Using a DependencyProperty as the backing store for Unit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register("Unit", typeof(object), typeof(TextBoxWithUnitInputControl), new PropertyMetadata(null)); 

        #endregion

    }
}
