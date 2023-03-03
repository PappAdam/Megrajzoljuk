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
    struct Vec3
    {
        internal double x;
        internal double y;
    }
    internal class Pencil
    {
        Vec3 pixelPosition = new Vec3 { x = 0, y = 0 };
        public int size { get; set; } = 1;
        public Color color { get; set; } = Colors.White;

        public Pencil(double x, double y)
        {
            pixelPosition.x = x;
            pixelPosition.y = y;
        }
    }
}
