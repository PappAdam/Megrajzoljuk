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
    struct Vec2
    {
        internal double x;
        internal double y;
    }
    internal class Pencil
    {
        Vec2 pencilPosition = new Vec2 { x = 225, y = 320 };
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
    }
}
