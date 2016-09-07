using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.DataStructures;
using CE = Cygnus.Extensions.ConsoleExtension;
namespace Cygnus.Expressions
{
    public class DotExpression : Expression, IAssignable
    {
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Dot;
            }
        }
        public Expression expression { get; private set; }
        public string field { get; private set; }
        public bool IsMethod { get; private set; }
        public Expression[] Arguments { get; private set; }
        public DotExpression(Expression expression, string field, bool IsMethod, Expression[] Arguments = null)
        {
            this.expression = expression;
            this.field = field;
            this.IsMethod = IsMethod;
            this.Arguments = Arguments;
        }
        public override Expression Eval(Scope scope)
        {
            if (IsMethod)
            {
                CygnusClass ThisClass = expression.AsConstant(scope).Value as CygnusClass;
                var func = (ThisClass.GetByDot(field, IsMethod) as FunctionExpression);
                return func.Update(new Expression[] { ThisClass }.Concat(Arguments).ToArray(), scope).Eval(scope).GetValue(scope);
            }
            else
                return GetByDot(expression, scope).GetByDot(field, IsMethod).Eval(scope);
        }
        public IDotAccessible GetByDot(Expression obj, Scope scope)
        {
            var Expr = obj.GetValue(scope);
            if (Expr is IDotAccessible)
                return Expr as IDotAccessible;
            else
                throw new ArgumentException(Expr.ToString() + " Cannot get element by dot");
        }
        public override void Display(Scope scope)
        {
            Eval(scope).Display(scope);
        }
        public void Assgin(Expression value, Scope scope)
        {
            GetByDot(expression, scope)
                .SetByDot(value.AsConstant(scope).Value, field, IsMethod);
        }
    }
}
