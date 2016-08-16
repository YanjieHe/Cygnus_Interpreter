using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override Expression Eval()
        {
            return this;
        }
        public override string ToString()
        {
            return "(Break)";
        }
    }
}
