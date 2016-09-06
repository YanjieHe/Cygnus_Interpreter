using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Cygnus.Expressions
{
    public class MemberExpression : Expression
    {
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Member;
            }
        }
        Expression expression;
        
        public override Expression Eval(Scope scope)
        {
            throw new NotImplementedException();
        }
    }
}
