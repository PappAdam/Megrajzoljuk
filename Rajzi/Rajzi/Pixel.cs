using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Rajzi
{
    internal class Pixel
    {
        public double pixellPositionX { get; set; }
        public double pixelPositionY { get; set; }
        
        public Pixel(double x, double y)
        {
            this.pixellPositionX = x;
            this.pixelPositionY = y;
        }

        struct Vec3
        {
            internal double x;
            internal double y;
        }

    }
}
