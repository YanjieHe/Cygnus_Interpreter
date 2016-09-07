using System;
using Cygnus.LexicalAnalyzer;
using Cygnus.DataStructures;
namespace Cygnus.Expressions
{
    public class UnaryExpression : Expression
    {
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Unary;
            }
        }
        public Expression Value { get; private set; }
        public ExpressionType Op { get; private set; }
        public UnaryExpression(ExpressionType Op, Expression Value)
        {
            this.Op = Op;
            this.Value = Value;
        }
        public override Expression Eval(Scope scope)
        {
            var value = Value.AsConstant(scope).Value as IComputable;

            switch (Op)
            {
                case ExpressionType.UnaryPlus:
                    return new ConstantExpression(value.UnaryPlus());
                case ExpressionType.UnaryMinus:
                case ExpressionType.Not:
                    return new ConstantExpression(value.Negate());
                default:
                    throw new NotSupportedException();
            }
        }
        public override string ToString()
        {
            return string.Format("(Unary: {0})", Op);
        }
    }
}
