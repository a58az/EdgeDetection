using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace csimages
{
    public static class ChangeColorModel
    {
        public static unsafe void RgbToYuv(ref Bitmap bmpColor, ref Bitmap bmpGrey)
        {
            BitmapData bmCData = bmpColor.LockBits(new Rectangle(0, 0, bmpColor.Width, bmpColor.Height),
                ImageLockMode.ReadOnly, bmpColor.PixelFormat);
            BitmapData bmGData = bmpGrey.LockBits(new Rectangle(0, 0, bmpGrey.Width, bmpGrey.Height),
                ImageLockMode.WriteOnly, bmpGrey.PixelFormat);

            int x, y, xG;
            int colorPixelSize = Image.GetPixelFormatSize(bmpColor.PixelFormat)/8;
            int greyPixelSize = Image.GetPixelFormatSize(bmpGrey.PixelFormat)/8;
            int fullWidth = bmCData.Width*colorPixelSize;
            int strideC = bmCData.Stride;
            int strideG = bmGData.Stride;
            int height = bmCData.Height;
            IntPtr scanC0 = bmCData.Scan0;
            IntPtr scanG0 = bmGData.Scan0;
            var rowC = (byte*) scanC0;
            var rowG = (byte*) scanG0;

            for (y = 0; y < height; y++)
            {
                for (x = 0, xG = 0; x < fullWidth; x += colorPixelSize, xG += greyPixelSize)
                {
                    rowG[xG] = (byte) (rowC[x]*0.114 + rowC[x + 1]*0.587 + rowC[x + 2]*0.299);
                }

                rowC += strideC;
                rowG += strideG;
            }

            GeneratePallete(ref bmpGrey);

            bmpColor.UnlockBits(bmCData);
            bmpGrey.UnlockBits(bmGData);
        }

        public static void GeneratePallete(ref Bitmap bmpGrey)
        {
            // Change palette for gray scale image
            ColorPalette cPal = bmpGrey.Palette;
            for (byte i = 0; i < 255; i++)
            {
                cPal.Entries[i] = Color.FromArgb(i, i, i);
            }
            cPal.Entries[255] = Color.FromArgb(255, 255, 255);
            bmpGrey.Palette = cPal;
        }


        public static Bitmap RgbToYuvMultiThread(ref Bitmap bmpColor, int threadCount = 1)
        {
            Bitmap bmpGrey = new Bitmap(bmpColor.Width, bmpColor.Height, PixelFormat.Format8bppIndexed);
            BitmapData bmCData = bmpColor.LockBits(new Rectangle(0, 0, bmpColor.Width, bmpColor.Height),
                ImageLockMode.ReadOnly, bmpColor.PixelFormat);
            BitmapData bmGData = bmpGrey.LockBits(new Rectangle(0, 0, bmpGrey.Width, bmpGrey.Height),
                ImageLockMode.ReadOnly, bmpGrey.PixelFormat);

//            var threadList = new List<Thread>();

            int rowStart = 0;
            int height = bmCData.Height;
            int strideG = bmGData.Stride;
            int strideC = bmCData.Stride;
            int colorPixelSize = Image.GetPixelFormatSize(bmCData.PixelFormat)/8;
            int greyPixelSize = Image.GetPixelFormatSize(bmGData.PixelFormat)/8;
            int fullWidth = bmCData.Width*colorPixelSize;
            IntPtr scanC = bmCData.Scan0;
            IntPtr scanG = bmGData.Scan0;

            int delta = height/threadCount;
            int div = height%threadCount;

            int i;

            for (i = 0; i < threadCount; i++)
                unsafe
                {
                    var c = (byte*) (scanC + rowStart*strideC);
                    var g = (byte*) (scanG + rowStart*strideG);
                    rowStart += delta;
//                threadList.Add(
//                    new Thread(
//                        () =>
//                        {
//                            __RgbToYuvMT(start, end, scanC, scanG, colorPixelSize, greyPixelSize, strideC, strideG,
//                                fullWidth);
//                        }
//                        ));
                    ThreadPool.QueueUserWorkItem(
                        state =>
                            __RgbToYuvMT(delta, c, g, colorPixelSize, greyPixelSize, strideC, strideG,
                                fullWidth));
                }
            if (div > 0) // останутся не обработанные строки
                unsafe
                {
                    var c = (byte*) (scanC + rowStart*strideC);
                    var g = (byte*) (scanG + rowStart*strideG);
                    int d = height - rowStart;
                    ThreadPool.QueueUserWorkItem(
                        state => __RgbToYuvMT(d, c, g, colorPixelSize, greyPixelSize, strideC, strideG, fullWidth));

                    //                threadList.Add(
                    //                    new Thread(
                    //                        () =>
                    //                        {
                    //                            __RgbToYuvMT(rowStart, rowEnd, scanC, scanG, colorPixelSize, greyPixelSize, strideC, strideG,
                    //                                fullWidth);
                    //                        }));
                }


//            int count = threadList.Count;
//            for (i = 0; i < count; i++)
//            {
//                threadList[i].Start();
//            }


            bmpColor.UnlockBits(bmCData);
            bmpGrey.UnlockBits(bmGData);

            // Change palette for gray scale image
            GeneratePallete(ref bmpGrey);

            return bmpGrey;
        }

        private static unsafe void __RgbToYuvMT(int delta, byte* rowC, byte* rowG, int colorPixelSize,
            int greyPixelSize, int strideC, int strideG, int fullWidth)
        {
            var sw = new Stopwatch();
            int x, y, xG;

            sw.Start();
            for (y = 0; y < delta; y++)
            {
                for (x = 0, xG = 0; x < fullWidth; x += colorPixelSize, xG += greyPixelSize)
                {
                    // r + g + b
                    rowG[xG] = (byte) (rowC[x]*29 + rowC[x + 1]*150 + rowC[x + 2]*77 >> 8);
                }

                rowC += strideC;
                rowG += strideG;
            }
            sw.Stop();
            Debug.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}