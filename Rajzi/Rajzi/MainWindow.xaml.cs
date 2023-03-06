using System;
using System.Collections.Generic;
using System.Linq;
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
        Button activeBtn;
        public MainWindow()
        {
            InitializeComponent();
            List<Element> elements = new List<Element>();
        }

        private void RunDraw(object sender, RoutedEventArgs e)
        {
            RunWindow newWindow = new RunWindow();
            newWindow.Show();
        }

        private void IsActive(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            if (activeBtn != null)
            {
                activeBtn.Background = (Brush)bc.ConvertFrom("#2F2235");
                activeBtn.Width = 110;
                if (activeBtn == sender)
                {
                    activeBtn.Background = (Brush)bc.ConvertFrom("#2F2235");
                    activeBtn.Width = 110;
                    activeBtn = null;
                    grMainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                    grMainGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Star);
                    grMainGrid.ColumnDefinitions[2].Width = new GridLength(5, GridUnitType.Star);
                }
                else
                {
                    activeBtn = sender as Button;
                    activeBtn.Background = (Brush)bc.ConvertFrom("#EB9486");
                    activeBtn.Width = 128;
                    grMainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                    grMainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                    grMainGrid.ColumnDefinitions[2].Width = new GridLength(4, GridUnitType.Star);;
                }
            }
            else
            {
                activeBtn = sender as Button;
                activeBtn.Background = (Brush)bc.ConvertFrom("#EB9486");
                activeBtn.Width = 128;
                grMainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                grMainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                grMainGrid.ColumnDefinitions[2].Width = new GridLength(4, GridUnitType.Star);
            }
        }

        private void MenuOpt(object sender, RoutedEventArgs e)
        {
            IsActive(sender, e);
        }
    }
}
