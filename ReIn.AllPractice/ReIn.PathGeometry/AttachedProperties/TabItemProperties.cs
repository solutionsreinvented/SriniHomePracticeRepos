using System;
using System.Windows;

using ReIn.TabPathGeometry.Models;

namespace ReIn.TabPathGeometry.AttachedProperties
{
    public class TabItemProperties
    {

        #region Tab Geometry Property Definition

        public static TabGeometry GetTabGeometry(DependencyObject obj)
        {
            return (TabGeometry)obj.GetValue(TabGeometryProperty);
        }

        public static void SetTabGeometry(DependencyObject obj, TabGeometry value)
        {
            obj.SetValue(TabGeometryProperty, value);
        }

        /// Using a DependencyProperty as the backing store for TabGeometry.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TabGeometryProperty =
            DependencyProperty.RegisterAttached("TabGeometry", typeof(TabGeometry), typeof(TabItemProperties), new FrameworkPropertyMetadata(null));

        //private static object OnTabGeomettryChanged(DependencyObject d, object baseValue)
        //{
        //    TabGeometry tabGeometry = d as TabGeometry;

        //    return null;
        //}

        #endregion


    }
}
