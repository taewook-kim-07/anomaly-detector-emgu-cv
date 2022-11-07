using AnomalyDetector.utils;
using Emgu.CV.UI;
using System.Diagnostics;

namespace AnomalyDetector
{
    public partial class Form_IMG_Viewer : Form
    {
        public Form_IMG_Viewer(ref database Database, string select_image_id)
        {
            InitializeComponent();

            this.Text = select_image_id + "번 이미지 뷰어";
            Database.ShowImage(ref panAndZoomPictureBox1, select_image_id);
        }

        private void panAndZoomPictureBox1_OnZoomScaleChange(object sender, EventArgs e)
        {
            if (panAndZoomPictureBox1.ZoomScale == 1)
            {
                panAndZoomPictureBox1.VerticalScrollBar.Hide();
                panAndZoomPictureBox1.HorizontalScrollBar.Hide();
                panAndZoomPictureBox1.VerticalScrollBar.Value = 0;
                panAndZoomPictureBox1.HorizontalScrollBar.Value = 0;
            }
            else panAndZoomPictureBox1.SetZoomScale(Math.Round(panAndZoomPictureBox1.ZoomScale, 0), new Point(0, 0));
        }
    }
}