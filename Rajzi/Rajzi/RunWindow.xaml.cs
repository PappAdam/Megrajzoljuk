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

namespace Rajzi
{
    public partial class RunWindow : Window
    {
        List<Pixel> pixels = new List<Pixel>();
        List<Vector> vectors = new List<Vector>();
        Pencil pencil = new Pencil();
        public RunWindow()
        {
            InitializeComponent();
            Point startPoint = new Point(0, 0);
            Point endPoint = new Point(2, 3);
            Point startPoint2 = new Point(1, 5);
            Point endPoint2 = new Point(4, 2);
            var metszet = vectorIntersection(startPoint, endPoint, startPoint2, endPoint2);
            teszt3.Content = metszet;
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
    }
}
