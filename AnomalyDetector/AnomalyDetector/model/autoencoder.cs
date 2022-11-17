using System.Diagnostics;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using static System.Net.Mime.MediaTypeNames;
using Org.BouncyCastle.Ocsp;
using System.Security.Cryptography;
using System.Threading.Channels;
using System;
using System.Numerics;
using MySqlX.XDevAPI.Common;

namespace AnomalyDetector.model
{
    public class autoencoder
    {
        private int INPUT_WIDTH;
        private int INPUT_HEIGHT;
        private int THRESHOLD;
        private string NAME;

        Net AE_MODEL;
        public autoencoder(string model_path, bool useCuda, bool readonnx = true, int threshold = 80, int input_width = 128, int input_height = 128)
        {
            INPUT_WIDTH = input_width;
            INPUT_HEIGHT = input_height;
            THRESHOLD = threshold;
            NAME = model_path.Substring(model_path.LastIndexOf('/') + 1, model_path.LastIndexOf('.') - model_path.LastIndexOf('/') - 1);

            Trace.WriteLine($"{NAME} {model_path}");
            if(readonnx)
                AE_MODEL = Emgu.CV.Dnn.DnnInvoke.ReadNetFromONNX(model_path);
            else
                AE_MODEL = Emgu.CV.Dnn.DnnInvoke.ReadNet(model_path);

            if (useCuda)
            {
                Trace.WriteLine("Running on GPU");
                AE_MODEL.SetPreferableBackend(Emgu.CV.Dnn.Backend.Cuda);
                AE_MODEL.SetPreferableTarget(Emgu.CV.Dnn.Target.Cuda);
            }
            else
            {
                Trace.WriteLine("Running on CPU");
                AE_MODEL.SetPreferableBackend(Emgu.CV.Dnn.Backend.OpenCV);
                AE_MODEL.SetPreferableTarget(Emgu.CV.Dnn.Target.Cpu);
            }
        }

        private Mat preprocessing(Mat source)
        {
            CvInvoke.CvtColor(source, source, Emgu.CV.CvEnum.ColorConversion.Rgb2Gray);
            CvInvoke.Resize(source, source, new Size(INPUT_WIDTH, INPUT_HEIGHT));
            return source;
        }

        public Mat generator(ref Mat input, out int anomaly_score)
        {
            // 128, 128, 1
            Mat input_image = preprocessing(input);
            Trace.WriteLine($"{NAME} > {input_image.Width}x{input_image.Height}");

            var blob = DnnInvoke.BlobFromImage(input_image, 1.0 / 255, new Size(INPUT_WIDTH, INPUT_HEIGHT), new MCvScalar(), true, false);

            AE_MODEL.SetInput(blob);

            Mat output = AE_MODEL.Forward();

            output = output.Reshape(0, INPUT_HEIGHT);
            CvInvoke.Multiply(output, new ScalarArray(255), output);
            output.ConvertTo(output, DepthType.Cv8U);

            //input_image.Save($"result_{NAME}_1.jpg");
            //output.Save($"result_{NAME}_2.jpg");

            Trace.WriteLine($"{NAME} >>> {output.Width}x{output.Height}");

            CvInvoke.AbsDiff(input_image, output, output);
            CvInvoke.Threshold(output, output, THRESHOLD, 255, ThresholdType.Binary);
            
            MCvScalar sum = CvInvoke.Sum(output);
            Trace.WriteLine($"{sum.V0:F2}  {sum.V1:F2}  {sum.V2:F2}  {sum.V3:F2}");
            anomaly_score = Convert.ToInt32(sum.V0 / 255);
            return output;
        }


    }
}
