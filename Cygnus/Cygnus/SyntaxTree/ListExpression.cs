using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SymbolTable;

namespace Cygnus.SyntaxTree
{
    public class ListExpression : Expression, ICollectionExpression, IEnumerable<Expression>, IEquatable<ListExpression>
    {
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.List;
            }
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

        public bool Equals(ListExpression other)
        {
            int n = Values.Count;
            if (n != other.Values.Count) return false;
            for (int i = 0; i < n; i++)
                if (!Values[i].Equals(other.Values[i]))
                    return false;
            return true;
        }

        public override Expression Eval(Scope scope)
        {
            for (int i = 0; i < Values.Count; i++)
                Values[i] = Values[i].Eval(scope);
            return this;
        }

        public List<Expression> Values { get; private set; }
        public ListExpression(List<Expression> Values)
        {
            this.Values = Values;
        }
        public ListExpression(Expression[] Values)
        {
            if (Values.Length == 0)
                this.Values = new List<Expression>();
            else
                this.Values = new List<Expression>(Values);
        }
        public ConstantExpression Length
        {
            get
            {
                return new ConstantExpression(Values.Count, ConstantType.Integer);
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

    }
}
