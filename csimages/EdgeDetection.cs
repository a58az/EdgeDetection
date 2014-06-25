using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace csimages
{
    internal class EdgeDetection
    {
        private readonly int[,] SobelMatrixX =
        {
            {-1, 0, 1},
            {-2, 0, 2},
            {-1, 0, 1}
        };

        private readonly int[,] SobelMatrixY =
        {
            {-1, -2, -1},
            {0, 0, 0},
            {1, 2, 1}
        };

        public static Bitmap SobelOperatorColor(ref Bitmap bmp)
        {
            var newBitmap = ChangeColorModel.RgbToYuvMultiThread(ref bmp, Environment.ProcessorCount);
            bmp = newBitmap;
            return SobelOperatorGreyScale(ref bmp);
        }

        public static unsafe Bitmap SobelOperatorGreyScale(ref Bitmap bmp)
        {
            var sobelBmp = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format8bppIndexed);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly,
                bmp.PixelFormat);
            BitmapData sobelData = sobelBmp.LockBits(new Rectangle(0, 0, sobelBmp.Width, sobelBmp.Height),
                ImageLockMode.WriteOnly, sobelBmp.PixelFormat);

            int height = bmp.Height - 1;
            int width = bmp.Width - 1;
            int stride = bmpData.Stride;
            var rowBmpUp = (byte*) bmpData.Scan0;
            var rowBmpCenter = (byte*) (bmpData.Scan0 + stride);
            var rowBmpDown = (byte*) (bmpData.Scan0 + 2*stride);
            var rowSobel = (byte*) (sobelData.Scan0 + stride);
            int i, j, start = 1;
            int xG, yG;
            byte byteUpLeft, byteUpCenter, byteUpRight;
            byte byteMiddleLeft, byteMiddleCenter, byteMiddleRight;
            byte byteDownLeft, byteDownCenter, byteDownRight;

            for (i = start; i < height; i++)
            {
                byteUpLeft = rowBmpUp[0];
                byteUpCenter = rowBmpUp[1];
                byteUpRight = rowBmpUp[2];
                byteMiddleLeft = rowBmpCenter[0];
                byteMiddleCenter = rowBmpCenter[1];
                byteMiddleRight = rowBmpCenter[2];
                byteDownLeft = rowBmpDown[0];
                byteDownCenter = rowBmpDown[1];
                byteDownRight = rowBmpDown[2];

                for (j = 2; j < width; j++)
                {
                    xG = yG = 0;
                     xG = (byteUpRight - byteUpLeft) + ((byteMiddleRight << 1) - (byteMiddleLeft << 1)) +
                          (byteDownRight - byteDownLeft);

//                    xG = (byteUpCenter + (byteMiddleCenter << 1) + byteDownCenter) * (byteMiddleLeft + byteMiddleRight);

                    yG = (byteDownLeft - byteUpLeft) + ((byteDownCenter << 1) - (byteUpCenter << 1)) +
                         (byteDownRight - byteUpRight);


                    if (16129 > (xG*xG + yG*yG)) rowSobel[j] = 0xFF;
                    else
                    {
                        rowSobel[j] = 0;
                    }
//
                    byteUpLeft = byteUpCenter;
                    byteUpCenter = byteUpRight;
                    byteUpRight = rowBmpUp[j];
                    byteMiddleLeft = byteMiddleCenter;
                    byteMiddleCenter = byteMiddleRight;
                    byteMiddleRight = rowBmpCenter[j];
                    byteDownLeft = byteDownCenter;
                    byteDownCenter = byteDownRight;
                    byteDownRight = rowBmpDown[j];
                }

                rowBmpUp = rowBmpCenter;
                rowBmpCenter = rowBmpDown;
                rowBmpDown += stride;
                rowSobel += stride;
            }

            bmp.UnlockBits(bmpData);
            sobelBmp.UnlockBits(sobelData);

            return sobelBmp;
        }
    }
}