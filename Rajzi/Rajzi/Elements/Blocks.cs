using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Rajzi.Elements
{
    public enum BlockType
    {
        Main,
        Statement,
        Loop,
        Variable,
        Action,
        Function
    }
    public class Blocks
    {
        static public void CreateBlockWithType(BlockType type, Container container)
        {
            Rectangle newRect = NewRectangle(150, 20, new Thickness());

            switch (type)
            {
                case BlockType.Main:
                    newRect.Fill = new SolidColorBrush(Color.FromRgb(41, 115, 115));
                    newRect.Margin = new Thickness();
                    break;
                case BlockType.Statement:
                    newRect.Fill = new SolidColorBrush(Color.FromRgb(194, 23, 23));
                    newRect.Margin = new Thickness(20 + 20 * (container.depth-1), 3, 0, 0);
                    break;
                case BlockType.Loop:
                    newRect.Fill = new SolidColorBrush(Color.FromRgb(194, 23, 23));
                    newRect.Margin = new Thickness(20 + 20 * (container.depth-1), 3, 0, 0);
                    break;
                case BlockType.Variable:
                    newRect.Margin = new Thickness(20 + 20 * (container.depth), 3, 0, 0);

                    newRect.Fill = new SolidColorBrush(Color.FromRgb(234, 146, 134));
                    break;
                case BlockType.Action:
                    newRect.Margin = new Thickness(20 + 20 * (container.depth), 3, 0, 0);
                    newRect.Fill = new SolidColorBrush(Color.FromRgb(233, 215, 88));
                    break;
                case BlockType.Function:
                    newRect.Margin = new Thickness(20 + 20 * (container.depth), 3, 0, 0);
                    newRect.Fill = new SolidColorBrush(Color.FromRgb(36, 200, 190));
                    break;
            }

            container.panel.Children.Add(newRect);
        }

        static public Rectangle NewRectangle(int width, int height, Thickness margin)
        {
            Rectangle rect = new Rectangle();
            rect.Height = height;
            rect.Width = width;
            rect.HorizontalAlignment = HorizontalAlignment.Left;
            rect.Margin = margin;

            return rect;
        }
    }

}
