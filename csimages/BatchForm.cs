using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace csimages
{
    public partial class BatchForm : Form
    {
        private const int ProgressBarAlign = 5;

        private const int PanelAlign = 6;

        private readonly string[] _imageFormats =
        {
            ".bmp", ".jpg", ".jpeg", ".png", ".gif", ".BMP", ".JPG", ".JPEG",
            ".PNG", ".GIF"
        };

        public BatchForm()
        {
            InitializeComponent();
        }

        private void BatchForm_Load(object sender, EventArgs e)
        {
        }

        private void BatchForm_Paint(object sender, PaintEventArgs e)
        {
        }

        private void BatchForm_Resize(object sender, EventArgs e)
        {
            listBox1.Width = ClientRectangle.Width - listBox1.Location.X*2;
            listBox1.Height = ClientRectangle.Height - listBox1.Location.Y*2 - ProgressBarAlign*2 - panel1.Height -
                              ProgressBarAlign*2;

            progressBar1.Top = listBox1.Top + listBox1.Height + ProgressBarAlign;
            progressBar1.Width = listBox1.Width;

            panel1.Top = progressBar1.Top + progressBar1.Height + PanelAlign;
            panel1.Width = listBox1.Width;
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[]) e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in files)
            {
                if (_imageFormats.Any(imageFormat => Path.GetExtension(file) == imageFormat))
                {
                    if (listBox1.FindString(file) == ListBox.NoMatches)
                    {
                        listBox1.Items.Add(file + " (" + FormatedFileSize(file) + ")");
                    }
                }
            }
        }

        private string FormatedFileSize(string path)
        {
            var fi = new FileInfo(path);
            double fileSize = fi.Length/1024.0;

            if (fileSize <= 0) return "0B";
            string[] uints = {"B", "KB", "MB", "GB", "TB"};

            var ui = (byte) (Math.Log(fileSize, 1024) + 1);
            var digitGroups = (int) (Math.Log10(fileSize)/Math.Log10(1024));

            fileSize /= Math.Pow(1024, digitGroups);

            return fileSize.ToString("F01") + uints[ui];
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
            bool flag = false;

            if (!e.Data.GetDataPresent(DataFormats.FileDrop, false)) return;

            foreach (string file in files)
            {
                if (_imageFormats.Any(imageFormat => Path.GetExtension(file) == imageFormat))
                {
                    flag = true;
                }
            }

            e.Effect = flag ? DragDropEffects.All : DragDropEffects.None;
        }

        private void BatchForm_Shown(object sender, EventArgs e)
        {
            BatchForm_Resize(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListBox.ObjectCollection items = listBox1.Items;
            Bitmap processBitmap;
            progressBar1.Maximum = listBox1.Items.Count;

            foreach (object item in items)
            {
                var filename = (item as string);
                filename = filename.Substring(0, filename.LastIndexOf("("));
                processBitmap = new Bitmap(filename);
                var bmpToSave = new Bitmap(processBitmap);
                if (cannyBox.Checked)
                {
                    bmpToSave = ChangeColorModel.RgbToYuvMultiThread(ref processBitmap, Environment.ProcessorCount);
                    bmpToSave = Blur.GaussianBlurGreyScale(ref bmpToSave);
                    bmpToSave = EdgeDetection.SobelOperatorGreyScale(ref bmpToSave);
                }
                string path = Path.GetDirectoryName(filename) + "\\filtered\\";
                string newName = path + Path.GetFileNameWithoutExtension(filename) + ("_new") +
                                 Path.GetExtension(filename);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string extension = Path.GetExtension(filename);

                // Select image format
                ImageFormat imgFrm;
                switch (extension.ToLowerInvariant())
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


                bmpToSave.Save(newName, imgFrm);
                progressBar1.Value++;
            }
        }
    }
}