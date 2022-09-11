using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pragmatic.IoC.Autofac
{
    public class DerivedButton : Button
    {
        static DerivedButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DerivedButton), new FrameworkPropertyMetadata(typeof(DerivedButton)));
        }



        public RoutedEvent ContentChangedEvent
        {
            get => (RoutedEvent)GetValue(ContentChangedEventProperty);
            set => SetValue(ContentChangedEventProperty, value);
        }

        // Using a DependencyProperty as the backing store for ContentChangedEvent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentChangedEventProperty =
            DependencyProperty.Register("ContentChangedEvent", typeof(RoutedEvent), typeof(DerivedButton), new PropertyMetadata(null));




    }
}
