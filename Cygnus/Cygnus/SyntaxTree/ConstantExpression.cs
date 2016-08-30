using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Cygnus.Errors;
using Cygnus.Extensions;
namespace Cygnus.SyntaxTree
{
    public class ConstantExpression : Expression, IEquatable<ConstantExpression>, IEnumerable<Expression>
    {
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Constant;
            }
        }
        public object Value { get; protected set; }
        public ConstantType type { get; protected set; }
        public ConstantExpression(object Value, ConstantType type)
        {
            this.Value = Value;
            this.type = type;
        }
        protected ConstantExpression(ConstantType type)
        {
            this.type = type;
        }
        public override void Display(Scope scope)
        {
            Console.Write(Value ?? "Null");
        }
        public double GetDouble()
        {
            return type == ConstantType.Integer ? (int)Value : (double)Value;
        }
        public T GetStruct<T>() where T : struct
        {
            return (T)Value;
        }
        public T GetClass<T>() where T : class
        {
            return Value as T;
        }
        public override string ToString()
        {
            return string.Format("(Constant: {0}  Type: {1})", Value, type);
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
            else if ((type | other.type) == (ConstantType.Integer | ConstantType.Double))
                return GetDouble().Equals(GetDouble());
            else
                return false;
        }
        public override Expression Eval(Scope scope)
        {
            return new ConstantExpression(Value, type);
        }
        public IEnumerator<Expression> GetEnumerator()
        {
            (type == ConstantType.String).OrThrows<NotSupportedException>(type.ToString());
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
        Vector = 32,
        Matrix = 64,
        Null, Void
    }
}
