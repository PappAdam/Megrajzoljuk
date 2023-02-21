using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rajzi
{
    internal abstract class Element 
    {
        internal Element? nextElement = null;
    }

    internal class Container : Element 
    {
        internal Element? firstChild = null;
        internal bool condition = false;
    }

    internal class Function<T> : Element
    {
        internal Func<T, Pencil, bool> func;
        internal Function()
        {

        }
    }
}
