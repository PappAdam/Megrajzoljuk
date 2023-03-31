using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace Rajzi.Elements
{
    public class BlockInput
    {
        public static void GetInput(Label sender, Element selectedElement, Container selectedContainer, MouseButtonEventHandler eventHandler, List<Variable> variables)
        {
            switch (sender.Name)
            {
                case "Input":
                    if (selectedElement == null)
                        break;

                    Parameter param = new Parameter();
                    if (selectedElement is Parameter)
                    {
                        param.createGrid(BlockType.Input, eventHandler, sender.Name, 0);
                        param.InitElement(selectedElement.container, eventHandler);
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
                                if (value == "true")
                                {
                                    var.Type = VariableType.Bool;
                                    var.value = true;
                                }
                                else if (value == "false") {
                                    var.Type = VariableType.Bool;
                                    var.value = false;
                                }
                                else
                                {
                                    var.Type = VariableType.String;
                                    var.value = value;
                                }
                            }

                            return var;
                        }));
                    }
                    break;

                case "Statement":
                    var c = new Statement();
                    c.InitElement(selectedContainer, eventHandler, "If", 1);
                    selectedContainer.push(c);
                    break;

                case "While":
                    var l = new Loop();
                    l.InitElement(selectedContainer, eventHandler, "While", 1);
                    selectedContainer.push(l);
                    break;

                case "Action":
                    var f = new Action();
                    f.func = new Func<Action, bool>(act =>
                    {
                        MessageBox.Show($"{act.parameters[0].value(null).value}");

                        return true;
                    });

                    f.InitElement(selectedContainer, eventHandler, "Print", 1);
                    selectedContainer.push(f);
                    break;

                case "CreateVariable":
                    var cv = new Action();
                    Variable var = new Variable();
                    cv.func = new Func<Action, bool>(act =>
                    {
                        var.value = act.parameters[0].value(null).value;
                        var.name = (String)cv.parameters[1].value(null).value;
                        variables.Add(var);
                        return true;
                    });

                    cv.InitElement(selectedContainer, eventHandler, "Add variable", 2);
                    selectedContainer.push(cv);
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
                    selectedContainer.push(set);
                    break;

                case "Compare":
                    var compare = new Parameter();
                    if (selectedElement is Parameter)
                    {
                        compare.createGrid(BlockType.Parameter, eventHandler, sender.Name, 2);

                        compare.InitElement(selectedElement.container, eventHandler);
                        var ind = Grid.GetColumn(selectedElement.grid);
                        Element.AddParameter(compare, ind, (Parameter)selectedElement, new Func<Variable, Variable>(_ =>
                        {
                            var val1 = compare.parameters[0].value(null).value;
                            var val2 = compare.parameters[0].value(null).value;
                            Variable newVar = new Variable();
                            newVar.Type = VariableType.Bool;
                            newVar.value = (int)val1 == (int)val2;

                            return newVar;
                        }));
                    }

                    break;

                case "Variable":
                    if (selectedElement == null)
                        break;

                    Parameter getparam = new Parameter();
                    if (selectedElement is Parameter)
                    {
                        getparam.createGrid(BlockType.GetVariable, eventHandler, sender.Name, 0);

                        getparam.InitElement(selectedElement.container, eventHandler);
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
    }
}
