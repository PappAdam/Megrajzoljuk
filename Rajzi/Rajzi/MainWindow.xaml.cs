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
using System.Windows.Media.Animation;
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
        Button activeMenuBtn;
        BrushConverter bc = new BrushConverter();     
        public MainWindow()
        {
            InitializeComponent();
            List<Element> elements = new List<Element>();
            for (int i = 0; i < 3; i++)
            {
                ColumnDefinition c = new ColumnDefinition();
                grToolbox.ColumnDefinitions.Add(c);
            }


        }

        private void RunDraw(object sender, RoutedEventArgs e)
        {
            RunWindow newWindow = new RunWindow();
            newWindow.Show();
        }

        //private void IsActive(object sender, RoutedEventArgs e)
        //{
        //    BrushConverter bc = new BrushConverter();
        //    if (activeBtn != null)
        //    {
        //        activeBtn.Background = (Brush)bc.ConvertFrom("#2F2235");
        //        activeBtn.Width = 110;
        //        if (activeBtn == sender)
        //        {
        //            activeBtn.Background = (Brush)bc.ConvertFrom("#2F2235");
        //            activeBtn.Width = 110;
        //            activeBtn = null;
        //            grMainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
        //            grMainGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Star);
        //            grMainGrid.ColumnDefinitions[2].Width = new GridLength(5, GridUnitType.Star);
        //        }
        //        else
        //        {
        //            activeBtn = sender as Button;
        //            activeBtn.Background = (Brush)bc.ConvertFrom("#EB9486");
        //            activeBtn.Width = 128;
        //            grMainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
        //            grMainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
        //            grMainGrid.ColumnDefinitions[2].Width = new GridLength(4, GridUnitType.Star);;                }
        //    }
        //    else
        //    {
        //        activeBtn = sender as Button;
        //        activeBtn.Background = (Brush)bc.ConvertFrom("#EB9486");
        //        activeBtn.Width = 128;
        //        grMainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
        //        grMainGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
        //        grMainGrid.ColumnDefinitions[2].Width = new GridLength(4, GridUnitType.Star);
        //    }

        //}

        public bool IsActive(Button btn)
        {
            switch (btn.Width)
            {
                case not 110:
                    return true;
                    break;
                default:
                    return false;
                    break;
            }
        }

        private void AddMenuContent(Button sender)
        {
            grToolbox.Children.Clear();

            

        }

        private void MenuOpt(object sender, RoutedEventArgs e)
        {
            //IsActive(sender, e);
            Button chosenMenuOpt = sender as Button;            
            switch (activeMenuBtn)
            {
                case not null:

                    if (activeMenuBtn == chosenMenuOpt)
                    {
                        BtnCollapseAnimation(activeMenuBtn);
                        activeMenuBtn = null;
                    }
                    else
                    {
                        BtnCollapseAnimation(activeMenuBtn);

                        BtnExpandAnimation(chosenMenuOpt);
                        activeMenuBtn = chosenMenuOpt;                        
                    }
                    break;
                default:
                    BtnExpandAnimation(chosenMenuOpt);
                    toolboxExpandAnimation(grMainGrid.ColumnDefinitions[1]);
                    activeMenuBtn = chosenMenuOpt;
                    break;
            }


        }

        private void BtnCollapseAnimation(Button activeBtn)
        {
            this.RegisterName(activeBtn.Name, activeBtn);
            DoubleAnimation btnCollapse = new DoubleAnimation();
            btnCollapse.From = 128;
            btnCollapse.To = 110;
            btnCollapse.Duration = new Duration(TimeSpan.FromMilliseconds(125));

            activeBtn.Background = (Brush)bc.ConvertFrom("#2F2235");

            // Configure the animation to target the button's Width property.
            Storyboard.SetTargetName(btnCollapse, activeBtn.Name);
            Storyboard.SetTargetProperty(btnCollapse, new PropertyPath(Button.WidthProperty));

            // Create a storyboard to contain the animation.
            Storyboard ButtonCollapseStorboard = new Storyboard();
            ButtonCollapseStorboard.Children.Add(btnCollapse);

            ButtonCollapseStorboard.Begin(activeBtn);
        }

        private void BtnExpandAnimation(Button nonActiveBtn)
        {
            this.RegisterName(nonActiveBtn.Name, nonActiveBtn);
            DoubleAnimation btnExpand = new DoubleAnimation();
            btnExpand.From = 110;
            btnExpand.To = 128;
            btnExpand.Duration = new Duration(TimeSpan.FromMilliseconds(125));

            nonActiveBtn.Background = (Brush)bc.ConvertFrom("#EB9486");

            // Configure the animation to target the button's Width property.
            Storyboard.SetTargetName(btnExpand, nonActiveBtn.Name);
            Storyboard.SetTargetProperty(btnExpand, new PropertyPath(Button.WidthProperty));

            // Create a storyboard to contain the animation.
            Storyboard ButtonExpandStorboard = new Storyboard();
            ButtonExpandStorboard.Children.Add(btnExpand);

            ButtonExpandStorboard.Begin(nonActiveBtn);
        }

        private void toolboxExpandAnimation(ColumnDefinition toolbox)
        {

            this.RegisterName(toolbox.Name, toolbox);
            DoubleAnimation toolboxExpand = new DoubleAnimation();
            toolboxExpand.From = toolbox.ActualWidth;
            toolboxExpand.To = gr_nav_holder.ActualWidth;
            
            toolboxExpand.Duration = new Duration(TimeSpan.FromMilliseconds(125));

            // Configure the animation to target the button's Width property.
            Storyboard.SetTargetName(toolboxExpand, toolbox.Name);
            Storyboard.SetTargetProperty(toolboxExpand, new PropertyPath(ColumnDefinition.WidthProperty));

            // Create a storyboard to contain the animation.
            Storyboard toolboxExpandeAnimation = new Storyboard();
            toolboxExpandeAnimation.Children.Add(toolboxExpand);

            toolboxExpandeAnimation.Begin(toolbox);


        }
    }
}
