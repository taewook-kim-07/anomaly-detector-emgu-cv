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
using MySqlX.XDevAPI.Common;

namespace AnomalyDetector
{
    public partial class Form1 : Form
    {
        database Database;

        ButtonStatus buttons = new ButtonStatus("assets/labels.txt");

        yolov5 yolo_model = new yolov5("assets/YOLOv5.onnx", true);
        autoencoder[] ae_model = new autoencoder[12];

        Emgu.CV.UI.ImageBox[] Cropped_ImageBox = new Emgu.CV.UI.ImageBox[12];
        Label[] Cropped_Label = new Label[12];
        
        static object lockMethod1 = new object();
        private string imagePath = "assets/test.jpg";
        int[] threshold = new int[]
        {
            110, 90, 120, 90, 120, 90,
            77, 100, 130, 90, 110, 110,
        };
        private int abnormal_point = 280;

        public Form1()
        {
            InitializeComponent();
            
            Parallel.For(0, 6, (i) =>
            {
                ae_model[i] = new autoencoder($"assets/AutoEncoder/AE_L{i + 1}.onnx", true, true, threshold[i]);
                //if (i < 4)
                //    ae_model[i + 6] = new autoencoder($"assets/AESC/frozen_models/AESC_R{i + 1}.pb", true, false, threshold[i + 6]);
                //else
                    ae_model[i + 6] = new autoencoder($"assets/AutoEncoder/AE_R{i + 1}.onnx", true, true, threshold[i + 6]);
            });
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
                Cropped_Label[i].Text = buttons.name(i);


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

        public byte[] ImageToByteArray(ref Mat imageIn)
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
            Stopwatch sw = new Stopwatch();
            sw.Start();

            imageBox_center.SetZoomScale(1.0, new Point(0, 0));

            Parallel.ForEach(Cropped_ImageBox, imagebox =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    imagebox.Controls.Clear();
                    imagebox.Image = null;
                }));
            });

            Parallel.ForEach(Cropped_Label, label =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    label.BackColor = Color.Red;
                }));
            });

            buttons.clear();


            var image = new Mat(imagePath);
            List<yolov5.YoloDetection> results = yolo_model.detect(ref image);

            var imageparts = new List<Image<Gray, byte>>();
            var original_image = new Mat();
            image.CopyTo(original_image);
            
            Parallel.ForEach(results, item =>
            {
                int score = 0;
                Mat crop_image = crop_color_frame(original_image, item.box);
                //CvInvoke.Rectangle(crop_image, new Rectangle(1, 1, crop_image.Width - 1, crop_image.Height - 1), new MCvScalar(0, 0, 255), 3);

                //int threshold = 0;
                //switch (item.class_id)
                //{
                //    case 0: threshold = Convert.ToInt32(textBox1.Text); break;
                //    case 1: threshold = Convert.ToInt32(textBox2.Text); break;
                //    case 2: threshold = Convert.ToInt32(textBox3.Text); break;
                //    case 3: threshold = Convert.ToInt32(textBox4.Text); break;
                //    case 4: threshold = Convert.ToInt32(textBox5.Text); break;
                //    case 5: threshold = Convert.ToInt32(textBox6.Text); break;
                //    case 6: threshold = Convert.ToInt32(textBox7.Text); break;
                //    case 7: threshold = Convert.ToInt32(textBox8.Text); break;
                //    case 8: threshold = Convert.ToInt32(textBox9.Text); break;
                //    case 9: threshold = Convert.ToInt32(textBox10.Text); break;
                //    case 10: threshold = Convert.ToInt32(textBox11.Text); break;
                //    case 11: threshold = Convert.ToInt32(textBox12.Text); break;
                //}
                Trace.WriteLine($"{buttons.name(item.class_id)} find");
                crop_image = ae_model[item.class_id].generator(ref crop_image, out score);
                bool normal = score < abnormal_point;

                this.BeginInvoke(new Action(() => {
                    Cropped_ImageBox[item.class_id].Image = crop_image;
                    Cropped_Label[item.class_id].BackColor = normal ? Color.LightGray : Color.Red;
                    Cropped_Label[item.class_id].Text = $"{buttons.name(item.class_id)} {score}";
                }));

                string text = $"{buttons.name(item.class_id)} {Math.Round(item.confidence, 2)}";
                buttons.add(item.class_id, normal);

                lock (lockMethod1)
                {
                    MCvScalar box_color = normal ? new MCvScalar(0, 255, 0) : new MCvScalar(0, 0, 255);
                    MCvScalar font_color = normal ? new MCvScalar(0, 0, 0)  : new MCvScalar(255, 255, 255);

                    CvInvoke.Rectangle(image, item.box, box_color, 1);
                    CvInvoke.Rectangle(image, new Rectangle(item.box.X - 1, item.box.Y - 14, item.box.Width + 1, 14), box_color, -1);
                    CvInvoke.PutText(image, text, new Point(item.box.X, item.box.Y), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.6, font_color, 1, Emgu.CV.CvEnum.LineType.AntiAlias);
                }
            });

            imageBox_center.Image = image;

            byte[] imageBytes = ImageToByteArray(ref image);

            string detail = buttons.isNormal();

            Database.Insert((detail.Length==0)?"PASS":"NG", detail, ref imageBytes);

            sw.Stop();
            toolStripStatusLabel1.Text = $"소요시간 {sw.Elapsed}";
            Trace.WriteLine($"{sw.Elapsed}");
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

        private void imageBox_center_OnZoomScaleChange(object sender, EventArgs e)
        {
            if (imageBox_center.ZoomScale == 1)
            {
                imageBox_center.VerticalScrollBar.Hide();
                imageBox_center.HorizontalScrollBar.Hide();
                imageBox_center.VerticalScrollBar.Value = 0;
                imageBox_center.HorizontalScrollBar.Value = 0;
            }
            else imageBox_center.SetZoomScale(Math.Round(imageBox_center.ZoomScale, 0), new Point(0, 0));
        }

        private void 이미지불러오기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                imagePath = openFileDialog1.FileName;
            }
        }
    }
}