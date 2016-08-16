using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SyntaxTree;
namespace Cygnus.SymbolTable
{
    public class FunctionTable : Dictionary<string, FunctionExpression>
    {
        public FunctionTable() : base()
        {

        }
    }
}
