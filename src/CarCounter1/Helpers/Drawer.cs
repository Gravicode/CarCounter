using DarknetYolo.Models;
using Emgu.CV;
using Emgu.CV.Reg;
using Emgu.CV.Structure;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace CarCounter1.Helpers
{
    public static class DrawResults
    {
        public static void DrawAndStore(string imageOutputFolder, string imageName,
                    IReadOnlyList<Result> results, Bitmap image)
        {
            using (var graphics = Graphics.FromImage(image))
            {
                foreach (var result in results)
                {
                    var x1 = result.BoundingBox[0];
                    var y1 = result.BoundingBox[1];
                    var x2 = result.BoundingBox[2];
                    var y2 = result.BoundingBox[3];

                    graphics.DrawRectangle(Pens.Red, x1, y1, x2 - x1, y2 - y1);

                    using (var brushes = new SolidBrush(Color.FromArgb(50, Color.Red)))
                    {
                        graphics.FillRectangle(brushes, x1, y1, x2 - x1, y2 - y1);
                    }

                    graphics.DrawString(result.Label + " " + result.Confidence.ToString("0.00"),
                                 new Font("Arial", 12), Brushes.Blue, new PointF(x1, y1));
                }

                image.Save(Path.Combine(imageOutputFolder, Path.ChangeExtension(imageName, "_yoloed"
                                        + Path.GetExtension(imageName))));
            }
        }

        public static Bitmap Draw(
                    IReadOnlyList<Result> results, Bitmap image, Tracker tracker)
        {
            using (var graphics = Graphics.FromImage(image))
            {
                foreach (var result in results)
                {
                    var x1 = result.BoundingBox[0];
                    var y1 = result.BoundingBox[1];
                    var x2 = result.BoundingBox[2];
                    var y2 = result.BoundingBox[3];

                    graphics.DrawRectangle(Pens.Red, x1, y1, x2 - x1, y2 - y1);

                    using (var brushes = new SolidBrush(Color.FromArgb(50, Color.Red)))
                    {
                        graphics.FillRectangle(brushes, x1, y1, x2 - x1, y2 - y1);
                    }

                    graphics.DrawString(result.Label + " " + result.Confidence.ToString("0.00"),
                                 new Font("Arial", 12), Brushes.Blue, new PointF(x1, y1));
                    if (tracker.TrackedList != null)
                    {
                        foreach (var target in tracker.TrackedList)
                        {
                            var pen = new Pen(target.Col, 2);
                            var p1 = new Point((int)target.Location.X, (int)target.Location.Y);
                            for (int i = target.Trails.Count - 1; i > 0; i--)
                            {
                                var p2f = target.Trails[i];
                                var p2 = new Point((int)p2f.X, (int)p2f.Y);
                                graphics.DrawLine(pen, p1, p2);
                                p1 = p2;
                            }
                        }
                    }
                }

                return image;
            }
        }
    }

    public static class DrawResults2
    {
        public static void DrawAndStore(string imageOutputFolder, string imageName,
                    IReadOnlyList<YoloPrediction> results, Bitmap image)
        {
            //CvInvoke.Rectangle(resize, new Rectangle(item.Rectangle.X - 2, item.Rectangle.Y - 33, item.Rectangle.Width + 4, 40), new MCvScalar(255, 0, 0), -1);
            //CvInvoke.PutText(resize, text, new Point(item.Rectangle.X, item.Rectangle.Y - 15), Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.6, new MCvScalar(255, 255, 255), 2);
            //CvInvoke.Rectangle(resize, item.Rectangle, new MCvScalar(255, 0, 0), 3);
            using (var graphics = image.ToMat())
            {
                foreach (var result in results)
                {
                    CvInvoke.Rectangle(graphics, new Rectangle(result.Rectangle.X - 2, result.Rectangle.Y - 33, result.Rectangle.Width + 4, 40), new MCvScalar(252, 11, 3), -1);
                    CvInvoke.Rectangle(graphics, result.Rectangle, new MCvScalar(255, 0, 0), 3);

                    string text = result.Label + " " + result.Confidence.ToString("0.00");

                    CvInvoke.PutText(graphics, text, new Point(result.Rectangle.X, result.Rectangle.Y - 15),
                        Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.6, new MCvScalar(255, 255, 255), 2);
                }

                graphics.ToBitmap().Save(Path.Combine(imageOutputFolder, 
                    Path.ChangeExtension(imageName, "_yoloed"
                    + Path.GetExtension(imageName))));
            }
        }

        public static Bitmap Draw(
                    IReadOnlyList<YoloPrediction> results, Bitmap image, DarknetYoloTracker tracker)
        {
            using (var graphics = image.ToMat())
            {
                foreach (var result in results)
                {
                    CvInvoke.Rectangle(graphics, new Rectangle(result.Rectangle.X - 2, result.Rectangle.Y - 33, result.Rectangle.Width + 4, 40), new MCvScalar(252, 11, 3), -1);
                    CvInvoke.Rectangle(graphics, result.Rectangle, new MCvScalar(255, 0, 0), 3);

                    string text = result.Label + " " + result.Confidence.ToString("0.00");

                    CvInvoke.PutText(graphics, text, new Point(result.Rectangle.X, result.Rectangle.Y - 15),
                        Emgu.CV.CvEnum.FontFace.HersheySimplex, 0.6, new MCvScalar(255, 255, 255), 2);

                    if (tracker.TrackedList != null)
                    {
                        foreach (var target in tracker.TrackedList)
                        {
                            var pen = new Pen(target.Col, 2);
                            var p1 = new Point((int)target.Location.X, (int)target.Location.Y);
                            for (int i = target.Trails.Count - 1; i > 0; i--)
                            {
                                var p2f = target.Trails[i];
                                var p2 = new Point((int)p2f.X, (int)p2f.Y);

                                CvInvoke.ArrowedLine(graphics, p1, p2, new MCvScalar(pen.Color.R, pen.Color.G, pen.Color.B), 2);
                                p1 = p2;
                            }
                        }
                    }
                }

                return graphics.ToBitmap();
            }
        }
    }
}
