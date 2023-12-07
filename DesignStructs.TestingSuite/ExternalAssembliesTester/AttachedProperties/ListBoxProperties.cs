using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ExternalAssembliesTester.AttachedProperties
{
    public static class ListBoxProperties
    {
        public static readonly DependencyProperty AutoScrollProperty = DependencyProperty.RegisterAttached("AutoScroll", typeof(bool), typeof(ListBoxProperties), new PropertyMetadata(false));

        public static readonly DependencyProperty AutoScrollHanderProperty = DependencyProperty.RegisterAttached("AutoScrollHander", typeof(AutoScrollHandler), typeof(ListBox));

        public static bool GetAutoScroll(ListBox instance) => (bool)instance.GetValue(AutoScrollProperty);

        public static void SetAutoScroll(ListBox instance, bool value)
        {
            if (instance.GetValue(AutoScrollHanderProperty) is AutoScrollHandler autoScrollHandler)
            {
                autoScrollHandler.Dispose();
                instance.SetValue(AutoScrollHanderProperty, null);
            }

            instance.SetValue(AutoScrollProperty, value);

            if (value)
            {
                instance.SetValue(AutoScrollHanderProperty, new AutoScrollHandler(instance));
            }
        }
    }

    public class AutoScrollHandler : DependencyObject, IDisposable
    {
        #region Parameterized Constructor

        public AutoScrollHandler(ListBox target)
        {
            Target = target;
            var binding = new Binding(nameof(ItemsSource)) { Source = Target };
            BindingOperations.SetBinding(this, ItemsSourceProperty, binding);
        } 

        #endregion

        #region Public Properties

        public IEnumerable ItemsSource { get => (IEnumerable)GetValue(ItemsSourceProperty); set => SetValue(ItemsSourceProperty, value); }

        #endregion

        #region Private Properties

        private ListBox Target { get; set; }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty ItemsSourceProperty = 
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(AutoScrollHandler), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnItemsSourcePropertyChanged)));

        #endregion

        #region Event Handlers

        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AutoScrollHandler)d).ItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
        }

        private void ItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (oldValue is INotifyCollectionChanged collectionChanged)
            {
                collectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(OnCollectionChanged);
            }

            collectionChanged = newValue as INotifyCollectionChanged;

            if (collectionChanged != null)
            {
                collectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(OnCollectionChanged);
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add || e.NewItems != null || e.NewItems.Count < 1)
            {
                return;
            }

            Target.ScrollIntoView(e.NewItems[e.NewItems.Count - 1]);
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            BindingOperations.ClearBinding(this, ItemsSourceProperty);
        }

        #endregion
    }
}
