using System;

namespace Cygnus.SyntaxTree
{
    public class ConstantExpression : Expression, IEquatable<ConstantExpression>
    {
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Constant;
            }
        }
        public object Value;
        public ConstantType constantType;
        public ConstantExpression(object value, ConstantType type)
        {
            Value = value;
            constantType = type;
        }
        public static implicit operator ConstantExpression (int value)
        {
            return new ConstantExpression(value, ConstantType.Integer);
        }
        public static implicit operator ConstantExpression(double value)
        {
            return new ConstantExpression(value, ConstantType.Double);
        }
        public static implicit operator ConstantExpression(string value)
        {
            return new ConstantExpression(value, ConstantType.String);
        }
        public static implicit operator ConstantExpression(bool value)
        {
            return new ConstantExpression(value, ConstantType.Boolean);
        }
        public override Expression Eval()
        {
            return this;
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
            return constantType == other.constantType && Value.Equals(other.Value);
        }
        public Expression Clone()
        {
            return new ConstantExpression(Value, constantType);
        }
    }
    public enum ConstantType
    {
        Integer, Double,
        String, Char,
        Void, Null,
        Boolean,
        Array,
    }
}
