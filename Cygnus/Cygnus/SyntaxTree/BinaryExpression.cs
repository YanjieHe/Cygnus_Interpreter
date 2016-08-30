using System;
using Cygnus.LexicalAnalyzer;
using System.Collections.Generic;
using Cygnus.Extensions;
using Cygnus.Errors;
using MathNet.Numerics.LinearAlgebra;
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
                    return Left.As<bool>(scope) && Right.As<bool>(scope);
                case Operator.Or:
                    return Left.As<bool>(scope) || Right.As<bool>(scope);
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
            (left is IAssignable).OrThrows<ArgumentException>("The left side of the equal-sign cannot be assigned");
            (left as IAssignable).Assgin(right, scope);
            return left;
        }
        private static Expression ArithemeticOp(Expression LeftOperand, Expression RightOperand, Operator op, Scope scope)
        {
            var left = LeftOperand.AsConstant(scope);
            var right = RightOperand.AsConstant(scope);
            switch (op)
            {
                case Operator.Add: return Add(left.type | right.type, left, right);
                case Operator.Subtract: return Subtract(left.type | right.type, left, right);
                case Operator.Multiply: return Multiply(left.type | right.type, left, right);
                case Operator.Divide: return Divide(left.type | right.type, left, right);
                case Operator.Power: return Power(left.type | right.type, left, right);
                default:
                    throw new NotSupportedException();
            }
        }
        private static Expression CompareOp(Expression LeftOperand, Expression RightOperand, Operator op, Scope scope)
        {
            var left = LeftOperand.AsConstant(scope);
            var right = RightOperand.AsConstant(scope);
            var cmp = Compare(left.type | right.type, left, right);
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
        private static Expression Add(ConstantType DataType, ConstantExpression LeftOperand, ConstantExpression RightOperand)
        {
            switch (DataType)
            {
                /* Integer and Double */
                case ConstantType.Integer | ConstantType.Integer:
                    return LeftOperand.GetStruct<int>() + RightOperand.GetStruct<int>();
                case ConstantType.Integer | ConstantType.Double:
                    return LeftOperand.GetDouble() + RightOperand.GetDouble();
                case ConstantType.Double | ConstantType.Double:
                    return LeftOperand.GetStruct<double>() + RightOperand.GetStruct<double>();
                /* String */
                case ConstantType.String | ConstantType.Integer:
                case ConstantType.String | ConstantType.Double:
                case ConstantType.String | ConstantType.Boolean:
                case ConstantType.String | ConstantType.String:
                case ConstantType.String | ConstantType.Vector:
                case ConstantType.String | ConstantType.Matrix:
                    return LeftOperand.GetClass<string>() + RightOperand.GetClass<string>();
                /* Vector */
                case ConstantType.Vector | ConstantType.Vector:
                    return LeftOperand.GetClass<Vector<double>>() + RightOperand.GetClass<Vector<double>>();
                case ConstantType.Vector | ConstantType.Integer:
                case ConstantType.Vector | ConstantType.Double:
                    if (LeftOperand.type == ConstantType.Vector)
                        return LeftOperand.GetClass<Vector<double>>() + RightOperand.GetDouble();
                    else
                        return LeftOperand.GetDouble() + RightOperand.GetClass<Vector<double>>();
                /* Matrix */
                case ConstantType.Matrix | ConstantType.Matrix:
                    return LeftOperand.GetClass<Matrix<double>>() + RightOperand.GetClass<Matrix<double>>();
                case ConstantType.Matrix | ConstantType.Integer:
                case ConstantType.Matrix | ConstantType.Double:
                    if (LeftOperand.type == ConstantType.Matrix)
                        return LeftOperand.GetClass<Matrix<double>>() + RightOperand.GetDouble();
                    else
                        return LeftOperand.GetDouble() + RightOperand.GetClass<Matrix<double>>();
                default:
                    throw new NotSupportedException();
            }
        }
        private static Expression Subtract(ConstantType DataType, ConstantExpression LeftOperand, ConstantExpression RightOperand)
        {
            switch (DataType)
            {
                /* Integer and Double */
                case ConstantType.Integer | ConstantType.Integer:
                    return LeftOperand.GetStruct<int>() - RightOperand.GetStruct<int>();
                case ConstantType.Integer | ConstantType.Double:
                    return LeftOperand.GetDouble() - RightOperand.GetDouble();
                case ConstantType.Double | ConstantType.Double:
                    return LeftOperand.GetStruct<double>() - RightOperand.GetStruct<double>();
                /* Vector */
                case ConstantType.Vector | ConstantType.Vector:
                    return LeftOperand.GetClass<Vector<double>>() - RightOperand.GetClass<Vector<double>>();
                case ConstantType.Vector | ConstantType.Integer:
                case ConstantType.Vector | ConstantType.Double:
                    if (LeftOperand.type == ConstantType.Vector)
                        return LeftOperand.GetClass<Vector<double>>() - RightOperand.GetDouble();
                    else
                        return LeftOperand.GetDouble() - RightOperand.GetClass<Vector<double>>();
                /* Matrix */
                case ConstantType.Matrix | ConstantType.Matrix:
                    return LeftOperand.GetClass<Matrix<double>>() - RightOperand.GetClass<Matrix<double>>();
                case ConstantType.Matrix | ConstantType.Integer:
                case ConstantType.Matrix | ConstantType.Double:
                    if (LeftOperand.type == ConstantType.Matrix)
                        return LeftOperand.GetClass<Matrix<double>>() - RightOperand.GetDouble();
                    else
                        return LeftOperand.GetDouble() - RightOperand.GetClass<Matrix<double>>();
                default:
                    throw new NotSupportedException();
            }
        }
        private static Expression Multiply(ConstantType DataType, ConstantExpression LeftOperand, ConstantExpression RightOperand)
        {
            switch (DataType)
            {
                /* Integer and Double */
                case ConstantType.Integer | ConstantType.Integer:
                    return LeftOperand.GetStruct<int>() * RightOperand.GetStruct<int>();
                case ConstantType.Integer | ConstantType.Double:
                    return LeftOperand.GetDouble() * RightOperand.GetDouble();
                case ConstantType.Double | ConstantType.Double:
                    return LeftOperand.GetStruct<double>() * RightOperand.GetStruct<double>();
                /* Vector */
                case ConstantType.Vector | ConstantType.Vector:
                    return LeftOperand.GetClass<Vector<double>>().PointwiseMultiply((RightOperand.GetClass<Vector<double>>()));
                case ConstantType.Vector | ConstantType.Integer:
                case ConstantType.Vector | ConstantType.Double:
                    if (LeftOperand.type == ConstantType.Vector)
                        return LeftOperand.GetClass<Vector<double>>() * RightOperand.GetDouble();
                    else
                        return LeftOperand.GetDouble() * RightOperand.GetClass<Vector<double>>();
                /* Matrix */
                case ConstantType.Matrix | ConstantType.Matrix:
                    return LeftOperand.GetClass<Matrix<double>>() * RightOperand.GetClass<Matrix<double>>();
                case ConstantType.Matrix | ConstantType.Integer:
                case ConstantType.Matrix | ConstantType.Double:
                    if (LeftOperand.type == ConstantType.Matrix)
                        return LeftOperand.GetClass<Matrix<double>>() * RightOperand.GetDouble();
                    else
                        return LeftOperand.GetDouble() * RightOperand.GetClass<Matrix<double>>();
                default:
                    throw new NotSupportedException();
            }
        }
        private static Expression Divide(ConstantType DataType, ConstantExpression LeftOperand, ConstantExpression RightOperand)
        {
            switch (DataType)
            {
                /* Integer and Double */
                case ConstantType.Integer | ConstantType.Integer:
                    return LeftOperand.GetStruct<int>() / RightOperand.GetStruct<int>();
                case ConstantType.Integer | ConstantType.Double:
                    return LeftOperand.GetDouble() / RightOperand.GetDouble();
                case ConstantType.Double | ConstantType.Double:
                    return LeftOperand.GetStruct<double>() / RightOperand.GetStruct<double>();
                /* Vector */
                case ConstantType.Vector | ConstantType.Vector:
                    return LeftOperand.GetClass<Vector<double>>() / RightOperand.GetClass<Vector<double>>();
                case ConstantType.Vector | ConstantType.Integer:
                case ConstantType.Vector | ConstantType.Double:
                    if (LeftOperand.type == ConstantType.Vector)
                        return LeftOperand.GetClass<Vector<double>>() / RightOperand.GetDouble();
                    else
                        return LeftOperand.GetDouble() / RightOperand.GetClass<Vector<double>>();
                /* Matrix */
                case ConstantType.Matrix | ConstantType.Matrix:
                    return LeftOperand.GetClass<Matrix<double>>() * (RightOperand.GetClass<Matrix<double>>().Inverse());
                case ConstantType.Matrix | ConstantType.Integer:
                case ConstantType.Matrix | ConstantType.Double:
                    if (LeftOperand.type == ConstantType.Matrix)
                        return LeftOperand.GetClass<Matrix<double>>() / RightOperand.GetDouble();
                    else
                        return LeftOperand.GetDouble() / RightOperand.GetClass<Matrix<double>>();
                default:
                    throw new NotSupportedException();
            }
        }
        private static Expression Power(ConstantType DataType, ConstantExpression LeftOperand, ConstantExpression RightOperand)
        {
            switch (DataType)
            {
                case ConstantType.Integer | ConstantType.Integer:
                    return (int)Math.Pow(LeftOperand.GetStruct<int>(), RightOperand.GetStruct<int>());
                case ConstantType.Integer | ConstantType.Double:
                    return Math.Pow(LeftOperand.GetDouble(), RightOperand.GetDouble());
                case ConstantType.Double | ConstantType.Double:
                    return Math.Pow(LeftOperand.GetStruct<double>(), RightOperand.GetStruct<double>());
                /* Vector */
                case ConstantType.Vector | ConstantType.Integer:
                case ConstantType.Vector | ConstantType.Double:
                    if (LeftOperand.type == ConstantType.Vector)
                        return LeftOperand.GetClass<Vector<double>>().PointwisePower(RightOperand.GetDouble());
                    else
                        goto default;
                /* Matrix */
                case ConstantType.Matrix | ConstantType.Integer:
                    (LeftOperand.type == ConstantType.Matrix).OrThrows<NotSupportedException>();
                    return LeftOperand.GetClass<Matrix<double>>().Power(RightOperand.GetStruct<int>());
                default:
                    throw new NotSupportedException();
            }
        }
        private static int Compare(ConstantType DataType, ConstantExpression LeftOperand, ConstantExpression RightOperand)
        {
            if (LeftOperand.type == RightOperand.type)
                return (LeftOperand.Value as IComparable).CompareTo(RightOperand.Value);
            switch (DataType)
            {
                case ConstantType.Integer | ConstantType.Double:
                    return LeftOperand.GetDouble().CompareTo(RightOperand.GetDouble());
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
