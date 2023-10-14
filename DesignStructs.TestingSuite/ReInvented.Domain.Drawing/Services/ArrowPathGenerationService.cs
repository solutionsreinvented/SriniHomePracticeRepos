using System.Windows.Media;
using System.Windows.Shapes;

namespace ReInvented.Domain.Drawing.Services
{
    public class ArrowPathGenerationService
    {
        public static Path Generate(PathGeometry geometry)
        {
            return Generate(geometry, Defaults.StrokeThickness);
        }
        public static Path Generate(PathGeometry geometry, double strokeThickness)
        {
            return Generate(geometry, strokeThickness, Defaults.Stroke, Defaults.Fill);
        }

        public static Path Generate(PathGeometry geometry, double strokeThickness, Brush stroke, Brush fill)
        {
            return Generate(geometry, strokeThickness, stroke, fill, Defaults.RotationAngle);
        }
        public static Path Generate(PathGeometry geometry, double strokeThickness, Brush stroke, Brush fill, double rotateDegrees)
        {
            return Generate(geometry, strokeThickness, stroke, fill, rotateDegrees, Defaults.CenterX, Defaults.CenterY);
        }
        public static Path Generate(PathGeometry geometry, double strokeThickness, Brush stroke, Brush fill, double rotateDegrees, double centerX, double centerY)
        {
            Path path = new Path { Data = geometry, StrokeThickness = strokeThickness, Stroke = stroke, Fill = fill };
            path.RenderTransform = new TransformGroup() { Children = { new RotateTransform(rotateDegrees, centerX, centerY) } };

            return path;
        }
    }
}
