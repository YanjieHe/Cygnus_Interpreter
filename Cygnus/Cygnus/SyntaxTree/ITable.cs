using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.SyntaxTree
{
    public interface ITable
    {
        Expression this[string Name] { get; set; }

    }
}
