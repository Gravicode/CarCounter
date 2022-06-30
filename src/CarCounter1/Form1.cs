using CarCounter.Models;
using CarCounter.Tools;
using CarCounter1.Data;
using CarCounter1.Helpers;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Data;

namespace CarCounter1
{
    public partial class Form1 : Form
    {
        int ImgHeight = 0, ImgWidth = 0;
        Point StartLocation;
        bool IsSelect = false;
        Rectangle SelectionArea = new Rectangle(0, 0, 0, 0);
        DataCounterService dataCounterService;
        bool IsCapturing = false;
        string SelectedFile;
        CancellationTokenSource source;
        Yolo yolo;
        int DelayTime = 20;
        public Form1()
        {
            InitializeComponent();
            dataCounterService = ObjectContainer.Get<DataCounterService>();
            yolo = new Yolo();
            BtnStart.Click += async (a, b) =>
            {
                if (source == null) source = new CancellationTokenSource();
                var task = new Thread(() => Capture(source.Token));
                task.Start();
            };
            BtnStop.Click += (a, b) =>
            {
                if (source == null) return;
                source.Cancel();
            };
            BtnSave.Click += (a, b) =>
            {
                yolo.SaveLog();
            };
            
            BtnSync.Click += async (a, b) =>
            {
                await SyncToCloud();
            };

            this.FormClosing += Form1_FormClosing;

            BtnOpen.Click += async (a, b) =>
            {
                var fname = OpenFileDialogForm();
                if (!string.IsNullOrEmpty(fname))
                {
                    SelectedFile = fname;
                    this.Text = $"Car Counter v1.0 ({SelectedFile})";
                }
            };
            pictureBox1.Resize += (object? sender, EventArgs e) =>
            {
                ImgHeight = pictureBox1.Height;
                ImgWidth = pictureBox1.Width;

            };
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Paint += (object? sender, PaintEventArgs e) =>
            {
                if (SelectionArea.Width > 0)
                {
                    var pen = new Pen(Color.LightGreen, 2);
                    e.Graphics.DrawRectangle(pen, SelectionArea);
                }
            };
            pictureBox1.MouseDown += (object? sender, MouseEventArgs e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    IsSelect = true;
                    StartLocation = e.Location;
                }
            };
            pictureBox1.MouseMove += (object? sender, MouseEventArgs e) =>
            {
                if (e.Button == MouseButtons.Left && IsSelect)
                {
                    SelectionArea.X = Math.Min(StartLocation.X, e.X);
                    SelectionArea.Y = Math.Min(StartLocation.Y, e.Y);
                    SelectionArea.Width = Math.Abs(StartLocation.X - e.X);
                    SelectionArea.Height = Math.Abs(StartLocation.Y - e.Y);
                    pictureBox1.Invalidate();
                }
            };
            pictureBox1.MouseUp += (object? sender, MouseEventArgs e) =>
            {
                if (e.Button == MouseButtons.Left && IsSelect)
                {
                    IsSelect = false;
                    //detector.SetSelectionArea(SelectionArea);

                }
                else if (e.Button == MouseButtons.Right)
                {
                    SelectionArea.X = 0;
                    SelectionArea.Y = 0;
                    SelectionArea.Width = 0;
                    SelectionArea.Height = 0;
                }
            };

        }

        async Task SyncToCloud()
        {
            try
            {
                var table = yolo.GetLogData();
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
                Console.WriteLine("Sync succeed");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Sync failed:{ex.ToString()}");
            }
            
        }

        public string? OpenFileDialogForm()
        {
            var openFileDialog1 = new OpenFileDialog()
            {
                FileName = "",
                Filter = "Video files (*.mp4)|*.mp4",
                Title = "Open video file"
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (File.Exists(openFileDialog1.FileName))
                    {
                        return openFileDialog1.FileName;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");

                }
            }
            return null;
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (source != null)
                source.Cancel();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        void Capture(CancellationToken token)
        {
            Rectangle selectRect = new Rectangle();
            if (IsCapturing) return;
            var capture = !string.IsNullOrEmpty(SelectedFile) ? new Emgu.CV.VideoCapture(SelectedFile) : new Emgu.CV.VideoCapture();
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

                            if (SelectionArea.Width > 0 && SelectionArea.Height > 0)
                            {
                                //cropping sesuai selection area
                                var ratioX = (double)SelectionArea.X / ImgWidth;
                                var ratioY = (double)SelectionArea.Y / ImgHeight;
                                var ratioWidth = (double)SelectionArea.Width / ImgWidth;
                                var ratioHeight = (double)SelectionArea.Height / ImgHeight;

                                selectRect = new Rectangle((int)(ratioX * nextFrame.Width), (int)(ratioY * nextFrame.Height), (int)(ratioWidth * nextFrame.Width), (int)(ratioHeight * nextFrame.Height));

                            }
                           
                            var bmp = yolo.Detect(img, selectRect);
                            this.pictureBox1?.Invoke((MethodInvoker)delegate
                            {
                                // Running on the UI thread
                                pictureBox1.Image = bmp;
                            });



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