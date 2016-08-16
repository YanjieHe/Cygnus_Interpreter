using System;

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
        public override Expression Eval()
        {
            var test = (ConstantExpression)Test.Eval();
            if (test.constantType != ConstantType.Boolean) throw new ArgumentException();
            else
            {
                if ((bool)test.Value)
                    return IfTrue.Eval();
                else
                    return new DefaultExpression(ConstantType.Void);
            }
        }
        public void Update(Expression Test, Expression IfTrue)
        {
            this.Test = Test;
            this.IfTrue = IfTrue;
        }
        public override string ToString()
        {
            return "(IfThen)";
        }
    }
}
