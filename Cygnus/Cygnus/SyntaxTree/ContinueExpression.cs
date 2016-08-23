using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.SyntaxTree
{
    public class ContinueExpression : Expression
    {
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Continue;
            }
        }

        public override Expression Eval(Scope scope)
        {
            return this;
        }
        public override string ToString()
        {
            return "(Continue)";
        }
    }
}
