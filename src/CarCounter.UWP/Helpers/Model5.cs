using CarCounter.UWP.Samples;
using Microsoft.AI.MachineLearning;
using Microsoft.AI.Skills.SkillInterface;
using Microsoft.AI.Skills.Vision.ObjectDetector;
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
    internal class Model5
    {

        Tracker tracker;
      


        bool initialized_ = false;
        ObjectDetectorDescriptor descriptor;
        ObjectDetectorSkill skill ; // If you don't specify an ISkillExecutionDevice, a default will be automatically selected
        ObjectDetectorBinding binding ;
        double ImageWidth;
        double ImageHeight;

        public Model5()
        {

        }

        public Tracker GetTracker()
        {
            return tracker;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ImageWidth"></param>
        /// <param name="ImageHeight"></param>
        /// <param name="DeviceIndex">CPU = 0, GPU = 1</param>
        /// <returns></returns>
        public async Task InitModelAsync(double ImageWidth, double ImageHeight, int DeviceIndex = 0)
        {
            this.ImageWidth = ImageWidth;
            this.ImageHeight = ImageHeight;
            
            this.tracker = new Tracker();
            //ISkillExecutionDevice device;
            descriptor = new ObjectDetectorDescriptor();
            var devices =await descriptor.GetSupportedExecutionDevicesAsync();
            var first = devices.FirstOrDefault();
            skill = await descriptor.CreateSkillAsync(first) as ObjectDetectorSkill; // If you don't specify an ISkillExecutionDevice, a default will be automatically selected
            binding = await skill.CreateSkillBindingAsync() as ObjectDetectorBinding;


            initialized_ = true;
        }

        HashSet<ObjectKind> Filter = new HashSet<ObjectKind> {  ObjectKind.Motorbike, ObjectKind.Car, ObjectKind.Truck, ObjectKind.Bus };
        //
        public async Task<List<ObjectDetectorResult>> EvaluateFrame(VideoFrame frame, Rectangle selectRect)
        {

            try
            {
                // Bind
                await binding.SetInputImageAsync(frame);

                // Evaluate
                await skill.EvaluateAsync(binding);
                var results = binding.DetectedObjects;
                if (Filter?.Count > 0)
                {
                    results = results.Where(det => Filter.Contains(det.Kind)).ToList();
                }

                if (results.Count > 0)
                {
                    var results2 = new List<Result>();
                    foreach(var item in results)
                    {
                        results2.Add(new Result(new float[] { (float)(item.Rect.X * ImageWidth), (float)(item.Rect.Y * ImageHeight), (float)((item.Rect.X+item.Rect.Width) * ImageWidth), (float)((item.Rect.Y+item.Rect.Height) * ImageHeight) }, item.Kind.ToString(), item.Confidence));
                    }
                    tracker.Process(results2, selectRect);
                }
                //var bmp = DrawResults.Draw(results, image, tracker);
                //return bmp;
                return results.ToList();
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
