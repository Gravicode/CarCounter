using CarCounter.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceStack.Redis;
using SkiaSharp;

namespace CarCounter.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CctvController : Controller
    {
        RedisManagerPool pool;
        public CctvController(RedisManagerPool manager)
        {
            this.pool = manager;


        }
        // /api/Sms/GetData
        [HttpPost("[action]")]
        public IActionResult SendImage(CCTVImage info)
        {
            try
            {
                //var bytes = Convert.FromBase64String(info.Image64);
                var client = pool.GetClient();
                client.Set(info.CctvName, info);

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public IActionResult GetImage(string Key)
        {
            var client = pool.GetClient();
            var img = client.Get<CCTVImage>(Key);
            if (img == null)
            {
                var stream = new MemoryStream(ImageNotAvailable());
                return File(stream, "image/jpeg");
            }
            else
            {
                var stream = new MemoryStream(img.ImageBytes);
                return File(stream, "image/jpeg");
            }
        }

        byte[] ImageNotAvailable()
        {
            /*
            System.Drawing.Image upImage = new Bitmap(320, 240);
            using (Graphics g = Graphics.FromImage(upImage))
            {
                var brush = new SolidBrush(Color.White);
                g.FillRectangle(brush, 0, 0, upImage.Width, upImage.Height);
                // For Transparent Watermark Text
                int opacity = 200; // range from 0 to 255

                //SolidBrush brush = new SolidBrush(Color.Red);
                brush = new SolidBrush(Color.FromArgb(opacity, Color.Red));
                Font font = new Font("Arial", 20);
                g.DrawString("NO CCTV IMAGE", font, brush, new PointF((upImage.Width/2) - 50, upImage.Height/2)); //txtWatermarkText.Text.Trim()
                var bytes = ImageHelper.ImageToByte(upImage);
                return bytes;
            }
            */
            var ImageWidth = 320;
            var ImageHeight = 240;
            var FontSize = 20;
            var text = "NO CCTV IMAGE";
            var imageInfo = new SKImageInfo(ImageWidth, ImageHeight, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
            using (var plainSkSurface = SKSurface.Create(imageInfo))
            {
                var plainCanvas = plainSkSurface.Canvas;
                var BackgroundColor = SKColors.White;
                plainCanvas.Clear(BackgroundColor);

                using (var paintInfo = new SKPaint())
                {
                    paintInfo.Typeface = SKTypeface.FromFamilyName("Calibri", SKFontStyle.Bold);
                    paintInfo.TextSize = FontSize;
                    paintInfo.Color = SKColors.Red;
                    paintInfo.IsAntialias = true;

                    var xToDraw = (ImageWidth - paintInfo.MeasureText(text)) / 2;
                    var yToDraw = (ImageHeight - FontSize) / 2 + FontSize;
                    plainCanvas.DrawText(text, xToDraw, yToDraw, paintInfo);
                }
                plainCanvas.Flush();

                return plainSkSurface.Snapshot().Encode().ToArray();

            }
        }
    }

}
