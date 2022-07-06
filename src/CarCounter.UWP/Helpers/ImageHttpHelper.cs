using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;

namespace CarCounter.UWP.Helpers
{
    public class ImageHttpHelper
    {
        HttpClient client;

        
        
        public ImageHttpHelper(string Username, string Password)
        {
            var credentials = new NetworkCredential(Username, Password);
            var handler = new HttpClientHandler { Credentials = credentials };
            
            client = new HttpClient(handler);
        }

        public async Task<VideoFrame> GetFrameFromHttp(string UrlCctv)
        {
            try
            {
                var data1 = await client.GetAsync(UrlCctv);// + rnd.Next(100));
                if (!data1.IsSuccessStatusCode)
                    return default;
                var data = await data1.Content.ReadAsByteArrayAsync();
                //BitmapImage bmp = new BitmapImage();
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

                    SoftwareBitmap displayableImage = SoftwareBitmap.Convert(outputBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                    VideoFrame frame = VideoFrame.CreateWithSoftwareBitmap(displayableImage);
                    return frame;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
          
            return default;
        }
    }
}
