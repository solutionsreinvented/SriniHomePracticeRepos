using ReInvented.Domain.Drawing.Services;
using ReInvented.Shared;

using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ReInvented.Domain.Drawing
{
    #region Primary Elements Generator
    public class ChartPrimaryElementsGenerationService
    {
        public static void Generate(Canvas plotCanvas, double centerX, double centerY, double diameter)
        {
            GenerateOrigin(plotCanvas, centerX, centerY);
            GenerateCoordinateAxes(plotCanvas, 25.0, 25.0, 80.0, 5);
            GenerateRadiusLine(plotCanvas, centerX, centerY, diameter);
        }

        public static void GenerateOrigin(Canvas plotCanvas, double centerX, double centerY)
        {
            Ellipse origin = DotGenerationService.GenerateDotPositionedOnCircle(centerX, centerY, 0, 2, 0, Brushes.Gray, Brushes.Gray, 1);
            _ = plotCanvas.Children.Add(origin);
        }

        public static void GenerateCoordinateAxes(Canvas plotCanvas, double left, double top, double length, double labelsMargin)
        {
            Path xAxis = ArrowPathGenerationService.Generate(ArrowPathGeometryGenerationService.Generate(left, top, length, 0.6, 5, 5), 0.8, Brushes.Gray, Brushes.Gray);
            Path zAxis = ArrowPathGenerationService.Generate(ArrowPathGeometryGenerationService.Generate(left, top, length, 0.6, 5, 5), 0.8, Brushes.Gray, Brushes.Gray, 90.0, left, top);

            TextBlock yAxisText = new TextBlock() { Text = "Y+ve Upwards", FontFamily = new FontFamily("Adobe Clean UX"), FontSize = 10 };///, Margin = new Thickness(left + 10, top + 10, 0, 0) };
            yAxisText.RenderTransform = new TransformGroup() { Children = { new RotateTransform(45.0, left, top) } };

            TextBlock xAxisLabel = new TextBlock() { Text = "X+ve", FontFamily = new FontFamily("Adobe Clean UX"), FontSize = 10 };
            TextBlock zAxisLabel = new TextBlock() { Text = "Z+ve", FontFamily = new FontFamily("Adobe Clean UX"), FontSize = 10 };


            _ = plotCanvas.Children.Add(xAxis);
            _ = plotCanvas.Children.Add(zAxis);
            _ = plotCanvas.Children.Add(xAxisLabel);
            _ = plotCanvas.Children.Add(zAxisLabel);
            _ = plotCanvas.Children.Add(yAxisText);

            Canvas.SetLeft(yAxisText, left);
            Canvas.SetTop(yAxisText, top + 30);

            Canvas.SetLeft(xAxisLabel, left + length + labelsMargin);
            Canvas.SetTop(xAxisLabel, top);

            Canvas.SetLeft(zAxisLabel, left);
            Canvas.SetTop(zAxisLabel, top + length + labelsMargin);

        }
        public static void GenerateRadiusLine(Canvas plotCanvas, double centerX, double centerY, double diameter)
        {
            Path radiusLine = ArrowPathGenerationService.Generate(ArrowPathGeometryGenerationService.Generate(centerX, centerY, diameter / 2 - 5, 0.2, 16, 16), 1, Brushes.Gray, Brushes.Gray, -35, centerX, centerY);
            _ = plotCanvas.Children.Add(radiusLine);
        }
    }
    #endregion

    public class DrawingToImageConversionService
    {
        public static void PrintToPNG(Canvas canvas)
        {
            try
            {
                // Render the Canvas to a RenderTargetBitmap
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)canvas.Width, (int)canvas.Height, 96, 96, PixelFormats.Pbgra32);
                renderTargetBitmap.Render(canvas);

                // Create a PNG encoder
                PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

                // Save the PNG image to a file
                string fileName = @"D:\02. Due\01. Everything Else\02. Resumé\canvas_image.png";
                using (System.IO.FileStream stream = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
                {
                    pngEncoder.Save(stream);
                }

                MessageBox.Show($"Canvas content saved as {fileName}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the image: {ex.Message}");
            }
        }
    }

    public class LabelGenerationService
    {
        public static (UIElement Rectangle, UIElement LabelText) GenerateLabelPositionedOnCircle(double centerX, double centerY, double circleRadius, double dotRadius, double angle, double labelGap, string label, Brush foreground, double rotateDegrees)
        {
            double labelPivotX = centerX + ((circleRadius + dotRadius + labelGap) * Math.Cos(angle.Radians()));
            double labelPivotY = centerY + ((circleRadius + dotRadius + labelGap) * Math.Sin(angle.Radians()));

            Rectangle rectangle = new Rectangle() { Width = 20, Height = 10, Fill = Brushes.Red, Stroke = Brushes.Gray, StrokeThickness = 0.6 };

            TextBlock labelText = new TextBlock()
            {
                Text = label,
                Foreground = foreground,
                Margin = new Thickness(labelGap, 0, 0, 0),
                FontFamily = new FontFamily("Adobe Clean UX"),
            };
            RotateTransform rotateTransform = new RotateTransform(rotateDegrees, dotRadius, dotRadius);

            labelText.RenderTransform = new TransformGroup() { Children = { rotateTransform } };
            rectangle.RenderTransform = new TransformGroup() { Children = { rotateTransform } };

            Canvas.SetLeft(labelText, labelPivotX);
            Canvas.SetTop(labelText, labelPivotY);

            Canvas.SetLeft(rectangle, labelPivotX);
            Canvas.SetTop(rectangle, labelPivotY - rectangle.Height / 2);

            return (rectangle, labelText);
        }
    }


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PlotPointsOnCircle(450.0, 36);
            DrawingToImageConversionService.PrintToPNG(plotCanvas);
        }
        private void PlotPointsOnCircle(double diameter, int pointCount)
        {
            plotCanvas.Width = 595;
            plotCanvas.Height = 600; ///842;

            double radius = diameter / 2.0;
            double centerX = plotCanvas.Width / 2.0;
            double centerY = plotCanvas.Height / 2.0;
            double angularSpacing = 360.0 / pointCount;

            // Draw the axes (arrows)

            ChartPrimaryElementsGenerationService.Generate(plotCanvas, centerX, centerY, diameter);


            for (int i = 0; i < pointCount; i++)
            {
                double angle = i * angularSpacing;

                _ = plotCanvas.Children.Add(DotGenerationService.GenerateDotPositionedOnCircle(centerX, centerY, radius, angle));

                if (i <= 2)
                {
                    (UIElement Rectangle, UIElement LabelText) result = LabelGenerationService.GenerateLabelPositionedOnCircle(centerX, centerY, radius + 10, Defaults.DotRadius, angle, 5, $"{i + 1}", Brushes.Black, angle);

                    _ = plotCanvas.Children.Add(result.Rectangle);
                    _ = plotCanvas.Children.Add(result.LabelText);
                }
                else
                {
                    (UIElement Rectangle, UIElement LabelText) result = LabelGenerationService.GenerateLabelPositionedOnCircle(centerX, centerY, radius + 10, Defaults.DotRadius, angle, 5, $"{i + 1}", Brushes.Black, 0);

                    _ = plotCanvas.Children.Add(result.Rectangle);
                    _ = plotCanvas.Children.Add(result.LabelText);
                }

            }



        }
    }
}
