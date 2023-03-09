using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        static public Grid CreateBlockWithType(BlockType type, Container container)
        {

            Grid newGrid = CreateNewGrid(150, 30, new Thickness(20 * container.depth, 3, 0, 0));

            switch (type)
            {
                case BlockType.Main:
                    ChangeGrid(newGrid, Color.FromRgb(15, 115, 115), Color.FromRgb(240, 240, 240), "MAIN");
                    break;
                case BlockType.Statement:
                    break;
                case BlockType.Loop:
                    ChangeGrid(newGrid, Color.FromRgb(194, 23, 23), Color.FromRgb(240, 240, 240), "LOOP");
                    break;  
                case BlockType.Variable:
                    break;
                case BlockType.Action:
                    ChangeGrid(newGrid, Color.FromRgb(233, 215, 88), Color.FromRgb(12, 20, 99), "ACTION");
                    newGrid.Margin = new Thickness(20 + 20 * container.depth, 3, 0, 0);
                    break;
                case BlockType.Function:
                    break;
            }

            container.panel.Children.Add(newGrid);
            return newGrid;
        }

        static public Grid CreateNewGrid(int width, int height, Thickness margin)
        {
            Grid newGrid = new Grid();
            newGrid.Width = width;
            newGrid.Height = height;
            newGrid.Margin = margin;
            newGrid.HorizontalAlignment = HorizontalAlignment.Left;
            var colDef = new ColumnDefinition();
            colDef.Width = new GridLength(width/2);
            newGrid.ColumnDefinitions.Add(colDef);
            colDef = new ColumnDefinition();
            colDef.Width = GridLength.Auto;
            newGrid.ColumnDefinitions.Add(colDef);

            return newGrid;
        }

        static public void ChangeGrid(Grid grid, Color rFillColor, Color lForegroundC, String labelContent)
        {
            var rect = new Rectangle();
            rect.Fill = new SolidColorBrush(rFillColor);
            Grid.SetColumn(rect, 0);
            var label = new Label();
            label.Content = labelContent;
            label.Foreground = new SolidColorBrush(lForegroundC);
            label.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetColumn(label, 0);

            grid.Children.Add(rect);
            grid.Children.Add(label);
        }

        static public Grid ExpandGrid(Grid grid)
        {
            var expGrid = CreateNewGrid(150, 30, new Thickness());
            ChangeGrid(expGrid, Color.FromRgb(30, 20, 20), Color.FromRgb(200, 200, 200), "Expanded");
            Grid.SetColumn(expGrid, 1);

            grid.Children.Add(expGrid);
            return expGrid;
        }
    }
}
