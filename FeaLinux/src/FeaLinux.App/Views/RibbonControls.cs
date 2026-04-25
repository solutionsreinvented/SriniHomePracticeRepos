using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using FeaLinux.Core.Enums;

namespace FeaLinux.App.Views;

// ── Mode → Bool converter for ribbon active state ──────────────────────
public class ModeConverter : IValueConverter
{
    public static readonly ModeConverter Instance = new();
    public object Convert(object v, Type t, object p, CultureInfo c)
        => v is Core.Enums.SelectionMode m && p is string s && m.ToString() == s;
    public object ConvertBack(object v, Type t, object p, CultureInfo c)
        => Enum.TryParse<Core.Enums.SelectionMode>(p?.ToString(), out var m) ? m : Core.Enums.SelectionMode.Select;
}

// ── Bool → Visibility ─────────────────────────────────────────────────
public class BoolToVisConverter : IValueConverter
{
    public static readonly BoolToVisConverter Instance = new();
    public object Convert(object v, Type t, object p, CultureInfo c)
        => v is true ? Visibility.Visible : Visibility.Collapsed;
    public object ConvertBack(object v, Type t, object p, CultureInfo c) => DependencyProperty.UnsetValue;
}

// ── Ribbon Group Control ───────────────────────────────────────────────
public class RibbonGroup : HeaderedItemsControl
{
    static RibbonGroup()
        => DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonGroup),
            new FrameworkPropertyMetadata(typeof(RibbonGroup)));

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        // Stack children horizontally
        if (ItemsPanel == null)
        {
            var factory = new FrameworkElementFactory(typeof(StackPanel));
            factory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            ItemsPanel = new ItemsPanelTemplate(factory);
        }
    }
}

// ── Ribbon simple button (emoji/text icon) ────────────────────────────
public class RibbonButton : Button
{
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(string), typeof(RibbonButton), new PropertyMetadata(""));
    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label), typeof(string), typeof(RibbonButton), new PropertyMetadata(""));
    public static readonly DependencyProperty ExtraClickProperty =
        DependencyProperty.Register("ExtraClick", typeof(RoutedEventHandler), typeof(RibbonButton));

    public string Icon { get => (string)GetValue(IconProperty); set => SetValue(IconProperty, value); }
    public string Label { get => (string)GetValue(LabelProperty); set => SetValue(LabelProperty, value); }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        Style = (Style)Application.Current.FindResource("ToolbarButtonStyle");
        Content = BuildContent();
    }

    private UIElement BuildContent() => new StackPanel
    {
        Orientation = Orientation.Vertical,
        HorizontalAlignment = HorizontalAlignment.Center,
        Children =
        {
            new TextBlock { Text = Icon, FontSize = 18, HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(0, 0, 0, 2) },
            new TextBlock { Text = Label, FontSize = 10, HorizontalAlignment = HorizontalAlignment.Center,
                            Foreground = (Brush)Application.Current.FindResource("TextSecondaryBrush") }
        }
    };
}

// ── Ribbon icon button (DrawingImage icon) ───────────────────────────
public class RibbonIconButton : Button
{
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(RibbonIconButton));
    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label), typeof(string), typeof(RibbonIconButton), new PropertyMetadata(""));
    public static readonly DependencyProperty IsActiveProperty =
        DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(RibbonIconButton),
            new PropertyMetadata(false, OnIsActiveChanged));

    public ImageSource Icon { get => (ImageSource)GetValue(IconProperty); set => SetValue(IconProperty, value); }
    public string Label { get => (string)GetValue(LabelProperty); set => SetValue(LabelProperty, value); }
    public bool IsActive { get => (bool)GetValue(IsActiveProperty); set => SetValue(IsActiveProperty, value); }

    private static void OnIsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is RibbonIconButton btn)
        {
            btn.Style = (bool)e.NewValue
                ? (Style)Application.Current.FindResource("ActiveToolbarButtonStyle")
                : (Style)Application.Current.FindResource("ToolbarButtonStyle");
        }
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        Style = (Style)Application.Current.FindResource("ToolbarButtonStyle");
        Loaded += (_, _) => Content = BuildContent();
    }

    private UIElement BuildContent() => new StackPanel
    {
        Orientation = Orientation.Vertical,
        HorizontalAlignment = HorizontalAlignment.Center,
        Children =
        {
            new Image { Source = Icon, Width = 22, Height = 22,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(0, 0, 0, 2) },
            new TextBlock { Text = Label, FontSize = 10, HorizontalAlignment = HorizontalAlignment.Center,
                            Foreground = (Brush)Application.Current.FindResource("TextSecondaryBrush") }
        }
    };
}

// ── Ribbon toggle button ──────────────────────────────────────────────
public class RibbonToggle : ToggleButton
{
    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label), typeof(string), typeof(RibbonToggle), new PropertyMetadata(""));
    public string Label { get => (string)GetValue(LabelProperty); set => SetValue(LabelProperty, value); }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        Style = (Style)Application.Current.FindResource("ToggleToolStyle");
        Loaded += (_, _) => Content = new StackPanel
        {
            Orientation = Orientation.Vertical,
            HorizontalAlignment = HorizontalAlignment.Center,
            Children =
            {
                new TextBlock { Text = "✓", FontSize = 14, HorizontalAlignment = HorizontalAlignment.Center },
                new TextBlock { Text = Label, FontSize = 10, HorizontalAlignment = HorizontalAlignment.Center }
            }
        };
    }
}
