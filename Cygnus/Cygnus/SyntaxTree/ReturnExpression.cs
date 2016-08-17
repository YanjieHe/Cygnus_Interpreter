using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SymbolTable;

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
        public override string ToString()
        {
            return "(return)";
        }
        public override Expression Eval(Scope scope)
        {
            return new ReturnExpression(expression.Eval(scope));
        }
    }
}
