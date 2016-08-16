using System.Collections.Generic;
using Cygnus.SyntaxTree;
namespace Cygnus.SymbolTable
{
    public class VariableTable : Dictionary<string, Expression>
    {
        public VariableTable() : base()
        {

        }
    }
}
