using System;
using System.Collections;
using System.Collections.Generic;

namespace Cygnus.SyntaxTree
{
    public class DictionaryExpression : Expression, IIndexable, IEnumerable<Expression>, IEquatable<DictionaryExpression>
    {
        public Dictionary<ConstantExpression, Expression> Dict { get; private set; }
        public DictionaryExpression(IEnumerable<KeyValuePair<ConstantExpression, Expression>> kvps)
        {
            Dict = new Dictionary<ConstantExpression, Expression>();
            foreach (var kvp in kvps)
                Dict.Add(kvp.Key, kvp.Value);
        }
        public DictionaryExpression(Dictionary<ConstantExpression, Expression> dict)
        {
            Dict = dict;
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

        public Expression this[Expression index, Scope scope]
        {
            get
            {
                return Dict[index.AsConstant(scope)];
            }
            set
            {
                Dict[index.AsConstant(scope)] = value;
            }
        }
        public IEnumerator<Expression> GetEnumerator()
        {
            foreach (var kvp in Dict)
            {
                yield return Array(new Expression[] { kvp.Key, kvp.Value });
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var kvp in Dict)
            {
                yield return Array(new Expression[] { kvp.Key, kvp.Value });
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

        public override Expression Eval(Scope scope)
        {
            return new DictionaryExpression(Dict);
        }
        public override string ToString()
        {
            return "(Dictionary)";
        }
    }
}
