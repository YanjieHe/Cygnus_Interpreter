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
                    return Left.AsBool(scope) && Right.AsBool(scope);
                case Operator.Or:
                    return Left.AsBool(scope) || Right.AsBool(scope);
                case Operator.Less:
                case Operator.Greater:
                case Operator.LessOrEquals:
                case Operator.GreaterOrEquals:
                    return CompareOp(Left, Right, Op, scope);
                case Operator.Equal:
                    return Left.Eval(scope).Equals(Right.Eval(scope));
                case Operator.NotEqual:
                    return !Left.Eval(scope).Equals(Right.Eval(scope));
                case Operator.Assign:
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
        private static Expression ArithemeticOp(Expression LeftOperand, Expression RightOperand, Operator op, Scope scope)
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
                case Operator.Add: return new ConstantExpression((left as IComputable).Add(right));
                case Operator.Subtract: return new ConstantExpression((left as IComputable).Subtract(right));
                case Operator.Multiply: return new ConstantExpression((left as IComputable).Multiply(right));
                case Operator.Divide: return new ConstantExpression((left as IComputable).Divide(right));
                case Operator.Power: return new ConstantExpression((left as IComputable).Power(right));
                default:
                    throw new NotSupportedException();
            }
        }
        private static Expression CompareOp(Expression LeftOperand, Expression RightOperand, Operator op, Scope scope)
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
                case Operator.Less: return cmp < 0;
                case Operator.Greater: return cmp > 0;
                case Operator.LessOrEquals: return cmp <= 0;
                case Operator.GreaterOrEquals: return cmp >= 0;
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
