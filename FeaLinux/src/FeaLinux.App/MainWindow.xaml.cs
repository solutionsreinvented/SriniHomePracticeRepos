using System.Windows;
using System.Windows.Input;
using FeaLinux.App.ViewModels;

namespace FeaLinux.App;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm;

    public MainWindow(MainViewModel vm, ModelTreeViewModel modelTreeVM,
        PropertiesViewModel propertiesVM, ViewportViewModel viewportVM)
    {
        InitializeComponent();
        _vm = vm;

        // Sub-VMs exposed so XAML can bind {Binding ModelTreeVM} etc.
        vm.ModelTreeVM    = modelTreeVM;
        vm.PropertiesVM   = propertiesVM;
        vm.ViewportVM     = viewportVM;

        DataContext = vm;
        KeyDown += OnKeyDown;

        // Wire analysis-complete to refresh the viewport
        vm.SceneRefreshRequested += () => ViewportView?.RebuildScene();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        // Ignore shortcuts when user is typing in the input bar
        if (TbX.IsFocused || TbY.IsFocused || TbZ.IsFocused) return;
        switch (e.Key)
        {
            case Key.F5:     _vm.RunAnalysisCommand.Execute(null); break;
            case Key.S when Keyboard.Modifiers == ModifierKeys.None:
                _vm.SetModeSelectCommand.Execute(null); break;
            case Key.N when Keyboard.Modifiers == ModifierKeys.None:
                _vm.SetModeAddNodeCommand.Execute(null); break;
            case Key.M when Keyboard.Modifiers == ModifierKeys.None:
                _vm.SetModeAddMemberCommand.Execute(null); break;
            case Key.Escape:
                _vm.SetModeSelectCommand.Execute(null); break;
        }
    }

    private void MenuExit_Click(object sender, RoutedEventArgs e) => Close();

    private void CancelMode_Click(object sender, RoutedEventArgs e)
        => _vm.SetModeSelectCommand.Execute(null);

    private void InputBar_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            _vm.ConfirmAddNodeCommand.Execute(null);
        else if (e.Key == Key.Escape)
            _vm.SetModeSelectCommand.Execute(null);
    }

    // Camera preset delegates
    private void View_Perspective(object sender, RoutedEventArgs e) => ViewportView?.SetPerspectiveView();
    private void View_Top(object sender, RoutedEventArgs e)         => ViewportView?.SetTopView();
    private void View_Front(object sender, RoutedEventArgs e)       => ViewportView?.SetFrontView();
    private void View_Right(object sender, RoutedEventArgs e)       => ViewportView?.SetRightView();
    private void View_FitAll(object sender, RoutedEventArgs e)      => ViewportView?.FitAll();
}