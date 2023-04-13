using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Rajzi
{
    internal class Pencil
    {
        public double pixelPositionX { get; set; } = 205;
        public double pixelPositionY { get; set; } = 310;
        public double size { get; set; } = 1;
        public Color color { get; set; } = Colors.White;

        public double rotate { get; set; } = -90;
        public bool polygon { get; set; } = false;

        public Pencil()
        {
        }
    }
}