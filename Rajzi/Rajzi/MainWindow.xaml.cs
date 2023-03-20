﻿using Rajzi.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;


namespace Rajzi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Container selectedContainer;
        Element selectedElement;
        Statement mainContainer = new Statement();
        int index = 0;
        public MainWindow()
        {
            InitializeComponent();
            this.mainContainer.depth = 0;
            this.mainContainer.index = 0;
            this.selectedContainer = mainContainer;
            this.mainContainer.condition = true;
            this.mainContainer.panel = new StackPanel();
            MainCanvas.Children.Add(this.mainContainer.panel);
            var grid = Blocks.CreateBlockWithType(BlockType.Main, this.mainContainer, new MouseButtonEventHandler(OnElementClick), 0);
            ((Label)grid.Children[0]).Tag = this.mainContainer;
        }

        private void AddElement(object sender)
        {
            var eventHandler = new MouseButtonEventHandler(OnElementClick);
            switch (((Rectangle)sender).Name) {
                case "Variable":
                    if (selectedElement == null)
                        break;

                    Parameter param = new Parameter();
                    if (selectedElement is Parameter)
                    {
                        param.InitElement(selectedElement.container, eventHandler);
                        var ind = Grid.GetColumn(selectedElement.grid);
                        Element.AddParameter(param, ind, (Parameter)selectedElement);
                    }
                    break;

                case "Statement":
                    var c = new Statement();
                    selectedContainer.push(c, ++index, eventHandler);
                    break;

                case "Loop":
                    var l = new Loop();
                    selectedContainer.push(l, ++index, eventHandler);
                    break;

                case "Action":
                    var f = new Action();
                    selectedContainer.push(f, ++index, eventHandler);
                    break;
            }
        }

        public void Run()
        {
            Element? el = this.mainContainer;
            while (el != null)
            {
                if (el is Container && ((Container)el).firstChild != null)
                {
                    ((Container)el).SetCondition();
                    if (((Container)el).condition)
                        el = ((Container)el).firstChild;
                }
                else
                {
                    ((Action)el).func((Action)el);
                    while (el != null && el.nextElement == null)
                    {
                        el = el.container;
                    }

                    if (el != null)
                    {
                        el = el.nextElement;
                    }
                }
            }
        }

        private void OnBlockDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddElement(sender);
        }

        public void OnElementClick(object sender, MouseButtonEventArgs e)
        {
            selectedElement = (Element)((Label)sender).Tag;
            if (selectedElement is Container)
            {
                selectedContainer = (Container)selectedElement;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Run();
        }
    }
}
