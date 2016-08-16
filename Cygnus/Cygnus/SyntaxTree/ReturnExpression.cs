using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.SyntaxTree
{
    public class ReturnExpression : Expression
    {
        public Expression expression { get; private set; }
        public ReturnExpression(Expression expression)
        {
            this.expression = expression;
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Return;
            }
        }

        public override Expression Eval()
        {
            return this;
        }
        public override string ToString()
        {
            return "(return)";
        }
    }
}
