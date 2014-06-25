using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace csimages
{
    public partial class Form1 : Form
    {
        private static readonly string[] ImageFormats =
        {
            ".bmp", ".jpg", ".jpeg", ".png", ".gif", ".BMP", ".JPG", ".JPEG",
            ".PNG", ".GIF"
        };

        public static Bitmap Bmp;
        private string _originalPath;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // PictureBox style
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            // FileTypes for openDialog
            openFileDialog1.Filter = @"Image Files (*.bmp, *.jpg, *.jpeg, *.png, *.gif)|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
            // FileTypes for saveDialog
            saveFileDialog1.Filter =
                @"BMP Files: (*.BMP)|*.BMP;|JPEG Files: (*.JPG;)|*.JPG|GIF Files: (*.GIF)|*.GIF|PNG Files: (*.PNG)|*.PNG|All Files|*.*";
            saveFileDialog1.DefaultExt = @"*.BMP";
            // Status bar
            toolStripStatusLabel1.Text = String.Empty;
            toolStripStatusLabel2.Text = String.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            Cursor = Cursors.WaitCursor;

//            string path = openFileDialog1.FileName;
            OpenImg(openFileDialog1.FileName);

            Cursor = Cursors.Default;
        }

        private void OpenImg(string path)
        {
            try
            {
                pictureBox1.Image = null;
                pictureBox1.Load(path);
                try
                {
                    Bmp.Dispose();
                }
                catch (Exception)
                {
                }
                finally
                {
                    Bmp = new Bitmap(pictureBox1.Image);
                }
                _originalPath = path;
                toolStripStatusLabel1.Text = String.Format("{0}x{1}px", Bmp.Width, Bmp.Height);
                toolStripStatusLabel2.Text = Path.GetFileName(path);
            }
            catch (ArgumentException)
            {
                MessageBox.Show(@"Wrong file type!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Bmp == null) return;
            if (Image.GetPixelFormatSize(Bmp.PixelFormat) <= 8)
            {
                Debug.WriteLine("Image is already gray scale!");
                return;
            }
            int threadCount = Environment.ProcessorCount;

            var stopwatch = new Stopwatch();
            stopwatch.Start();
//            ChangeColorModel.RgbToYuv(ref Bmp, ref bmpGrey);
            Bitmap bmpGrey = ChangeColorModel.RgbToYuvMultiThread(ref Bmp, threadCount);
            stopwatch.Stop();
            Debug.WriteLine("Total time:" + stopwatch.ElapsedMilliseconds);

            pictureBox1.Image = bmpGrey;
            Bmp = bmpGrey;
            pictureBox1.Update();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

            ImageFormat imgFrm;
            string path = saveFileDialog1.FileName;
            string extension = Path.GetExtension(path);

            // No extension
            if (string.IsNullOrEmpty(extension))
            {
                throw new Exception("No file extension!");
            }

            // Select image format
            switch (extension.ToLower())
            {
                case ".bmp":
                    imgFrm = ImageFormat.Bmp;
                    break;
                case ".jpg":
                    imgFrm = ImageFormat.Jpeg;
                    break;
                case ".png":
                    imgFrm = ImageFormat.Png;
                    break;
                case ".gif":
                    imgFrm = ImageFormat.Gif;
                    break;
                default:
                    imgFrm = ImageFormat.Bmp;
                    break;
            }

            pictureBox1.Image.Save(path, imgFrm);
        }

        private void batchButton_Click_1(object sender, EventArgs e)
        {
            var bf = new BatchForm();
            bf.Show(this);
        }

        private void histogramButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var hf = new HistoForm();
            hf.Show(this);
            Cursor = Cursors.Default;
        }

        private void loadOriginalImage()
        {
            pictureBox1.Image = null;
            pictureBox1.Load(_originalPath);
            Bmp.Dispose();
            Bmp = new Bitmap(pictureBox1.Image);
            toolStripStatusLabel1.Text = String.Format("{0}x{1}px.", Bmp.Width, Bmp.Height);
        }

        private void fft_Click(object sender, EventArgs e)
        {
            var s = new Stopwatch();
            s.Start();
            Bitmap blured = Blur.GaussianBlurGreyScale(ref Bmp);
            s.Stop();
            Bmp = blured;
            pictureBox1.Image = Bmp;
            Debug.WriteLine(s.ElapsedMilliseconds); 
        }

        private void segmentation_Click(object sender, EventArgs e)
        {
            Bitmap segmentBitmap = Bmp;
            Binarization.OtsuSegmentation(ref Bmp);
            pictureBox1.Image = Bmp;
            pictureBox1.Update();
        }

        private void sobel_Click(object sender, EventArgs e)
        {
            var s = new Stopwatch();
            s.Start();
            Bitmap sobelImg = EdgeDetection.SobelOperatorGreyScale(ref Bmp);
            s.Stop();
            Debug.WriteLine(s.ElapsedMilliseconds);
            pictureBox1.Image = sobelImg;
            Bmp = sobelImg;
            pictureBox1.Update();
        }

        private void pictureBox1_DragOver(object sender, DragEventArgs e)
        {
            var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
            bool flag = false;
            if (files.Length == 1)
            {
                if (ImageFormats.Any(imageFormat => Path.GetExtension(files[0]) == imageFormat))
                {
                    flag = true;
                }
            }

            e.Effect = flag ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
            int counter = 0;
            if (files.Length == 1)
            {
                if (ImageFormats.Any(imageFormat => Path.GetExtension(files[0]) == imageFormat))
                {
                    OpenImg(files[0]);
                    counter++;
                }
            }
            Debug.WriteLine(counter);
        }
    }
}