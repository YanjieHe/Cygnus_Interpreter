using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.Errors
{
    public class ParameterException : SyntaxException
    {
        public ParameterException() : base("[Parameter Exception]") { }

        public ParameterException(string message) : base("[Parameter Exception]: " + message)
        {
        }

        public ParameterException(string format, params object[] args) : base("[Parameter Exception]: " + format, args)
        {
        }
    }
}
