
using AutoImageWaterMarker.Extensions;
using AutoImageWaterMarker.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;

namespace AutoImageWaterMarker.Watermark
{
    public static class Watermarker
    {
        public static Image DrawOn(string imagePath, WatermarkerConfig config)
        {
            var image = Image.FromFile(imagePath);
            var watermarkPath = $@"{Directory.GetCurrentDirectory()}\Images\watermark.png";
            var watermark = Properties.Resources.watermark;
            //var watermark = Image.FromFile(watermarkPath);

            if (!config.Margin.AllEqual(0) || config.ScaleRatio != 1.0f)
                watermark = RedrawWatermarkWithMargins(watermark, config.Margin, config.ScaleRatio);

            if (image == null)
                throw new NullReferenceException("Image to draw on cannot be null.");
            if (watermark == null)
                throw new NullReferenceException("Watermark cannot be null.");

            if (config.Opacity < 0 || config.Opacity > 1)
                throw new ArgumentOutOfRangeException("Opacity");

            if (config.ScaleRatio <= 0)
                throw new ArgumentOutOfRangeException("ScaleRatio");

            watermark.RotateFlip(config.RotateFlip);

            var watermarkPosition = image.GetWatermarkPosition(watermark, config);
            var destRect = new Rectangle(watermarkPosition.X, watermarkPosition.Y, watermark.Width, watermark.Height);
            var colorMatrix = new ColorMatrix();
            colorMatrix.Matrix33 = config.Opacity;
            var imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            if (config.TransparentColor != Color.Empty)
            {
                imageAttributes.SetColorKey(config.TransparentColor, config.TransparentColor);
            }

            using (var graphics = Graphics.FromImage(image))
            {
                graphics.DrawImage(watermark, destRect, 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);
            }

            return image;
        }
        
        private static Bitmap RedrawWatermarkWithMargins(this Bitmap watermark, Thickness margin, float scaleRatio)
        {
            // Create a new bitmap with new sizes (size + margins) and draw the watermark
            var newWidth = Convert.ToInt32(watermark.Width * scaleRatio);
            var newHeight = Convert.ToInt32(watermark.Height * scaleRatio);
            var sourceRect = new Rectangle(margin.Left.ToInt(), margin.Top.ToInt(), newWidth, newHeight);
            var destRect = new Rectangle(0, 0, watermark.Width, watermark.Height);

            var bitmap = new Bitmap(newWidth + margin.Left.ToInt() + margin.Right.ToInt(), newHeight + margin.Top.ToInt() + margin.Bottom.ToInt());
            bitmap.SetResolution(watermark.HorizontalResolution, watermark.VerticalResolution);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawImage(watermark, sourceRect, destRect, GraphicsUnit.Pixel);
            }

            return bitmap;
        }

        private static System.Drawing.Point GetWatermarkPosition(this Image image, Bitmap watermark, WatermarkerConfig config)
        {
            int x = 0;
            int y = 0;

            switch (config.Position)
            {
                case WatermarkPosition.Absolute:
                    x = config.PositionX; y = config.PositionY;
                    break;
                case WatermarkPosition.TopLeft:
                    x = 0; y = 0;
                    break;
                case WatermarkPosition.TopRight:
                    x = image.Width - watermark.Width; y = 0;
                    break;
                case WatermarkPosition.TopMiddle:
                    x = (image.Width - watermark.Width) / 2; y = 0;
                    break;
                case WatermarkPosition.BottomLeft:
                    x = 0; y = image.Height - watermark.Height;
                    break;
                case WatermarkPosition.BottomRight:
                    x = image.Width - watermark.Width; y = image.Height - watermark.Height;
                    break;
                case WatermarkPosition.BottomMiddle:
                    x = (image.Width - watermark.Width) / 2; y = image.Height - watermark.Height;
                    break;
                case WatermarkPosition.MiddleLeft:
                    x = 0; y = (image.Height - watermark.Height) / 2;
                    break;
                case WatermarkPosition.MiddleRight:
                    x = image.Width - watermark.Width; y = (image.Height - watermark.Height) / 2;
                    break;
                case WatermarkPosition.Center:
                    x = (image.Width - watermark.Width) / 2; y = (image.Height - watermark.Height) / 2;
                    break;
                default:
                    break;
            }

            return new System.Drawing.Point(x, y);
        }
    }
}
