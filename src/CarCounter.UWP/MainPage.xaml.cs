using CarCounter.UWP.Helpers;
using Emgu.CV;
using Microsoft.AI.Skills.Vision.ObjectDetector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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
using Rectangle = Windows.UI.Xaml.Shapes.Rectangle;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CarCounter.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        PointF StartLocation;
        bool IsSelect = false;
        private MediaCapture _media_capture;
        private Model5 _model;
        private DispatcherTimer _timer;
        private readonly SolidColorBrush _fill_brush = new SolidColorBrush(Colors.Transparent);
        private readonly SolidColorBrush _line_brush = new SolidColorBrush(Colors.DarkGreen);
        private readonly double _line_thickness = 2.0;
        ImageHttpHelper Grabber;

        public MainPage()
        {
            this.InitializeComponent();
            button_go.IsEnabled = false;
            this.Loaded += OnPageLoaded;


            WebCam.PointerPressed += (s,e) =>
            {
                //if (e.Pointer.PointerDeviceType == MouseButtons.Left)
                {
                    IsSelect = true;
                    var pos = e.GetCurrentPoint(this.WebCam).Position;
                    StartLocation = new PointF ((float)pos.X,(float)pos.Y);
                }
            };
            WebCam.PointerMoved += (s,e) =>
            {
                if (IsSelect)
                {
                    var pos = e.GetCurrentPoint(this.WebCam).Position;
                    SelectionArea.X = (int)Math.Min(StartLocation.X, pos.X);
                    SelectionArea.Y = (int)Math.Min(StartLocation.Y, pos.Y);
                    SelectionArea.Width = (int)Math.Abs(StartLocation.X - pos.X);
                    SelectionArea.Height = (int)Math.Abs(StartLocation.Y - pos.Y);
                    RefreshSelection();
                }
            };
            WebCam.PointerReleased += (s,e) =>
            {
                if (IsSelect)
                {
                    IsSelect = false;
                    RefreshSelection();
                }
                /*
                else if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse && e.Pointer.  Button == MouseButtons.Right)
                {
                    SelectionArea.X = 0;
                    SelectionArea.Y = 0;
                    SelectionArea.Width = 0;
                    SelectionArea.Height = 0;
                }*/
            };

           

            //for cctv image grabber through http
            //this.Grabber = new ImageHttpHelper(AppConstants.Username,AppConstants.Password);
        }
        Rectangle selection;
        System.Drawing.Rectangle selectRect;
        void RefreshSelection()
        {
            if (selection == null)
            {
                OverlayCanvas2.Children.Clear();
                selection = new Rectangle();
                selection.Width = SelectionArea.Width;
                selection.Height = SelectionArea.Height;
                selection.Fill = new SolidColorBrush(Windows.UI.Colors.LightSkyBlue);
                selection.Stroke = new SolidColorBrush(Windows.UI.Colors.DarkBlue);
                selection.Opacity = 0.3;
                selection.StrokeThickness = this._line_thickness;
                Canvas.SetLeft(selection, SelectionArea.X);
                Canvas.SetTop(selection, SelectionArea.Y);
                this.OverlayCanvas2.Children.Add(selection);
            }else
            if (SelectionArea.Width > 0 && SelectionArea.Height > 0)
            {
                selection.Width = SelectionArea.Width;
                selection.Height = SelectionArea.Height;
                Canvas.SetLeft(selection, SelectionArea.X);
                Canvas.SetTop(selection, SelectionArea.Y);

            }
        }
        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            _ = InitModelAsync();
            _ = InitCameraAsync();
        }
        private async Task InitCameraAsync()
        {
            if (_media_capture == null || _media_capture.CameraStreamState == Windows.Media.Devices.CameraStreamState.Shutdown || _media_capture.CameraStreamState == Windows.Media.Devices.CameraStreamState.NotStreaming)
            {
                if (_media_capture != null)
                {
                    _media_capture.Dispose();
                }

                MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings();
                var cameras = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
                var camera = cameras.FirstOrDefault();
                settings.VideoDeviceId = camera.Id;

                _media_capture = new MediaCapture();
                await _media_capture.InitializeAsync(settings);
                WebCam.Source = _media_capture;
            }

            if (_media_capture.CameraStreamState == Windows.Media.Devices.CameraStreamState.NotStreaming)
            {
                await _media_capture.StartPreviewAsync();
                WebCam.Visibility = Visibility.Visible;
            }
        }

        double ImgWidth, ImgHeight;
        private async Task InitModelAsync()
        {
            ShowStatus("Loading yolo.onnx model...");
            try
            {
                #region model1
                _model = new Model5();
                
                ImgWidth = WebCam.Width;
                ImgHeight = WebCam.Height;
                
                await _model.InitModelAsync(ImgWidth, ImgHeight);
                #endregion
                ShowStatus("ready");
                button_go.IsEnabled = true;
            }
            catch (Exception ex)
            {
                ShowStatus(ex.Message);
            }
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

                if (SelectionArea.Width > 0 && SelectionArea.Height > 0)
                {
                    //cropping sesuai selection area
                    var ratioX = (double)SelectionArea.X / ImgWidth;
                    var ratioY = (double)SelectionArea.Y / ImgHeight;
                    var ratioWidth = (double)SelectionArea.Width / ImgWidth;
                    var ratioHeight = (double)SelectionArea.Height / ImgHeight;

                    selectRect = new System.Drawing.Rectangle((int)(ratioX * ImgWidth), (int)(ratioY * ImgHeight), (int)(ratioWidth * ImgWidth), (int)(ratioHeight * ImgHeight));

                }
                //if (frame != null)
                {
                    frame = new VideoFrame(Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8, (int)ImgWidth, (int)ImgHeight);
                    await _media_capture.GetPreviewFrameAsync(frame);

                    var results = await _model.EvaluateFrame(frame, selectRect);
                    
                    await DrawBoxes5(results, frame, _model.GetTracker());
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



        // draw bounding boxes on the output frame based on evaluation result
        private async Task DrawBoxes(List<CarCounter.UWP.Helpers.Model.DetectionResult> detections, VideoFrame frame)
        {
            this.OverlayCanvas.Children.Clear();
            for (int i = 0; i < detections.Count; ++i)
            {
                int top = (int)(detections[i].bbox[0] * ImgHeight);
                int left = (int)(detections[i].bbox[1] * ImgWidth);
                int bottom = (int)(detections[i].bbox[2] * ImgHeight);
                int right = (int)(detections[i].bbox[3] * ImgWidth);

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

        private async Task DrawBoxes4(List<Result> detections, VideoFrame frame)
        {
            this.OverlayCanvas.Children.Clear();
            var brush = new ImageBrush();
            var bitmap_source = new SoftwareBitmapSource();
            await bitmap_source.SetBitmapAsync(frame.SoftwareBitmap);

            brush.ImageSource = bitmap_source;
            // brush.Stretch = Stretch.Fill;

            //this.OverlayCanvas.Background = brush;
            for (int i = 0; i < detections.Count; ++i)
            {
                int top = (int)(detections[i].BoundingBox[0]);
                int left = (int)(detections[i].BoundingBox[1]);
                int bottom = (int)(detections[i].BoundingBox[2]);
                int right = (int)(detections[i].BoundingBox[3]);


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

                textBlock.Text = detections[i].Label;
                // Hide
                textBlock.Visibility = Visibility.Collapsed;
                border.Background = backgroundColorBrush;
                border.Child = textBlock;

                Canvas.SetLeft(border, detections[i].BoundingBox[1]);
                Canvas.SetTop(border, detections[i].BoundingBox[0]);
                textBlock.Visibility = Visibility.Visible;
                // Add to canvas
                this.OverlayCanvas.Children.Add(border);
            }
        } 
        
        private async Task DrawBoxes5(List<ObjectDetectorResult> detections, VideoFrame frame, Helpers.Tracker tracker)
        {
            this.OverlayCanvas.Children.Clear();
            var brush = new ImageBrush();
            var bitmap_source = new SoftwareBitmapSource();
            await bitmap_source.SetBitmapAsync(frame.SoftwareBitmap);

            brush.ImageSource = bitmap_source;

            for (int i = 0; i < detections.Count; ++i)
            {
                int top = (int)(detections[i].Rect.Top * ImgHeight);
                int left = (int)(detections[i].Rect.Left * ImgWidth);
                int bottom = (int)(detections[i].Rect.Bottom * ImgHeight);
                int right = (int)(detections[i].Rect.Right * ImgWidth);

                

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

                textBlock.Text = detections[i].Kind.ToString();
                // Hide
                textBlock.Visibility = Visibility.Collapsed;
                border.Background = backgroundColorBrush;
                border.Child = textBlock;

                Canvas.SetLeft(border, detections[i].Rect.Left * ImgWidth );
                Canvas.SetTop(border, detections[i].Rect.Top * ImgHeight );
                textBlock.Visibility = Visibility.Visible;
                // Add to canvas
                this.OverlayCanvas.Children.Add(border);
            }

            if (tracker.TrackedList != null)
            {
                foreach (var target in tracker.TrackedList)
                {  
                    var p1 = new PointF(target.Location.X, target.Location.Y);
                    for (int i = target.Trails.Count - 1; i > 0; i--)
                    {
                        var p2f = target.Trails[i];
                        var p2 = new PointF(p2f.X, p2f.Y);
                        var line = new Line();
                        line.X1 = p1.X;
                        line.Y1 = p1.Y;
                        line.X2 = p2.X;
                        line.Y2 = p2.Y;
                        var fill = new SolidColorBrush(Windows.UI.Color.FromArgb(180, target.Col.R, target.Col.G, target.Col.B));
                        line.Fill = fill;
                        line.Stroke = fill;
                        line.StrokeThickness = this._line_thickness;
                        OverlayCanvas.Children.Add(line);
                        p1 = p2;
                    }
                }
            }
        }
        StorageFile currentFile;
        private async void button_go_Click(object sender, RoutedEventArgs e)
        {
            /*
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
                    frame = VideoFrame.CreateWithSoftwareBitmap(currentImage);
                    

                }
            }
            else
            {
                //this.textBlock.Text = "Operation cancelled.";
            }
            */

            if (_timer == null)
            {
                // now start processing frames, no need to do more than 30 per second!
                _timer = new DispatcherTimer()
                {
                    
                    Interval = TimeSpan.FromMilliseconds(DelayTime)
                };
                _timer.Tick += OnTimerTick;
                _timer.Start();
            }
        }

        void ShowStatus(string text)
        {
            textblock_status.Text = text;
        }

        async Task GrabImageFromHttp()
        {
            this.frame = await this.Grabber.GetFrameFromHttp(AppConstants.CctvHttp);
        }
        private async void OnTimerTick(object sender, object e)
        {
            //grab from http
            //await GrabImageFromHttp();
            // don't wait for this async task to finish

            _ = ProcessFrame();
        }
        #region capture from RTSP/HTTP
        int DelayTime = 30;
        System.Drawing.Rectangle SelectionArea = new System.Drawing.Rectangle(0,0,0,0);
        bool IsCapturing = false;
        VideoFrame frame;
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
                                frame = VideoFrame.CreateWithSoftwareBitmap(displayableImage);

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
        #endregion
    }
}
