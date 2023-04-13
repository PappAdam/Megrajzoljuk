using Rajzi.Elements;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
        Label droppedLabel;
        Button activeMenuBtn;
        Statement mainContainer = new Statement();
        List<Variable> variables = new List<Variable>();
        bool isDragging = false;
        BrushConverter bc = new BrushConverter();
        bool startupResize = true;

        public string[] menoOptContent =
        {
            "Action_Print;While_While Loop;Statement_If statement",
            "CreateVariable_Add variable;Input_Get Input;Variable_Get Variable;SetVariable_Set Variable",
            "Compare_Compare;Add_Add;Subtr_Subtract;Multip_Multiply;Divide_Divide;Logical_Logical",
            "Forward_Forward;PencilSize_Pencil Size;Rotate_Rotate;Polygon_Polygon;Color_Color;PencilPosition_Pencil Position;goToLine_go To Line"
        };

        public MainWindow()
        {
            InitializeComponent();
            this.mainContainer.depth = 0;
            this.mainContainer.index = 0;
            this.selectedContainer = mainContainer;
            this.mainContainer.condition = true;
            this.mainContainer.panel = new StackPanel();
            MainCanvas.Children.Add(this.mainContainer.panel);
            var grid = Blocks.CreateBlockWithType(BlockType.Main, this.mainContainer, new MouseEventHandler(OnHover), "Main", 0);
            ((Label)grid.Children[0]).Tag = this.mainContainer;
            grContent.ColumnDefinitions[0].Width = new GridLength(130, GridUnitType.Pixel);
            grContent.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Pixel); 
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

        private void AddMenuContent(Button sender, string[] contents)
        {
            stckToolbox.Children.Clear();

            int activeBtnNum = int.Parse(sender.Name.ToString().Remove(0, sender.Name.Length - 1));
            string[] actContent = contents[activeBtnNum - 1].Split(';');
            for (int c = 0; c < actContent.Length; c++)
            {
                Label lbl = new Label();
                   
                lbl.Height = 50;                
                lbl.Width = 150;                
                string[] splittedElement = actContent[c].Split("_");
                lbl.Name= splittedElement[0];
                lbl.Content = $"{splittedElement[1]}";
                lbl.MouseLeftButtonDown += new MouseButtonEventHandler(onDragStart);
                lbl.FontSize = 18;
                Thickness margin = lbl.Margin;
                margin.Top = 15;
                lbl.Margin = margin;
                lbl.Background = (Brush)bc.ConvertFrom("#E9D758");
                //Style = "{StaticResource MenuOptLblStyle}"
                Style style = this.FindResource("MenuOptLblStyle") as Style;
                lbl.Style = style;

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
                        borderCollapseAnimation(brToolbox);
                        toolboxCollapseAnimation(stckToolbox);
                        canvasExpandAnimation(MainCanvas);
                        activeMenuBtn = null;
                    }
                    else
                    {
                        BtnCollapseAnimation(activeMenuBtn);
                        BtnExpandAnimation(chosenMenuOpt);
                        AddMenuContent(chosenMenuOpt, menoOptContent);
                        activeMenuBtn = chosenMenuOpt;
                    }
                    break;
                default:
                    BtnExpandAnimation(chosenMenuOpt);                    
                    canvasCollapseAnimation(MainCanvas);
                    borderExpandeAnimation(brToolbox);
                    toolboxExpandAnimation(stckToolbox);
                    AddMenuContent(chosenMenuOpt, menoOptContent);
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
            toolboxExpand.To = 200;            
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

        private void borderExpandeAnimation(Border border)
        {
            brToolbox.Height = grContent.ActualHeight * 0.97;
            this.RegisterName(border.Name, border);

            DoubleAnimation borderExpand = new DoubleAnimation();

            borderExpand.From = border.Width;
            borderExpand.To = 200;
            borderExpand.Duration = new Duration(TimeSpan.FromMilliseconds(250));

            Storyboard.SetTargetName(borderExpand, border.Name);
            Storyboard.SetTargetProperty(borderExpand, new PropertyPath(Border.WidthProperty));
            Storyboard borderExpandeAnimation = new Storyboard();
            borderExpandeAnimation.Children.Add(borderExpand);

            borderExpandeAnimation.Begin(border);
        }

        private void borderCollapseAnimation(Border border)
        {
            this.RegisterName(border.Name, border);

            DoubleAnimation borderCollapse = new DoubleAnimation();

            borderCollapse.From = border.Width;
            borderCollapse.To = 0;
            borderCollapse.Duration = new Duration(TimeSpan.FromMilliseconds(250));

            Storyboard.SetTargetName(borderCollapse, border.Name);
            Storyboard.SetTargetProperty(borderCollapse, new PropertyPath(Border.WidthProperty));
            Storyboard borderCollapseAnimation = new Storyboard();
            borderCollapseAnimation.Children.Add(borderCollapse);

            borderCollapseAnimation.Begin(border);
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
                    if (MainCanvas.ActualWidth < grContent.ActualWidth -(stckToolbox.ActualWidth+gr_nav_holder.ActualWidth))
                    {
                        canvasExpandResize(MainCanvas);
                    }
                    else if (MainCanvas.ActualWidth > grContent.ActualWidth - (stckToolbox.ActualWidth + gr_nav_holder.ActualWidth))
                    {
                        canvasCollapseResize(MainCanvas);
                    }
                    break;
                case true:
                    startupResize = false;
                    break;
            }
        }

        private void AddElement(object sender)
        {
            var eventHandler = new MouseEventHandler(OnHover);
            var manageElement = new MouseButtonEventHandler(ManageElement);
            BlockInput.GetInput((Label)sender, selectedElement, selectedContainer, eventHandler, manageElement, variables);
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

        public void OnHover(object sender, MouseEventArgs e)
        {
            selectedElement = (Element)((Label)sender).Tag;
            if (selectedElement is Container)
            {
                selectedContainer = (Container)selectedElement;                
            }
            else
            {
                var containerTemp = selectedElement.container;

                while (containerTemp is not Container)
                {
                    containerTemp = containerTemp.container;
                }

                selectedContainer = containerTemp as Container;
            }
        }

        public void ManageElement(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                selectedElement.RemoveElement();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Delete)
            //{
            //    if (selectedElement == selectedContainer)
            //    {
            //        selectedContainer = (Container)selectedElement.container;
            //    }
            //    if (selectedElement != null)
            //        selectedElement.RemoveElement();
            //}
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BlockInput.win3.Hide();
            BlockInput.win3 = new RunWindow();
            BlockInput.win3.Show();
            this.Run();
        }

        private void onDragStart(object sender, MouseEventArgs e)
        {
            droppedLabel = sender as Label;
            selectedElement = null;
            selectedContainer = null;
            isDragging= true;
        }

        private void DragStop(object sender, MouseButtonEventArgs e)
        {
            if (isDragging == true)
            {
                isDragging = false;
                if (selectedContainer != null)
                    AddElement(droppedLabel);
            }
        }
    }
}
