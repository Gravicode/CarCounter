using CarCounter.Models;
using CarCounter.Tools;
using CarCounter.UWP.Helpers;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AI.Skills.Vision.ObjectDetector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.Capture;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Rectangle = Windows.UI.Xaml.Shapes.Rectangle;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CarCounter.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CctvPage : Page, IDisposable
    {
        public enum StreamSourceTypes { WebCam, RTSP, HttpImage }
        StreamSourceTypes Mode = StreamSourceTypes.RTSP;

        PointF StartLocation;
        bool IsSelect = false;
        private MediaCapture _media_capture;
        private Model5 _model;
        private DispatcherTimer _timer;
        private DispatcherTimer PushTimer;
        private DispatcherTimer StatTimer;
        private readonly SolidColorBrush _fill_brush = new SolidColorBrush(Colors.Transparent);
        private readonly SolidColorBrush _line_brush = new SolidColorBrush(Colors.DarkGreen);
        private readonly double _line_thickness = 2.0;
        ImageHttpHelper Grabber;
        int DelayTime = 30;
        System.Drawing.Rectangle SelectionArea = new System.Drawing.Rectangle(0, 0, 0, 0);
        VideoFrame frame;
        Rectangle selection;
        System.Drawing.Rectangle selectRect;
        bool IsTracking = false;
        //RtspHelper rtsp;
        private bool processing;
        private Stopwatch watch;
        private int count;
        double ImgWidth, ImgHeight;
        DataCounterService dataCounterService;

        public CctvPage()
        {
            this.InitializeComponent();

            LoadConfig();
            SetupGrpc();
            SetupComponents();
        }

        void SetupGrpc()
        {
            var channel = GrpcChannel.ForAddress(
              AppConstants.GrpcUrl, new GrpcChannelOptions
              {
                  MaxReceiveMessageSize = 8 * 1024 * 1024, // 5 MB
                  MaxSendMessageSize = 8 * 1024 * 1024, // 2 MB                
                  HttpHandler = new GrpcWebHandler(new HttpClientHandler())
              });
            ObjectContainer.Register<GrpcChannel>(channel);
            ObjectContainer.Register<DataCounterService>(new DataCounterService(channel));
            ObjectContainer.Register<CCTVService>(new CCTVService(channel));
            ObjectContainer.Register<GatewayService>(new GatewayService(channel));
        }
        void LoadConfig()
        {
            AppConfig.Load();
            if (!string.IsNullOrEmpty(AppConstants.SelectionArea))
            {
                SelectionArea = JsonSerializer.Deserialize<System.Drawing.Rectangle>(AppConstants.SelectionArea);
                RefreshSelection();
            }

        }
        void SetupComponents()
        {
            button_go.IsEnabled = false;
            this.Loaded += OnPageLoaded;
            this.Unloaded += (a, b) => { AppConfig.Save(); };

            OverlayCanvas.PointerPressed += (s, e) =>
            {
                PointerPoint ptrPt = e.GetCurrentPoint(this.OverlayCanvas);

                if (ptrPt.Properties.IsLeftButtonPressed)
                {
                    IsSelect = true;
                    var pos = e.GetCurrentPoint(this.OverlayCanvas).Position;
                    StartLocation = new PointF((float)pos.X, (float)pos.Y);
                }
            };
            OverlayCanvas.PointerMoved += (s, e) =>
            {
                if (IsSelect)
                {
                    var pos = e.GetCurrentPoint(this.OverlayCanvas).Position;
                    SelectionArea.X = (int)Math.Min(StartLocation.X, pos.X);
                    SelectionArea.Y = (int)Math.Min(StartLocation.Y, pos.Y);
                    SelectionArea.Width = (int)Math.Abs(StartLocation.X - pos.X);
                    SelectionArea.Height = (int)Math.Abs(StartLocation.Y - pos.Y);
                    RefreshSelection();
                }
            };
            OverlayCanvas.PointerReleased += (s, e) =>
            {
                PointerPoint ptrPt = e.GetCurrentPoint(this.OverlayCanvas);
                if (IsSelect)
                {
                    IsSelect = false;
                    RefreshSelection();
                }
                /*
                else if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
                {
                    SelectionArea.X = 0;
                    SelectionArea.Y = 0;
                    SelectionArea.Width = 0;
                    SelectionArea.Height = 0;
                }*/
            };
            dataCounterService = ObjectContainer.Get<DataCounterService>();
            if (PushTimer == null)
            {
                // set per menit
                PushTimer = new DispatcherTimer()
                {

                    Interval = TimeSpan.FromMilliseconds(60 * 1000)
                };
                PushTimer.Tick += PushTimerTick;
                PushTimer.Start();

            }
            if (StatTimer == null)
            {
                // set per menit
                StatTimer = new DispatcherTimer()
                {

                    Interval = TimeSpan.FromMilliseconds(2000)
                };
                StatTimer.Tick += StatTimerTick;
                StatTimer.Start();

            }
            ChkAutoStart.IsChecked = AppConstants.AutoStart;
        }
        private void StatTimerTick(object sender, object e)
        {
            if (_model != null)
            {
                var tracker = _model.GetTracker();
                var stat = tracker.GetStatTable();
                Grid1.ItemsSource = stat;
            }
        }
        public System.Collections.ObjectModel.ObservableCollection<dynamic> ToDynamic(DataTable dt)
        {
            var dynamicDt = new System.Collections.ObjectModel.ObservableCollection<dynamic>();
            foreach (System.Data.DataRow row in dt.Rows)
            {
                dynamic dyn = new System.Dynamic.ExpandoObject();
                dynamicDt.Add(dyn);
                //Converting the DataTable collcetion in Dynamic collection    
                foreach (System.Data.DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            return dynamicDt;
        }


        private async void PushTimerTick(object sender, object e)
        {
            if (ChkAutoPush.IsChecked.Value)
                await SyncToCloud();
        }

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
            }
            else
            if (SelectionArea.Width > 0 && SelectionArea.Height > 0)
            {
                selection.Width = SelectionArea.Width;
                selection.Height = SelectionArea.Height;
                Canvas.SetLeft(selection, SelectionArea.X);
                Canvas.SetTop(selection, SelectionArea.Y);

            }
            AppConstants.SelectionArea = JsonSerializer.Serialize(SelectionArea);
        }

        async Task<VideoFrame> Capture(UIElement element)
        {
            // Render to an image at the current system scale and retrieve pixel contents
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(element);
            var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();
            SoftwareBitmap outputBitmap = SoftwareBitmap.CreateCopyFromBuffer(pixelBuffer, BitmapPixelFormat.Bgra8, renderTargetBitmap.PixelWidth, renderTargetBitmap.PixelHeight, BitmapAlphaMode.Ignore);
            var current = VideoFrame.CreateWithSoftwareBitmap(outputBitmap);
            return current;
        }
        private async void OnPageLoaded(object sender, RoutedEventArgs e)
        {

            await InitModelAsync(DeviceComboBox.SelectedIndex);
            VlcPlayer.Visibility = Visibility.Collapsed;
            WebCam.Visibility = Visibility.Collapsed;
            switch (Mode)
            {
                case StreamSourceTypes.RTSP:
                    VlcPlayer.Visibility = Visibility.Visible;
                    //rtsp = new RtspHelper();
                    //await rtsp.StartStream();

                    //rtsp.FrameReceived += (a, ev) => {
                    //var bmp = SoftwareBitmap.CreateCopyFromBuffer(ev.BitmapFrame.PixelBuffer, BitmapPixelFormat.Bgra8, ev.BitmapFrame.PixelWidth, ev.BitmapFrame.PixelHeight);
                    //frame = VideoFrame.CreateWithSoftwareBitmap(bmp);
                    //};
                    //VlcPlayer.Source = AppConstants.Cctv1;

                    string FILE_TOKEN = "{1BBC4B94-BE33-4D79-A0CB-E5C6CDB9D107}";
                    var fileOpenPicker = new FileOpenPicker();
                    fileOpenPicker.FileTypeFilter.Add("*");
                    var file = await fileOpenPicker.PickSingleFileAsync();
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace(FILE_TOKEN, file);
                    VlcPlayer.Source = $"winrt://{FILE_TOKEN}";
                    VlcPlayer.Play();
                    break;
                case StreamSourceTypes.WebCam:
                    WebCam.Visibility = Visibility.Visible;
                    _ = InitCameraAsync();
                    break;
                case StreamSourceTypes.HttpImage:

                    //for cctv image grabber through http
                    this.Grabber = new ImageHttpHelper(AppConstants.Username, AppConstants.Password);
                    break;
            }

            if (AppConstants.AutoStart)
                await StartTracking();
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


        private async Task InitModelAsync(int DevId = 0)
        {
            ShowStatus("Loading yolo.onnx model...");
            try
            {
                #region model1
                if (_model == null)
                {

                    _model = new Model5();

                    ImgWidth = OverlayCanvas.Width;
                    ImgHeight = OverlayCanvas.Height;
                }
                await _model.InitModelAsync(ImgWidth, ImgHeight, DevId);

                //set filter objects
                _model.SetFilter(ObjectKind.Motorbike, ObjectKind.Car, ObjectKind.Truck, ObjectKind.Bus);

                #endregion
                ShowStatus("ready");
                Log("Model is loaded..");
                button_go.IsEnabled = true;
            }
            catch (Exception ex)
            {
                ShowStatus(ex.Message);
            }
        }
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
                switch (Mode)
                {
                    case StreamSourceTypes.RTSP:
                        //VlcPlayer.Play();
                        frame = await Capture(VlcPlayer);
                        break;
                    case StreamSourceTypes.WebCam:
                        frame = new VideoFrame(Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8, (int)ImgWidth, (int)ImgHeight);
                        await _media_capture.GetPreviewFrameAsync(frame);
                        break;
                    case StreamSourceTypes.HttpImage:
                        frame = await Grabber.GetFrameFromHttp(AppConstants.CctvHttp);
                        break;
                }

                if (frame != null)
                {
                    var results = await _model.EvaluateFrame(frame, selectRect);
                    Log($"detected objects : {results.Count}");
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

        private async Task DrawBoxes5(List<ObjectDetectorResult> detections, VideoFrame frame, Helpers.Tracker tracker)
        {
            this.OverlayCanvas.Children.Clear();
            var brush = new ImageBrush();
            var bitmap_source = new SoftwareBitmapSource();
            await bitmap_source.SetBitmapAsync(frame.SoftwareBitmap);

            brush.ImageSource = bitmap_source;
            brush.Stretch = Stretch.Fill;
            this.OverlayCanvas.Background = brush;

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

                Canvas.SetLeft(border, detections[i].Rect.Left * ImgWidth);
                Canvas.SetTop(border, detections[i].Rect.Top * ImgHeight);
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

        private async void button_go_Click(object sender, RoutedEventArgs e)
        {
            await StartTracking();
        }

        async Task StartTracking()
        {
            if (!IsTracking)
            {
                var deviceIndex = DeviceComboBox.SelectedIndex;
                if (_model.DeviceIndex != deviceIndex)
                {
                    await InitModelAsync(deviceIndex);
                }


                if (_timer == null)
                {
                    // now start processing frames, no need to do more than 30 per second!
                    _timer = new DispatcherTimer()
                    {

                        Interval = TimeSpan.FromMilliseconds(DelayTime)
                    };
                    _timer.Tick += OnTimerTick;

                }
                _timer.Start();
                IsTracking = true;
                Log("Tracking is enabled..");
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

            }
            else
            {
                if (_timer != null)
                {
                    _timer.Stop();
                }
                IsTracking = false;
                Log("Tracking is disabled..");

            }
        }

        void ShowStatus(string text)
        {
            textblock_status.Text = text;
        }

        private async void OnTimerTick(object sender, object e)
        {
            // don't wait for this async task to finish

            _ = ProcessFrame();
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            AppConfig.Save();
            Log("Config is saved.");
        }

        void Log(string Message)
        {
            TxtStatus.Text = $"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} => {Message}";
        }

        async Task SyncToCloud()
        {
            try
            {
                var tracker = _model.GetTracker();
                var table = tracker.GetLogTable();
                if (table != null)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var newItem = new DataCounter();
                        newItem.Jenis = dr["Jenis"].ToString();
                        newItem.Tanggal = Convert.ToDateTime(dr["Waktu"]);
                        newItem.Merek = "-";
                        newItem.Gateway = AppConstants.Gateway;
                        newItem.Lokasi = AppConstants.Lokasi;
                        var res = await dataCounterService.InsertData(newItem);
                    }
                }
                tracker.ClearLogTable();
                Console.WriteLine("Sync succeed");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Sync failed:{ex.ToString()}");
            }

        }
        #region unused functions

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

        private void ChkAutoStart_Checked(object sender, RoutedEventArgs e)
        {
            AppConstants.AutoStart = true;
        }

        private void ChkAutoStart_Unchecked(object sender, RoutedEventArgs e)
        {
            AppConstants.AutoStart = false;
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
        #endregion

        public void Dispose()
        {
            _media_capture?.Dispose();
            _timer.Stop();
            PushTimer.Stop();
            StatTimer.Stop();
            frame?.Dispose();
            if (watch.IsRunning)
            {
                watch.Stop();
            }
            VlcPlayer.Stop();
        }
    }
}
