// AutoEncoder with skip connections
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.Diagnostics;

namespace AnomalyDetector.model
{
    public class AESC
    {
        private int INPUT_WIDTH;
        private int INPUT_HEIGHT;
        private int THRESHOLD;
        private string NAME;
        private InferenceSession inferenceSession;

        public AESC(string model_path, int threshold = 80, int input_width = 128, int input_height = 128)
        {
            INPUT_WIDTH = input_width;
            INPUT_HEIGHT = input_height;
            THRESHOLD = threshold;
            NAME = model_path.Substring(model_path.LastIndexOf('/') + 1, model_path.LastIndexOf('.') - model_path.LastIndexOf('/') - 1);

            inferenceSession = new InferenceSession(model_path);
        }


        private float[][] preprocessing(ref Mat source)
        {
            CvInvoke.CvtColor(source, source, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
            CvInvoke.Resize(source, source, new Size(INPUT_WIDTH, INPUT_HEIGHT));

            Bitmap image = source.ToBitmap();
            Bitmap img = new Bitmap(image);
            var result = new float[img.Height][];
            for (int i = 0; i < img.Height; i++)
            {
                result[i] = new float[img.Width];
                for (int j = 0; j < img.Width; j++)
                {
                    var pixel = img.GetPixel(j, i);
                    //var gray = RgbToGray(pixel);

                    float normalized = pixel.R / 255f; // Normalize the Gray value to 0-1 range
                    result[i][j] = normalized;
                }
            }
            return result;
        }

        public Mat generator(ref Mat image, out int anomaly_score)
        {
            var input = preprocessing(ref image);

            var modelInputLayerName = inferenceSession.InputMetadata.Keys.Single();
            var imageFlattened = input.SelectMany(x => x).ToArray();
            int[] dimensions = { 1, 128, 128, 1 };
            var inputTensor = new DenseTensor<float>(imageFlattened, dimensions);
            var modelInput = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(modelInputLayerName, inputTensor)
            };
            var predict = inferenceSession.Run(modelInput);
            var test = ((DenseTensor<float>)predict.Single().Value).ToArray();

            byte[,,] byteInput = new byte[128, 128, 1];
            Parallel.For(0, 128, (i) =>
            {
                Parallel.For(0, 128, (j) =>
                {
                    byteInput[i, j, 0] = (byte)(test[i * 128 + j] * 255);
                });
            }); 

            Image<Gray, byte> distImage = new Image<Gray, byte>(byteInput);


            Mat output = new Mat();
            CvInvoke.AbsDiff(image, distImage.Mat, output);
            CvInvoke.Threshold(output, output, THRESHOLD, 255, ThresholdType.Binary);

            MCvScalar sum = CvInvoke.Sum(output);
            anomaly_score = Convert.ToInt32(sum.V0 / 255);

            image.Save($"ret_{NAME}_1.jpg");
            distImage.Save($"ret_{NAME}_2.jpg");
            output.Save($"diff_{NAME}_1.jpg");
            Trace.WriteLine($"{sum.V0:F2}  {sum.V1:F2}  {sum.V2:F2}  {sum.V3:F2}");

            return output;
        }
    }
}