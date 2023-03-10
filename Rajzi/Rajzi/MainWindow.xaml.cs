using Rajzi.Elements;
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
            var grid = Blocks.CreateBlockWithType(BlockType.Main, this.mainContainer);
            grid.Tag = this.mainContainer;
            this.mainContainer.panel.Children[0].MouseLeftButtonDown += new MouseButtonEventHandler(OnBlockClick);
        }

        private void AddElement(object sender)
        {
            switch (((Rectangle)sender).Name) {
                case "Variable":
                    if (selectedElement == null)
                        break;


                    Parameter param = new Parameter();
                    param.InitElement(selectedElement);
                    selectedElement.AddParameter(param, 0);
                    selectedElement.parameters[0].grid.Children[0].MouseLeftButtonDown += new MouseButtonEventHandler(OnBlockClick);
                    ((Rectangle)selectedElement.parameters[0].grid.Children[0]).Tag = selectedElement.parameters[0];
                    break;

                case "Container":
                    var c = new Container();
                    selectedContainer.push(c, ++index);
                    ((Grid)c.panel.Children[0]).Children[0].MouseLeftButtonDown += new MouseButtonEventHandler(OnBlockClick);
                    break;

                case "Function":
                    var f = new Action();
                    selectedContainer.push(f, ++index);
                    f.grid.Children[0].MouseLeftButtonDown += new MouseButtonEventHandler(OnBlockClick);
                    break;
            }
        }

        private void OnBlockDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (e.ClickCount == 2)
            //{
                AddElement(sender);
            //}
        }

        public void OnBlockClick(object sender, MouseButtonEventArgs e)
        {
            selectedElement = (Element)((Grid)sender).Tag;
            if (selectedElement is Container)
                selectedContainer = (Container)selectedElement;
        }
    }
}
