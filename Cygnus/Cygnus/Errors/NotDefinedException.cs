using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.Errors
{
    public class NotDefinedException : SyntaxException
    {
        public NotDefinedException() : base("[Not Defined Exception]") { }
        public NotDefinedException(params object[] args)
            : base("[Not Defined Exception]: " + string.Join(", ", args) + (args.Length == 1 ? " is " : "are ") + "not defined")
        {
        }
    }
}
