using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Rajzi.Elements
{
    public enum BlockType
    {
        Main,
        Statement,
        Loop,
        GetVariable,
        Input,
        EmptyParam,
        Compare,
        Action,
        Logical,
        Math,
    }
    public class Blocks
    {
        static public Grid CreateBlockWithType(BlockType type, Container container, MouseEventHandler eventHandler, String name, int columns = 1)
        {

            Grid newGrid = CreateNewGrid(150, 30, container != null ? new Thickness(20 * container.depth, 3, 0, 0) : new Thickness(), columns + 1, eventHandler );

            switch (type)
            {
                case BlockType.Main:
                    ChangeGrid(newGrid, Color.FromRgb(15, 115, 115), Color.FromRgb(240, 240, 240), name);
                    break;

                case BlockType.Statement:
                    ChangeGrid(newGrid, Color.FromRgb(194, 23, 23), Color.FromRgb(240, 240, 240), name);
                    break;

                case BlockType.Loop:
                    ChangeGrid(newGrid, Color.FromRgb(194, 23, 23), Color.FromRgb(240, 240, 240), name);
                    break;

                case BlockType.Input:
                    ChangeGrid(newGrid, Color.FromRgb(20, 50, 88), Color.FromRgb(240, 240, 240), name);
                    var tb = new TextBox();
                    tb.Width = 60;
                    Grid.SetColumn(tb, 1);
                    var cd = new ColumnDefinition();
                    newGrid.ColumnDefinitions.Add(cd);
                    newGrid.Children.Add(tb);
                    break;

                case BlockType.GetVariable:
                    ChangeGrid(newGrid, Color.FromRgb(20, 50, 88), Color.FromRgb(240, 240, 240), name);
                    var tb1 = new TextBox();
                    tb1.Width = 60;
                    Grid.SetColumn(tb1, 1);
                    var cd1 = new ColumnDefinition();
                    newGrid.ColumnDefinitions.Add(cd1);
                    newGrid.Children.Add(tb1);
                    break;

                case BlockType.EmptyParam:
                    ChangeGrid(newGrid, Color.FromRgb(60, 60, 60), Color.FromRgb(240, 240, 240), name);
                    break;

                case BlockType.Compare:
                    ChangeGrid(newGrid, Color.FromRgb(20, 50, 88), Color.FromRgb(240, 240, 240), name);
                    var compSign = new ComboBox();
                    compSign.Width = 50;
                    compSign.Items.Add("=");
                    compSign.Items.Add("!=");
                    compSign.Items.Add(">=");
                    compSign.Items.Add("<=");
                    compSign.Items.Add("<");
                    compSign.Items.Add(">");
                    var compSignCD = new ColumnDefinition();
                    newGrid.ColumnDefinitions.Add(compSignCD);
                    newGrid.Children.Add(compSign);
                    Grid.SetColumn(compSign, 3);
                    break;

                case BlockType.Logical:
                    ChangeGrid(newGrid, Color.FromRgb(20, 50, 88), Color.FromRgb(240, 240, 240), name);
                    var logOp = new ComboBox();
                    logOp.Width = 50;
                    logOp.Items.Add("OR");
                    logOp.Items.Add("AND");
                    logOp.Items.Add("XOR");

                    var logOpCD = new ColumnDefinition();
                    newGrid.ColumnDefinitions.Add(logOpCD);
                    newGrid.Children.Add(logOp);
                    Grid.SetColumn(logOp, 3);
                    break;

                case BlockType.Action:
                    ChangeGrid(newGrid, Color.FromRgb(233, 215, 88), Color.FromRgb(12, 20, 99), name);
                    newGrid.Margin = new Thickness(20 + 20 * container.depth, 3, 0, 0);
                    break;

                case BlockType.Math:
                    ChangeGrid(newGrid, Color.FromRgb(129, 30, 60), Color.FromRgb(30, 120, 80), name);
                    break;
            }

            if (container != null) 
                container.panel.Children.Add(newGrid);
            return newGrid;
        }

        static public Grid CreateNewGrid(int width, int height, Thickness margin, int columns, MouseEventHandler eventHandler)
        {
            Grid newGrid = new Grid();
            newGrid.Height = height;
            newGrid.Margin = margin;
            newGrid.HorizontalAlignment = HorizontalAlignment.Left;
            ColumnDefinition colDef = new ColumnDefinition();
            newGrid.ColumnDefinitions.Add(colDef);
            newGrid.Children.Add(new Label());
            newGrid.Children[0].MouseEnter += eventHandler;

            for (int i = 0; i < columns-1; i++)
            {
                var grid = CreateBlockWithType(BlockType.EmptyParam, null, eventHandler, "param", 0);
                colDef = new ColumnDefinition();
                Grid.SetColumn(grid, i + 1);
                newGrid.ColumnDefinitions.Add(colDef);
                grid.Children[0].MouseEnter += eventHandler;
                newGrid.Children.Add(grid);
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

        static public void ExpandGrid(Grid grid, int index, Parameter clickedParam)
        {
            Grid.SetColumn(grid, index);

            if (clickedParam != null)
            {
                Grid dstGrid = clickedParam.container.grid;
                dstGrid.Children.RemoveAt(index);
                dstGrid.Children.Insert(index, grid);
            }
        }
    }
}
