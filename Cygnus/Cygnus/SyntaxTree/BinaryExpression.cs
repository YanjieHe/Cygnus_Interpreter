using System;
using Cygnus.LexicalAnalyzer;

namespace Cygnus.SyntaxTree
{
    public class BinaryExpression : Expression
    {
        public Operator Op;
        public Expression Left { get; private set; }
        public Expression Right { get; private set; }
        public BinaryExpression(Operator Op, Expression Left, Expression Right)
        {
            this.Op = Op;
            this.Left = Left;
            this.Right = Right;
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Binary;
            }
        }
        public override Expression Eval(Scope scope)
        {
            switch (Op)
            {
                case Operator.Add:
                case Operator.Subtract:
                case Operator.Multiply:
                case Operator.Divide:
                case Operator.Power:
                    return ArithemeticOp(Left, Right, Op, scope);
                case Operator.And:
                    return Left.Eval(scope).As<bool>(scope) && Right.Eval(scope).As<bool>(scope);
                case Operator.Or:
                    return Left.Eval(scope).As<bool>(scope) || Right.Eval(scope).As<bool>(scope);
                case Operator.Less:
                case Operator.Greater:
                case Operator.LessOrEquals:
                case Operator.GreaterOrEquals:
                    return CompareOp(Left, Right, Op, scope);
                case Operator.Equals:
                    return Left.Eval(scope).Equals(Right.Eval(scope));
                case Operator.NotEqualTo:
                    return !Left.Eval(scope).Equals(Right.Eval(scope));
                case Operator.Assgin:
                    return AssginOp(Left, Right, scope);
                default:
                    throw new NotSupportedException();
            }
            throw new NotSupportedException();

        }
        public Expression AssginOp(Expression left, Expression right, Scope scope)
        {
            switch (left.NodeType)
            {
                case ExpressionType.Index:
                    {
                        var rvalue = right.Eval(scope);
                        var lvalue = (IndexExpression)left;
                        lvalue.SetValue(rvalue, scope);
                        return left;
                    }
                case ExpressionType.Parameter:
                    {
                        var rvalue = right.Eval(scope);
                        ((ParameterExpression)left).Assgin(rvalue, scope);
                        return left;
                    }
                default:
                    throw new ArgumentException("The left side of the equal-sign cannot be assigned");
            }
        }
        public static Expression ArithemeticOp(Expression LeftOperand, Expression RightOperand, Operator op, Scope scope)
        {
            var left = LeftOperand.GetValue(scope) as IComputable;
            var right = RightOperand.GetValue(scope);
            switch (op)
            {
                case Operator.Add:
                    return left.Add(right);
                case Operator.Subtract:
                    return left.Subtract(right);
                case Operator.Multiply:
                    return left.Multiply(right);
                case Operator.Divide:
                    return left.Divide(right);
                case Operator.Power:
                    return left.Power(right);
                default:
                    throw new NotSupportedException();
            }
        }
        public static Expression CompareOp(Expression LeftOperand, Expression RightOperand, Operator op, Scope scope)
        {
            var left = LeftOperand.AsConstant(scope);
            var right = RightOperand.AsConstant(scope);
            var cmpLeft = left.Value as IComparable;
            var cmpRight = right.Value as IComparable;
            if (cmpLeft == null || cmpRight == null) throw new NotSupportedException();
            switch (op)
            {
                case Operator.Less:
                    return cmpLeft.CompareTo(cmpRight) < 0;
                case Operator.Greater:
                    return cmpLeft.CompareTo(cmpRight) > 0;
                case Operator.LessOrEquals:
                    return cmpLeft.CompareTo(cmpRight) <= 0;
                case Operator.GreaterOrEquals:
                    return cmpLeft.CompareTo(cmpRight) >= 0;
                default:
                    throw new NotSupportedException();
            }
        }
        public static double GetDouble(ConstantExpression Expr)
        {
            switch (Expr.constantType)
            {
                case ConstantType.Integer: return (int)Expr.Value;
                case ConstantType.Double: return (double)Expr.Value;
                default:
                    throw new NotSupportedException();
            }
        }
        public override string ToString()
        {
            return string.Format("(Binary: {0})", Op);
        }
    }
}
