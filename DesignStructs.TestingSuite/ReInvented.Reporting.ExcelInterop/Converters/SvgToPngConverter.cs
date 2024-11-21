using System;
using System.IO;
using System.Text.RegularExpressions;

using HtmlAgilityPack;

using SkiaSharp;

//using SkiaSharp;

public class SvgToPngConverter
{
    public static string DecodeUnicodeString(string input)
    {
        string decoded = Regex.Unescape(input);
        //decoded = decoded.Substring(1, decoded.Length - 1);
        //decoded = decoded.Substring(0, decoded.Length - 1);

        return decoded;
    }

    public void ConvertSvgToPng(HtmlNode svgNode, string outputPath, int width, int height)
    {
        if (svgNode.Name != "svg")
        {
            throw new InvalidOperationException("The provided HtmlNode is not an <svg> element.");
        }

        string svgContent = svgNode.OuterHtml;
        ConvertSvgToPng(svgContent, outputPath, width, height);
    }

    public static void ConvertSvgToPng(string svgContent, string outputPath, int width, int height)
    {
        string decodedSvgContent = DecodeUnicodeString(svgContent);
        using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(decodedSvgContent)))
        {
            SkiaSharp.Extended.Svg.SKSvg svg = new SkiaSharp.Extended.Svg.SKSvg();
            _ = svg.Load(stream);

            using (SKBitmap bitmap = new SKBitmap(width, height))
            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                //canvas.Clear();
                canvas.DrawRect(0, 0, width, height, new SKPaint { Color = SKColors.Transparent });

                SKSize svgSize = svg.Picture.CullRect.Size;
                float scaleX = width / svgSize.Width;
                float scaleY = height / svgSize.Height;
                float scale = Math.Min(scaleX, scaleY);

                canvas.Scale((float)scale);
                canvas.DrawPicture(svg.Picture);

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

    public static byte[] ConvertSvgToPng(string svgContent, int width, int height)
    {
        string decodedSvgContent = DecodeUnicodeString(svgContent);
        using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(decodedSvgContent)))
        {
            SkiaSharp.Extended.Svg.SKSvg svg = new SkiaSharp.Extended.Svg.SKSvg();
            _ = svg.Load(stream);

            using (SKBitmap bitmap = new SKBitmap(width, height))
            using (SKCanvas canvas = new SKCanvas(bitmap))
            {
                //canvas.Clear();
                canvas.DrawRect(0, 0, width, height, new SKPaint { Color = SKColors.Transparent });

                SKSize svgSize = svg.Picture.CullRect.Size;
                float scaleX = width / svgSize.Width;
                float scaleY = height / svgSize.Height;
                float scale = Math.Min(scaleX, scaleY);

                canvas.Scale((float)scale);
                canvas.DrawPicture(svg.Picture);

                using (SKImage image = SKImage.FromBitmap(bitmap))
                using (SKData data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    return data.ToArray();
                }
            }
        }
    }

}

