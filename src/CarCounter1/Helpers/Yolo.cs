using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCounter1.Helpers
{
    public class Yolo:IDisposable
    {
        Helpers.Tracker tracker;
        private const string _modelPath = @"yolov4.onnx";
        private static readonly string[] _classesNames = new string[] {
            "person", "bicycle", "car", "motorbike", "aeroplane", "bus", "train", "truck", "boat", "traffic light", "fire hydrant", "stop sign", "parking meter",
            "bench", "bird", "cat", "dog", "horse", "sheep", "cow", "elephant", "bear", "zebra", "giraffe", "backpack", "umbrella", "handbag", "tie", "suitcase",
            "frisbee", "skis", "snowboard", "sports ball", "kite", "baseball bat", "baseball glove", "skateboard", "surfboard", "tennis racket", "bottle", "wine glass",
            "cup", "fork", "knife", "spoon", "bowl", "banana", "apple", "sandwich", "orange", "broccoli", "carrot", "hot dog", "pizza", "donut", "cake", "chair", "sofa",
            "pottedplant", "bed", "diningtable", "toilet", "tvmonitor", "laptop", "mouse", "remote", "keyboard", "cell phone", "microwave", "oven", "toaster", "sink",
            "refrigerator", "book", "clock", "vase", "scissors", "teddy bear", "hair drier", "toothbrush" };
        
        private static readonly string[] filter = new string[] {
            "bicycle", "car", "motorbike", "bus", "truck" };
        
        /*
        private static readonly string[] _classesNames = new string[] {
            "Car", "Motorcycle", "Truck", "Bus", "Bicycle" };
        */
        Trainer trainer;
        ITransformer trainedModel;
        Predictor predictor;
        public Yolo()
        {
            //Directory.CreateDirectory(_imageOutputFolder);
            trainer = new Trainer();

            Console.WriteLine("Build and train YOLO V4 model...");
            trainedModel = trainer.BuildAndTrain(_modelPath);

            Console.WriteLine("Create predictor...");
            predictor = new Predictor(trainedModel);

            tracker = new Helpers.Tracker();
        }
        public async Task<Bitmap> Detect(Bitmap image,Rectangle selectRect)
        {
            Console.WriteLine("Run predictions on images...");

            var predict = predictor.Predict(image);
            var results = predict.GetResults(_classesNames,filter);
            tracker.Process(results, selectRect);
            var bmp = DrawResults.Draw(results, image,tracker);
            return bmp;

        }

        public DataTable GetLogData()
        {
            return tracker.GetLogTable();
        }

        public void SaveLog()
        {
            tracker.SaveToLog();
        }
        public void Dispose()
        {
        }
    }

}

