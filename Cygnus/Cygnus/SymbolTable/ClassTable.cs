using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.Expressions;
namespace Cygnus.SymbolTable
{
    public class ClassTable : Dictionary<string, ClassExpression>
    {
        public ClassTable() : base()
        {

        }
    }
}
