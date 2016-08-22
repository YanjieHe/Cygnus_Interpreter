using System.Collections.Generic;
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
