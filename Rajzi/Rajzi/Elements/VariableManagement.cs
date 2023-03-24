using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rajzi.Elements
{
    public enum VariableType
    {
        None,
        RGB,
        Number,
        Bool,
        String,
        Vector2,
    }

    public class Variable
    {
        public object value { get; set; } = null;
        public VariableType Type { get; set; } = VariableType.None;
        public String name;
    }

    public class Parameter : Element
    {
        public Func<Variable ,Variable>? value { get; set;} = null;
        public int containedElementDepth = 0;

        public void createGrid(BlockType type, MouseButtonEventHandler eventHandler)
        {
            String name;
            switch (type) {
                case BlockType.GetVariable:
                    name = "Get";
                    break;
                case BlockType.Input:
                    name = "Input";
                    break;
                default:
                    name = "haha";
                    break;
            }

            this.grid = Blocks.CreateBlockWithType(type, null, eventHandler, name, 0);
        }

        public override void InitElement(Element container, MouseButtonEventHandler eventHandler, String name, int cols = 0)
        {
            this.container = container;
            this.InitParameters(eventHandler);
        }

        public Parameter()
        {

        }
    }
}
