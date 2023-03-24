using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rajzi
{
    internal class Pixel
    {
        public double pixelPositionX { get; set; }
        public double pixelPositionY { get; set; }

        public Pixel(double x, double y)
        {
            this.pixelPositionX = x;
            this.pixelPositionY = y;
        }

        struct Vec3
        {
            internal double x;
            internal double y;
        }

    }
}
