using System.Collections.Generic;
using Cygnus.Expressions;
namespace Cygnus.SymbolTable
{
    public class FunctionTable : Dictionary<string, FunctionExpression>
    {
        public FunctionTable() : base()
        {

        }
    }
}
