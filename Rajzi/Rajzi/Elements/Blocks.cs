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

            Grid newGrid = CreateNewGrid(150, 30, new Thickness(20 + 20 * container.depth, 3, 0, 0));

            var label = new Label();
            switch (type)
            {
                case BlockType.Main:
                    newGrid.Background = new SolidColorBrush(Color.FromRgb(41, 115, 115));
                    label.Content = "MAIN";
                    label.Foreground = Brushes.WhiteSmoke;
                    newGrid.Children.Add(label);
                    break;
                case BlockType.Statement:

                    break;
                case BlockType.Loop:

                    break;
                case BlockType.Variable:

                    break;
                case BlockType.Action:

                    break;
                case BlockType.Function:

                    break;
            }

            container.panel.Children.Add(newGrid);
        }

        static public Grid CreateNewGrid(int width, int height, Thickness margin)
        {
            Grid newGrid = new Grid();
            newGrid.Width = width;
            newGrid.Height = height;
            newGrid.Margin = margin;
            newGrid.HorizontalAlignment = HorizontalAlignment.Left;
            var colDef = new ColumnDefinition();
            colDef.Width = GridLength.Auto;
            newGrid.ColumnDefinitions.Add(colDef);

            return newGrid;
        }
    }

}
