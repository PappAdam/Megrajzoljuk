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
        public List<Parameter> parameters { get; set; } = new List<Parameter>();

        public void AddParameter(Parameter parameter, int index)
        {
            this.parameters.Add(parameter);
            if (this is Action)
            {
                Blocks.ExpandGrid(((Action)this).grid, this.parameters[index].grid, index);
            }
            else if (this is Container)
            {
                Blocks.ExpandGrid((Grid)((Container)this).panel.Children[0], this.parameters[index].grid, index);
            }
            else if (this is Parameter)
            {
                Blocks.ExpandGrid(((Parameter)this).grid, this.parameters[index].grid, index);
            }

            this.parameters[index].value = new Func<Variable[], Variable>(x =>
            {
                if (x != null)
                {
                    return x[0];
                }
                throw new NullReferenceException();
            });
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
            this.nextElement = element;

        }

        public void MoveElementBefore(Element element)
        {
            element.nextElement = this;
        }

        public abstract void InitElement(Element container);
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

        public override void InitElement(Element container)
        {
            this.panel = new StackPanel();
            this.depth = ((Container)container).depth + 1;
            var grid = Blocks.CreateBlockWithType(BlockType.Loop, this);
            ((Label)grid.Children[0]).Tag = this;
            ((Container)container).panel.Children.Add(this.panel);
        }

        public void push(Element element, int index)
        {
            element.index = index;
            element.InitElement(this);
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
    }

    public class Action : Element
    {
        public Func<Pencil, bool>? func;
        public Grid? grid;

        public override void InitElement(Element container)
        {
            var grid = Blocks.CreateBlockWithType(BlockType.Action, (Container)container);
            ((Label)grid.Children[0]).Tag = this;
            ((Action)this).grid = grid;
        }
    }
}
 
