﻿using CarCounter.UWP.Common;
using CarCounter.UWP.Samples;
using Microsoft.AI.MachineLearning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CarCounter.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        private static extern void GetSystemTimePreciseAsFileTime(out long filetime);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto, PreserveSig = true, SetLastError = false)]
        public static extern IntPtr GetActiveWindow();

        LearningModelSession _session = null;
        LearningModelSession tensorizationSession_ = null;

        LearningModelDevice dmlDevice;
        LearningModelDevice cpuDevice;

        bool initialized_ = false;


        private readonly string[] _labels =
            {
                "person",
                "bicycle",
                "car",
                "motorbike",
                "aeroplane",
                "bus",
                "train",
                "truck",
                "boat",
                "traffic light",
                "fire hydrant",
                "stop sign",
                "parking meter",
                "bench",
                "bird",
                "cat",
                "dog",
                "horse",
                "sheep",
                "cow",
                "elephant",
                "bear",
                "zebra",
                "giraffe",
                "backpack",
                "umbrella",
                "handbag",
                "tie",
                "suitcase",
                "frisbee",
                "skis",
                "snowboard",
                "sports ball",
                "kite",
                "baseball bat",
                "baseball glove",
                "skateboard",
                "surfboard",
                "tennis racket",
                "bottle",
                "wine glass",
                "cup",
                "fork",
                "knife",
                "spoon",
                "bowl",
                "banana",
                "apple",
                "sandwich",
                "orange",
                "broccoli",
                "carrot",
                "hot dog",
                "pizza",
                "donut",
                "cake",
                "chair",
                "sofa",
                "pottedplant",
                "bed",
                "diningtable",
                "toilet",
                "tvmonitor",
                "laptop",
                "mouse",
                "remote",
                "keyboard",
                "cell phone",
                "microwave",
                "oven",
                "toaster",
                "sink",
                "refrigerator",
                "book",
                "clock",
                "vase",
                "scissors",
                "teddy bear",
                "hair drier",
                "toothbrush"
        };

        internal struct DetectionResult
        {
            public string label;
            public List<float> bbox;
            public double prob;
        }

        class Comparer : IComparer<DetectionResult>
        {
            public int Compare(DetectionResult x, DetectionResult y)
            {
                return y.prob.CompareTo(x.prob);
            }
        }



        private bool IsCpu
        {
            get
            {
                return DeviceComboBox.SelectedIndex == 0;
            }
        }

        public MainPage()
        {
            this.InitializeComponent();

            dmlDevice = new LearningModelDevice(LearningModelDeviceKind.DirectX);
            cpuDevice = new LearningModelDevice(LearningModelDeviceKind.Cpu);

            var modelName = "yolov4.onnx";
            var modelPath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "Assets", modelName);
            var model = LearningModel.LoadFromFilePath(modelPath);
            _session = CreateLearningModelSession(model);

            initialized_ = true;

            
        }

     

        private LearningModelSession CreateLearningModelSession(LearningModel model)
        {
            var kind =
                (DeviceComboBox.SelectedIndex == 0) ?
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

        private void DeviceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        
        private async void OpenButton_Clicked(object sender, RoutedEventArgs e)
        {
            var file = ImageHelper.PickImageFiles();
            if (file != null)
            {
                var currentImage = await CreateSoftwareBitmapFromStorageFile(file);
                RenderImageInMainPanel(currentImage);
                BasicGridView.SelectedItem = null;
            }
        }

        private async void SampleInputsGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var gridView = sender as GridView;
            var thumbnail = gridView.SelectedItem as CarCounter.UWP.Controls.Thumbnail;
            if (thumbnail != null)
            {
                var image = thumbnail.ImageUri;
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(image));
                var softwareBitmap = await CreateSoftwareBitmapFromStorageFile(file);


                tensorizationSession_ =
                    CreateLearningModelSession(
                        TensorizationModels.ReshapeFlatBufferNHWC(
                            1,
                            4,
                            softwareBitmap.PixelHeight,
                            softwareBitmap.PixelWidth,
                            416,
                            416));


                // Tensorize
                var stream = await file.OpenAsync(FileAccessMode.Read);
                var decoder = await BitmapDecoder.CreateAsync(stream);
                var bitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                var pixelDataProvider = await decoder.GetPixelDataAsync();
                var bytes = pixelDataProvider.DetachPixelData();
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

                var key = results.Outputs.Keys.First();
                var output1 = results.Outputs[key] as TensorFloat;

                var data = output1.GetAsVectorView();
                var detections = ParseResult(data.ToList<float>().ToArray());

                Comparer cp = new Comparer();
                detections.Sort(cp);
                var final = NMS(detections);

                RenderImageInMainPanel(softwareBitmap);
            }
        }

        private static async Task<SoftwareBitmap> CreateSoftwareBitmapFromStorageFile(StorageFile file)
        {
            var stream = await file.OpenAsync(FileAccessMode.Read);
            var decoder = await BitmapDecoder.CreateAsync(stream);
            return await decoder.GetSoftwareBitmapAsync();
        }

        private void RenderImageInMainPanel(SoftwareBitmap softwareBitmap)
        {
            SoftwareBitmap displayBitmap = softwareBitmap;
            //Image control only accepts BGRA8 encoding and Premultiplied/no alpha channel. This checks and converts
            //the SoftwareBitmap we want to bind.
            if (displayBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8 ||
                displayBitmap.BitmapAlphaMode != BitmapAlphaMode.Premultiplied)
            {
                displayBitmap = SoftwareBitmap.Convert(displayBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
            }

            // get software bitmap souce
            var source = new SoftwareBitmapSource();
            source.SetBitmapAsync(displayBitmap).GetAwaiter();
            // draw the input image
            InputImage.Source = source;
        }


        // parse the result from WinML evaluation results to self defined object struct
        private List<DetectionResult> ParseResult(float[] results)
        {
            int c_values = 84;
            int c_boxes = results.Length / c_values;
            float confidence_threshold = 0.5f;
            List<DetectionResult> detections = new List<DetectionResult>();
            for (int i_box = 0; i_box < c_boxes; i_box++)
            {
                float max_prob = 0.0f;
                int label_index = -1;
                for (int j_confidence = 4; j_confidence < c_values; j_confidence++)
                {
                    int index = i_box * c_values + j_confidence;
                    if (results[index] > max_prob)
                    {
                        max_prob = results[index];
                        label_index = j_confidence - 4;
                    }
                }
                if (max_prob > confidence_threshold)
                {
                    List<float> bbox = new List<float>();
                    bbox.Add(results[i_box * c_values + 0]);
                    bbox.Add(results[i_box * c_values + 1]);
                    bbox.Add(results[i_box * c_values + 2]);
                    bbox.Add(results[i_box * c_values + 3]);

                    detections.Add(new DetectionResult()
                    {
                        label = _labels[label_index],
                        bbox = bbox,
                        prob = max_prob
                    });
                }
            }
            return detections;
        }

        // Non-maximum Suppression(NMS), a technique which filters the proposals 
        // based on Intersection over Union(IOU)
        private List<DetectionResult> NMS(IReadOnlyList<DetectionResult> detections,
            float IOU_threshold = 0.45f,
            float score_threshold = 0.3f)
        {
            List<DetectionResult> final_detections = new List<DetectionResult>();
            for (int i = 0; i < detections.Count; i++)
            {
                int j = 0;
                for (j = 0; j < final_detections.Count; j++)
                {
                    if (ComputeIOU(final_detections[j], detections[i]) > IOU_threshold)
                    {
                        break;
                    }
                }
                if (j == final_detections.Count)
                {
                    final_detections.Add(detections[i]);
                }
            }
            return final_detections;
        }

        // Compute Intersection over Union(IOU)
        private float ComputeIOU(DetectionResult DRa, DetectionResult DRb)
        {
            float ay1 = DRa.bbox[0];
            float ax1 = DRa.bbox[1];
            float ay2 = DRa.bbox[2];
            float ax2 = DRa.bbox[3];
            float by1 = DRb.bbox[0];
            float bx1 = DRb.bbox[1];
            float by2 = DRb.bbox[2];
            float bx2 = DRb.bbox[3];

            //Debug.Assert(ay1 < ay2);
            //Debug.Assert(ax1 < ax2);
            //Debug.Assert(by1 < by2);
            //Debug.Assert(bx1 < bx2);

            // determine the coordinates of the intersection rectangle
            float x_left = Math.Max(ax1, bx1);
            float y_top = Math.Max(ay1, by1);
            float x_right = Math.Min(ax2, bx2);
            float y_bottom = Math.Min(ay2, by2);

            if (x_right < x_left || y_bottom < y_top)
                return 0;
            float intersection_area = (x_right - x_left) * (y_bottom - y_top);
            float bb1_area = (ax2 - ax1) * (ay2 - ay1);
            float bb2_area = (bx2 - bx1) * (by2 - by1);
            float iou = intersection_area / (bb1_area + bb2_area - intersection_area);

            //Debug.Assert(iou >= 0 && iou <= 1);
            return iou;
        }
    }
}
