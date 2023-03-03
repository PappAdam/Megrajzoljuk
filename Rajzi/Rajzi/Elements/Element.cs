using Rajzi.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rajzi
{
    public abstract class Element 
    {
        public Container? parentElement { get; set; } = null;
        public Element? prevElement { get; set; } = null;
        public Element? nextElement { get; set; } = null;

        public void InsertElementAfter(Element element)
        {
            element.parentElement = this.parentElement;
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
        public int depth = 0;

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
                element.parentElement = this;
                this.containedElementCount++;
            }
            else
            {
                this[containedElementCount - 1].InsertElementAfter(element);
                this.containedElementCount++;
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
