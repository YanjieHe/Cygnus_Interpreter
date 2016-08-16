using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.Errors;
namespace Cygnus.SyntaxTree
{
    public class DictionaryExpression : Expression, ICollectionExpression, IEnumerable<Expression>, IEquatable<DictionaryExpression>
    {
        public Dictionary<ConstantExpression, Expression> Dict { get; private set; }
        public DictionaryExpression(Expression[][] kvps)
        {
            Dict = new Dictionary<ConstantExpression, Expression>();
            if (kvps != null)
                for (int i = 0; i < kvps.Length; i++)
                {
                    if (kvps[i].Length != 2)
                        throw new SyntaxException("The length of the key-value pair of the dictionary must be 2");
                    Dict[kvps[i][0].GetValue<ConstantExpression>(ExpressionType.Constant)] = kvps[i][1];
                }
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Dictionary;
            }
        }

        public ConstantExpression Length
        {
            get
            {
                return new ConstantExpression(Dict.Count, ConstantType.Integer);
            }
        }

        public Expression this[Expression index]
        {
            get
            {
                return Dict[index.GetValue<ConstantExpression>(ExpressionType.Constant)];
            }
            set
            {
                Dict[index.GetValue<ConstantExpression>(ExpressionType.Constant)] = value;
            }
        }
        public override Expression Eval()
        {
            return this;
        }

        public IEnumerator<Expression> GetEnumerator()
        {
            foreach (var kvp in Dict)
            {
                yield return new ArrayExpression(new Expression[] { kvp.Key, kvp.Value });
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var kvp in Dict)
            {
                yield return new ArrayExpression(new Expression[] { kvp.Key, kvp.Value });
            }
        }

        public bool Equals(DictionaryExpression other)
        {
            if (Dict.Count != other.Dict.Count) return false;
            foreach (var kvp in Dict)
            {
                Expression value = null;
                if (other.Dict.TryGetValue(kvp.Key, out value))
                {
                    if (!kvp.Value.Equals(value))
                        return false;
                }
                else return false;
            }
            return true;
        }
    }
}
