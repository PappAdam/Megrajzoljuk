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
        List<Variable> variables = new List<Variable>();
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
            var grid = Blocks.CreateBlockWithType(BlockType.Main, this.mainContainer, new MouseButtonEventHandler(OnElementClick), "Main", 0);
            ((Label)grid.Children[0]).Tag = this.mainContainer;
        }

        private void AddElement(object sender)
        {
            var eventHandler = new MouseButtonEventHandler(OnElementClick);
            switch (((Label)sender).Name) {
                case "Input":
                    if (selectedElement == null)
                        break;

                    Parameter param = new Parameter();
                    if (selectedElement is Parameter)
                    {
                        param.createGrid(BlockType.Input, eventHandler);
                        param.InitElement(selectedElement.container, eventHandler, "Input", 1);
                        var ind = Grid.GetColumn(selectedElement.grid);
                        Element.AddParameter(param, ind, (Parameter)selectedElement, new Func<Variable, Variable>(variable =>
                        {
                            var value = ((TextBox)param.grid.Children[1]).Text;
                            Variable var = new Variable();
                            var.Type = VariableType.Bool;
                            var.value = new double[1];
                            
                            try
                            {
                                int val = Convert.ToInt32(value);
                                var.value = val;
                            }
                            catch
                            {
                                var.value = value;
                            }

                            return var;
                        }));
                    }
                    break;

                case "Statement":
                    var c = new Statement();
                    c.InitElement(selectedContainer, eventHandler, "If", 1);
                    selectedContainer.push(c, ++index);
                    break;

                case "Loop":
                    var l = new Loop();
                    l.InitElement(selectedContainer, eventHandler, "Loop", 3);
                    selectedContainer.push(l, ++index);
                    break;

                case "Action":
                    var f = new Action();
                    f.func = new Func<Action, bool>(act =>
                    {
                        MessageBox.Show($"{act.parameters[0].value(null).value}");

                        return true;
                    });

                    f.InitElement(selectedContainer, eventHandler, "Print", 1);
                    selectedContainer.push(f, ++index);
                    break;

                case "CreateVariable":
                    var cv = new Action();
                    Variable var = new Variable();
                    cv.func = new Func<Action, bool>(act =>
                    {
                        var.value = act.parameters[0].value(null).value;
                        var.name = (String)cv.parameters[1].value(null).value;
                        return true;
                    });

                    cv.InitElement(selectedContainer, eventHandler, "Add variable", 2);
                    selectedContainer.push(cv, ++index);
                    variables.Add(var);
                    break;

                case "SetVariable":
                    var set = new Action();
                    set.func = new Func<Action, bool>(act =>
                    {
                        Variable var = set.parameters[0].value(null);
                        var.value = set.parameters[1].value(null).value;
                        return true;
                    });

                    set.InitElement(selectedContainer, eventHandler, "Set variable", 2);
                    selectedContainer.push(set, ++index);
                    break;

                case "Variable":
                    if (selectedElement == null)
                        break;

                    Parameter getparam = new Parameter();
                    if (selectedElement is Parameter)
                    {
                        getparam.createGrid(BlockType.GetVariable, eventHandler);

                        getparam.InitElement(selectedElement.container, eventHandler, "Input");
                        var ind = Grid.GetColumn(selectedElement.grid);
                        Element.AddParameter(getparam, ind, (Parameter)selectedElement, new Func<Variable, Variable>(_ =>
                        {
                            var value = ((TextBox)getparam.grid.Children[1]).Text;

                            foreach (var v in variables)
                            {
                                if (v.name == value)
                                {
                                    return v;
                                }
                            }

                            throw new Exception("Failed to find variable");
                        }));
                    }
                    break;
            }
        }

        public void Run()
        {
            Element? el = this.mainContainer;
            while (el != null)
            {
                if (el is Container && ((Container)el).firstChild != null)
                {
                    if (el != this.mainContainer)
                        ((Container)el).SetCondition();


                    if (((Container)el).condition)
                        el = ((Container)el).firstChild;
                    else
                        el = el.nextElement;
                }
                else
                {
                    ((Action)el).func((Action)el);

                    while (el != null && el.nextElement == null)
                    {
                        el = el.container;
                    }
                    if (el != null)
                    {
                        el = el.nextElement;
                    }
                }
            }
        }

        private void OnBlockDoubleClick(object sender, MouseButtonEventArgs e)
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Run();
        }
    }
}
