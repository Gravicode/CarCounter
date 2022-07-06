using Emgu.CV;
using RtspClientSharp;
using RtspClientSharp.RawFrames;
using RtspClientSharp.RawFrames.Audio;
using RtspClientSharp.RawFrames.Video;
using RtspDecoder.RawFramesDecoding;
using RtspDecoder.RawFramesDecoding.DecodedFrames;
using RtspDecoder.RawFramesDecoding.FFmpeg;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
using PixelFormat = RtspDecoder.RawFramesDecoding.PixelFormat;

namespace CarCounter.UWP.Helpers
{
    public class CctvDataEventArgs:EventArgs
    {
        public DateTime Created { get; set; }
        public WriteableBitmap BitmapFrame { get; set; }
        public SoftwareBitmap Frame { get; set; }
    }
    public class RtspHelper
    {
        

        private int _width;
        private int _height;
        private TransformParameters _transformParameters;

        public EventHandler<CctvDataEventArgs> FrameReceived;
       
        public RtspHelper()
        {

        }
        bool IsCapturing=false;
        public async Task StartStream2()
        {
            var capture = new Emgu.CV.VideoCapture(AppConstants.Cctv1);
            IsCapturing = true;
            while (true)
            {
                using (var nextFrame = capture.QueryFrame())
                {
                    var bitmap = nextFrame.ToBitmap();
                    using (var stream = new Windows.Storage.Streams.InMemoryRandomAccessStream())
                    {
                        bitmap.Save(stream.AsStream(), ImageFormat.Jpeg);//choose the specific image format by your own bitmap source
                        Windows.Graphics.Imaging.BitmapDecoder decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);
                        SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                        FrameReceived?.Invoke(this, new CctvDataEventArgs() { Created = DateTime.Now, Frame = softwareBitmap });

                    }

                }
                Thread.Sleep(100);
            }
        }
        public async Task StartStream()
        {
            var serverUri = new Uri(AppConstants.Cctv1);//"rtsp://192.168.1.77:554/ucast/11"
            //var credentials = new NetworkCredential("admin", "123qweasd!");
            var connectionParameters = new ConnectionParameters(serverUri);
            //var connectionParameters = new ConnectionParameters(serverUri, credentials);
            connectionParameters.RtpTransport = RtpTransportProtocol.TCP;
            using (var rtspClient = new RtspClient(connectionParameters))
            {
                rtspClient.FrameReceived += async (sender, frame) =>
                {
                    //process (e.g. decode/save to file) encoded frame here or 
                    //make deep copy to use it later because frame buffer (see FrameSegment property) will be reused by client
                    switch (frame)
                    {
                        case RawH264IFrame h264IFrame:
                        case RawH264PFrame h264PFrame:
                        case RawJpegFrame jpegFrame:
                        case RawAACFrame aacFrame:
                        case RawG711AFrame g711AFrame:
                        case RawG711UFrame g711UFrame:
                        case RawPCMFrame pcmFrame:
                        case RawG726Frame g726Frame:
                            Debug.WriteLine(frame.Type.ToString());
                            if (frame.Type == RtspClientSharp.RawFrames.FrameType.Video)
                            {
                                
                                //var arr = frame.FrameSegment.ToArray();
                                //var ms = new MemoryStream(arr);
                                //System.Drawing.Image bitmap = Image.FromStream(ms);
                                //SoftwareBitmap softwareBitmap;
                                //using (var stream = new Windows.Storage.Streams.InMemoryRandomAccessStream())
                                //{
                                //    bitmap.Save(stream.AsStream(), ImageFormat.Jpeg);//choose the specific image format by your own bitmap source
                                //    Windows.Graphics.Imaging.BitmapDecoder decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);
                                //    softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                                //}
                                var wb = await ConvertFrame(frame);
                                //FrameReceived?.Invoke(this, new CctvDataEventArgs() { Created = DateTime.Now, BitmapFrame = wb });
                            }
                            
                            break;
                    }
                };

                CancellationTokenSource source = new CancellationTokenSource();
                await rtspClient.ConnectAsync(source.Token);
                await rtspClient.ReceiveAsync(source.Token);
            }
        }


        private readonly Dictionary<FFmpegVideoCodecId, FFmpegVideoDecoder> _videoDecodersMap =
            new Dictionary<FFmpegVideoCodecId, FFmpegVideoDecoder>();

        public void Dispose()
        {
            DropAllVideoDecoders();
        }

        private void DropAllVideoDecoders()
        {
            foreach (FFmpegVideoDecoder decoder in _videoDecodersMap.Values)
                decoder.Dispose();

            _videoDecodersMap.Clear();
        }
        IntPtr ptr;
        private async Task<WriteableBitmap> ConvertFrame(RawFrame rawFrame)
        {
            WriteableBitmap _writeableBitmap = null;
            if (!(rawFrame is RawVideoFrame rawVideoFrame))
                return default;

            FFmpegVideoDecoder decoder = GetDecoderForFrame(rawVideoFrame);

            IDecodedVideoFrame decodedFrame = decoder.TryDecode(rawVideoFrame);

            if (decodedFrame != null)
            {
                if (_width == 0 || _height == 0)
                    return default;



                try
                {
                    var wb = new WriteableBitmap(_width, _height);
                    byte[] imageArray = new byte[_width * _height * 4];
                    /*
                    for (int i = 0; i < imageArray.Length; i += 4)
                    {
                        //BGRA format
                        imageArray[i] = 0; // Blue
                        imageArray[i + 1] = 0;  // Green
                        imageArray[i + 2] = 255; // Red
                        imageArray[i + 3] = 255; // Alpha
                    }*/
                    var bmp = new Bitmap(_width, _height);
                    var bpp = Image.GetPixelFormatSize(bmp.PixelFormat);
                    var stride = (_width * bpp + 7) / 8;
                    
                    IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(imageArray));
                    decodedFrame.TransformTo(ptr, stride, _transformParameters);

                    using (Stream stream = wb.PixelBuffer.AsStream())
                    {
                        //write to bitmap
                        await stream.WriteAsync(imageArray, 0, imageArray.Length);
                    }

                    _writeableBitmap = wb;

                   


                }
                finally
                {

                }


            }
            return _writeableBitmap;
        }

        private FFmpegVideoDecoder GetDecoderForFrame(RawVideoFrame videoFrame)
        {
            FFmpegVideoCodecId codecId = DetectCodecId(videoFrame);
            if (!_videoDecodersMap.TryGetValue(codecId, out FFmpegVideoDecoder decoder))
            {
                decoder = FFmpegVideoDecoder.CreateDecoder(codecId);
                _videoDecodersMap.Add(codecId, decoder);
            }

            return decoder;
        }

        private FFmpegVideoCodecId DetectCodecId(RawVideoFrame videoFrame)
        {
            if (videoFrame is RawJpegFrame)
                return FFmpegVideoCodecId.MJPEG;
            if (videoFrame is RawH264Frame)
                return FFmpegVideoCodecId.H264;

            throw new ArgumentOutOfRangeException(nameof(videoFrame));
        }

        

        private unsafe void UpdateBackgroundColor(IntPtr backBufferPtr, int backBufferStride)
        {
            var _fillColor = Color.White;
            byte* pixels = (byte*)backBufferPtr;
            int color = _fillColor.A << 24 | _fillColor.R << 16 | _fillColor.G << 8 | _fillColor.B;

            Debug.Assert(pixels != null, nameof(pixels) + " != null");

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                    ((int*)pixels)[j] = color;

                pixels += backBufferStride;
            }
        }
    }
}
