using System;

namespace Cygnus.SyntaxTree
{
    public sealed class ConstantExpression : Expression, IComputable, IEquatable<ConstantExpression>
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
                this.type = ConstantType.Null;
            else if (value is int)
                this.type = ConstantType.Integer;
            else if (value is double)
                this.type = ConstantType.Double;
            else if (value is bool)
                this.type = ConstantType.Boolean;
            else if (value is char)
                this.type = ConstantType.Char;
            else if (value is string)
                this.type = ConstantType.String;
            else
                throw new NotSupportedException(value.ToString());
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
                if (type == ConstantType.Integer && other.type == ConstantType.Integer)
                {
                    return ((int)value) + ((int)other.value);
                }
                else if (type == ConstantType.Double && other.type == ConstantType.Double)
                {
                    return ((double)value) + ((double)other.value);
                }
                else if (type == ConstantType.Integer && other.type == ConstantType.Double)
                {
                    return ((int)value) + ((double)other.value);
                }
                else if (type == ConstantType.Double && other.type == ConstantType.Integer)
                {
                    return ((double)value) + ((int)other.value);
                }
                else if (type == ConstantType.String || other.type == ConstantType.String)
                {
                    return value.ToString() + other.value.ToString();
                }
                else throw new NotSupportedException();
            }
            else
                throw new ArithmeticException();
        }

        public Expression Subtract(Expression Other)
        {
            if (Other.NodeType == ExpressionType.Constant)
            {
                var other = Other as ConstantExpression;
                if (type == ConstantType.Integer && other.type == ConstantType.Integer)
                {
                    return ((int)value) - ((int)other.value);
                }
                else if (type == ConstantType.Double && other.type == ConstantType.Double)
                {
                    return ((double)value) - ((double)other.value);
                }
                else if (type == ConstantType.Integer && other.type == ConstantType.Double)
                {
                    return ((int)value) - ((double)other.value);
                }
                else if (type == ConstantType.Double && other.type == ConstantType.Integer)
                {
                    return ((double)value) - ((int)other.value);
                }
                else throw new NotSupportedException();
            }
            else
                throw new ArithmeticException();
        }

        public Expression Multiply(Expression Other)
        {
            if (Other.NodeType == ExpressionType.Constant)
            {
                var other = Other as ConstantExpression;
                if (type == ConstantType.Integer && other.type == ConstantType.Integer)
                {
                    return ((int)value) * ((int)other.value);
                }
                else if (type == ConstantType.Double && other.type == ConstantType.Double)
                {
                    return ((double)value) * ((double)other.value);
                }
                else if (type == ConstantType.Integer && other.type == ConstantType.Double)
                {
                    return ((int)value) * ((double)other.value);
                }
                else if (type == ConstantType.Double && other.type == ConstantType.Integer)
                {
                    return ((double)value) * ((int)other.value);
                }
                else throw new NotSupportedException();
            }
            else
                throw new ArithmeticException();
        }

        public Expression Divide(Expression Other)
        {
            if (Other.NodeType == ExpressionType.Constant)
            {
                var other = Other as ConstantExpression;
                if (type == ConstantType.Integer && other.type == ConstantType.Integer)
                {
                    return ((int)value) / ((int)other.value);
                }
                else if (type == ConstantType.Double && other.type == ConstantType.Double)
                {
                    return ((double)value) / ((double)other.value);
                }
                else if (type == ConstantType.Integer && other.type == ConstantType.Double)
                {
                    return ((int)value) / ((double)other.value);
                }
                else if (type == ConstantType.Double && other.type == ConstantType.Integer)
                {
                    return ((double)value) / ((int)other.value);
                }
                else throw new NotSupportedException();
            }
            else
                throw new ArithmeticException();
        }

        public Expression Power(Expression Other)
        {
            if (Other.NodeType == ExpressionType.Constant)
            {
                var other = Other as ConstantExpression;
                if (type == ConstantType.Integer && other.type == ConstantType.Integer)
                {
                    return (int)Math.Pow(((int)value), ((int)other.value));
                }
                else if (type == ConstantType.Double && other.type == ConstantType.Double)
                {
                    return Math.Pow(((double)value), ((double)other.value));
                }
                else if (type == ConstantType.Integer && other.type == ConstantType.Double)
                {
                    return Math.Pow((int)value, (double)other.value);
                }
                else if (type == ConstantType.Double && other.type == ConstantType.Integer)
                {
                    return Math.Pow(((double)value), ((int)other.value));
                }
                else throw new NotSupportedException();
            }
            else
                throw new ArithmeticException();
        }
    }
    public enum ConstantType
    {
        Integer, Double,
        String, Char,
        Void, Null,
        Boolean,
    }
}
