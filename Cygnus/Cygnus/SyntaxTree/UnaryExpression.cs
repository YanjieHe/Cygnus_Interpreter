using System;
using Cygnus.LexicalAnalyzer;

namespace Cygnus.SyntaxTree
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
        public Operator Op { get; private set; }
        public UnaryExpression(Operator Op, Expression Value)
        {
            this.Op = Op;
            this.Value = Value;
        }
        public override Expression Eval(Scope scope)
        {
            var value = Value.GetValue(scope);
            return UnaryOp(value, Op);
        }
        public Expression UnaryOp(Expression expression, Operator op)
        {
            if (expression.NodeType == ExpressionType.Constant)
            {
                var expr = expression as ConstantExpression;
                switch (expr.constantType)
                {
                    case ConstantType.Integer:
                        if (Op == Operator.UnaryPlus)
                            return +(int)expr.Value;
                        else if (Op == Operator.UnaryMinus)
                            return -(int)expr.Value;
                        else goto default;
                    case ConstantType.Double:
                        if (Op == Operator.UnaryPlus)
                            return +(double)expr.Value;
                        else if (Op == Operator.UnaryMinus)
                            return -(double)expr.Value;
                        else goto default;
                    case ConstantType.Boolean:
                        if (Op == Operator.Not)
                            return !(bool)expr.Value;
                        else goto default;
                    default:
                        throw new NotSupportedException("Not supported unary operator '" + Op + "'");
                }
            }
            else if (expression.NodeType == ExpressionType.Matrix)
            {
                var expr = expression as MatrixExpression;
                if (Op == Operator.UnaryPlus)
                    return new MatrixExpression(+expr.Data);
                else if (Op == Operator.UnaryMinus)
                    return new MatrixExpression(-expr.Data);
                else throw new NotSupportedException();
            }
            else
                throw new NotSupportedException();
        }
        public override string ToString()
        {
            return string.Format("(Unary: {0})", Op);
        }
    }
}
