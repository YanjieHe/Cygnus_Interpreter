using System;
using Cygnus.SymbolTable;

namespace Cygnus.SyntaxTree
{
    public sealed class ConstantExpression : Expression, IEquatable<ConstantExpression>
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
        public override Expression Eval(Scope scope)
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
