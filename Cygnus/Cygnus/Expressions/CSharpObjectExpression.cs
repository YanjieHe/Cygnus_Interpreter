using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.Expressions
{
    public class CSharpObjectExpression : Expression
    {
        public object Value { get; private set; }
        public Type type { get; private set; }
        public CSharpObjectExpression(object Value)
        {
            this.Value = Value;
            type = Value.GetType();
        }
        public CSharpObjectExpression(object Value, Type type)
        {
            this.Value = Value;
            this.type = type;
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.CSharpObject;
            }
        }
        public override Expression Eval(Scope scope)
        {
            return this;
        }
    }
}
