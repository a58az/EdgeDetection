using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace csimages
{
    internal class Binarization
    {
        public static unsafe void OtsuSegmentation(ref Bitmap bmp)
        {

            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, bmp.PixelFormat);

            int threethold = OtsuMethod(ref bmData);
            for (int y = 0; y < bmData.Height; y++)
            {
                var row = (byte*) (bmData.Scan0 + y*bmData.Stride);
                for (int x = 0; x < bmData.Width; x++)
                {
                    row[x] = (byte) ((row[x] <= threethold) ? 0 : 255);
                }
            }

            bmp.UnlockBits(bmData);
        }

        private static unsafe int OtsuMethod(ref BitmapData bmData)
        {

            var hist = new int[256];
            Array.Clear(hist, 0, hist.Length);

            int height = bmData.Height;
            int width = bmData.Width;
            int stride = bmData.Stride;
            var row = (byte*) (bmData.Scan0);
            int i;

            for (int j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    hist[row[i]]++;
                }
                row += stride;
            }

            float sum = 0; // Total
            for (i = 1; i < 256; i++)
            {
                sum += i*hist[i];
            }

            float sumB = 0; // Current
            int wB = 0; // Weight Background
            int wF; // Weight Foreground

            int threshold = 0;
            int total = width*height;
            float mD; // Mean difference 
            float currentMax, totalMax = 0;


            for (i = 0; i < 256; i++)
            {
                // Weight Background
                wB += hist[i];
                if (wB == 0) continue;

                // Weight Foreground
                wF = total - wB;
                if (wF == 0) break;

                sumB += i*hist[i];

                // Means
                mD = sumB/wB - (sum - sumB)/wF;

                // Variance between classes 
                // http://upload.wikimedia.org/math/8/9/4/894892ba54d00a159aaebfb515c6643d.png
                currentMax = wB*wF*mD*mD;

                // If new maximum found
                if (currentMax > totalMax)
                {
                    totalMax = currentMax;
                    threshold = i; // New threshold
                }
            }

            return threshold;
        }
    }
}