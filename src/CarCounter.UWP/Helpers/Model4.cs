using CarCounter.UWP.Samples;
using Microsoft.AI.MachineLearning;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace CarCounter.UWP.Helpers
{
    internal class Model4
    {
       
        Tracker tracker;
        private static readonly string[] _classesNames = new string[] {
            "person", "bicycle", "car", "motorbike", "aeroplane", "bus", "train", "truck", "boat", "traffic light", "fire hydrant", "stop sign", "parking meter",
            "bench", "bird", "cat", "dog", "horse", "sheep", "cow", "elephant", "bear", "zebra", "giraffe", "backpack", "umbrella", "handbag", "tie", "suitcase",
            "frisbee", "skis", "snowboard", "sports ball", "kite", "baseball bat", "baseball glove", "skateboard", "surfboard", "tennis racket", "bottle", "wine glass",
            "cup", "fork", "knife", "spoon", "bowl", "banana", "apple", "sandwich", "orange", "broccoli", "carrot", "hot dog", "pizza", "donut", "cake", "chair", "sofa",
            "pottedplant", "bed", "diningtable", "toilet", "tvmonitor", "laptop", "mouse", "remote", "keyboard", "cell phone", "microwave", "oven", "toaster", "sink",
            "refrigerator", "book", "clock", "vase", "scissors", "teddy bear", "hair drier", "toothbrush" };

        private static readonly string[] filter = new string[] {
            "bicycle", "car", "motorbike", "bus", "truck" };

       
        public ImagePrediction Prediction { get; set; }
        LearningModelSession _session = null;
        LearningModelSession tensorizationSession_ = null;


        bool initialized_ = false;


        public Model4()
        {
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ImageWidth"></param>
        /// <param name="ImageHeight"></param>
        /// <param name="DeviceIndex">CPU = 0, GPU = 1</param>
        /// <returns></returns>
        public async Task InitModelAsync(double ImageWidth, double ImageHeight, int DeviceIndex=0)
        {
            this.tracker = new Tracker();

            this.Prediction = new ImagePrediction();
            this.Prediction.ImageHeight = (float)ImageHeight;
            this.Prediction.ImageWidth = (float)ImageWidth;

          
            var modelName = "yolov4.onnx";
            var modelPath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "Assets", modelName);
            var model = LearningModel.LoadFromFilePath(modelPath);
            _session = CreateLearningModelSession(model,DeviceIndex);

            initialized_ = true;
        }

        private LearningModelSession CreateLearningModelSession(LearningModel model, int DeviceIndex = 0)
        {
            var kind =
                (DeviceIndex == 0) ?
                    LearningModelDeviceKind.Cpu :
                    LearningModelDeviceKind.DirectXHighPerformance;
            var device = new LearningModelDevice(kind);
            var options = new LearningModelSessionOptions()
            {
                CloseModelOnSessionCreation = true              // Close the model to prevent extra memory usage
            };
            var session = new LearningModelSession(model, device, options);
            return session;
        }

        public async Task<List<Result>> EvaluateFrame(VideoFrame frame, Rectangle selectRect)
        {

            try
            {
                tensorizationSession_ =
                        CreateLearningModelSession(
                            TensorizationModels.ReshapeFlatBufferNHWC(
                                1,
                                4,
                                frame.SoftwareBitmap.PixelHeight,
                                frame.SoftwareBitmap.PixelWidth,
                                416,
                                416));


                // Tensorize
                WriteableBitmap writeableBitmap = new WriteableBitmap(frame.SoftwareBitmap.PixelWidth, frame.SoftwareBitmap.PixelHeight);
                frame.SoftwareBitmap.CopyToBuffer(writeableBitmap.PixelBuffer);
                var bytes = writeableBitmap.PixelBuffer.ToArray();
                var buffer = bytes.AsBuffer(); // Does this do a copy??
                var inputRawTensor = TensorUInt8Bit.CreateFromBuffer(new long[] { 1, buffer.Length }, buffer);
               
                // 3 channel NCHW
                var tensorizeOutput = TensorFloat.Create(new long[] { 1, 416, 416, 3 });
                var b = new LearningModelBinding(tensorizationSession_);
                b.Bind(tensorizationSession_.Model.InputFeatures[0].Name, inputRawTensor);
                b.Bind(tensorizationSession_.Model.OutputFeatures[0].Name, tensorizeOutput);
                tensorizationSession_.Evaluate(b, "");

                // Resize
                var resizeBinding = new LearningModelBinding(_session);
                resizeBinding.Bind(_session.Model.InputFeatures[0].Name, tensorizeOutput);
                var results = _session.Evaluate(resizeBinding, "");

                //var key = results.Outputs.Keys.First();
                var output1 = results.Outputs["Identity:0"] as TensorFloat;
                var output2 = results.Outputs["Identity_1:0"] as TensorFloat;
                var output3 = results.Outputs["Identity_2:0"] as TensorFloat;             
                
                Prediction.Identity = output1.GetAsVectorView().ToArray();
                Prediction.Identity1 = output2.GetAsVectorView().ToArray();
                Prediction.Identity2 = output3.GetAsVectorView().ToArray();
                
                var res = Prediction.GetResults(_classesNames, filter);
                tracker.Process(res, selectRect);
                //var bmp = DrawResults.Draw(results, image, tracker);
                //return bmp;
                return res.ToList();
                //RenderImageInMainPanel(softwareBitmap);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return default;

            }

            
        }

        public DataTable GetLogData()
        {
            return tracker.GetLogTable();
        }

        public void SaveLog()
        {
            tracker.SaveToLog();
        }
    }
}
