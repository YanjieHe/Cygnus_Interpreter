using System;
using System.Collections;
using System.Collections.Generic;

namespace Cygnus.SyntaxTree
{
    public class IEnumerableExpression : Expression, IEnumerable<Expression>, IEquatable<IEnumerableExpression>
    {
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.IEnumerable;
            }
        }
        public IEnumerable<Expression> Collection { get; private set; }
        public IEnumerableExpression(IEnumerable<Expression> Collection)
        {
            this.Collection = Collection;
        }
        public IEnumerator<Expression> GetEnumerator()
        {
            foreach (var item in Collection)
                yield return item;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        public bool Equals(IEnumerableExpression other)
        {
            using (var x = this.GetEnumerator())
            using (var y = other.GetEnumerator())
                while (true)
                {
                    var x_move = x.MoveNext();
                    var y_move = y.MoveNext();
                    if (x_move != y_move) return false;
                    if (x_move)
                        if (!x.Current.Equals(y.Current)) return false;
                        else break;
                }
            return true;
        }
        public override Expression Eval(Scope scope)
        {
            return this;
        }
        public override string ToString()
        {
            return "(IEnumerable)";
        }
    }
}
