using System.Linq;
using System.Windows;
using System.Windows.Media;

using ReInvented.Shared.Stores;

namespace ReIn.TabPathGeometry.Models
{
    public class TabGeometryModel : PropertyStore
    {
        #region Default Constructor

        public TabGeometryModel()
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

        public PathGeometry LeftPlacementClosed => GetLeftPlacementClosedGeometry();

        public PathGeometry TopPlacementClosed => GetTopPlacementClosedGeometry();

        public PathGeometry RightPlacementClosed => GetRightPlacementClosedGeometry();

        public PathGeometry BottomPlacementClosed => GetBottomPlacementClosedGeometry();

        #endregion

        #region Private Functions - Open Geometry

        private PathGeometry GetLeftPlacementOpenGeometry()
        {
            PathGeometry pathGeometry = new PathGeometry();

            PathFigure pathFigure = new PathFigure()
            {
                StartPoint = new Point()
                {
                    X = B - (StrokeThickness / 2),
                    Y = 0 + (StrokeThickness / 2)
                },
                Segments = new PathSegmentCollection()
                {
                    CreateNewLineSegment(Db + (StrokeThickness / 2), 0 + (StrokeThickness / 2)),
                    CreateNewLineSegment(0 + (StrokeThickness / 2), Dh + (StrokeThickness / 2)),
                    CreateNewLineSegment(0 + (StrokeThickness / 2), H - (StrokeThickness / 2)),
                    CreateNewLineSegment(B - (StrokeThickness / 2), H - (StrokeThickness / 2))
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
                    X = 0 + (StrokeThickness / 2),
                    Y = H - (StrokeThickness / 2)
                },
                Segments = new PathSegmentCollection()
                {
                    CreateNewLineSegment(0 + (StrokeThickness / 2), 0 + (StrokeThickness / 2)),
                    CreateNewLineSegment(B - Db - (StrokeThickness / 2), 0 + (StrokeThickness / 2)),
                    CreateNewLineSegment(B - (StrokeThickness / 2), Dh + (StrokeThickness / 2)),
                    CreateNewLineSegment(B - (StrokeThickness / 2), H - StrokeThickness / 2)
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

        #region Private Functions - Closed Geometry

        private PathGeometry GetLeftPlacementClosedGeometry()
        {
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure()
            {
                IsClosed = true,
                StartPoint = new Point()
                {
                    X = B - StrokeThickness,
                    Y = 0 + (StrokeThickness / 2)
                },
                Segments = new PathSegmentCollection()
                {
                    CreateNewLineSegment(Db + (StrokeThickness / 2), 0 + (StrokeThickness / 2)),
                    CreateNewLineSegment(0 + (StrokeThickness / 2), Dh + (StrokeThickness / 2)),
                    CreateNewLineSegment(0 + (StrokeThickness / 2), H - (StrokeThickness / 2)),
                    CreateNewLineSegment(B - StrokeThickness, H - (StrokeThickness / 2))
                }
            };

            pathGeometry.Figures.Add(pathFigure);
            return pathGeometry;
        }

        private PathGeometry GetTopPlacementClosedGeometry()
        {
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure()
            {
                IsClosed = true,
                StartPoint = new Point()
                {
                    X = 0 + (StrokeThickness / 2),
                    Y = H - StrokeThickness
                },
                Segments = new PathSegmentCollection()
                {
                    CreateNewLineSegment(0 + (StrokeThickness / 2), 0 + (StrokeThickness / 2)),
                    CreateNewLineSegment(B - Db - (StrokeThickness / 2), 0 + (StrokeThickness / 2)),
                    CreateNewLineSegment(B - (StrokeThickness / 2), Dh + (StrokeThickness / 2)),
                    CreateNewLineSegment(B - (StrokeThickness / 2), H - StrokeThickness)
                }
            };

            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        private PathGeometry GetRightPlacementClosedGeometry()
        {
            PathGeometry leftPlacementClosed = GetLeftPlacementClosedGeometry();
            leftPlacementClosed.Transform = new ScaleTransform() { CenterX = B / 2, CenterY = H / 2, ScaleX = -1 };

            return leftPlacementClosed;
        }

        private PathGeometry GetBottomPlacementClosedGeometry()
        {
            PathGeometry topPlacementClosed = GetTopPlacementClosedGeometry();
            topPlacementClosed.Transform = new ScaleTransform() { CenterX = B / 2, CenterY = H / 2, ScaleY = -1 };

            return topPlacementClosed;
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

        private void RaiseNotifications()
        {
            RaiseMultiplePropertiesChanged(nameof(LeftPlacementOpen), nameof(TopPlacementOpen), nameof(RightPlacementOpen), nameof(BottomPlacementOpen));
            RaiseMultiplePropertiesChanged(nameof(LeftPlacementClosed), nameof(TopPlacementClosed), nameof(RightPlacementClosed), nameof(BottomPlacementClosed));
        }

        #endregion

    }
}
