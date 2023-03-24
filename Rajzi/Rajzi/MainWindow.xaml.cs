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
        bool menuIsActive = false;
        public MainWindow()
        {
            InitializeComponent();
            grMainGrid.ColumnDefinitions[0].Width = new GridLength(130, GridUnitType.Pixel);
            grMainGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Pixel);
            List<Element> elements = new List<Element>();
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
            grToolbox.Children.Clear();
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
                        toolboxCollapseAnimation(grToolbox);
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
                    toolboxExpandAnimation(grToolbox);
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

        private void toolboxExpandAnimation(Grid toolbox)
        {   
            grToolbox.Height = grMainGrid.ActualHeight*0.97;
            this.RegisterName(toolbox.Name, toolbox);
            DoubleAnimation toolboxExpand = new DoubleAnimation();
            toolboxExpand.From = toolbox.Width;
            toolboxExpand.To = gr_nav_holder.ActualWidth-15;
            toolboxExpand.Duration = new Duration(TimeSpan.FromMilliseconds(250));

            Storyboard.SetTargetName(toolboxExpand, toolbox.Name);
            Storyboard.SetTargetProperty(toolboxExpand, new PropertyPath(Grid.WidthProperty));

            Storyboard toolboxExpandeAnimation = new Storyboard();
            toolboxExpandeAnimation.Children.Add(toolboxExpand);

            toolboxExpandeAnimation.Begin(toolbox);

            menuIsActive = true;

        }
        private void toolboxCollapseAnimation(Grid toolbox)
        {
            this.RegisterName(toolbox.Name, toolbox);
            DoubleAnimation toolboxExpand = new DoubleAnimation();
            toolboxExpand.From = toolbox.ActualWidth;
            toolboxExpand.To = 0;
            toolboxExpand.Duration = new Duration(TimeSpan.FromMilliseconds(250));

            Storyboard.SetTargetName(toolboxExpand, toolbox.Name);
            Storyboard.SetTargetProperty(toolboxExpand, new PropertyPath(Grid.WidthProperty));

            Storyboard toolboxCollapseAnimation = new Storyboard();
            toolboxCollapseAnimation.Children.Add(toolboxExpand);

            toolboxCollapseAnimation.Begin(toolbox);

            menuIsActive = false;
        }
        private void canvasCollapseAnimation(Canvas canvas)
        {
            canvas.HorizontalAlignment = HorizontalAlignment.Right;
            this.RegisterName(canvas.Name, canvas);
            DoubleAnimation canvasCollapse = new DoubleAnimation();
            canvasCollapse.From = canvas.ActualWidth;
            canvasCollapse.To = grMainGrid.ActualWidth-260;
            canvasCollapse.Duration = new Duration(TimeSpan.FromMilliseconds(250));

            Storyboard.SetTargetName(canvasCollapse, canvas.Name);
            Storyboard.SetTargetProperty(canvasCollapse, new PropertyPath(Canvas.WidthProperty));

            Storyboard canvasCollapseAnimation = new Storyboard();
            canvasCollapseAnimation.Children.Add(canvasCollapse);

            canvasCollapseAnimation.Begin(canvas);

            grMainGrid.ColumnDefinitions[0].Width = new GridLength(130, GridUnitType.Pixel);
            grMainGrid.ColumnDefinitions[1].Width = new GridLength(130, GridUnitType.Pixel);

            Canvas.Width = canvas.ActualWidth;
        }
        private void canvasExpandAnimation(Canvas canvas)
        {
            canvas.HorizontalAlignment = HorizontalAlignment.Right;
            this.RegisterName(canvas.Name, canvas);
            DoubleAnimation canvasCollapse = new DoubleAnimation();
            canvasCollapse.From = canvas.ActualWidth;
            canvasCollapse.To = grMainGrid.ActualWidth - 130;
            canvasCollapse.Duration = new Duration(TimeSpan.FromMilliseconds(250));

            Storyboard.SetTargetName(canvasCollapse, canvas.Name);
            Storyboard.SetTargetProperty(canvasCollapse, new PropertyPath(Canvas.WidthProperty));

            Storyboard canvasCollapseAnimation = new Storyboard();
            canvasCollapseAnimation.Children.Add(canvasCollapse);

            canvasCollapseAnimation.Begin(canvas);

            Canvas.Width = canvas.ActualWidth;
        }

        private void sizeChange(object sender, SizeChangedEventArgs e)
        {
            grToolbox.Height = grMainGrid.ActualHeight*0.97;
            //Canvas.Width = grMainGrid.ActualWidth - (grToolbox.ActualWidth + gr_nav_holder.ActualWidth);
            //MessageBox.Show($"grMain {grMainGrid.ActualWidth}, toolbox{grToolbox.ActualWidth}, navHolder{gr_nav_holder.ActualWidth} => {grMainGrid.ActualWidth - (grToolbox.ActualWidth + gr_nav_holder.ActualWidth)}, canvas {Canvas.ActualWidth}");
        }
    }
}
