using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media;
using System.Runtime.Intrinsics;
using System.IO;
using System.Diagnostics;

namespace Rajzi
{
    public partial class RunWindow : Window
    {
        List<Pixel> pixels = new List<Pixel>();
        List<Vector> vectors = new List<Vector>();
        Pencil pencil = new Pencil();
        private TranslateTransform transform = new TranslateTransform(0, 0);
        private bool _isMouseDown;
        private Point _startPoint;
        private int counter = 0;
        public RunWindow()
        {
            InitializeComponent();
            grid1.MouseLeftButtonDown += Grid1_MouseLeftButtonDown;
            grid1.MouseLeftButtonUp += Grid1_MouseLeftButtonUp;
            grid1.MouseMove += Grid1_MouseMove;

            //Random random = new Random();
            //for (int i = 100; i > 0; i--)
            //{
            //    Color randomColor = Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
            //    pencil.changeColor(randomColor);
            //    for (int e = 0; e < 360; e++)
            //    {
            //        forward(i*10);
            //        forward(-1*i*10);
            //        pencil.changeRotate(e);
            //        pencil.changeSize(i/2);
            //    }
            //}
        }
        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
            {
                var matTrans = grid2.RenderTransform as MatrixTransform;
                var pos1 = e.GetPosition(grid1);

                var scale = e.Delta > 0 ? 1.1 : 1 / 1.1;

                var mat = matTrans.Matrix;
                mat.ScaleAt(scale, scale, pos1.X, pos1.Y);
                matTrans.Matrix = mat;
                e.Handled = true;
            }
        private void goToAddPixel(double x, double y)
        {
            pixels.Insert(0, new Pixel(x, y));
            Rectangle rec = new Rectangle();
            rec.Width = pencil.size;
            rec.Height = pencil.size;
            rec.Fill = new SolidColorBrush(pencil.color);
            Canvas.Children.Add(rec);
            pencil.changePosition(x, y);
            Canvas.SetLeft(rec, x - pencil.size/2);
            Canvas.SetTop(rec, y - pencil.size / 2);
        }
        private void goToAddLine(double x, double y)
        {
            Line line = new Line();
            line.Stroke = new SolidColorBrush(pencil.color);
            line.StrokeThickness = pencil.size;
            line.X1 = pencil.pixelPositionX;
            line.Y1 = pencil.pixelPositionY;
            line.X2 = x;
            line.Y2 = y;
            vectors.Insert(0, new Vector(line.X1, line.Y1, line.X2, line.Y2));
            pencil.changePosition(line.X2, line.Y2);
            Canvas.Children.Add(line);
        }
        private void forward(double forward)
        {
            Line line = new Line();
            line.Stroke = new SolidColorBrush(pencil.color);
            line.StrokeThickness = pencil.size;
            line.X1 = pencil.pixelPositionX;
            line.Y1 = pencil.pixelPositionY;
            line.X2 = pencil.pixelPositionX + Math.Cos(Math.PI / 180 * pencil.rotate) * forward;
            line.Y2 = pencil.pixelPositionY + Math.Sin(Math.PI / 180 * pencil.rotate) * forward;
            vectors.Insert(0, new Vector(line.X1, line.Y1, line.X2, line.Y2));
            pencil.changePosition(line.X2, line.Y2);
            Canvas.Children.Add(line);
        }


        public static Tuple<double, double> vectorIntersection(Point startPoint, Point endPoint, Point startPoint2, Point endPoint2)
        {
            double slope = (endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X);
            double yIntercept = startPoint.Y - slope * startPoint.X;
            var vec1 = Tuple.Create(slope, yIntercept);

            double slope2 = (endPoint2.Y - startPoint2.Y) / (endPoint2.X - startPoint2.X);
            double yIntercept2 = startPoint2.Y - slope2 * startPoint2.X;
            var vec2 = Tuple.Create(slope2, yIntercept2);

            double x = (vec2.Item2 - vec1.Item2) / (vec1.Item1 - vec2.Item1);
            double y = vec1.Item1 * x + vec1.Item2;
            var point = Tuple.Create(x, y);

            if (((point.Item1 >= startPoint.X && point.Item1 <= endPoint.X) || (point.Item1 <= startPoint.X && point.Item1 >= endPoint.X)) && ((point.Item1 >= startPoint2.X && point.Item1 <= endPoint2.X) || (point.Item1 <= startPoint2.X && point.Item1 >= endPoint2.X)))
            {
                return Tuple.Create(x, y);
            }
            else
            {
                return Tuple.Create(double.NaN, double.NaN);
            }
        }



        public static class RenderVisualService
        {
            private const double defaultDpi = 96.0;

            public static void RenderToPNGFile(Visual targetControl, string filename)
            {
                var renderTargetBitmap = GetRenderTargetBitmapFromControl(targetControl);

                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

                var result = new BitmapImage();

                try
                {
                    using (var fileStream = new FileStream(filename, FileMode.Create))
                    {
                        encoder.Save(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Hiba volt a fájl mentésében: {ex.Message}");
                }
            }

            private static BitmapSource GetRenderTargetBitmapFromControl(Visual targetControl, double dpi = defaultDpi)
            {
                if (targetControl == null) return null;

                var bounds = VisualTreeHelper.GetDescendantBounds(targetControl);
                var renderTargetBitmap = new RenderTargetBitmap((int)(bounds.Width * dpi / 96.0),
                                                                (int)(bounds.Height * dpi / 96.0),
                                                                dpi,
                                                                dpi,
                                                                PixelFormats.Pbgra32);

                var drawingVisual = new DrawingVisual();

                using (var drawingContext = drawingVisual.RenderOpen())
                {
                    var visualBrush = new VisualBrush(targetControl);
                    drawingContext.DrawRectangle(visualBrush, null, new Rect(new Point(), bounds.Size));
                }

                renderTargetBitmap.Render(drawingVisual);
                return renderTargetBitmap;
            }
        }

        private void PngSave(object sender, RoutedEventArgs e)
        {
            RenderVisualService.RenderToPNGFile(Canvas, $"{FileName.Text}.png");
        }


        private void Grid1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;
            _startPoint = e.GetPosition(Canvas);

        }

        private void Grid1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
        }

        private void Grid1_MouseMove(object sender, MouseEventArgs e)
        {
            var currentPoint = e.GetPosition(Canvas);
            var offset = currentPoint - _startPoint;
            _startPoint = currentPoint;

            if (_isMouseDown)
            {
                Canvas.Margin = new Thickness(Canvas.Margin.Left + offset.X,Canvas.Margin.Top + offset.Y, 0, 0);
            }
        }

    }
}
