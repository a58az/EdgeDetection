using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace csimages
{
    internal class FFT
    {
        private Complex[,] _complexImage;
        private bool _fourierTransform;
        private int _heightImg;
        private int _widthImg;

        public unsafe FFT(ref Bitmap imageBitmap)
        {
            if (imageBitmap.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                Debug.WriteLine("Only grayscale image.");
                return;
            }

            //Lock bits
            BitmapData bmpData = imageBitmap.LockBits(new Rectangle(0, 0, imageBitmap.Width, imageBitmap.Height),
                ImageLockMode.ReadOnly, imageBitmap.PixelFormat);

            int width = bmpData.Width;
            int height = bmpData.Height;
            int complexWidth = MathTools.IsPowerOf2(width) ? width : MathTools.RoundUpToPow2(width);
            int complexHeight = MathTools.IsPowerOf2(height) ? height : MathTools.RoundUpToPow2(height);

            FFTInit(complexWidth, complexHeight);

            var row = (byte*) bmpData.Scan0;
            int stride = bmpData.Stride;
            int i, j;

            for (i = 0; i < height; i++)
            {
                for (j = 0; j < width; j++)
                {
                    _complexImage[i, j] = new Complex(row[j], 0);
                }
                for (j = width; j < complexWidth; j++)
                {
                    _complexImage[i, j] = Complex.Zero;
                }

                row += stride;
            }

            for (i = height; i < complexHeight; i++)
//            Parallel.For(height, complexHeight, i1 =>
            {
                for (j = 0; j < complexWidth; j++)
                {
                    _complexImage[i, j] = Complex.Zero;
                }
            }

            imageBitmap.UnlockBits(bmpData);
        }

        private void FFTInit(int width, int height)
        {
            _complexImage = new Complex[height, width];
            _widthImg = width;
            _heightImg = height;
            _fourierTransform = false;
        }

        public void FFTTransform()
        {
//            FourierTransform.FFT2(_complexImage, FourierTransform.Direction.Forward);
        }
    }
}