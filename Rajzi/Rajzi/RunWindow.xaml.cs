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

namespace Rajzi
{
    /// <summary>
    /// Interaction logic for RunWindow.xaml
    /// </summary>
    public partial class RunWindow : Window
    {
        List<Pixel> pixels = new List<Pixel>();
        Pencil pencil = new Pencil();
        public RunWindow()
        {
            InitializeComponent();


            pencil.changeSize(1);
            double cx = 300;
            double cy = 300;
            double r = 50;
            for (int i = 0; i <= 360; i++)
            {
                if (i == 180)
                {
                    pencil.changeSize(5);
                }
                double radians = i * Math.PI / 180;
                double x = cx + r * Math.Cos(radians);
                double y = cy + r * Math.Sin(radians);
                AddPixel(x, y);
                // x és y értékek használata...
            }
            pencil.changeSize(10);
            pencil.changeColor(Colors.Brown);
            AddPixel(325, 290);
            AddPixel(275, 290);

            cx = 300;
            cy = 310;
            r = 30;
            pencil.changeSize(4);
            pencil.changeColor(Colors.Red);
            for (int i = 0; i <= 180; i++)
            {
                double radians = i * Math.PI / 180;
                double x = cx + r * Math.Cos(radians);
                double y = cy + r * Math.Sin(radians);
                AddPixel(x, y);
            }

            AddLine(100, 100);
            AddLine(0, -100);



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
        private void AddPixel(double x, double y)
        {
            pixels.Insert(0, new Pixel(x, y));
            Rectangle rec = new Rectangle();
            rec.Width = pencil.size;
            rec.Height = pencil.size;
            rec.Fill = new SolidColorBrush(pencil.color);
            Canvas.Children.Add(rec);
            Canvas.SetLeft(rec, x - pencil.size/2);
            Canvas.SetTop(rec, y - pencil.size / 2);
        }
        private void AddLine(double x, double y)
        {
            Line line = new Line();
            line.Stroke = Brushes.Red; // a vonal színe
            line.StrokeThickness = pencil.size; // a vonal vastagsága
            line.X1 = pencil.pixellPositionX; // az első pont x koordinátája
            line.Y1 = pencil.pixelPositionY; // az első pont y koordinátája
            line.X2 = pencil.pixellPositionX + x; // a második pont x koordinátája
            line.Y2 = pencil.pixelPositionY + y;
            pencil.pixellPositionX = pencil.pixellPositionX + x;
            pencil.pixelPositionY = pencil.pixelPositionY + y;
            Canvas.Children.Add(line);
        }
    }
}
