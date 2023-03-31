using Rajzi.Elements;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        List<Variable> variables = new List<Variable>();
        public MainWindow()
        {
            InitializeComponent();
            this.mainContainer.depth = 0;
            this.mainContainer.index = 0;
            this.selectedContainer = mainContainer;
            this.mainContainer.condition = true;
            this.mainContainer.panel = new StackPanel();
            MainCanvas.Children.Add(this.mainContainer.panel);
            var grid = Blocks.CreateBlockWithType(BlockType.Main, this.mainContainer, new MouseButtonEventHandler(OnElementClick), "Main", 0);
            ((Label)grid.Children[0]).Tag = this.mainContainer;
        }

        private void AddElement(object sender)
        {
            var eventHandler = new MouseButtonEventHandler(OnElementClick);
            BlockInput.GetInput((Label)sender, selectedElement, selectedContainer, eventHandler, variables);
        }

        public void Run()
        {
            Element? el = this.mainContainer;
            while (el != null)
            {
                if (el is Container)
                {
                    if (el != this.mainContainer)
                        ((Container)el).SetCondition();

                    if (((Container)el).condition && ((Container)el).firstChild != null)
                        el = ((Container)el).firstChild;
                    else
                        el = el.nextElement;
                }
                else
                {
                    ((Action)el).func((Action)el);

                    bool loop = false;
                    while (el != null && el.nextElement == null)
                    {
                        el = el.container;
                        if (el is Loop)
                        {
                            ((Container)el).SetCondition();
                            if (((Container)el).condition)
                            {
                                el = ((Container)el).firstChild;
                                loop = true;
                                break;
                            }
                        }
                    }
                    if (!loop && el != null)
                    {
                        el = el.nextElement;
                    }
                }
            }
        }

        private void OnBlockClick(object sender, MouseButtonEventArgs e)
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (selectedElement == selectedContainer)
                {
                    selectedContainer = (Container)selectedElement.container;
                }
                if (selectedElement != null)
                    selectedElement.RemoveElement();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Run();
        }
    }
}
