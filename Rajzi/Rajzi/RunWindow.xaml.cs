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
            //Point startPoint = new Point(0, 1);
            //Point endPoint = new Point(1, 3);
            //var equation = GetEquation(startPoint, endPoint);
            //teszt1.Content = equation;
            //Point startPoint_2 = new Point(2, 2);
            //Point endPoint_2 = new Point(5, -1);
            //var equation_2 = GetEquation(startPoint_2, endPoint_2);
            //teszt2.Content = equation_2;
            //var done = GetIntersection(equation, equation_2);
            //teszt3.Content = done;
            Point startPoint = new Point(1, 1);
            Point endPoint = new Point(3, 7);
            Point a = new Point(1, 3);
            bool contains = VectorContainsPoint(startPoint, endPoint, a);
            teszt3.Content = contains;
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













        public static Tuple<double, double> GetEquation(Point startPoint, Point endPoint)
        {
            double slope = (endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X);
            double yIntercept = startPoint.Y - slope * startPoint.X;

            return Tuple.Create(slope, yIntercept);
        }

        public static Tuple<double, double> GetIntersection(Tuple<double, double> vec1, Tuple<double, double> vec2)
        {
            double x = (vec2.Item2 - vec1.Item2) / (vec1.Item1 - vec2.Item1);
            double y = vec1.Item1 * x + vec1.Item2;
            return Tuple.Create(x, y);
        }

        public static bool VectorContainsPoint(Point startPoint, Point endPoint, Point a)
        {
            System.Windows.Vector systemVector = new System.Windows.Vector(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);
            System.Windows.Vector apVector = new System.Windows.Vector(a.X - startPoint.X, a.Y - startPoint.Y);

            double dotProduct = systemVector.X * apVector.Y - systemVector.Y * apVector.X;
            return dotProduct == 0;
//            Azt kell megvizsgálnunk, hogy a 3.pont az adott két pontból kiinduló vektoron van - e, vagy sem.

//Ha az adott két pont az A és B pontok, és a 3.pont a C pont, akkor a vektorunk AB.

//A vektor AB irányvektorát a két pont különbségvektora adja meg:
//AB = B - A

//Ezt a vektort felhasználva megállapíthatjuk, hogy a C pont a vektoron van - e vagy sem. Ehhez az AC vektort is kiszámítjuk:

//AC = C - A

//Ha a C pont a vektoron van, akkor az AC vektor skalárszorzata megegyezik az AB vektor skalárszorzatával, hiszen az AB vektor az AC vektorra merőleges lesz, és a skalárszorzatuk nullát ad:

//(AB) * (AC) = 0

//Ezt a skalárszorzatot az alábbi módon számoljuk ki:
//(AB) * (AC) = (B - A) * (C - A) = (Bx - Ax) * (Cx - Ax) + (By - Ay) * (Cy - Ay) + (Bz - Az) * (Cz - Az)

//Ha az eredmény 0, akkor a C pont az AB vektoron van. Ha az eredmény nem 0, akkor a C pont nem metszi az AB vektort.




        }

    }
}
