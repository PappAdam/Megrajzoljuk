using Rajzi.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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


namespace Rajzi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StackPanel containerObj;
        Statement mainContainer = new Statement();
        public MainWindow()
        {
            InitializeComponent();
            this.containerObj = MainBlockContainer;
            this.mainContainer.depth = 0;
            this.mainContainer.condition = true;
        }

        private Rectangle NewRectangle(int width, int height, Thickness margin, Brush fill)
        {
            Rectangle rect = new Rectangle();
            rect.Height = height;
            rect.Width = width;
            rect.Fill = fill;
            rect.HorizontalAlignment = HorizontalAlignment.Left;
            rect.Margin = margin;

            return rect;
        }

        private void AddElement(object sender)
        {
            switch (((Rectangle)sender).Name) {
                case "Variable":
                    break;

                case "Container":
                    //StackPanel newStackPanel = new StackPanel();
                    //Rectangle newContainerRect = NewRectangle(150, 20, new Thickness(20, 4, 0, 0), ((Rectangle)sender).Fill);
                    //newContainerRect.MouseLeftButtonDown += new MouseButtonEventHandler(OnStackPanelClick);
                    //newStackPanel.Children.Add(newContainerRect);
                    //this.containerObj.Children.Add(newStackPanel);
                    mainContainer.push(new Container());
                    break;

                case "Function":
                    //Rectangle newFnRect = NewRectangle(150, 20, new Thickness(20, 4, 0, 0), ((Rectangle)sender).Fill);
                    //this.containerObj.Children.Add(newFnRect);
                    //Function newFunction = new Function();
                    //newFunction.func = new Func<Pencil, bool>(pen => { return newFunction.variable != null; });
                    //elements.Add(newFunction);
                    mainContainer.push(new Function());
                    break;
            }

            Debug.Content = mainContainer.containedElementCount;
        }

        private void OnBlockDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                AddElement(sender);
            }
        }

        private void OnStackPanelClick(object sender, MouseButtonEventArgs e)
        {
            this.containerObj = (StackPanel)(((Rectangle)sender).Parent);
        }
    }
}
