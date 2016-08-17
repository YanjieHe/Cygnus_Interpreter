using System;
using Cygnus.SymbolTable;

namespace Cygnus.SyntaxTree
{
    public class IfThenExpression : Expression
    {
        public Expression Test { get; private set; }
        public Expression IfTrue { get; private set; }
        public IfThenExpression(Expression Test, Expression IfTrue)
        {
            this.Test = Test;
            this.IfTrue = IfTrue;
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.IfThen;
            }
        }
        public override string ToString()
        {
            return "(IfThen)";
        }

        public override Expression Eval(Scope scope)
        {
            var test = (ConstantExpression)Test.Eval(scope);
            if (test.constantType != ConstantType.Boolean) throw new ArgumentException();
            else
            {
                if ((bool)test.Value)
                    return IfTrue.Eval(scope);
                else
                    return new ConstantExpression(null, ConstantType.Void);
            }
        }
    }
}
