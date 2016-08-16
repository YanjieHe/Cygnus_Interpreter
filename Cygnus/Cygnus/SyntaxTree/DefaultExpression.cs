using System;

namespace Cygnus.SyntaxTree
{
    public class DefaultExpression : Expression,IEquatable<DefaultExpression>
    {
        public ConstantType defaultType;
        public DefaultExpression(ConstantType defaultType)
        {
            this.defaultType = defaultType;
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Default;
            }
        }
        public override Expression Eval()
        {
            return new ConstantExpression(DefaultForType(), defaultType);
        }
        public override string ToString()
        {
            return string.Format("(Default: {0})", defaultType);
        }
        public object DefaultForType()
        {
            switch (defaultType)
            {
                case ConstantType.Integer:
                    return default(int);
                case ConstantType.Double:
                    return default(double);
                case ConstantType.String:
                    return default(string);
                case ConstantType.Char:
                    return default(char);
                case ConstantType.Void:
                case ConstantType.Null:
                    return null;
                case ConstantType.Boolean:
                    return default(bool);
                case ConstantType.Array:
                    return new ArrayExpression(new Expression[0]);
                default:
                    throw new NotSupportedException();
            }
        }

        public bool Equals(DefaultExpression other)
        {
            return defaultType == other.defaultType;
        }
    }
}
