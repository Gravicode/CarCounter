using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Windows.Media;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace CarCounter.UWP.Helpers
{
    public static class DrawResults
    {
        private static readonly SolidColorBrush _fill_brush = new SolidColorBrush(Colors.Transparent);
        private static readonly SolidColorBrush _line_brush = new SolidColorBrush(Colors.DarkGreen);
        private static readonly double _line_thickness = 2.0;
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

                    using (var brushes = new SolidBrush(System.Drawing.Color.FromArgb(50, System.Drawing.Color.Red)))
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

                    using (var brushes = new SolidBrush(System.Drawing.Color.FromArgb(50, System.Drawing.Color.Red)))
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
                            for(int i=target.Trails.Count-1;i>0;i--)
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

        public static async Task DrawUWP(
                    IReadOnlyList<Result> detections, VideoFrame frame, Tracker tracker, Canvas OverlayCanvas, int ImgWidth, int ImgHeight)
        {

            OverlayCanvas.Children.Clear();
            for (int i = 0; i < detections.Count; ++i)
            {
                int top = (int)(detections[i].BoundingBox[0] * ImgHeight);
                int left = (int)(detections[i].BoundingBox[1] * ImgWidth);
                int bottom = (int)(detections[i].BoundingBox[2] * ImgHeight);
                int right = (int)(detections[i].BoundingBox[3] * ImgWidth);

                var brush = new ImageBrush();
                var bitmap_source = new SoftwareBitmapSource();
                await bitmap_source.SetBitmapAsync(frame.SoftwareBitmap);

                brush.ImageSource = bitmap_source;
                // brush.Stretch = Stretch.Fill;

                OverlayCanvas.Background = brush;

                var r = new Windows.UI.Xaml.Shapes.Rectangle();
                r.Tag = i;
                r.Width = right - left;
                r.Height = bottom - top;
                r.Fill = _fill_brush;
                r.Stroke = _line_brush;
                r.StrokeThickness = _line_thickness;
                r.Margin = new Thickness(left, top, 0, 0);

                OverlayCanvas.Children.Add(r);
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
                OverlayCanvas.Children.Add(border);

            }
        }
    }
}
