using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace Rajzi
{
    internal class Pencil
    {
        public double pixellPositionX { get; set; } = 205;
        public double pixelPositionY { get; set; } = 310;
        public double size { get; set; } = 1;
        public Color color { get; set; } = Colors.White;

        public Pencil()
        {
        }

        public void changeSize(double size)
        {
            this.size = size;
        }
        public void changeColor(Color color)
        {
            this.color = color;
        }
        public void changePosition(double x, double y)
        {
            pixellPositionX = x;
            pixelPositionY = y;

        }
    }
}
