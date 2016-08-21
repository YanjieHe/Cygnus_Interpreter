using System;
using Cygnus.LexicalAnalyzer;
using Cygnus.SymbolTable;

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
            if (Op == Operator.Assgin)
                return AssginOp(Left, Right, scope);
            var lvalue = Left.Eval(scope);
            var rvalue = Right.Eval(scope);

            var left = lvalue.GetValue<ConstantExpression>(ExpressionType.Constant, scope);
            var right = rvalue.GetValue<ConstantExpression>(ExpressionType.Constant, scope);

            switch (Op)
            {
                case Operator.Add:
                case Operator.Subtract:
                case Operator.Multiply:
                case Operator.Divide:
                case Operator.Power:
                    return ArithemeticOp(left, right, Op);
                case Operator.And:
                    return (bool)left.Value && (bool)right.Value;
                case Operator.Or:
                    return (bool)left.Value || (bool)right.Value;
                case Operator.Less:
                case Operator.Greater:
                case Operator.LessOrEquals:
                case Operator.GreaterOrEquals:
                    return CompareOp(left, right, Op);
                case Operator.Equals:
                case Operator.NotEqualTo:
                    return EqualsOp(left, right, Op);
                case Operator.Assgin:
                    break;
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
        public static ConstantExpression ArithemeticOp(ConstantExpression left, ConstantExpression right, Operator op)
        {
            if (left.constantType == ConstantType.Integer && right.constantType == ConstantType.Integer)
            {
                switch (op)
                {
                    case Operator.Add:
                        return Constant((int)left.Value + (int)right.Value, ConstantType.Integer);
                    case Operator.Subtract:
                        return Constant((int)left.Value - (int)right.Value, ConstantType.Integer);
                    case Operator.Multiply:
                        return Constant((int)left.Value * (int)right.Value, ConstantType.Integer);
                    case Operator.Divide:
                        return Constant((int)left.Value / (int)right.Value, ConstantType.Integer);
                    case Operator.Power:
                        return Constant((int)Math.Pow((int)left.Value, (int)right.Value), ConstantType.Integer);
                    default: throw new NotSupportedException();
                }

            }
            else if ((left.constantType == ConstantType.Integer && right.constantType == ConstantType.Double)
                   || (left.constantType == ConstantType.Double && right.constantType == ConstantType.Integer)
                   || (left.constantType == ConstantType.Double && right.constantType == ConstantType.Double))
            {
                switch (op)
                {
                    case Operator.Add:
                        return Constant(GetDouble(left) + GetDouble(right), ConstantType.Double);
                    case Operator.Subtract:
                        return Constant(GetDouble(left) - GetDouble(right), ConstantType.Double);
                    case Operator.Multiply:
                        return Constant(GetDouble(left) * GetDouble(right), ConstantType.Double);
                    case Operator.Divide:
                        return Constant(GetDouble(left) / GetDouble(right), ConstantType.Double);
                    case Operator.Power:
                        return Constant(Math.Pow(GetDouble(left), GetDouble(right)), ConstantType.Double);
                    default: throw new NotSupportedException();
                }
            }
            else if (op == Operator.Add
                && (left.constantType == ConstantType.String
                || right.constantType == ConstantType.String))
            {
                return Constant(left.Value.ToString() + right.Value.ToString(), ConstantType.String);
            }
            else throw new NotSupportedException();
        }
        public static ConstantExpression CompareOp(ConstantExpression left, ConstantExpression right, Operator op)
        {
            var cmpLeft = left.Value as IComparable;
            var cmpRight = right.Value as IComparable;
            if (cmpLeft == null || cmpRight == null) throw new NotSupportedException();
            switch (op)
            {
                case Operator.Less:
                    return Constant(cmpLeft.CompareTo(cmpRight) < 0, ConstantType.Boolean);
                case Operator.Greater:
                    return Constant(cmpLeft.CompareTo(cmpRight) > 0, ConstantType.Boolean);
                case Operator.LessOrEquals:
                    return Constant(cmpLeft.CompareTo(cmpRight) <= 0, ConstantType.Boolean);
                case Operator.GreaterOrEquals:
                    return Constant(cmpLeft.CompareTo(cmpRight) >= 0, ConstantType.Boolean);
            }
            throw new NotSupportedException();
        }
        public static ConstantExpression EqualsOp(ConstantExpression left, ConstantExpression right, Operator op)
        {
            switch (op)
            {
                case Operator.Equals:
                    return Expression.Constant(left.Equals(right), ConstantType.Boolean);
                case Operator.NotEqualTo:
                    return Expression.Constant(!left.Equals(right), ConstantType.Boolean);
            }
            throw new NotSupportedException();
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
