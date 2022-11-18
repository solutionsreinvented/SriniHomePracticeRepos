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

        /// Completed
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
                    new LineSegment()
                    {
                        Point = new Point()
                        {
                            X = Db, Y = 0
                        }
                    },
                    new LineSegment()
                    {
                        Point = new Point()
                        {
                            X = 0, Y = Dh
                        }
                    },
                    new LineSegment()
                    {
                        Point = new Point()
                        {
                            X = 0, Y = H
                        }
                    },
                    new LineSegment()
                    {
                        Point = new Point()
                        {
                            X = B, Y = H
                        }
                    }
                }
            };

            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        /// Completed
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
                    new LineSegment()
                    {
                        Point = new Point()
                        {
                            X = 0, Y = 0
                        }
                    },
                    new LineSegment()
                    {
                        Point = new Point()
                        {
                            X = B-Db, Y = 0
                        }
                    },
                    new LineSegment()
                    {
                        Point = new Point()
                        {
                            X = B, Y = Dh
                        }
                    },
                    new LineSegment()
                    {
                        Point = new Point()
                        {
                            X = B, Y = H
                        }
                    }
                }
            };

            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        private PathGeometry GetRightPlacementOpenGeometry()
        {
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure()
            {
                StartPoint = new Point() { X = 0, Y = 0 },
                Segments = new PathSegmentCollection()
                {
                    new LineSegment()
                    {
                        Point = new Point(){X = 0, Y = H }
                    },
                    new LineSegment(){Point = new Point(){X = B-Db, Y = H } },
                    new LineSegment(){Point = new Point(){X = B, Y = H-Dh } },
                    new LineSegment(){Point = new Point(){X = B, Y = 0 } }
                }
            };

            pathGeometry.Figures.Add(pathFigure);

            return pathGeometry;
        }

        /// Completed
        private PathGeometry GetBottomPlacementOpenGeometry()
        {
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure()
            {
                StartPoint = new Point() { X = 0, Y = 0 },
                Segments = new PathSegmentCollection()
                {
                    new LineSegment(){Point = new Point(){X = 0, Y = H } },
                    new LineSegment(){Point = new Point(){X = B-Db, Y = H } },
                    new LineSegment(){Point = new Point(){X = B, Y = H-Dh } },
                    new LineSegment(){Point = new Point(){X = B, Y = 0 } }
                }
            };

            pathGeometry.Figures.Add(pathFigure);

            ///pathGeometry.Transform = new RotateTransform() { Angle = 45, CenterX = B / 2, CenterY = H / 2 };

            return pathGeometry;
        }


        private void RaiseNotifications()
        {
            RaiseMultiplePropertiesChanged(nameof(TopPlacementOpen));
        }

    }
}
