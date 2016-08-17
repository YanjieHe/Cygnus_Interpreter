using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.Errors;
using Cygnus.SymbolTable;

namespace Cygnus.SyntaxTree
{
    public class DictionaryExpression : Expression, ICollectionExpression, IEnumerable<Expression>, IEquatable<DictionaryExpression>
    {
        public Dictionary<ConstantExpression, Expression> Dict { get; private set; }
        public DictionaryExpression(Expression[][] kvps)
        {
            Dict = new Dictionary<ConstantExpression, Expression>();
            for (int i = 0; i < kvps.Length; i++)
            {
                if (kvps[i].Length != 2)
                    throw new SyntaxException("The length of the key-value pair of the dictionary must be 2");
                Dict[kvps[i][0] as ConstantExpression] = kvps[i][1];
            }
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
                return Dict[index.Eval(scope).GetValue<ConstantExpression>(ExpressionType.Constant, scope)];
            }
            set
            {
                Dict[index.Eval(scope).GetValue<ConstantExpression>(ExpressionType.Constant, scope)] = value;
            }
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

        public override Expression Eval(Scope scope)
        {
            return new DictionaryExpression(Dict);
        }
    }
}
