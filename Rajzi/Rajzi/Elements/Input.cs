﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Reflection.Metadata;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace Rajzi.Elements
{
    public class BlockInput
    {
        public static void GetInput(Label sender, Element selectedElement, Container selectedContainer, MouseEventHandler eventHandler, List<Variable> variables)
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

                            try
                            {
                                double val = Convert.ToDouble(value);
                                var.value = val;
                                var.Type = VariableType.Number;
                            }
                            catch
                            {
                                var.Type = VariableType.Bool;
                                if (value == "true")
                                {
                                    var.value = true;
                                }
                                else if (value == "false") {
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
                        var.value = null;
                        var.name = (String)cv.parameters[0].value(null).value;
                        variables.Add(var);
                        return true;
                    });

                    cv.InitElement(selectedContainer, eventHandler, "Add variable", 1);
                    selectedContainer.push(cv);                    
                    break;

                case "SetVariable":
                    var set = new Action();
                    set.func = new Func<Action, bool>(act =>
                    {
                        Variable var = set.parameters[0].value(null);
                        var.value = set.parameters[1].value(null).value;
                        var.Type = set.parameters[1].value(null).Type;
                        return true;
                    });

                    set.InitElement(selectedContainer, eventHandler, "Set variable", 2);
                    selectedContainer.push(set);
                    break;

                case "Compare":
                    var compare = new Parameter();
                    if (selectedElement is Parameter)
                    {
                        compare.createGrid(BlockType.Compare, eventHandler, sender.Name, 2);
                        compare.InitElement(selectedElement.container, eventHandler);
                        var ind = Grid.GetColumn(selectedElement.grid);
                        Element.AddParameter(compare, ind, (Parameter)selectedElement, new Func<Variable, Variable>(_ =>
                        {
                            var val1 = compare.parameters[0].value(null);
                            var val2 = compare.parameters[1].value(null);
                            var compSign = ((ComboBox)compare.grid.Children[3]).SelectedValue.ToString();
                            Variable newVar = new Variable();
                            newVar.Type = VariableType.Bool;
                            switch (val1.Type)
                            {
                                case VariableType.Number:
                                    switch (compSign)
                                    {
                                        case "=":
                                            newVar.value = (double)val1.value == (double)val2.value;
                                            break;

                                        case "!=":
                                            newVar.value = (double)val1.value != (double)val2.value;
                                            break;

                                        case ">=":
                                            newVar.value = (double)val1.value >= (double)val2.value;
                                            break;

                                        case "<=":
                                            newVar.value = (double)val1.value <= (double)val2.value;
                                            break;

                                        case ">":
                                            newVar.value = (double)val1.value > (double)val2.value;
                                            break;

                                        case "<":
                                            newVar.value = (double)val1.value < (double)val2.value;
                                            break;

                                        default:
                                            throw new Exception("Couldn't recognise compare sign");
                                    }
                                    break;

                                case VariableType.Bool:
                                    switch (compSign)
                                    {
                                        case "=":
                                            newVar.value = (bool)val1.value == (bool)val2.value;
                                            break;

                                        case "!=":
                                            newVar.value = (bool)val1.value != (bool)val2.value;
                                            break;

                                        default:
                                            throw new Exception("Couldn't recognise compare sign");
                                    }
                                    break;
                            }

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

                case "Add":
                    if (selectedElement == null)
                        break;

                    Parameter addparam = new Parameter();
                    if (selectedElement is Parameter)
                    {
                        addparam.createGrid(BlockType.Math, eventHandler, sender.Name, 2);

                        addparam.InitElement(selectedElement.container, eventHandler);
                        var ind = Grid.GetColumn(selectedElement.grid);
                        Element.AddParameter(addparam, ind, (Parameter)selectedElement, new Func<Variable, Variable>(_ =>
                        {
                            Variable var = new Variable();
                            var.Type = VariableType.Number;

                            if (addparam.parameters[0].value(null).Type != VariableType.Number || addparam.parameters[1].value(null).Type != VariableType.Number)
                            {
                                throw new Exception("Add operation only usable between Number type");
                            }

                            var.value = (double)addparam.parameters[0].value(null).value + (double)addparam.parameters[1].value(null).value;

                            return var;
                        }));
                    }
                    break;

                case "Subtr":
                    if (selectedElement == null)
                        break;

                    Parameter subparam = new Parameter();
                    if (selectedElement is Parameter)
                    {
                        subparam.createGrid(BlockType.Math, eventHandler, sender.Name, 2);

                        subparam.InitElement(selectedElement.container, eventHandler);
                        var ind = Grid.GetColumn(selectedElement.grid);
                        Element.AddParameter(subparam, ind, (Parameter)selectedElement, new Func<Variable, Variable>(_ =>
                        {
                            Variable var = new Variable();
                            var.Type = VariableType.Number;

                            if (subparam.parameters[0].value(null).Type != VariableType.Number || subparam.parameters[1].value(null).Type != VariableType.Number)
                            {
                                throw new Exception("Subtract operation only usable between Number type");
                            }

                            var.value = (double)subparam.parameters[0].value(null).value - (double)subparam.parameters[1].value(null).value;

                            return var;
                        }));
                    }
                    break;

                case "Multip":
                    if (selectedElement == null)
                        break;

                    Parameter mulparam = new Parameter();
                    if (selectedElement is Parameter)
                    {
                        mulparam.createGrid(BlockType.Math, eventHandler, sender.Name, 2);

                        mulparam.InitElement(selectedElement.container, eventHandler);
                        var ind = Grid.GetColumn(selectedElement.grid);
                        Element.AddParameter(mulparam, ind, (Parameter)selectedElement, new Func<Variable, Variable>(_ =>
                        {
                            Variable var = new Variable();
                            var.Type = VariableType.Number;

                            if (mulparam.parameters[0].value(null).Type != VariableType.Number || mulparam.parameters[1].value(null).Type != VariableType.Number)
                            {
                                throw new Exception("Multiply operation only usable between Number type");
                            }

                            var.value = (double)mulparam.parameters[0].value(null).value * (double)mulparam.parameters[1].value(null).value;

                            return var;
                        }));
                    }
                    break;

                case "Divide":
                    if (selectedElement == null)
                        break;

                    Parameter divparam = new Parameter();
                    if (selectedElement is Parameter)
                    {
                        divparam.createGrid(BlockType.Math, eventHandler, sender.Name, 2);

                        divparam.InitElement(selectedElement.container, eventHandler);
                        var ind = Grid.GetColumn(selectedElement.grid);
                        Element.AddParameter(divparam, ind, (Parameter)selectedElement, new Func<Variable, Variable>(_ =>
                        {
                            Variable var = new Variable();
                            var.Type = VariableType.Number;

                            if (divparam.parameters[0].value(null).Type != VariableType.Number || divparam.parameters[1].value(null).Type != VariableType.Number)
                            {
                                throw new Exception("Divide operation only usable between Number type");
                            }

                            var.value = (double)divparam.parameters[0].value(null).value / (double)divparam.parameters[1].value(null).value;

                            return var;
                        }));
                    }
                    break;

                case "Logical":
                    var logical = new Parameter();
                    if (selectedElement is Parameter)
                    {
                        logical.createGrid(BlockType.Logical, eventHandler, sender.Name, 2);
                        logical.InitElement(selectedElement.container, eventHandler);
                        var ind = Grid.GetColumn(selectedElement.grid);
                        Element.AddParameter(logical, ind, (Parameter)selectedElement, new Func<Variable, Variable>(_ =>
                        {
                            var val1 = logical.parameters[0].value(null);
                            var val2 = logical.parameters[1].value(null);
                            var compSign = ((ComboBox)logical.grid.Children[3]).SelectedValue.ToString();
                            Variable newVar = new Variable();
                            newVar.Type = VariableType.Bool;
                            if(val1.Type != VariableType.Bool && val1.Type != VariableType.Bool)
                            {
                                throw new Exception("Logical operations require boolean types");
                            }
                            switch (compSign)
                            {
                                case "AND":
                                    newVar.value = (bool)val1.value && (bool)val2.value;
                                    break;

                                case "OR":
                                    newVar.value = (bool)val1.value || (bool)val2.value;
                                    break;

                                default:
                                    throw new Exception("Couldn't recognise compare sign");
                            }

                            return newVar;
                        }));
                    }

                    break;
            }
        }
    }
}
