using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.DataStructures;
namespace Cygnus.Expressions
{
    public class ClassExpression : Expression, IDotAccessible
    {
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Class;
            }
        }
        public CygnusClass cygnusClass { get; private set; }
        public CygnusClass Parent { get; private set; }
        public ClassExpression(string ClassName, Scope ClassScope, CygnusClass Parent = null)
        {
            this.Parent = Parent;
            cygnusClass = new CygnusClass(ClassName, ClassScope, this.Parent);
        }
        public ClassExpression(CygnusClass cygnusClass)
        {
            this.cygnusClass = cygnusClass;
        }
        public override Expression Eval(Scope scope)
        {
            return new ConstantExpression(cygnusClass);
        }
        public ClassExpression Update(Expression[] Values, Scope scope)
        {
            var newclass = cygnusClass.InitClass(Values, scope);
            return new ClassExpression(newclass);
        }

        public Expression GetByDot(string field, bool IsMethod)
        {
            return cygnusClass.GetByDot(field, IsMethod);
        }

        public void SetByDot(CygnusObject obj, string field, bool IsMethod)
        {
            cygnusClass.SetByDot(obj, field, IsMethod);
        }
    }
}
