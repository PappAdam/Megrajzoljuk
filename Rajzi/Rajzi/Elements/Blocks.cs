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
        static public Grid CreateBlockWithType(BlockType type, Container container, int columns = 4)
        {

            Grid newGrid = CreateNewGrid(150, 30, container != null ? new Thickness(20 * container.depth, 3, 0, 0) : new Thickness(), columns );

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
                    ChangeGrid(newGrid, Color.FromRgb(20, 50, 88), Color.FromRgb(240, 240, 240), "VARIABLE");
                    break;
                case BlockType.Action:
                    ChangeGrid(newGrid, Color.FromRgb(233, 215, 88), Color.FromRgb(12, 20, 99), "ACTION");
                    newGrid.Margin = new Thickness(20 + 20 * container.depth, 3, 0, 0);
                    break;
                case BlockType.Function:
                    break;
            }


            if (container != null) 
                container.panel.Children.Add(newGrid);
            return newGrid;
        }

        static public Grid CreateNewGrid(int width, int height, Thickness margin, int columns)
        {
            Grid newGrid = new Grid();
            newGrid.Height = height;
            newGrid.Margin = margin;
            newGrid.HorizontalAlignment = HorizontalAlignment.Left;
            ColumnDefinition colDef = new ColumnDefinition();
            newGrid.ColumnDefinitions.Add(colDef);
            newGrid.Children.Add(new Label());

            for (int i = 0; i < columns-1; i++)
            {
                var label = new Label();
                label.Background = new SolidColorBrush(Color.FromRgb(Convert.ToByte(255 / columns * i), Convert.ToByte(255 / columns * i), Convert.ToByte(255 / columns * i)));
                colDef = new ColumnDefinition();
                colDef.Width = new GridLength(30);
                Grid.SetColumn(label, i + 1);
                newGrid.ColumnDefinitions.Add(colDef);
                newGrid.Children.Add(label);
            }

            return newGrid;
        }

        static public void ChangeGrid(Grid grid, Color rFillColor, Color lForegroundC, String labelContent, int index = 0)
        {
            var label = (Label)grid.Children[0];
            label.Background = new SolidColorBrush(rFillColor);
            label.Content = labelContent;
            label.Foreground = new SolidColorBrush(lForegroundC);
            label.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetColumn(label, index);

            label.Loaded += (sender, e) =>
            {
                double labelWidth = label.ActualWidth;
                grid.ColumnDefinitions[index].Width = new GridLength(labelWidth);
            };
        }

        static public void ExpandGrid(Grid srcGrid, Grid dstGrid, int index)
        {
            Grid.SetColumn(dstGrid, index + 1);

            srcGrid.Children[index + 1].Visibility = Visibility.Collapsed;
            srcGrid.Children.RemoveAt(index+1);
            srcGrid.Children.Insert(index+1, dstGrid);
        }
    }
}
