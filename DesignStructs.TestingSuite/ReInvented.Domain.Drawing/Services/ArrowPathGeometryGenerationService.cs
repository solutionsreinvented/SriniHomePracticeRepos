using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace ReInvented.Domain.Drawing.Services
{
    public class ArrowPathGeometryGenerationService
    {
        public static PathGeometry Generate(double totalLength)
        {
            return Generate(totalLength, Defaults.ArrowThickness);
        }
        public static PathGeometry Generate(double totalLength, double thickness)
        {
            return Generate(totalLength, thickness, Defaults.ArrowWidthRatio, Defaults.ArrowHeightRatio);
        }

        public static PathGeometry Generate(double totalLength, double thickness, double arrowWidthRatio, double arrowHeightRatio)
        {
            return Generate(Defaults.CenterX, Defaults.CenterY, totalLength, thickness, arrowWidthRatio, arrowHeightRatio);
        }

        public static PathGeometry Generate(double startX, double startY, double totalLength, double thickness, double arrowWidthRatio, double arrowHeightRatio)
        {
            double arrowWidth = arrowWidthRatio * thickness;
            double arrowHeight = arrowHeightRatio * thickness;
            double straightLineLength = totalLength - arrowWidth;
            double halfThickness = thickness / 2.0;

            List<PathSegment> segments = new List<PathSegment>()
            {
                new LineSegment(new Point(startX, startY + halfThickness), true),
                new LineSegment(new Point(startX + straightLineLength, startY + halfThickness), true),
                new LineSegment(new Point(startX + straightLineLength, startY + arrowHeight / 2.0), true),
                new LineSegment(new Point(startX + straightLineLength + arrowWidth, startY), true),
                new LineSegment(new Point(startX + straightLineLength, startY + (-1.0) * arrowHeight / 2.0), true),
                new LineSegment(new Point(startX + straightLineLength, startY + (-1.0) * halfThickness), true),
                new LineSegment(new Point(startX, startY + (-1.0) * halfThickness), true)
            };

            PathFigure arrowFigure = new PathFigure(new Point(startX, startY), segments, true);

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(arrowFigure);

            return pathGeometry;
        }
    }
}
