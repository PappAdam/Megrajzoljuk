using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rajzi.Elements
{
    enum VariableType
    {
        RGB,
        Number,
        Bool,
        String,
    }

    public class Variable
    {
        double[] value;
        public Variable(double[] value)
        {
            this.value = value;
        }
    }
}
