using System.Linq;
using System.Windows;
using System.Windows.Media;

using ReInvented.Shared.Stores;

namespace ReIn.TabControlStyling.Models
{
    /// <summary>
    /// Provides geometry for TabItem in a TabControl according to the placement of the TabStrip.
    /// </summary>
    public class TabGeometry : PropertyStore
    {
        #region Default Constructor

        public TabGeometry()
        {

        }

        #endregion

        #region Public Properties

        public double H { get => Get<double>(); set { Set(value); RaiseNotifications(); } }

        public double B { get => Get<double>(); set { Set(value); RaiseNotifications(); } }

        public double Dh { get => Get<double>(); set { Set(value); RaiseNotifications(); } }

        public double Db { get => Get<double>(); set { Set(value); RaiseNotifications(); } }

        public double StrokeThickness { get => Get<double>(); set { Set(value); RaiseNotifications(); } }

        #endregion

        #region Read-only Properties - Open Geometry

        public PathGeometry LeftPlacementOpen => GetLeftPlacementOpenGeometry();

        public PathGeometry TopPlacementOpen => GetTopPlacementOpenGeometry();

        public PathGeometry RightPlacementOpen => GetRightPlacementOpenGeometry();

        public PathGeometry BottomPlacementOpen => GetBottomPlacementOpenGeometry();

        #endregion

        #region Read-only Properties - Closed Geometry

        public PathGeometry LeftPlacementClosed => GetClosedGeometry(GetLeftPlacementOpenGeometry());

        public PathGeometry TopPlacementClosed => GetClosedGeometry(GetTopPlacementOpenGeometry());

        public PathGeometry RightPlacementClosed => GetClosedGeometry(GetRightPlacementOpenGeometry());

        public PathGeometry BottomPlacementClosed => GetClosedGeometry(GetBottomPlacementOpenGeometry());

        #endregion

        #region Private Functions

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

        #endregion

        #region Private Helpers

        private PathGeometry GetClosedGeometry(PathGeometry pathGeometry)
        {
            pathGeometry.Figures.FirstOrDefault().IsClosed = true;

            return pathGeometry;
        }

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

        private void RaiseNotifications()
        {
            RaiseMultiplePropertiesChanged(nameof(LeftPlacementOpen), nameof(TopPlacementOpen), nameof(RightPlacementOpen), nameof(BottomPlacementOpen));
            RaiseMultiplePropertiesChanged(nameof(LeftPlacementClosed), nameof(TopPlacementClosed), nameof(RightPlacementClosed), nameof(BottomPlacementClosed));
        }

        #endregion
    }
}
