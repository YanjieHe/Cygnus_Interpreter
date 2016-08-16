using System;

namespace Cygnus.SyntaxTree
{
    public class IfThenElseExpression : Expression
    {
        public Expression Test { get; private set; }
        public Expression IfTrue { get; private set; }
        public Expression IfFalse { get; private set; }

        public IfThenElseExpression(Expression Test, Expression IfTrue, Expression IfFalse)
        {
            this.Test = Test;
            this.IfTrue = IfTrue;
            this.IfFalse = IfFalse;
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.IfThenElse;
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
                    return IfFalse.Eval();
            }
        }
        public void Update(Expression Test, Expression IfTrue)
        {
            this.Test = Test;
            this.IfTrue = IfTrue;
        }
        public override string ToString()
        {
            return "(IfThenElse)";
        }
    }
}
