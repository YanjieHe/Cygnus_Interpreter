using System;
using System.Collections;
using System.Collections.Generic;

namespace Cygnus.SyntaxTree
{
    public class ArrayExpression : Expression, ICollectionExpression, IEnumerable<Expression>, IEquatable<ArrayExpression>
    {
        public Expression[] Values { get; private set; }
        public ArrayExpression(Expression[] Values)
        {
            this.Values = Values;
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Array;
            }
        }

        public ConstantExpression Length
        {
            get
            {
                return new ConstantExpression(Values.Length, ConstantType.Integer);
            }
        }

        public Expression this[Expression index]
        {
            get
            {
                return Values[(int)index.Eval().GetValue<ConstantExpression>(ExpressionType.Constant).Value];
            }
            set
            {
                Values[(int)index.Eval().GetValue<ConstantExpression>(ExpressionType.Constant).Value] = value;
            }
        }
        public override Expression Eval()
        {
            for (int i = 0; i < Values.Length; i++)
            {
                Values[i] = Values[i].Eval();
            }
            return this;
        }
        public IEnumerator<Expression> GetEnumerator()
        {
            foreach (var item in Values)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool Equals(ArrayExpression other)
        {
            int n = Values.Length;
            if (n != other.Values.Length) return false;
            for (int i = 0; i < n; i++)
                if (!Values[i].Equals(other.Values[i]))
                    return false;
            return true;
        }
    }
}
