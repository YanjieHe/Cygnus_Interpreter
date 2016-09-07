using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.Expressions
{
    public class ClassInitExpression : Expression
    {
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.ClassInit;
            }
        }
        string ClassName;
        public Expression[] Arguments;
        public ClassInitExpression(string ClassName, Expression[] Arguments)
        {
            this.ClassName = ClassName;
            this.Arguments = Arguments;
        }
        public override Expression Eval(Scope scope)
        {
            return Scope.classtable[ClassName].Update(Arguments, scope);
        }
    }
}
