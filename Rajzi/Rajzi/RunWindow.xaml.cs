using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Rajzi
{
    /// <summary>
    /// Interaction logic for RunWindow.xaml
    /// </summary>
    public partial class RunWindow : Window
    {
        public RunWindow()
        {
            InitializeComponent();
            TextBox textBox = new TextBox { Text = "Test" };
            Canvas.Children.Add(textBox);
        }

        private void MouseScroll(object sender, MouseWheelEventArgs e)
        {
            Point mousePosition = Mouse.GetPosition(Screen);
            Teszt.Content = mousePosition.X - Screen.Width / 5;
            Teszt2.Content = mousePosition.Y;
            ScaleTransform.CenterX = mousePosition.X - Screen.Width / 5;
            ScaleTransform.CenterY = mousePosition.Y;
            if (e.Delta > 0)
            {
                ScaleTransform.ScaleX *= 1.1;
                ScaleTransform.ScaleY *= 1.1;
            }
            else
            {
                ScaleTransform.ScaleX /= 1.1;
                ScaleTransform.ScaleY /= 1.1;
            }
        }
    }
}
