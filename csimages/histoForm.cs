using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace csimages
{
    public partial class HistoForm : Form
    {
        public HistoForm()
        {
            InitializeComponent();
        }

        private void histoForm_Load(object sender, EventArgs e)
        {
            if (Form1.Bmp == null)
            {
                Debug.WriteLine("Firstly open image!");
                return;
            }

            Cursor = Cursors.WaitCursor;
            histoChart.Series.Clear();

            var s = new Stopwatch();
            //LockBits
            BitmapData histoBmp = Form1.Bmp.LockBits(new Rectangle(0, 0, Form1.Bmp.Width, Form1.Bmp.Height),
                ImageLockMode.ReadOnly, Form1.Bmp.PixelFormat);

            s.Start();
            switch (Image.GetPixelFormatSize(Form1.Bmp.PixelFormat))
            {
                case 1:
                case 8:
                    GreyScaleHisto(ref histoBmp);
                    break;
                case 24:
                case 32:
                    RgbHisto(ref histoBmp);
                    break;
            }
            s.Stop();
            Form1.Bmp.UnlockBits(histoBmp);

            Debug.WriteLine(s.ElapsedMilliseconds);

            Cursor = Cursors.Default;
        }

        private unsafe void GreyScaleHisto(ref BitmapData histoBmp)
        {
            var histoList = new uint[256];
            Array.Clear(histoList, 0, histoList.Length);
            int j;
            int height = histoBmp.Height;
            int stride = histoBmp.Stride;
            int width = histoBmp.Width;
            byte* row;
            IntPtr intPtr = histoBmp.Scan0;


            Parallel.For(0, height, i =>
            {
                row = (byte*) (intPtr + i*stride);
                for (j = 0; j < width; j++)
                {
                    histoList[row[j]]++;
                }
            });


            var grSeries = new Series("GreyScaleSeries");

            histoChart.Series.Add(grSeries);
            histoChart.Series[0].Points.DataBindY(histoList);
            histoChart.Series[0].Color = Color.Black;
            histoChart.Series[0].ChartType = SeriesChartType.Spline;
        }

        private unsafe void RgbHisto(ref BitmapData histoBmp)
        {
            var rList = new uint[256];
            var gList = new uint[256];
            var bList = new uint[256];

            int pixelSize = Image.GetPixelFormatSize(histoBmp.PixelFormat)/8;
            int j;
            byte* row;
            IntPtr hScan = histoBmp.Scan0;
            int hStride = histoBmp.Stride;
            int width = histoBmp.Width*pixelSize;
            int height = histoBmp.Height;

            for (int i = 0; i < height; i++)
            {
                row = (byte*) (hScan + i*hStride);
                for (j = 0; j < width; j += pixelSize)
                {
                    rList[row[j]]++; //r
                    gList[row[j + 1]]++; //g
                    bList[row[j + 2]]++; //b
                }
            }

            var rSeries = new Series("RedSeries"); //red series
            var gSeries = new Series("GreenSeries"); //green series
            var bSeries = new Series("BlueSeries"); //blue series

            histoChart.Series.Add(rSeries);
            histoChart.Series[0].Color = Color.Red;
            histoChart.Series[0].ChartType = SeriesChartType.Spline;
            histoChart.Series[0].Points.DataBindY(rList);
            histoChart.Series.Add(gSeries);
            histoChart.Series[1].Color = Color.Green;
            histoChart.Series[1].ChartType = SeriesChartType.Spline;
            histoChart.Series[1].Points.DataBindY(gList);
            histoChart.Series.Add(bSeries);
            histoChart.Series[2].Color = Color.Blue;
            histoChart.Series[2].ChartType = SeriesChartType.Spline;
            histoChart.Series[2].Points.DataBindY(bList);
        }
    }
}