using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace OnnxRuntime_ImageClassification
{
    class OnnxImageClassification
    {
        static public string RunSession(string onnxFilePath, string imageFilePath, int inputWidth = 224, int inputHeight = 224, bool ImShow = false)
        {
            Mat imgSrc = Cv2.ImRead(imageFilePath, ImreadModes.Color);
            string dstString = RunSessionAndDrawMat(onnxFilePath, imgSrc, inputWidth, inputHeight, ImShow);
            imgSrc.Dispose();

            return dstString;
        }

        static public string RunSessionAndDrawMat(string onnxFilePath, Mat imgSrc, int inputWidth = 224, int inputHeight = 224, bool ImShow = false)
        {
            using (var session = new InferenceSession(onnxFilePath))
            {
                return RunSessionAndDrawMat(session, imgSrc, inputWidth, inputHeight, ImShow);
            }
        }

        static public string RunSessionAndDrawMat(InferenceSession session, Mat imgSrc, int inputWidth = 224, int inputHeight = 224, bool ImShow = false)
        {
            var input = getDenseTensorFromMat(imgSrc, inputWidth, inputHeight);

            var inputShape = new DenseTensor<float>(new[] { 1, 2 });
            inputShape[0, 0] = imgSrc.Height;
            inputShape[0, 1] = imgSrc.Width;

            var inputMeta = session.InputMetadata;
            var inputName = inputMeta.First().Key;
            var inputDims = inputMeta.First().Value.Dimensions;

            var inputs = new NamedOnnxValue[] { NamedOnnxValue.CreateFromTensor(inputName, input) };
            /*
            var inputs = new List<NamedOnnxValue>
                    {
                        NamedOnnxValue.CreateFromTensor("inputImage", input),
                    };
                    */
            List<string> LineOutput = new List<string>();

            using (var results = session.Run(inputs))
            {
                Tensor<float> scores = results[0].AsTensor<float>();

                // Process results
                int indicesLength = (int)scores.Length;
                for (int i = 0; i < indicesLength; i++)
                {
                    var score = scores[0,i];
                    LineOutput.Add(score.ToString("g4"));
                }
            }
            /*
            if (ImShow)
            {
                Cv2.ImShow("Image", imgSrc);
                Cv2.WaitKey(0);
                Cv2.DestroyAllWindows();
            }
            */
            return string.Join("\t", LineOutput.Take(2).ToArray());
        }

        static private DenseTensor<float> getDenseTensorFromMat(Mat src, int tensorWidth, int tensorHeight)
        {
            var dstTensor = new DenseTensor<float>(new[] { 1, 3, tensorWidth, tensorHeight });

            Size newSize = new Size(tensorWidth, tensorHeight);
            Mat dst = new Mat();
            Cv2.Resize(src, dst, newSize);

            for (int y = 0; y < tensorHeight; y++)
            {
                for (int x = 0; x < tensorWidth; x++)
                {
                    Vec3b color = dst.At<Vec3b>(y, x);
                    dstTensor[0, 0, y, x] = ((float)color.Item2) / 255f;
                    dstTensor[0, 1, y, x] = ((float)color.Item1) / 255f;
                    dstTensor[0, 2, y, x] = ((float)color.Item0) / 255f;
                }
            }

            dst.Dispose();
            return dstTensor;
        }

    }
}
