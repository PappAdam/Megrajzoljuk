﻿using Rajzi.Elements;
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
        public Grid grid { get; set; } = null;
        public List<Parameter> parameters { get; set; } = new List<Parameter>();

        public static void AddParameter(Parameter parameter, int index, Parameter selectedParam)
        {
            selectedParam.container.parameters.Add(parameter);

            Blocks.ExpandGrid(parameter.grid, index, selectedParam);

            selectedParam.container.parameters[index].value = new Func<Variable[], Variable>(x =>
            {
                if (x != null)
                {
                    return x[0];
                }
                throw new NullReferenceException();
            });
        }

        public void InitParameters(MouseButtonEventHandler eventHandler)
        {
            for (int i = 1; i < this.grid.Children.Count; i++)
            {
                this.parameters.Add(new Parameter());
                ((Label)(((Grid)grid.Children[i]).Children[0])).Tag = this.parameters[i - 1];
                this.parameters[i - 1].container = this;
                this.parameters[i - 1].grid = (Grid)this.grid.Children[i];
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
            this.nextElement = element;

        }

        public void MoveElementBefore(Element element)
        {
            element.nextElement = this;
        }

        public abstract void InitElement(Element container, MouseButtonEventHandler eventHandler);
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

        public override void InitElement(Element container, MouseButtonEventHandler eventHandler)
        {
            this.panel = new StackPanel();
            this.depth = ((Container)container).depth + 1;
            this.grid = Blocks.CreateBlockWithType(BlockType.Loop, this, eventHandler, 3);
            ((Label)grid.Children[0]).Tag = this;
            ((Label)grid.Children[0]).MouseLeftButtonDown += eventHandler;
            ((Container)container).panel.Children.Add(this.panel);
            this.InitParameters(eventHandler);
        }

        public void push(Element element, int index, MouseButtonEventHandler mainBlockEventHandler)
        {
            element.index = index;
            element.InitElement(this, mainBlockEventHandler);
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

        public override void InitElement(Element container, MouseButtonEventHandler eventHandler)
        {
            var grid = Blocks.CreateBlockWithType(BlockType.Action, (Container)container, eventHandler);
            ((Label)grid.Children[0]).Tag = this;
            ((Label)grid.Children[0]).MouseLeftButtonDown += eventHandler;
            ((Action)this).grid = grid;
            this.InitParameters(eventHandler);
        }
    }
}
 
