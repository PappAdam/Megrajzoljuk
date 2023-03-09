using Rajzi.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Rajzi
{
    public abstract class Element : IEnumerable<Element>
    {
        public int index = 0;
        public Container? container { get; set; } = null;
        public Element? prevElement { get; set; } = null;
        public Element? nextElement { get; set; } = null;
        public ManageVariable? variable { get; set; } = null;

        public void AddParameter()
        {
            this.variable = new ManageVariable();
            this.variable.value = new Func<Variable[], Variable>(x =>
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
            if (element.nextElement != null)
                element.nextElement.prevElement = element;

            element.prevElement = this;
        }

        public void MoveElementBefore(Element element)
        {
            if (element.prevElement != null)
                element.prevElement.nextElement = element.nextElement;

            if (element.nextElement != null)
                element.nextElement.prevElement = element.prevElement;

            if (this.prevElement != null)
                element.prevElement = this.prevElement;

            element.nextElement = this;
            this.prevElement = element;
        }
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

        public void push(Element element, int index)
        {
            element.index = index;
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

            if (element is Container)
            {
                ((Container)element).panel = new StackPanel();
                ((Container)element).depth = this.depth + 1;
                var grid = Blocks.CreateBlockWithType(BlockType.Loop, (Container)element);
                grid.Tag = element;
                this.panel.Children.Add(((Container)element).panel);
            }
            else if (element is Action)
            {
                var grid = Blocks.CreateBlockWithType(BlockType.Action, this);
                grid.Tag = element;
                ((Action)element).grid = grid;
            }
        }
    }

    public class Action : Element
    {
        public Func<Pencil, bool>? func;
        public Grid? grid;
        public Action()
        {

        }
    }
}
 