#region

using System.Drawing;
using System.Drawing.Imaging;

#endregion

namespace csimages
{
    internal class Blur
    {
        private static readonly byte[,] GaussMatrix =
        {
            {1, 2, 1},
            {2, 4, 2},
            {1, 2, 1}
        };

        public static unsafe Bitmap GaussianBlurGreyScale(ref Bitmap bitmap)
        {
            var returnBitmap = new Bitmap(bitmap.Width, bitmap.Height, bitmap.PixelFormat);
            BitmapData bmData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly,
                bitmap.PixelFormat);
            BitmapData newBmData = returnBitmap.LockBits(new Rectangle(0, 0, returnBitmap.Width, returnBitmap.Height),
                ImageLockMode.WriteOnly, returnBitmap.PixelFormat);

            int width = bitmap.Width - 1;
            int height = bitmap.Height - 1;
            int stride = bmData.Stride;
            const int start = 1;
            var rowUp = (byte*) (bmData.Scan0);
            var rowCentr = (byte*) (bmData.Scan0 + stride);
            var rowDown = (byte*) (bmData.Scan0 + 2*stride);
            var currentNewRow = (byte*) (newBmData.Scan0 + stride);
            int color, i, j;
//            int x, x1;
            byte rowUpLeft, rowUpCenter, rowUpRight;
            byte rowCenterLeft, rowCenterCenter, rowCenterRight;
            byte rowDownLeft, rowDownCenter, rowDownRight;

            for (i = start; i < height; i++)
            {
                //                     Optimization for RAM
                rowUpLeft = rowUp[0];
                rowUpCenter = rowUp[1];
                rowUpRight = rowUp[2];

                rowCenterLeft = rowCentr[0];
                rowCenterCenter = rowCentr[1];
                rowCenterRight = rowCentr[2];

                rowDownLeft = rowDown[0];
                rowDownCenter = rowDown[1];
                rowDownRight = rowDown[2];

                for (j = 2; j < width; j++)
                {
                    color = 0;

                    color += rowUpLeft + rowUpRight + (rowUpCenter << 1);
                    color += (rowCenterLeft << 1) + (rowCenterRight << 1) + (rowCenterCenter << 2);
                    color += rowDownLeft + rowDownRight + (rowDownCenter << 1);

                    color >>= 4;
                    currentNewRow[j - 2] = (byte) color;

//                     Optimization for RAM
                    rowUpLeft = rowUpCenter;
                    rowUpCenter = rowUpRight;
                    rowUpRight = rowUp[j];

                    rowCenterLeft = rowCenterCenter;
                    rowCenterCenter = rowCenterRight;
                    rowCenterRight = rowCentr[j];

                    rowDownLeft = rowDownCenter;
                    rowDownCenter = rowDownRight;
                    rowDownRight = rowDown[j];
                }

                rowUp = rowCentr;
                rowCentr = rowDown;
                rowDown += stride;
                currentNewRow += stride;
            }

            returnBitmap.UnlockBits(newBmData);
            bitmap.UnlockBits(bmData);

            ChangeColorModel.GeneratePallete(ref returnBitmap);

            return returnBitmap;
        }
    }
}