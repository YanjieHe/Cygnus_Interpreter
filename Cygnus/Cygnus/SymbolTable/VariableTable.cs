using System.Collections.Generic;
using Cygnus.Expressions;
namespace Cygnus.SymbolTable
{
    public class VariableTable : Dictionary<string, Expression>
    {
        public VariableTable() : base()
        {

        }
    }
}
