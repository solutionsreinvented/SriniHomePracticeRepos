
using ReInvented.Domain.Drawing.Services;
using ReInvented.Shared;

using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ReInvented.Domain.Drawing
{
    public class DotGenerationService
    {
        public static Ellipse GenerateDotPositionedOnCircle(double circleRadius, double angle)
        {
            return GenerateDotPositionedOnCircle(Defaults.CenterX, Defaults.CenterY, circleRadius, angle);
        }

        public static Ellipse GenerateDotPositionedOnCircle(double centerX, double centerY, double circleRadius, double angle)
        {
            return GenerateDotPositionedOnCircle(centerX, centerY, circleRadius, Defaults.DotRadius, angle, Defaults.Stroke, Defaults.StrokeThickness);
        }

        public static Ellipse GenerateDotPositionedOnCircle(double centerX, double centerY, double circleRadius, double dotRadius, double angle, Brush stroke, double strokeThickness)
        {
            return GenerateDotPositionedOnCircle(centerX, centerY, circleRadius, dotRadius, angle, stroke, Defaults.Fill, strokeThickness);
        }

        public static Ellipse GenerateDotPositionedOnCircle(double centerX, double centerY, double circleRadius, double dotRadius, double angle, Brush stroke, Brush fill, double strokeThickness)
        {
            double dotCenterX = centerX + (circleRadius * Math.Cos(angle.Radians()));
            double dotCenterY = centerY - (circleRadius * Math.Sin(angle.Radians()));

            Ellipse ellipse = new Ellipse
            {
                Width = 2 * dotRadius,
                Height = 2 * dotRadius,
                Stroke = stroke,
                StrokeThickness = strokeThickness,
                Fill = fill
            };

            Canvas.SetLeft(ellipse, dotCenterX - (ellipse.Width / 2.0));
            Canvas.SetTop(ellipse, dotCenterY - (ellipse.Height / 2.0));

            return ellipse;
        }
    }
}
