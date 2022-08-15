using CarCounter.UWP.Data;
using CarCounter.UWP.Samples;
using Microsoft.AI.MachineLearning;
using Microsoft.AI.Skills.SkillInterface;
using Microsoft.AI.Skills.Vision.ObjectDetector;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        ILogger<Model5> _logger;
        Tracker tracker;
        public int DeviceIndex { get; set; }
        bool initialized_ = false;
        ObjectDetectorDescriptor descriptor;
        ObjectDetectorSkill skill; // If you don't specify an ISkillExecutionDevice, a default will be automatically selected
        ObjectDetectorBinding binding;
        double ImageWidth;
        double ImageHeight;

        public Model5()
        {
            _logger = DI.Pool.GetService<ILoggerFactory>()
                .CreateLogger<Model5>();
            this.tracker = new Tracker();
        }

        public void SetFilter(params ObjectKind[] objects)
        {
            Filter.Clear();
            foreach (var item in objects)
            {
                Filter.Add(item);
            }
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
            _logger.LogInformation("Commencing InitModel");
            this.DeviceIndex = DeviceIndex;
            this.ImageWidth = ImageWidth;
            this.ImageHeight = ImageHeight;

            //ISkillExecutionDevice device;
            descriptor = new ObjectDetectorDescriptor();
            var devices = (await descriptor.GetSupportedExecutionDevicesAsync()).ToList();
            var selectedDevice = devices[DeviceIndex];
            skill = await descriptor.CreateSkillAsync(selectedDevice) as ObjectDetectorSkill; // If you don't specify an ISkillExecutionDevice, a default will be automatically selected
            binding = await skill.CreateSkillBindingAsync() as ObjectDetectorBinding;

            initialized_ = true;
            _logger.LogInformation("InitModel succeed");
        }

        HashSet<ObjectKind> Filter = new HashSet<ObjectKind> { };
        //
        public async Task<List<ObjectDetectorResult>> EvaluateFrame(VideoFrame frame, Rectangle selectRect)
        {
            _logger.LogInformation("Commencing evaluate frame");
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
                    foreach (var item in results)
                    {
                        results2.Add(new Result(new float[] { (float)(item.Rect.X * ImageWidth), (float)(item.Rect.Y * ImageHeight), (float)((item.Rect.X + item.Rect.Width) * ImageWidth), (float)((item.Rect.Y + item.Rect.Height) * ImageHeight) }, item.Kind.ToString(), item.Confidence));
                    }
                    tracker.Process(results2, selectRect);
                }
                //var bmp = DrawResults.Draw(results, image, tracker);
                //return bmp;
                _logger.LogInformation("Evaluate frame succeed");
                return results.ToList();
                //RenderImageInMainPanel(softwareBitmap);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Evaluate frame failed");
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
