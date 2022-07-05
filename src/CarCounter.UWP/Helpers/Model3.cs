using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media;
using System.Data;
using System.Drawing;
using Windows.Storage;
using Windows.Graphics.Imaging;
using System;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

namespace CarCounter.UWP.Helpers
{
    public class Model3
    {
        #region helper
        private async Task<byte[]> GetBytes(SoftwareBitmap softwareBitmap)
        {
            // get encoded jpeg bytes
            var data = await EncodedBytes(softwareBitmap, BitmapEncoder.JpegEncoderId);
            return data;
            // todo: save the bytes to a DB, etc
        }

        private async Task<byte[]> EncodedBytes(SoftwareBitmap soft, Guid encoderId)
        {
            byte[] array = null;

            // First: Use an encoder to copy from SoftwareBitmap to an in-mem stream (FlushAsync)
            // Next:  Use ReadAsync on the in-mem stream to get byte[] array

            using (var ms = new InMemoryRandomAccessStream())
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(encoderId, ms);
                encoder.SetSoftwareBitmap(soft);

                try
                {
                    await encoder.FlushAsync();
                }
                catch (Exception ex) { return new byte[0]; }

                array = new byte[ms.Size];
                await ms.ReadAsync(array.AsBuffer(), (uint)ms.Size, InputStreamOptions.None);
            }
            return array;
        }
        #endregion

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

        public yolov4Model Model { get; set; }
        public ImagePrediction Prediction { get; set; }

        public Model3()
        {
            
        }

        public async Task InitModelAsync(double ImageWidth, double ImageHeight)
        {
            this.tracker = new Tracker();
            Model = await yolov4Model.CreateFromAsset();
            this.Prediction = new ImagePrediction();
            this.Prediction.ImageHeight = (float)ImageHeight;
            this.Prediction.ImageWidth = (float)ImageWidth;
        }

       
        private static async Task<SoftwareBitmap> CreateSoftwareBitmapFromStorageFile(StorageFile file)
        {
            var stream = await file.OpenAsync(FileAccessMode.Read);
            var decoder = await BitmapDecoder.CreateAsync(stream);
            return await decoder.GetSoftwareBitmapAsync();
        }
        public async Task<List<Result>> EvaluateFrame(VideoFrame frame, Rectangle selectRect)
        {
            
            var output = await Model.EvaluateAsync(frame);
            Prediction.Identity = output.Identity00.GetAsVectorView().ToArray();
            Prediction.Identity1 = output.Identity_100.GetAsVectorView().ToArray();
            Prediction.Identity2 = output.Identity_200.GetAsVectorView().ToArray();
            var results = Prediction.GetResults(_classesNames, filter);
            tracker.Process(results, selectRect);
            //var bmp = DrawResults.Draw(results, image, tracker);
            //return bmp;
            return results.ToList();
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
