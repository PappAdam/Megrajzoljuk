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
        List<Pencil> pixels = new List<Pencil>();
        public RunWindow()
        {
            InitializeComponent();
            TextBox textBox = new TextBox { Text = "Test" };
            //Canvas.Children.Add(textBox);
            for (int i = 0; i < 100; i++)
            {
                AddPixel(200, 25 + i);
                AddPixel(300, 25 + i);
                AddPixel(200 + i, 25);
                AddPixel(200 + i, 125);
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
        private void AddPixel(double x, double y)
        {
            pixels.Insert(0, new Pencil(x, y));
            Rectangle rec = new Rectangle();
            rec.Width = pixels[0].size;
            rec.Height = pixels[0].size;
            rec.Fill = new SolidColorBrush(pixels[0].color);
            Canvas.Children.Add(rec);
            Canvas.SetLeft(rec, x);
            Canvas.SetTop(rec, y);
        }
    }
}
