using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SymbolTable;

namespace Cygnus.SyntaxTree
{
    public class BreakExpression : Expression
    {
        public BreakExpression() { }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Break;
            }
        }

        public override Expression Eval(Scope scope)
        {
            return this;
        }

        public override string ToString()
        {
            return "(Break)";
        }
    }
}
