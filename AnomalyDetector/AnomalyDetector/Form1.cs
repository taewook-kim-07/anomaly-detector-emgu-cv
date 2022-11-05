using System.Diagnostics;

using Emgu.CV;

using AnomalyDetector;
using AnomalyDetector.model;
using Emgu.CV.Structure;

namespace AnomalyDetector
{
    public partial class Form1 : Form
    {
        load_labels labels = new load_labels("assets/labels.txt");
        yolov5 yolo_model = new yolov5("assets/best.onnx", true);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //var (width, height) = (System.Windows.Forms.SystemInformation.VirtualScreen.Width, System.Windows.Forms.SystemInformation.VirtualScreen.Height);
            //tableLayoutPanel1.Size = new Size(width, height);
        }

        private void 시작ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var image = new Mat("assets/test.jpg");
            List<yolov5.YoloDetection> results = yolo_model.detect(ref image);
            Debug.WriteLine($"{results.Count}개 찾음");

            foreach (var item in results)
            {
                string text = $"{labels.Index(item.class_id)} {Math.Round(item.confidence, 2)}";
                CvInvoke.Rectangle(image, item.box, new MCvScalar(0, 255, 255), 3);
                CvInvoke.Rectangle(image, new Rectangle(item.box.X - 2, item.box.Y - 10, item.box.Width + 4, 10), new MCvScalar(0, 255, 0), -1);
                CvInvoke.PutText(image, text, new Point(item.box.X, item.box.Y), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.6, new MCvScalar(255, 255, 255), 1, Emgu.CV.CvEnum.LineType.AntiAlias);
            }

            

            //panAndZoomPictureBox1.Image = Emgu.CV.BitmapExtension.ToBitmap(image);
            imageBox1.Image = image;
            //image.Save("assets/output.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            sw.Stop();
            Debug.WriteLine($"{sw.Elapsed}");
        }
        private void 불러오기ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}