using System;
using Cygnus.LexicalAnalyzer;
using System.Collections.Generic;
using Cygnus.Extensions;
using Cygnus.Errors;
using Cygnus.DataStructures;
namespace Cygnus.Expressions
{
    public class BinaryExpression : Expression
    {
        public ExpressionType Op;
        public Expression Left { get; private set; }
        public Expression Right { get; private set; }
        public BinaryExpression(ExpressionType Op, Expression Left, Expression Right)
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
                case ExpressionType.Add:
                case ExpressionType.Subtract:
                case ExpressionType.Multiply:
                case ExpressionType.Divide:
                case ExpressionType.Power:
                    return ArithemeticOp(Left, Right, Op, scope);
                case ExpressionType.And:
                    return Left.AsBool(scope) && Right.AsBool(scope);
                case ExpressionType.Or:
                    return Left.AsBool(scope) || Right.AsBool(scope);
                case ExpressionType.Less:
                case ExpressionType.Greater:
                case ExpressionType.LessOrEquals:
                case ExpressionType.GreaterOrEquals:
                    return CompareOp(Left, Right, Op, scope);
                case ExpressionType.Equal:
                    return EqualsOp(Left, Right, scope);
                case ExpressionType.NotEqual:
                    return !EqualsOp(Left, Right, scope);
                case ExpressionType.Assign:
                    return AssginOp(Left, Right, scope);
                default:
                    throw new NotSupportedException();
            }
            throw new NotSupportedException();
        }
        public Expression AssginOp(Expression left, Expression right, Scope scope)
        {
            (left is IAssignable).OrThrows<ArgumentException>("The left side of the equal-sign cannot be assigned");
            (left as IAssignable).Assgin(right, scope);
            return left;
        }
        private static Expression ArithemeticOp(Expression LeftOperand, Expression RightOperand, ExpressionType op, Scope scope)
        {
            var left = LeftOperand.AsConstant(scope).Value;
            var right = RightOperand.AsConstant(scope).Value;
            if (left.type != right.type)
            {
                if (left.type.Width > right.type.Width)
                {
                    right = left.FromObject(right);
                }
                else if (left.type.Width < right.type.Width)
                {
                    left = right.FromObject(left);
                }
                else throw new NotImplementedException();
            }
            switch (op)
            {
                case ExpressionType.Add: return new ConstantExpression((left as IComputable).Add(right));
                case ExpressionType.Subtract: return new ConstantExpression((left as IComputable).Subtract(right));
                case ExpressionType.Multiply: return new ConstantExpression((left as IComputable).Multiply(right));
                case ExpressionType.Divide: return new ConstantExpression((left as IComputable).Divide(right));
                case ExpressionType.Power: return new ConstantExpression((left as IComputable).Power(right));
                default:
                    throw new NotSupportedException();
            }
        }
        private static Expression CompareOp(Expression LeftOperand, Expression RightOperand, ExpressionType op, Scope scope)
        {
            var left = LeftOperand.AsConstant(scope).Value;
            var right = RightOperand.AsConstant(scope).Value;
            if (left.type != right.type)
            {
                if (left.type.Width > right.type.Width)
                {
                    right = left.FromObject(right);
                }
                else if (left.type.Width < right.type.Width)
                {
                    left = right.FromObject(left);
                }
                else throw new NotImplementedException();
            }
            var cmp = (left as IComparable).CompareTo(right);
            switch (op)
            {
                case ExpressionType.Less: return cmp < 0;
                case ExpressionType.Greater: return cmp > 0;
                case ExpressionType.LessOrEquals: return cmp <= 0;
                case ExpressionType.GreaterOrEquals: return cmp >= 0;
                default:
                    throw new NotSupportedException();
            }
        }
        private static bool EqualsOp(Expression LeftOperand, Expression RightOperand, Scope scope)
        {
            var left = LeftOperand.AsConstant(scope).Value;
            var right = RightOperand.AsConstant(scope).Value;
            if (left.IsNull() && right.IsNull())
                return true;
            else if (left.IsNull() || right.IsNull())
                return false;
            else
            {
                if (left.type != right.type)
                {
                    if (left.type.Width > right.type.Width)
                    {
                        right = left.FromObject(right);
                    }
                    else if (left.type.Width < right.type.Width)
                    {
                        left = right.FromObject(left);
                    }
                    else throw new NotImplementedException();
                }
                return left.Equals(right);
            }
        }
        public override string ToString()
        {
            return string.Format("(Binary: {0})", Op);
        }
    }
}
