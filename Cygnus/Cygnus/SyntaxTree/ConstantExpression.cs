using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Cygnus.Errors;
using Cygnus.Extensions;
namespace Cygnus.SyntaxTree
{
    public sealed class ConstantExpression : Expression, IComputable, IEquatable<ConstantExpression>, IEnumerable<Expression>
    {
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Constant;
            }
        }
        public object Value
        {
            get { return value; }
            set { this.value = value; }
        }
        public ConstantType constantType
        {
            get { return type; }
            set { type = value; }
        }
        private object value;
        private ConstantType type;
        public ConstantExpression(object value, ConstantType type)
        {
            this.value = value;
            this.type = type;
        }
        public ConstantExpression(object value)
        {
            this.value = value;
            if (value == null)
                type = ConstantType.Null;
            else if (value is int)
                type = ConstantType.Integer;
            else if (value is double)
                type = ConstantType.Double;
            else if (value is bool)
                type = ConstantType.Boolean;
            else if (value is char)
                type = ConstantType.Char;
            else if (value is string)
                type = ConstantType.String;
            else
                throw new NotSupportedException(value.ToString());
        }
        public override void Display()
        {
            Console.Write(Value ?? "Null");
        }
        public double GetDouble()
        {
            switch (constantType)
            {
                case ConstantType.Integer: return (int)Value;
                case ConstantType.Double: return (double)Value;
                default:
                    throw new NotSupportedException();
            }
        }
        public override string ToString()
        {
            return string.Format("(Constant: {0}  Type: {1})", Value, constantType);
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals(obj);
        }
        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public bool Equals(ConstantExpression other)
        {
            if (type == other.type)
            {
                if (type == ConstantType.Null || type == ConstantType.Void)
                    return true;
                else
                    return Value.Equals(other.Value);
            }
            else
                return false;
        }
        public override Expression Eval(Scope scope)
        {
            return new ConstantExpression(Value, constantType);
        }

        public Expression Add(Expression Other)
        {
            if (Other.NodeType == ExpressionType.Constant)
            {
                var other = Other as ConstantExpression;
                switch (type | other.type)
                {
                    case ConstantType.Integer | ConstantType.Integer:
                        return ((int)value) + ((int)other.value);
                    case ConstantType.Double | ConstantType.Double:
                        return ((double)value) + ((double)other.value);
                    case ConstantType.Integer | ConstantType.Double:
                        return GetDouble() - other.GetDouble();
                    case ConstantType.String | ConstantType.Integer:
                    case ConstantType.String | ConstantType.Double:
                    case ConstantType.String | ConstantType.Boolean:
                    case ConstantType.String | ConstantType.Char:
                    case ConstantType.String | ConstantType.String:
                    case ConstantType.String | ConstantType.Null:
                        return value.ToString() + other.value.ToString();
                    default:
                        throw new NotSupportedException();
                }
            }
            else if (Other.NodeType == ExpressionType.Matrix)
            {
                return new MatrixExpression(GetDouble() + (Other as MatrixExpression).Data);
            }
            else
                throw new ArithmeticException();
        }

        public Expression Subtract(Expression Other)
        {
            if (Other.NodeType == ExpressionType.Constant)
            {
                var other = Other as ConstantExpression;
                switch (type | other.type)
                {
                    case ConstantType.Integer | ConstantType.Integer:
                        return ((int)value) - ((int)other.value);
                    case ConstantType.Double | ConstantType.Double:
                        return ((double)value) - ((double)other.value);
                    case ConstantType.Integer | ConstantType.Double:
                        return GetDouble() - other.GetDouble();
                    default:
                        throw new NotSupportedException();
                }
            }
            else if (Other.NodeType == ExpressionType.Matrix)
            {
                return new MatrixExpression(GetDouble() - (Other as MatrixExpression).Data);
            }
            else
                throw new ArithmeticException();
        }

        public Expression Multiply(Expression Other)
        {
            if (Other.NodeType == ExpressionType.Constant)
            {
                var other = Other as ConstantExpression;
                switch (type | other.type)
                {
                    case ConstantType.Integer | ConstantType.Integer:
                        return ((int)value) * ((int)other.value);
                    case ConstantType.Double | ConstantType.Double:
                        return ((double)value) * ((double)other.value);
                    case ConstantType.Integer | ConstantType.Double:
                        return GetDouble() * other.GetDouble();
                    default:
                        throw new NotSupportedException();
                }
            }
            else if (Other.NodeType == ExpressionType.Matrix)
            {
                return new MatrixExpression(GetDouble() * (Other as MatrixExpression).Data);
            }
            else
                throw new ArithmeticException();
        }
        public Expression Divide(Expression Other)
        {
            if (Other.NodeType == ExpressionType.Constant)
            {
                var other = Other as ConstantExpression;
                switch (type | other.type)
                {
                    case ConstantType.Integer | ConstantType.Integer:
                        return ((int)value) / ((int)other.value);
                    case ConstantType.Double | ConstantType.Double:
                        return ((double)value) / ((double)other.value);
                    case ConstantType.Integer | ConstantType.Double:
                        return GetDouble() / other.GetDouble();
                    default:
                        throw new NotSupportedException();
                }
            }
            else if (Other.NodeType == ExpressionType.Matrix)
            {
                return new MatrixExpression(GetDouble() / (Other as MatrixExpression).Data);
            }
            else
                throw new ArithmeticException();
        }

        public Expression Power(Expression Other)
        {
            if (Other.NodeType == ExpressionType.Constant)
            {
                var other = Other as ConstantExpression;
                switch (type | other.type)
                {
                    case ConstantType.Integer | ConstantType.Integer:
                        return (int)Math.Pow((int)value, (int)other.value);
                    case ConstantType.Double | ConstantType.Double:
                        return Math.Pow(((double)value), ((double)other.value));
                    case ConstantType.Integer | ConstantType.Double:
                        return Math.Pow(GetDouble(), other.GetDouble());
                    default:
                        throw new NotSupportedException();
                }
            }
            else
                throw new ArithmeticException();
        }

        public IEnumerator<Expression> GetEnumerator()
        {
            (constantType == ConstantType.String).OrThrows<NotSupportedException>(constantType.ToString());
            foreach (var item in Value as string)
                yield return item.ToString();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this.AsEnumerable();
        }
    }
    public enum ConstantType : byte
    {
        Integer = 1,
        Double = 2,
        Boolean = 4,
        Char = 8,
        String = 16,
        Void = 32,
        Null = 64,
    }
}
