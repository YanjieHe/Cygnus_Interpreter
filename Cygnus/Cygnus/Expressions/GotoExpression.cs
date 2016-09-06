using System;
using E = System.Linq.Expressions.Expression;
namespace Cygnus.Expressions
{
    public class GotoExpression : Expression
    {
        public GotoExpressionKind Kind { get; private set; }
        public Expression Value { get; private set; }

        public override ExpressionType NodeType
        {
            get
            {
                switch (Kind)
                {
                    case GotoExpressionKind.Goto: break;
                    case GotoExpressionKind.Return:
                        return ExpressionType.Return;
                    case GotoExpressionKind.Break:
                        return ExpressionType.Break;
                    case GotoExpressionKind.Continue:
                        return ExpressionType.Continue;
                }
                throw new NotImplementedException();
            }
        }
        public GotoExpression(GotoExpressionKind Kind, Expression Value = null)
        {
            this.Kind = Kind;
            this.Value = Value;
        }
        public override Expression Eval(Scope scope)
        {
            if (Kind == GotoExpressionKind.Return)
                return new GotoExpression(GotoExpressionKind.Return, Value.Eval(scope));
            else
                return this;
        }
        public override string ToString()
        {
            return Kind.ToString();
        }
    }
    public enum GotoExpressionKind : byte
    {
        Goto = 0,
        Return = 1,
        Break = 2,
        Continue = 3
    }
}


