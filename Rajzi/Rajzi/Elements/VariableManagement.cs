using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
    }

    public class VariableManagement : Variable
    {
        public Variable? variable { get; set; } = null;
    }
}
