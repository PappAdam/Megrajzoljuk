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
            Blocks.CreateBlockWithType(BlockType.Main, this.mainContainer);
            this.mainContainer.panel.Children[0].MouseLeftButtonDown += new MouseButtonEventHandler(OnStackPanelClick);
        }

        private void AddElement(object sender)
        {
            switch (((Rectangle)sender).Name) {
                case "Variable":
                    break;

                case "Container":
                    var c = new Container();
                    selectedContainer.push(c, ++index);
                    c.panel.Children[0].MouseLeftButtonDown += new MouseButtonEventHandler(OnStackPanelClick);
                    break;

                case "Function":
                    var f = new Function();
                    selectedContainer.push(f, ++index);
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

        public void OnStackPanelClick(object sender, MouseButtonEventArgs e)
        {
            object clickedElement = null;
            if (sender == ((StackPanel)((Grid)sender).Parent).Children[0])
                clickedElement = ((Grid)sender).Parent as StackPanel;
            else
                clickedElement = (Grid)sender;

            foreach (Element el in this.mainContainer)
            {
                if (clickedElement != null && el is Container && ((Container)el).panel == clickedElement)
                {
                    selectedContainer = (Container)el;
                    break;
                }
            }
        }
    }
}
