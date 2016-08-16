using System;
using System.Collections.Generic;
using Cygnus.SyntaxTree;
namespace Cygnus.SymbolTable
{
    public class BuiltInMethodTable : Dictionary<string, Func<Expression[], Expression>>
    {
        public BuiltInMethodTable() : base()
        {

        }
    }
}
