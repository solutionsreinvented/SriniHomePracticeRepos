using System.Windows;
using System.Windows.Media.Animation;
using System;
using System.Windows.Controls;

namespace Practice.Animations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ///GridLengthAnimation();
            InitializeComponent();
            //TextBlock
        }

        public void GridLengthAnimation()
        {
            Storyboard storyboard = new Storyboard();

            var animation = new GridLengthAnimation()
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 500)),
                From = navigationGridColumn.Width,
                //To = new GridLength(IsGridColumnVisible ? GridColumnPreviousHeight : 0, GridUnitType.Pixel),
            };

            Storyboard.SetTarget(animation, this.navigationGridColumn);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Width"));

            storyboard.Children.Add(animation);
            storyboard.Begin();
        }
    }
}
