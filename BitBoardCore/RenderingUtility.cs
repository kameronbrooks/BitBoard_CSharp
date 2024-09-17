using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace BitBoardCore
{
    public static class RenderingUtility
    {
        public static ColorMatrix GetColorMatrix(Color color)
        {
            return new ColorMatrix([
                [(float)(color.R / 255f), 0.0f, 0.0f, 0.0f, 0.0f],
                [ 0.0f, (float)(color.G / 255f), 0.0f, 0.0f, 0.0f ],
                [ 0.0f, 0.0f, (float)(color.B / 255f), 0.0f, 0.0f ],
                [ 0.0f, 0.0f, 0.0f, 1.0f, 0.0f ],
                [ 0.0f, 0.0f, 0.0f, 0.0f, 1.0f ]
            ]);
        }

        public static void RenderImage(Graphics g, Image img, Rectangle rect, ColorMatrix colorMatrix)
        {
            // Create ImageAttributes and set the ColorMatrix
            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            g.DrawImage(img, rect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imageAttributes);
        }
    }
}
