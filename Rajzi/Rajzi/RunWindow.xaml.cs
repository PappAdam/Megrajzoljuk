using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Rajzi
{
    public partial class RunWindow : Window
    {
        List<Vector> vectors = new List<Vector>();
        Pencil pencil = new Pencil();
        private TranslateTransform transform = new TranslateTransform(0, 0);
        private bool _isMouseDown;
        private Point _startPoint;
        private int counter = 0;
        TranslateTransform translateTransform = new TranslateTransform(0, 0);
        List<Polygon> polygonok = new List<Polygon>();
        PointCollection points = new PointCollection();
        public RunWindow()
        {
            InitializeComponent();
            grid1.MouseLeftButtonDown += Grid1_MouseLeftButtonDown;
            grid1.MouseLeftButtonUp += Grid1_MouseLeftButtonUp;
            grid1.MouseMove += Grid1_MouseMove;

            //Random random = new Random();
            //for (int i = 5; i > 0; i--)
            //{
            //    Color randomColor = Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256));
            //    pencil.changeColor(randomColor);
            //    for (int e = 0; e < 360; e++)
            //    {
            //        forward(i * 10);
            //        forward(-1 * i * 10);
            //        pencil.changeRotate(e);
            //        pencil.changeSize(i / 2);
            //    }
            //}
        }

        public void goToAddLine(double x, double y)
        {
            Line line = new Line();
            line.Stroke = new SolidColorBrush(pencil.color);
            line.StrokeThickness = pencil.size;
            line.X1 = pencil.pixelPositionX;
            line.Y1 = pencil.pixelPositionY;
            line.X2 = x;
            line.Y2 = y;
            vectors.Insert(0, new Vector(line.X1, line.Y1, line.X2, line.Y2));
            changePosition(line.X2, line.Y2);
            Canvas.Children.Add(line);
        }
        public void forward(double forward)
        {
            Line line = new Line();
            line.Stroke = new SolidColorBrush(pencil.color);
            line.StrokeThickness = pencil.size;
            line.X1 = pencil.pixelPositionX;
            line.Y1 = pencil.pixelPositionY;
            line.X2 = pencil.pixelPositionX + Math.Cos(Math.PI / 180 * pencil.rotate) * forward;
            line.Y2 = pencil.pixelPositionY + Math.Sin(Math.PI / 180 * pencil.rotate) * forward;
            vectors.Insert(0, new Vector(line.X1, line.Y1, line.X2, line.Y2));
            changePosition(line.X2, line.Y2);
            Canvas.Children.Add(line);
            if (pencil.polygon == true)
            {
                points.Add(new Point(line.X2, line.Y2));
            }
        }

        public void changeSize(double size)
        {
            pencil.size = size;
        }
        public void changeColor(double alpha, double red, double green, double blue)
        {
            pencil.color = Color.FromArgb((byte)(alpha * 255), (byte)(red * 255), (byte)(green * 255), (byte)(blue * 255));
        }

        public void Rotate(double rotate, string direction)
        {
            if (direction == "RIGHT")
            {
                pencil.rotate += rotate;
            }
            else if (direction == "LEFT")
            {
                pencil.rotate -= rotate;
            }
            else
            {
                pencil.rotate = rotate - 90;
            }
        }

        public void changePosition(double x, double y)
        {
            pencil.pixelPositionX = x;
            pencil.pixelPositionY = y;
        }

        public void Polygon(bool polygon)
        {
            pencil.polygon = polygon;
            if (pencil.polygon == true)
            {
                Polygon myPolygon = new Polygon();
                PointCollection points = new PointCollection();
                points.Add(new Point(pencil.pixelPositionX, pencil.pixelPositionY));
            }
            else
            {
                Polygon myPolygon = new Polygon();
                polygonok.Add(myPolygon);
                polygonok[polygonok.Count() - 1].Points = points;
                polygonok[polygonok.Count() - 1].Fill = new SolidColorBrush(pencil.color);
                PolygonPanel.Children.Add(polygonok[polygonok.Count() - 1]);
            }
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
                RenderTargetBitmap renderTargetBitmap = null;
                try
                {
                    renderTargetBitmap = new RenderTargetBitmap((int)(bounds.Width * dpi / 96.0),
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
                }
                catch (Exception)
                {
                    MessageBox.Show("There was an error during save!");
                }
                
                return renderTargetBitmap;

            }
        }

        private void PngSave(object sender, RoutedEventArgs e)
        {
            RenderVisualService.RenderToPNGFile(move, $"{FileName.Text}.png");
        }


        private void Grid1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;
            _startPoint = e.GetPosition(move);

        }

        private void Grid1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = false;
        }

        private void Grid1_MouseMove(object sender, MouseEventArgs e)
        {
            var currentPoint = e.GetPosition(move);
            var offset = currentPoint - _startPoint;
            _startPoint = currentPoint;

            if (_isMouseDown)
            {
                foreach (UIElement child in Canvas.Children)
                {
                    double left = Canvas.GetLeft(child);
                    double top = Canvas.GetTop(child);
                    if (!double.IsNaN(left))
                    {
                        Canvas.SetLeft(child, left + offset.X);
                    }
                    else
                    {
                        Canvas.SetLeft(child, offset.X);
                    }
                    if (!double.IsNaN(top))
                    {
                        Canvas.SetTop(child, top + offset.Y);
                    }
                    else
                    {
                        Canvas.SetTop(child, offset.Y);
                    }
                }
            }
        }

        private void WindowClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
