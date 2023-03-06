﻿using System;
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
        public double pixelPositionX { get; set; } = 205;
        public double pixelPositionY { get; set; } = 310;
        public double size { get; set; } = 1;
        public Color color { get; set; } = Colors.White;

        public double rotate { get; set; } = -90;

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
        public void changeRotate(double rotate)
        {
            this.rotate = rotate - 90;
        }
        public void changePosition(double x, double y)
        {
            this.pixelPositionX = x;
            this.pixelPositionY = y;
        }
    }
}
