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
        bool startupResize = true;        
        public MainWindow()
        {
            InitializeComponent();
            grContent.ColumnDefinitions[0].Width = new GridLength(130, GridUnitType.Pixel);
            grContent.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Pixel);
            List<Element> elements = new List<Element>();;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            AdjustWindowSize();
        }

        /// <summary>
        /// Minimized Button_Clicked
        /// </summary>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Adjusts the WindowSize to correct parameters when Maximize button is clicked
        /// </summary>
        private void AdjustWindowSize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                MaxButton.Content = "1";
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                MaxButton.Content = "2";
            }

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void RunDraw(object sender, RoutedEventArgs e)
        {
            RunWindow newWindow = new RunWindow();
            newWindow.Show();
        }

        public bool IsActive(Button btn)
        {
            switch (btn.Width)
            {
                case not 110:
                    return true;
                default:
                    return false;
            }
        }

        private void AddMenuContent(Button sender)
        {
            stckToolbox.Children.Clear();
            for (int element = 0; element < 4; element++)
            {
                Label lbl = new Label();
                lbl.Height = 50;
                lbl.Width = 150;
                lbl.Content = $"opt_{element}";
                lbl.FontSize = 24;
                lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                lbl.VerticalContentAlignment = VerticalAlignment.Center;
                stckToolbox.Children.Add(lbl);
            }
        }

        private void MenuOpt(object sender, RoutedEventArgs e)
        {
            Button chosenMenuOpt = sender as Button;            
            switch (activeMenuBtn)
            {
                case not null:

                    if (activeMenuBtn == chosenMenuOpt)
                    {
                        BtnCollapseAnimation(activeMenuBtn);
                        toolboxCollapseAnimation(stckToolbox);
                        canvasExpandAnimation(Canvas);
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
                    canvasCollapseAnimation(Canvas);
                    toolboxExpandAnimation(stckToolbox);
                    AddMenuContent(chosenMenuOpt);
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

            Storyboard.SetTargetName(btnExpand, nonActiveBtn.Name);
            Storyboard.SetTargetProperty(btnExpand, new PropertyPath(Button.WidthProperty));

            Storyboard ButtonExpandStorboard = new Storyboard();
            ButtonExpandStorboard.Children.Add(btnExpand);

            ButtonExpandStorboard.Begin(nonActiveBtn);
        }

        private void toolboxExpandAnimation(StackPanel toolbox)
        {   
            stckToolbox.Height = grContent.ActualHeight*0.97;
            this.RegisterName(toolbox.Name, toolbox);
            DoubleAnimation toolboxExpand = new DoubleAnimation();
            toolboxExpand.From = toolbox.Width;
            toolboxExpand.To = 235;
            toolboxExpand.Duration = new Duration(TimeSpan.FromMilliseconds(250));

            Storyboard.SetTargetName(toolboxExpand, toolbox.Name);
            Storyboard.SetTargetProperty(toolboxExpand, new PropertyPath(StackPanel.WidthProperty));

            Storyboard toolboxExpandeAnimation = new Storyboard();
            toolboxExpandeAnimation.Children.Add(toolboxExpand);

            toolboxExpandeAnimation.Begin(toolbox);

        }
        private void toolboxCollapseAnimation(StackPanel toolbox)
        {
            toolbox.Children.Clear();
            this.RegisterName(toolbox.Name, toolbox);
            DoubleAnimation toolboxExpand = new DoubleAnimation();
            toolboxExpand.From = toolbox.ActualWidth;
            toolboxExpand.To = 0;
            toolboxExpand.Duration = new Duration(TimeSpan.FromMilliseconds(250));

            Storyboard.SetTargetName(toolboxExpand, toolbox.Name);
            Storyboard.SetTargetProperty(toolboxExpand, new PropertyPath(StackPanel.WidthProperty));

            Storyboard toolboxCollapseAnimation = new Storyboard();
            toolboxCollapseAnimation.Children.Add(toolboxExpand);

            toolboxCollapseAnimation.Begin(toolbox);
        }
        private void canvasCollapseAnimation(Canvas canvas)
        {
            canvas.HorizontalAlignment = HorizontalAlignment.Right;
            this.RegisterName(canvas.Name, canvas);
            DoubleAnimation canvasCollapse = new DoubleAnimation();

            canvasCollapse.From = canvas.ActualWidth;
            canvasCollapse.To = grContent.ActualWidth-380;
            canvasCollapse.Duration = new Duration(TimeSpan.FromMilliseconds(250));

            Storyboard.SetTargetName(canvasCollapse, canvas.Name);
            Storyboard.SetTargetProperty(canvasCollapse, new PropertyPath(Canvas.WidthProperty));

            Storyboard canvasCollapseAnimation = new Storyboard();
            canvasCollapseAnimation.Children.Add(canvasCollapse);

            canvasCollapseAnimation.Begin(canvas);

            grContent.ColumnDefinitions[0].Width = new GridLength(130, GridUnitType.Pixel);
            grContent.ColumnDefinitions[1].Width = new GridLength(250, GridUnitType.Pixel);
        }
        private void canvasExpandAnimation(Canvas canvas)
        {
            canvas.HorizontalAlignment = HorizontalAlignment.Right;
            this.RegisterName(canvas.Name, canvas);
            DoubleAnimation canvasCollapse = new DoubleAnimation();

            canvasCollapse.From = canvas.ActualWidth;
            canvasCollapse.To = grContent.ActualWidth - gr_nav_holder.ActualWidth;
            canvasCollapse.Duration = new Duration(TimeSpan.FromMilliseconds(250));

            Storyboard.SetTargetName(canvasCollapse, canvas.Name);
            Storyboard.SetTargetProperty(canvasCollapse, new PropertyPath(Canvas.WidthProperty));

            Storyboard canvasCollapseAnimation = new Storyboard();
            canvasCollapseAnimation.Children.Add(canvasCollapse);

            canvasCollapseAnimation.Begin(canvas);
        }

        private void canvasExpandResize(Canvas canvas)
        {
            canvas.HorizontalAlignment = HorizontalAlignment.Right;
            this.RegisterName(canvas.Name, canvas);
            DoubleAnimation canvasCollapse = new DoubleAnimation();
            canvasCollapse.From = canvas.ActualWidth;
            switch (activeMenuBtn)
            {
                case null:
                    canvasCollapse.To = grContent.ActualWidth - stckToolbox.ActualWidth - gr_nav_holder.ActualWidth;
                    break;
                case not null:
                    canvasCollapse.To = grContent.ActualWidth - stckToolbox.ActualWidth - gr_nav_holder.ActualWidth -15;
                    break;
                default:
                    break;
            }
            canvasCollapse.Duration = new Duration(TimeSpan.FromMilliseconds(1));

            Storyboard.SetTargetName(canvasCollapse, canvas.Name);
            Storyboard.SetTargetProperty(canvasCollapse, new PropertyPath(Canvas.WidthProperty));

            Storyboard canvasCollapseAnimation = new Storyboard();
            canvasCollapseAnimation.Children.Add(canvasCollapse);

            canvasCollapseAnimation.Begin(canvas);
        }

        private void canvasCollapseResize(Canvas canvas)
        {
            canvas.HorizontalAlignment = HorizontalAlignment.Right;
            this.RegisterName(canvas.Name, canvas);
            DoubleAnimation canvasCollapse = new DoubleAnimation();
            canvasCollapse.From = canvas.ActualWidth;
            switch (activeMenuBtn)
            {
                case null:
                    canvasCollapse.To = grContent.ActualWidth - stckToolbox.ActualWidth - gr_nav_holder.ActualWidth;
                    break;
                case not null:
                    canvasCollapse.To = grContent.ActualWidth - stckToolbox.ActualWidth - gr_nav_holder.ActualWidth - 15;
                    break;
                default:
                    break;
            }
            canvasCollapse.Duration = new Duration(TimeSpan.FromMilliseconds(1));

            Storyboard.SetTargetName(canvasCollapse, canvas.Name);
            Storyboard.SetTargetProperty(canvasCollapse, new PropertyPath(Canvas.WidthProperty));

            Storyboard canvasCollapseAnimation = new Storyboard();
            canvasCollapseAnimation.Children.Add(canvasCollapse);

            canvasCollapseAnimation.Begin(canvas);

            grContent.ColumnDefinitions[0].Width = new GridLength(130, GridUnitType.Pixel);
            grContent.ColumnDefinitions[1].Width = new GridLength(250, GridUnitType.Pixel);
        }

        private void sizeChange(object sender, SizeChangedEventArgs e)
        {
            stckToolbox.Height = grContent.ActualHeight*0.97;
            switch (startupResize)
            {
                case false:
                    if (Canvas.ActualWidth < grContent.ActualWidth -(stckToolbox.ActualWidth+gr_nav_holder.ActualWidth))
                    {
                        canvasExpandResize(Canvas);
                    }
                    else if (Canvas.ActualWidth > grContent.ActualWidth - (stckToolbox.ActualWidth + gr_nav_holder.ActualWidth))
                    {
                        canvasCollapseResize(Canvas);
                    }
                    break;
                case true:
                    startupResize = false;
                    break;
                default:
                    break;
            }
        }
    }
}
