using CarCounter.UWP.Helpers;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CarCounter.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Model4 _model;
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
            //for cctv image grabber through http
            //this.Grabber = new ImageHttpHelper(AppConstants.Username,AppConstants.Password);
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
                #region model1
                _model = new Model4();
                await _model.InitModelAsync(WebCam.Width, WebCam.Height,1);
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
                if (frame != null)
                {
                    //var frame = current;//new VideoFrame(Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8, (int)WebCam.Width, (int)WebCam.Height);
                    var results = await _model.EvaluateFrame(frame,SelectionArea);
                    await DrawBoxes3(results, frame);
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

        private async Task DrawBoxes3(List<Result> detections, VideoFrame frame)
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
                int top = (int)(detections[i].BoundingBox[0] * WebCam.Height);
                int left = (int)(detections[i].BoundingBox[1] * WebCam.Width);
                int bottom = (int)(detections[i].BoundingBox[2] * WebCam.Height);
                int right = (int)(detections[i].BoundingBox[3] * WebCam.Width);

              

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

                Canvas.SetLeft(border, detections[i].BoundingBox[1] * 416 + 2);
                Canvas.SetTop(border, detections[i].BoundingBox[0] * 416 + 2);
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
                    frame = VideoFrame.CreateWithSoftwareBitmap(currentImage);
                    

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
                    //Interval = TimeSpan.FromMilliseconds(30)
                    Interval = TimeSpan.FromMilliseconds(1000)
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
        int DelayTime = 20;
        System.Drawing.Rectangle SelectionArea = new System.Drawing.Rectangle(0, 0, 0, 0);
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
