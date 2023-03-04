using Rajzi.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Rajzi
{
    public abstract class Element
    {
        public Container? container { get; set; } = null;
        public Element? prevElement { get; set; } = null;
        public Element? nextElement { get; set; } = null;

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

            if (this[containedElementCount-1] is Container)
            {
                ((Container)element).panel = new StackPanel();
                ((Container)element).depth = this.depth + 1;
                Blocks.CreateBlockWithType(BlockType.Loop, (Container)element);
                this.panel.Children.Add(((Container)element).panel);
            }
            else
            {
                Blocks.CreateBlockWithType(BlockType.Action, this);
            }
        }
    }

    public class Function : Element
    {
        public Func<Pencil, bool>? func;
        public Variable? variable;
        public Function()
        {

        }
    }
}
