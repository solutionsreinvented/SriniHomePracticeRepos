using System;
using System.Windows;
using System.Windows.Media;

using ReIn.TabPathGeometry.Enums;

using ReInvented.Shared.Stores;

namespace ReIn.TabPathGeometry.Models
{
    public class TabGeometryModel : PropertyStore
    {
        public TabGeometryModel()
        {

        }

        public TabGeometryKind Kind { get => Get<TabGeometryKind>(); set => Set(value); }

        public double H { get => Get<double>(); set { Set(value); RaiseNotifications(); } }

        public double B { get => Get<double>(); set => Set(value); }

        public double Dh { get => Get<double>(); set => Set(value); }

        public double Db { get => Get<double>(); set => Set(value); }

        public PathGeometry LeftPlacementOpen => GetLeftPlacementOpenGeometry();

        public PathGeometry TopPlacementOpen => GetTopPlacementOpenGeometry();

        public PathGeometry RightPlacementOpen => GetRightPlacementOpenGeometry();

        public PathGeometry BottomPlacementOpen => GetBottomPlacementOpenGeometry();

        private PathGeometry GetLeftPlacementOpenGeometry()
        {
            ///return GetPlacementOpenGeometry(B, 0, TabGeometryKind.LeftPlacementOpen);
            ///


            return GetTopPlacementOpenGeometry().Transform = new RotateTransform() { Angle = }
        }

        private PathGeometry GetTopPlacementOpenGeometry()
        {
            return GetPlacementOpenGeometry(0, H, TabGeometryKind.TopPlacementOpen);
        }

        private PathGeometry GetRightPlacementOpenGeometry()
        {
            return GetPlacementOpenGeometry(0, 0, TabGeometryKind.RightPlacementOpen);
        }

        private PathGeometry GetBottomPlacementOpenGeometry()
        {
            return GetPlacementOpenGeometry(0, 0, TabGeometryKind.BottomPlacementOpen);
        }


        #region Private Functions

        private PathGeometry GetPlacementOpenGeometry(double startPointX, double startPointY, TabGeometryKind tabGeometryKind)
        {
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure()
            {
                StartPoint = new Point()
                {
                    X = startPointX,
                    Y = startPointY
                },
                Segments = GetPathSegmentsFor(tabGeometryKind)
            };

            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        private PathSegmentCollection GetPathSegmentsFor(TabGeometryKind tabGeometryKind)
        {
            PathSegmentCollection segmentCollection = new PathSegmentCollection();

            switch (tabGeometryKind)
            {
                case TabGeometryKind.LeftPlacementOpen:
                    segmentCollection.Add(CreateNewLineSegment(Db, 0));
                    segmentCollection.Add(CreateNewLineSegment(0, Dh));
                    segmentCollection.Add(CreateNewLineSegment(0, H));
                    segmentCollection.Add(CreateNewLineSegment(B, H));
                    break;
                case TabGeometryKind.TopPlacementOpen:
                    segmentCollection.Add(CreateNewLineSegment(0, 0));
                    segmentCollection.Add(CreateNewLineSegment(B - Db, 0));
                    segmentCollection.Add(CreateNewLineSegment(B, Dh));
                    segmentCollection.Add(CreateNewLineSegment(B, H));
                    break;
                case TabGeometryKind.RightPlacementOpen:
                    segmentCollection.Add(CreateNewLineSegment(B - Db, 0));
                    segmentCollection.Add(CreateNewLineSegment(B, Dh));
                    segmentCollection.Add(CreateNewLineSegment(B, H));
                    segmentCollection.Add(CreateNewLineSegment(0, H));
                    break;
                case TabGeometryKind.BottomPlacementOpen:
                    segmentCollection.Add(CreateNewLineSegment(0, H));
                    segmentCollection.Add(CreateNewLineSegment(B - Db, H));
                    segmentCollection.Add(CreateNewLineSegment(B, H - Dh));
                    segmentCollection.Add(CreateNewLineSegment(B, 0));
                    break;
                case TabGeometryKind.LeftPlacementClosed:
                    break;
                case TabGeometryKind.TopPlacementClosed:
                    break;
                case TabGeometryKind.RightPlacementClosed:
                    break;
                case TabGeometryKind.BottomPlacementClosed:
                    break;
                default:
                    break;
            }

            return segmentCollection;

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

        #endregion

        #region Private Helpers

        private void RaiseNotifications()
        {
            RaiseMultiplePropertiesChanged(nameof(LeftPlacementOpen), nameof(TopPlacementOpen), nameof(RightPlacementOpen), nameof(BottomPlacementOpen));
        }

        #endregion

    }
}
