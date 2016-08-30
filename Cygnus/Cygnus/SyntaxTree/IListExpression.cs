using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.Extensions;
namespace Cygnus.SyntaxTree
{
    public class IListExpression<T> : Expression, IEnumerable<Expression>, IEquatable<IListExpression<T>>, IIndexable where T : IList<Expression>
    {
        public T Values;
        public IListExpression(T Values)
        {
            this.Values = Values;
        }
        public ConstantExpression Length
        {
            get
            {
                return Constant(Values.Count, ConstantType.Integer);
            }
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.IList;
            }
        }

        public Expression this[Expression index, Scope scope]
        {
            get
            {
                return Values[index.Eval(scope).As<int>(scope)];
            }
            set
            {
                Values[index.Eval(scope).As<int>(scope)] = value;
            }
        }

        public override Expression Eval(Scope scope)
        {
            for (int i = 0; i < Values.Count; i++)
            {
                Values[i] = Values[i].Eval(scope);
            }
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
        public override void Display()
        {
            Values.DisplayList();
        }
        public bool Equals(IListExpression<T> other)
        {
            int n = Values.Count;
            if (n != other.Values.Count) return false;
            for (int i = 0; i < n; i++)
                if (!Values[i].Equals(other.Values[i]))
                    return false;
            return true;
        }
        public override string ToString()
        {
            return "(IList)";
        }
    }
}
