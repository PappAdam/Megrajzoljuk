using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Rajzi
{
    struct Vec3
    {
        internal double x;
        internal double y;
    }
    internal class Pixel
    {
        Vec3 pixelPosition = new Vec3 { x = 0, y = 0 };

        public Pixel(double x, double y)
        {
            this.pixelPosition.x = x;
            this.pixelPosition.y = y;
        }

        struct Vec3
        {
            internal double x;
            internal double y;
        }

    }
}
