using Rajzi.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Rajzi
{
    public abstract class Element : IEnumerable<Element>
    {
        public int index = 0;
        public Element? container { get; set; } = null;
        public Element? nextElement { get; set; } = null;
        public Element? prevElement { get; set; } = null;
        public Grid grid { get; set; } = null;
        public List<Parameter> parameters { get; set; } = new List<Parameter>();

        public static void AddParameter(Parameter parameter, int index, Parameter selectedParam, Func<Variable, Variable> value)
        {
            selectedParam.container.parameters[selectedParam.Index] = parameter;

            Blocks.ExpandGrid(parameter.grid, index, selectedParam);

            parameter.value = value;
        }

        public void InitParameters()
        {
            int i = 0;
            foreach (var child in this.grid.Children)
            {
                if (child is Grid) {
                    this.parameters.Add(new Parameter());
                    this.parameters[i].container = this;
                    ((Label)((child as Grid).Children[0])).Tag = this.parameters[i];
                    this.parameters[i].grid = child as Grid;
                    this.parameters[i].Index = i;
                    i++;
                }
            }
        }

        public IEnumerator<Element> GetEnumerator()
        {
            Element? el = this;
            while (el != null)
            {
                yield return el;
                if (el is Container && ((Container)el).firstChild != null)
                {
                    el = ((Container)el).firstChild;
                }
                else
                {
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void InsertElementAfter(Element element)
        {
            element.container = this.container;
            element.nextElement = this.nextElement;
            element.prevElement = this;
            if (this.nextElement != null)
                this.nextElement.prevElement = element;

            this.nextElement = element;
        }


        public void RemoveElement()
        {
            if (this.nextElement != null) 
                this.nextElement.prevElement = this.prevElement;
            if (this.prevElement != null)
                this.prevElement.nextElement = this.nextElement;

            if (this.container is Container)
            {
                if (this is Container)
                {
                    ((Container)this.container).panel.Children.Remove(((Container)this).panel);
                }
                else
                {
                    ((Container)this.container).panel.Children.Remove(this.grid);
                }
                if (((Container)this.container).firstChild == this)
                {
                    ((Container)this.container).firstChild = this.nextElement;
                }
                ((Container)this.container).containedElementCount -= 1;
            }
        }

        public abstract void InitElement(Element container, MouseEventHandler eventHandler, MouseButtonEventHandler removeElement, String name, int cols = 0);
    }
        
    public class Container : Element
    {
        public Element? firstChild = null;
        public int containedElementCount = 0;
        public bool condition = false;
        public int depth = 1;
        public StackPanel panel;

        public Element this[int index]
        {
            get
            {
                Element? element = firstChild;
                int i = 0;

                while (i < index && element != null)
                {
                    element = element.nextElement;
                    i++;
                }
                if (element == null)
                {
                    throw new IndexOutOfRangeException();
                }

                return element;
            }
        }

        public override void InitElement(Element container, MouseEventHandler eventHandler, MouseButtonEventHandler removeElement, String name, int cols = 0)
        {
            this.panel = new StackPanel();
            this.depth = ((Container)container).depth + 1;
            if (this is Loop)
            {
                this.grid = Blocks.CreateBlockWithType(BlockType.Loop, this, eventHandler, "Loop", cols);
            }
            else
            {
                this.grid = Blocks.CreateBlockWithType(BlockType.Statement, this, eventHandler, "If", cols);
            }
            ((Label)grid.Children[0]).Tag = this;
            ((Label)grid.Children[0]).MouseRightButtonDown += removeElement;
            ((Container)container).panel.Children.Add(this.panel);
            this.InitParameters();
        }

        public void push(Element element)
        {
            if (this.firstChild == null)
            {
                this.firstChild = element;
                element.container = this;
                this.containedElementCount++;
            }
            else
            {
                this[containedElementCount - 1].InsertElementAfter(element);
                this.containedElementCount++;
            }
        }

        public void SetCondition()
        {
            Variable var = (Variable)this.parameters[0].value(null);
            if (var.value is bool)
            {
                this.condition = (bool)var.value;
            }
            else
            {
                this.condition = false;
            }
        }
    }

    public class Action : Element
    {
        public Func<Action, bool>? func;

        public override void InitElement(Element container, MouseEventHandler eventHandler, MouseButtonEventHandler removeElement, String name, int cols = 0)
        {
            var grid = Blocks.CreateBlockWithType(BlockType.Action, (Container)container, eventHandler, name, cols);
            ((Label)grid.Children[0]).Tag = this;
            ((Label)grid.Children[0]).MouseRightButtonDown += removeElement;
            ((Action)this).grid = grid;
            this.InitParameters();
        }
    }
}
 
