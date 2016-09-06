namespace Cygnus.Expressions
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
            var test = Test.As<bool>(scope);
            if (test)
                return IfTrue.Eval(scope);
            else
                return Void();
        }
    }
}
