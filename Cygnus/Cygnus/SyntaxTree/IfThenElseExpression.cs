using System;
using Cygnus.SymbolTable;

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
        public override string ToString()
        {
            return "(IfThenElse)";
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
                    return IfFalse.Eval(scope);
            }
        }
    }
}
