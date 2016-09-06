using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.Extensions;
using Cygnus.DataStructures;
namespace Cygnus.Expressions
{
    public class NewArrayExpression : Expression
    {
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.NewArray;
            }
        }
        public Expression[] expressions { get; private set; }
        public NewArrayExpression(params Expression[] expressions)
        {
            this.expressions = expressions;
        }
        public override Expression Eval(Scope scope)
        {
            var array = expressions.Map(i => i.AsConstant(scope).Value);
            return new CygnusArray(array);
        }
    }
}
