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
        public double[]? value { get; set; } = null;
        public VariableType Type { get; set; } = VariableType.None;
    }

    public class Parameter : Element
    {
        public Func<Parameter ,Variable>? value { get; set;} = null;
        public int containedElementDepth = 0;

        public override void InitElement(Element container, MouseButtonEventHandler eventHandler)
        {
            this.container = container;
            this.grid = Blocks.CreateBlockWithType(BlockType.Variable, null, eventHandler, 0);
            this.InitParameters(eventHandler);
        }

        public Parameter()
        {

        }
    }
}
