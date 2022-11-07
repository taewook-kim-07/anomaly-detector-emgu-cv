using System.Diagnostics;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.ML;
using Emgu.CV.UI;

using AnomalyDetector;
using AnomalyDetector.model;
using AnomalyDetector.utils;
using Emgu.CV.Util;

namespace AnomalyDetector
{
    public partial class Form1 : Form
    {
        database Database;

        load_labels labels = new load_labels("assets/labels.txt");
        yolov5 yolo_model = new yolov5("assets/best.onnx", true);

        Emgu.CV.UI.ImageBox[] Cropped_ImageBox = new Emgu.CV.UI.ImageBox[12];
        Label[] Cropped_Label = new Label[12];
        
        static object lockMethod1 = new object();

        public Form1()
        {
            InitializeComponent();            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for(int i=0; i<12; ++i)
            {
                Cropped_Label[i] = new Label();
                Cropped_Label[i].Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                Cropped_Label[i].BackColor = Color.LightGray;
                Cropped_Label[i].Margin = new Padding(2, 0, 2, 0);
                Cropped_Label[i].TextAlign = ContentAlignment.MiddleCenter;
                Cropped_Label[i].Font = new Font("Arial", 16, FontStyle.Bold);
                Cropped_Label[i].Text = labels.Index(i);


                Cropped_ImageBox[i] = new Emgu.CV.UI.ImageBox();
                Cropped_ImageBox[i].Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                Cropped_ImageBox[i].BackColor = Color.LightGray;
                Cropped_ImageBox[i].Margin = new Padding(2, 0, 2, 0);
                Cropped_ImageBox[i].SizeMode = PictureBoxSizeMode.Zoom;
                Cropped_ImageBox[i].Enabled = false;

                TableLayoutPanel tlp = new TableLayoutPanel();
                tlp.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                tlp.ColumnCount = 1;
                tlp.RowCount = 2;
                tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
                tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 90));

                tlp.Controls.Add(Cropped_Label[i], 0, 0);
                tlp.Controls.Add(Cropped_ImageBox[i], 0, 1);

                if (i < 6)
                    tableLayoutPanel_left.Controls.Add(tlp, i % 2, Convert.ToInt32(i / 2));
                else
                    tableLayoutPanel_right.Controls.Add(tlp, i % 2, Convert.ToInt32(i % 6 / 2));
            }

            Database = new database("localhost", "3306", "test_db", "root", "root");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Database.Dispose();
        }

        Mat crop_color_frame(Mat input, Rectangle crop_region)
        {
            Image<Bgr, Byte> buffer_im = input.ToImage<Bgr, Byte>();
            buffer_im.ROI = crop_region;

            Image<Bgr, Byte> cropped_im = buffer_im.Copy();
            return cropped_im.Mat;
        }

        public byte[] ImageToByteArray(ref Emgu.CV.Mat imageIn)
        {
            using (var buffer = new VectorOfByte())
            {
                CvInvoke.Imencode(".jpg", imageIn, buffer);  //Must use .jpg not jpg
                byte[] jpgBytes = buffer.ToArray();
                return jpgBytes;
            }
        }

        private void 시작ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageBox_center.SetZoomScale(1.0, new Point(0, 0));

            foreach(var imagebox in Cropped_ImageBox)
            {
                imagebox.Controls.Clear();
                imagebox.Image = null;
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var image = new Mat("assets/test.jpg");
            List<yolov5.YoloDetection> results = yolo_model.detect(ref image);

            var imageparts = new List<Image<Gray, byte>>();
            var original_image = new Mat();
            image.CopyTo(original_image);

            Parallel.ForEach(results, item =>
            {
                Mat crop_image = crop_color_frame(original_image, item.box);
                //CvInvoke.Rectangle(crop_image, new Rectangle(1, 1, crop_image.Width - 1, crop_image.Height - 1), new MCvScalar(0, 0, 255), 3);

                this.BeginInvoke(new Action(() => { 
                    Cropped_ImageBox[item.class_id].Image = crop_image;
                    Cropped_Label[item.class_id].BackColor = (item.confidence < 0.87) ? Color.Red : Color.LightGray;

                }));

                string text = $"{labels.Index(item.class_id)} {Math.Round(item.confidence, 2)}";
                lock (lockMethod1)
                {
                    CvInvoke.Rectangle(image, item.box, new MCvScalar(0, 255, 255), 3);
                    CvInvoke.Rectangle(image, new Rectangle(item.box.X - 2, item.box.Y - 10, item.box.Width + 4, 10), new MCvScalar(0, 255, 0), -1);
                    CvInvoke.PutText(image, text, new Point(item.box.X, item.box.Y), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.6, new MCvScalar(255, 255, 255), 1, Emgu.CV.CvEnum.LineType.AntiAlias);
                }
            });

            imageBox_center.Image = image;

            byte[] imageBytes = ImageToByteArray(ref image);
            Database.Insert("NG", "F", ref imageBytes);

            sw.Stop();
            toolStripStatusLabel1.Text = $"소요시간 {sw.Elapsed}";
            Debug.WriteLine($"{sw.Elapsed}");
        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 기록ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "Form_DataViewer")
                {
                    frm.WindowState = FormWindowState.Normal;
                    frm.Activate();
                    return;
                }
            }
            Form_DataViewer form2 = new Form_DataViewer(ref Database);
            form2.Show();
        }

    }
}