﻿using System;
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
            line.Stroke = new SolidColorBrush(pencil.color);
            line.StrokeThickness = pencil.size;
            line.X1 = pencil.pixellPositionX;
            line.Y1 = pencil.pixelPositionY;
            line.X2 = pencil.pixellPositionX + x;
            line.Y2 = pencil.pixelPositionY + y;
            vectors.Insert(0, new Vector(line.X1, line.Y1, line.X2, line.Y2));
            pencil.changePosition(pencil.pixellPositionX + x, pencil.pixelPositionY + y);
            Canvas.Children.Add(line);
        }
    }
}
