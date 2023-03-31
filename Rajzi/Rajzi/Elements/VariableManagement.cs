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

        public void createGrid(BlockType type, MouseButtonEventHandler eventHandler, String name, int cols = 0)
        {
            this.grid = Blocks.CreateBlockWithType(type, null, eventHandler, name, cols);
        }

        // Dont use it, does nothing
        public override void InitElement(Element container, MouseButtonEventHandler eventHandler, String name, int cols = 0){}

        public void InitElement(Element container, MouseButtonEventHandler eventHandler)
        {
            this.container = container;
            this.InitParameters(eventHandler);
            ((Label)this.grid.Children[0]).Tag = this;
        }

        public Parameter()
        {

        }
    }
}
