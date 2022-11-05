using System.Diagnostics;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;

namespace AnomalyDetector.model
{
    public class yolov5
    {
        private float MIN_CONFIDENCE;
        private float NMS_THRESHOLD;
        private int INPUT_WIDTH;
        private int INPUT_HEIGHT;

        private Net YoloModel;
        private Object _lockObject = new Object();

        public struct YoloDetection
        {
            public int class_id;
            public float confidence;
            public Rectangle box;
        };

        public yolov5(string model_path, bool useCuda = false,
                        float minconfidence = 0.2f, float nms_threshold = 0.4f, int input_width = 640, int input_height = 640)
        {
            MIN_CONFIDENCE = minconfidence;
            NMS_THRESHOLD  = nms_threshold;
            INPUT_WIDTH    = input_width;
            INPUT_HEIGHT   = input_height;

            // Net YoloModel = DnnInvoke.ReadNet(model_path);
            YoloModel = Emgu.CV.Dnn.DnnInvoke.ReadNetFromONNX(model_path);
            if (useCuda)
            {
                Debug.Print("Running on GPU");
                YoloModel.SetPreferableBackend(Emgu.CV.Dnn.Backend.Cuda);
                YoloModel.SetPreferableTarget(Emgu.CV.Dnn.Target.CudaFp16);
            }
            else
            {
                Debug.Print("Running on CPU");
                YoloModel.SetPreferableBackend(Emgu.CV.Dnn.Backend.OpenCV);
                YoloModel.SetPreferableTarget(Emgu.CV.Dnn.Target.Cpu);
            }
        }

        private Mat preprocessing(Mat source)
        {
            int width = source.Cols;
            int height = source.Rows;
            int max = Math.Max(width, height);

            Mat ret = new Mat(max, max, DepthType.Cv8U, 3);
            source.CopyTo(ret);
            return ret;
        }

        public List<YoloDetection> detect(ref Mat image)
        {
            var (w, h) = (image.Width, image.Height);
            var (x_factor, y_factor) = ((float)w / INPUT_WIDTH, (float)h / INPUT_HEIGHT);

            Mat input_image = preprocessing(image);
            var blob = DnnInvoke.BlobFromImage(input_image, 1.0/255, new Size(INPUT_WIDTH, INPUT_HEIGHT), new MCvScalar(), true, false);
            YoloModel.SetInput(blob);
            
            VectorOfMat layerOutputs = new VectorOfMat();
            YoloModel.Forward(layerOutputs, YoloModel.UnconnectedOutLayersNames);


            List<int> classIDs = new List<int>();
            List<float> confidences = new List<float>();
            List<Rectangle> boxes = new List<Rectangle>();

            Parallel.For(0, layerOutputs.Size, (k) =>
            {
                // lo: [1,25200,17]
                float[,,] output = (float[,,])layerOutputs[k].GetData();

                Parallel.For(0, output.GetLength(1), (i) =>
                {
                    if (output[0, i, 4] < MIN_CONFIDENCE) return;   // Parallel.For은 함수 호출형식이므로 return으로 continue를 함


                    Parallel.For(5, output.GetLength(2), (j) =>
                    {
                        output[0, i, j] = output[0, i, j] * output[0, i, 4];    // mul_conf = obj_conf * cls_conf;
                    });


                    Parallel.For(5, output.GetLength(2), (j) =>
                    {
                        if (output[0, i, j] < MIN_CONFIDENCE) return;   // skip low mul_conf results

                        float left = output[0, i, 0] - output[0, i, 2] / 2;
                        float top = output[0, i, 1] - output[0, i, 3] / 2;
                        float right = output[0, i, 0] + output[0, i, 2] / 2;
                        float bottom = output[0, i, 1] + output[0, i, 3] / 2;
                        left *= x_factor;
                        right *= x_factor;
                        top *= y_factor;
                        bottom *= y_factor;
                        // 공유 자원을 각 For문으로 접근하여 데이터 손상이 발생하므로 Lock으로 동시에 처리 못하게 함
                        lock (_lockObject)
                        {
                            classIDs.Add(j - 5);
                            confidences.Add(output[0, i, j]);
                            int cnt = boxes.Count;
                            boxes.Add(Rectangle.FromLTRB(
                                (int)Math.Ceiling(left), (int)Math.Ceiling(top),
                                (int)Math.Ceiling(right), (int)Math.Ceiling(bottom)
                            ));
                            // Debug.WriteLine($"{boxes.Count}: {(int)Math.Ceiling(left)}, {(int)Math.Ceiling(top)}, {(int)Math.Ceiling(right)}, {(int)Math.Ceiling(bottom)}");
                        }
                            
                    });
                });
            });

            Debug.Print($"{boxes.Count} , {confidences.Count}");
            int[] bIndexes = DnnInvoke.NMSBoxes(boxes.ToArray(), confidences.ToArray(), MIN_CONFIDENCE, NMS_THRESHOLD);

            List<YoloDetection> filteredBoxes = new List<YoloDetection>();
            Parallel.ForEach(bIndexes, idx =>
            {
                lock (_lockObject)
                {
                    filteredBoxes.Add(new YoloDetection()
                    {
                        class_id = classIDs[idx],
                        confidence = (float)Math.Round(confidences[idx], 4),
                        box = boxes[idx],
                    });
                }
            });
            return filteredBoxes;
        }

}
}
