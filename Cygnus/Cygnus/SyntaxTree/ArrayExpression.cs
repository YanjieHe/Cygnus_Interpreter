using System;
using System.Collections;
using System.Collections.Generic;
using Cygnus.SymbolTable;

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
        public Expression this[Expression index, Scope scope]
        {
            get
            {
                return Values[(int)index.Eval(scope).GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value];
            }
            set
            {
                Values[(int)index.Eval(scope).GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value] = value;
            }
        }
        public override Expression Eval(Scope scope)
        {
            for (int i = 0; i < Values.Length; i++)
                Values[i] = Values[i].Eval(scope);
            return this;
        }
        public IEnumerator<Expression> GetEnumerator()
        {
            foreach (var item in Values)
                yield return item;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
