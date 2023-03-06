using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rajzi
{
    internal class Vector
    {
        public Point startPoint { get; set; }
        public Point endPoint { get; set; }

        public Vector(double x1, double y1, double x2, double y2)
        {
            startPoint = new Point(x1, y1);
            endPoint = new Point(x2, y2);
        }
    }
}
