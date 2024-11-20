using System;
using System.IO;
using System.Text.RegularExpressions;

using SkiaSharp;

//using SkiaSharp;

public class SvgToPngConverter
{

    public static string DecodeUnicodeString(string input)
    {
        string decoded = Regex.Unescape(input);
        decoded = decoded.Substring(1, decoded.Length - 1);
        decoded = decoded.Substring(0, decoded.Length - 1);

        return decoded;
    }

    public static void ConvertSvgToPng(string svgContent, string outputPath, int width, int height)
    {
        string decodedSvgContent = DecodeUnicodeString(svgContent);
        using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(decodedSvgContent)))
        {
            SkiaSharp.Extended.Svg.SKSvg svg = new SkiaSharp.Extended.Svg.SKSvg();
            // Load the SVG from the memory stream
            _ = svg.Load(stream);

            // Create a bitmap and canvas for rendering
            using (SKBitmap bitmap = new SKBitmap(width, height))
            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                canvas.Clear();

                // Calculate scaling
                SKSize svgSize = svg.Picture.CullRect.Size;
                float scaleX = width / svgSize.Width;
                float scaleY = height / svgSize.Height;
                float scale = Math.Min(scaleX, scaleY);

                // Apply scaling to the canvas
                canvas.Scale((float)scale);

                // Draw the SVG picture onto the canvas
                canvas.DrawPicture(svg.Picture);

                // Save the bitmap as PNG
                using (SKImage image = SKImage.FromBitmap(bitmap))
                using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    using (FileStream fileStream = File.OpenWrite(outputPath))
                    {
                        data.SaveTo(fileStream);
                    }
                }
            }
        }
    }
}

