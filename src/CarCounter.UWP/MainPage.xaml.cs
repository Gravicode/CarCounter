using CarCounter.UWP.Helpers;
using CarCounter.UWP.Samples;
using Emgu.CV;
using Microsoft.AI.MachineLearning;
using Nager.VideoStream;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Path = System.IO.Path;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CarCounter.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer _timer;
        private readonly SolidColorBrush _fill_brush = new SolidColorBrush(Colors.Transparent);
        private readonly SolidColorBrush _line_brush = new SolidColorBrush(Colors.DarkGreen);
        private readonly double _line_thickness = 2.0;

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

        public MainPage()
        {
            this.InitializeComponent();
            button_go.IsEnabled = false;
            this.Loaded += OnPageLoaded;
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            _ = InitModelAsync();
            //_ = InitCameraAsync();
        }

        private async Task InitModelAsync()
        {
            ShowStatus("Loading yolo.onnx model...");
            try
            {
                dmlDevice = new LearningModelDevice(LearningModelDeviceKind.DirectX);
                cpuDevice = new LearningModelDevice(LearningModelDeviceKind.Cpu);

                var modelName = "yolov4.onnx";
                var modelPath = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "Assets", modelName);
                var model = LearningModel.LoadFromFilePath(modelPath);
                _session = CreateLearningModelSession(model);

                initialized_ = true;
                ShowStatus("ready");
                button_go.IsEnabled = true;
            }
            catch (Exception ex)
            {
                ShowStatus(ex.Message);
            }
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


        private bool processing;
        private Stopwatch watch;
        private int count;

        private async Task ProcessFrame()
        {
            if (processing)
            {
                // if we can't keep up to 30 fps, then ignore this tick.
                return;
            }
            try
            {
                if (watch == null)
                {
                    watch = new Stopwatch();
                    watch.Start();
                }

                processing = true;
                if (currentFile != null)
                {
                    //var frame = current;//new VideoFrame(Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8, (int)WebCam.Width, (int)WebCam.Height);

                    var softwareBitmap = await CreateSoftwareBitmapFromStorageFile(currentFile);

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
                    var stream = await currentFile.OpenAsync(FileAccessMode.Read);
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

                    //RenderImageInMainPanel(softwareBitmap);

                    await DrawBoxes(final, current);
                    count++;
                    if (watch.ElapsedMilliseconds > 1000)
                    {
                        ShowStatus(string.Format("{0} fps", count));
                        count = 0;
                        watch.Restart();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                processing = false;
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
            //InputImage.Source = source;
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
        private float Sigmoid(float val)
        {
            var x = (float)Math.Exp(val);
            return x / (1.0f + x);
        }

        // draw bounding boxes on the output frame based on evaluation result
        private async Task DrawBoxes(List<DetectionResult> detections, VideoFrame frame)
        {
            this.OverlayCanvas.Children.Clear();
            for (int i = 0; i < detections.Count; ++i)
            {
                int top = (int)(detections[i].bbox[0] * WebCam.Height);
                int left = (int)(detections[i].bbox[1] * WebCam.Width);
                int bottom = (int)(detections[i].bbox[2] * WebCam.Height);
                int right = (int)(detections[i].bbox[3] * WebCam.Width);

                var brush = new ImageBrush();
                var bitmap_source = new SoftwareBitmapSource();
                await bitmap_source.SetBitmapAsync(frame.SoftwareBitmap);

                brush.ImageSource = bitmap_source;
                // brush.Stretch = Stretch.Fill;

                this.OverlayCanvas.Background = brush;

                var r = new Rectangle();
                r.Tag = i;
                r.Width = right - left;
                r.Height = bottom - top;
                r.Fill = this._fill_brush;
                r.Stroke = this._line_brush;
                r.StrokeThickness = this._line_thickness;
                r.Margin = new Thickness(left, top, 0, 0);

                this.OverlayCanvas.Children.Add(r);
                // Default configuration for border
                // Render text label


                var border = new Border();
                var backgroundColorBrush = new SolidColorBrush(Colors.Black);
                var foregroundColorBrush = new SolidColorBrush(Colors.SpringGreen);
                var textBlock = new TextBlock();
                textBlock.Foreground = foregroundColorBrush;
                textBlock.FontSize = 18;

                textBlock.Text = detections[i].label;
                // Hide
                textBlock.Visibility = Visibility.Collapsed;
                border.Background = backgroundColorBrush;
                border.Child = textBlock;

                Canvas.SetLeft(border, detections[i].bbox[1] * 416 + 2);
                Canvas.SetTop(border, detections[i].bbox[0] * 416 + 2);
                textBlock.Visibility = Visibility.Visible;
                // Add to canvas
                this.OverlayCanvas.Children.Add(border);
            }
        }
        StorageFile currentFile;
        private async void button_go_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Downloads;
            //picker.FileTypeFilter.Add(".mp4");
            //picker.FileTypeFilter.Add(".avi");
            //picker.FileTypeFilter.Add(".mov");
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");

            currentFile = await picker.PickSingleFileAsync();
            if (currentFile != null)
            {
                // Application now has read/write access to the picked file
                //this.textBlock.Text = "Picked photo: " + file.Name;
                AppConstants.Cctv1 = currentFile.Path;
                var stream = await currentFile.OpenAsync(Windows.Storage.FileAccessMode.Read);
                var image = new BitmapImage();
                image.SetSource(stream);

                ImageEncodingProperties properties = ImageEncodingProperties.CreateJpeg();

                var decoder = await BitmapDecoder.CreateAsync(stream);
                var outputBitmap = await decoder.GetSoftwareBitmapAsync();

                if (outputBitmap != null)
                {

                    //SoftwareBitmap outputBitmap = SoftwareBitmap.CreateCopyFromBuffer(data.AsBuffer(), BitmapPixelFormat.Bgra8, bmp.PixelWidth, bmp.PixelHeight, BitmapAlphaMode.Premultiplied);
                    var currentImage = SoftwareBitmap.Convert(outputBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                    VideoFrame frame = VideoFrame.CreateWithSoftwareBitmap(currentImage);
                    current = frame;

                }
            }
            else
            {
                //this.textBlock.Text = "Operation cancelled.";
            }


            if (_timer == null)
            {
                // now start processing frames, no need to do more than 30 per second!
                _timer = new DispatcherTimer()
                {
                    Interval = TimeSpan.FromMilliseconds(30)
                };
                _timer.Tick += OnTimerTick;
                _timer.Start();
            }
        }

        void ShowStatus(string text)
        {
            textblock_status.Text = text;
        }


        private void OnTimerTick(object sender, object e)
        {
            // don't wait for this async task to finish
            _ = ProcessFrame();
        }
        int DelayTime = 20;
        System.Drawing.Rectangle SelectionArea = new System.Drawing.Rectangle(0, 0, 0, 0);
        bool IsCapturing = false;
        VideoFrame current;
        async void Capture(CancellationToken token)
        {
            System.Drawing.Rectangle selectRect = new System.Drawing.Rectangle();
            if (IsCapturing) return;
            var capture = !string.IsNullOrEmpty(AppConstants.Cctv1) ? new Emgu.CV.VideoCapture(AppConstants.Cctv1) : new Emgu.CV.VideoCapture();
            IsCapturing = true;
            while (true)
            {
                using (var nextFrame = capture.QueryFrame())
                {

                    if (nextFrame != null)
                    {
                        var img = nextFrame.ToBitmap();
                        if (img != null)
                        {
                            // enable resize
                            //Mat resize = new Mat();
                            //CvInvoke.Resize(nextFrame, resize, new Size(480, 480), 0, 0, Inter.Linear);
                            /*
                            if (SelectionArea.Width > 0 && SelectionArea.Height > 0)
                            {
                                //cropping sesuai selection area
                                var ratioX = (double)SelectionArea.X / ImgWidth;
                                var ratioY = (double)SelectionArea.Y / ImgHeight;
                                var ratioWidth = (double)SelectionArea.Width / ImgWidth;
                                var ratioHeight = (double)SelectionArea.Height / ImgHeight;

                                selectRect = new System.Drawing.Rectangle((int)(ratioX * nextFrame.Width), (int)(ratioY * nextFrame.Height), (int)(ratioWidth * nextFrame.Width), (int)(ratioHeight * nextFrame.Height));

                            }*/
                            var data = img.ToByteArray(ImageFormat.Jpeg);
                            SoftwareBitmap outputBitmap = null;
                            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                            {
                                await stream.WriteAsync(data.AsBuffer());
                                stream.Seek(0);
                                //await bmp.SetSourceAsync(stream);
                                //new
                                ImageEncodingProperties properties = ImageEncodingProperties.CreateJpeg();

                                var decoder = await BitmapDecoder.CreateAsync(stream);
                                outputBitmap = await decoder.GetSoftwareBitmapAsync();
                            }

                            if (outputBitmap != null)
                            {

                                //SoftwareBitmap outputBitmap = SoftwareBitmap.CreateCopyFromBuffer(data.AsBuffer(), BitmapPixelFormat.Bgra8, bmp.PixelWidth, bmp.PixelHeight, BitmapAlphaMode.Premultiplied);
                                SoftwareBitmap displayableImage = SoftwareBitmap.Convert(outputBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                                VideoFrame frame = VideoFrame.CreateWithSoftwareBitmap(displayableImage);
                                //do evaluation
                                current = frame;
                                //await ExecuteFrame(frame, index);


                            }
                            //var bmp = await yolo.Detect(img, selectRect);
                            //var bmp = await yolo.Detect(resize.ToBitmap(), selectRect);

                            //this.pictureBox1?.Invoke((MethodInvoker)delegate
                            //{
                            //    // Running on the UI thread
                            //    pictureBox1.Image = bmp;
                            //});

                        }

                    }

                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                }
                Thread.Sleep(DelayTime);
            }
            IsCapturing = false;
        }
    }
}
