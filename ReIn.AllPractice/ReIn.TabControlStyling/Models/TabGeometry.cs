using System.Windows;
using System.Windows.Media;

namespace ReIn.TabControlStyling.Models
{
    public class TabGeometry : FrameworkElement
    {
        public TabGeometry()
        {

        }


        /// Using a DependencyProperty as the backing store for H.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HProperty =
            DependencyProperty.Register("H", typeof(double), typeof(TabGeometry), new PropertyMetadata(0.0, OnPropertyChanged));

        /// Using a DependencyProperty as the backing store for B.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register("B", typeof(double), typeof(TabGeometry), new PropertyMetadata(0.0, OnPropertyChanged));

        /// Using a DependencyProperty as the backing store for Db.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DbProperty =
            DependencyProperty.Register("Db", typeof(double), typeof(TabGeometry), new PropertyMetadata(0.0, OnPropertyChanged));

        /// Using a DependencyProperty as the backing store for Dh.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DhProperty =
            DependencyProperty.Register("Dh", typeof(double), typeof(TabGeometry), new PropertyMetadata(0.0, OnPropertyChanged));


        /// Using a DependencyProperty as the backing store for LeftPlacementOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyPropertyKey LeftPlacementOpenPropertyKey =
            DependencyProperty.RegisterReadOnly("LeftPlacementOpen", typeof(PathGeometry), typeof(TabGeometry), new PropertyMetadata(null));

        public static readonly DependencyProperty LeftPlacementOpenProperty = LeftPlacementOpenPropertyKey.DependencyProperty;


        /// Using a DependencyProperty as the backing store for TopPlacementOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyPropertyKey TopPlacementOpenPropertyKey =
            DependencyProperty.RegisterReadOnly("TopPlacementOpen", typeof(PathGeometry), typeof(TabGeometry), new PropertyMetadata(null));

        public static readonly DependencyProperty TopPlacementOpenProperty = TopPlacementOpenPropertyKey.DependencyProperty;


        /// Using a DependencyProperty as the backing store for RightPlacementOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyPropertyKey RightPlacementOpenPropertyKey =
            DependencyProperty.RegisterReadOnly("RightPlacementOpen", typeof(PathGeometry), typeof(TabGeometry), new PropertyMetadata(null));

        public static readonly DependencyProperty RightPlacementOpenProperty = RightPlacementOpenPropertyKey.DependencyProperty;


        /// Using a DependencyProperty as the backing store for BottomPlacementOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyPropertyKey BottomPlacementOpenPropertyKey =
            DependencyProperty.RegisterReadOnly("BottomPlacementOpen", typeof(PathGeometry), typeof(TabGeometry), new PropertyMetadata(null));

        public static readonly DependencyProperty BottomPlacementOpenProperty = BottomPlacementOpenPropertyKey.DependencyProperty;


        #region PropertyChanged Callback

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TabGeometry tabGeometry = d as TabGeometry;

            tabGeometry.SetValue(TopPlacementOpenPropertyKey, tabGeometry.GetTopPlacementOpenGeometry());
            tabGeometry.SetValue(LeftPlacementOpenPropertyKey, tabGeometry.GetLeftPlacementOpenGeometry());
            tabGeometry.SetValue(RightPlacementOpenPropertyKey, tabGeometry.GetRightPlacementOpenGeometry());
            tabGeometry.SetValue(BottomPlacementOpenPropertyKey, tabGeometry.GetBottomPlacementOpenGeometry());

        }

        #endregion

        #region Private Functions - Open Geometry

        private PathGeometry GetRightPlacementOpenGeometry()
        {
            PathGeometry leftPlacementOpen = GetLeftPlacementOpenGeometry();
            leftPlacementOpen.Transform = new ScaleTransform() { CenterX = B / 2, CenterY = H / 2, ScaleX = -1 };

            return leftPlacementOpen;
        }

        private PathGeometry GetBottomPlacementOpenGeometry()
        {
            PathGeometry topPlacementOpen = GetTopPlacementOpenGeometry();
            topPlacementOpen.Transform = new ScaleTransform() { CenterX = B / 2, CenterY = H / 2, ScaleY = -1 };

            return topPlacementOpen;
        }

        private PathGeometry GetTopPlacementOpenGeometry()
        {
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure()
            {
                StartPoint = new Point()
                {
                    X = 0,
                    Y = H
                },
                Segments = new PathSegmentCollection()
                {
                    CreateNewLineSegment(0, 0),
                    CreateNewLineSegment(B - Db, 0),
                    CreateNewLineSegment(B, Dh),
                    CreateNewLineSegment(B, H)
                }
            };

            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        private PathGeometry GetLeftPlacementOpenGeometry()
        {
            PathGeometry pathGeometry = new PathGeometry();

            PathFigure pathFigure = new PathFigure()
            {
                StartPoint = new Point()
                {
                    X = B,
                    Y = 0
                },
                Segments = new PathSegmentCollection()
                {
                    CreateNewLineSegment(Db, 0),
                    CreateNewLineSegment(0, Dh),
                    CreateNewLineSegment(0, H),
                    CreateNewLineSegment(B, H)
                }
            };

            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        #endregion

        #region Private Helpers

        private LineSegment CreateNewLineSegment(double x, double y)
        {
            return new LineSegment()
            {
                Point = new Point()
                {
                    X = x,
                    Y = y
                }
            };
        }

        #endregion

        #region CLR Properties - Read-only

        public PathGeometry LeftPlacementOpen => (PathGeometry)GetValue(LeftPlacementOpenProperty);

        public PathGeometry TopPlacementOpen => (PathGeometry)GetValue(TopPlacementOpenProperty);

        public PathGeometry RightPlacementOpen => (PathGeometry)GetValue(RightPlacementOpenProperty);

        public PathGeometry BottomPlacementOpen => (PathGeometry)GetValue(BottomPlacementOpenProperty);

        #endregion

        #region CLR Properties

        public double H
        {
            get => (double)GetValue(HProperty);
            set => SetValue(HProperty, value);
        }

        public double B
        {
            get => (double)GetValue(BProperty);
            set => SetValue(BProperty, value);
        }

        public double Db
        {
            get => (double)GetValue(DbProperty);
            set => SetValue(DbProperty, value);
        }

        public double Dh
        {
            get => (double)GetValue(DhProperty);
            set => SetValue(DhProperty, value);
        }

        #endregion

    }
}
