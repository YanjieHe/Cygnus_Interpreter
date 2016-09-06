using System;
using System.Collections.Generic;
using Cygnus.Expressions;
namespace Cygnus.SymbolTable
{
    public class BuiltInMethodTable : Dictionary<string, Func<Expression[], Scope, Expression>>
    {
        public BuiltInMethodTable() : base()
        {

        }
    }
}
