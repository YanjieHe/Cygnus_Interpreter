using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.SyntaxTree
{
    public class KeyValuePairExpression : Expression, ITable
    {
        public KeyValuePair<ConstantExpression, Expression> keyValuePair { get; private set; }
        public KeyValuePairExpression(KeyValuePair<ConstantExpression, Expression> keyValuePair)
        {
            this.keyValuePair = keyValuePair;
        }

        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.KeyValuePair;
            }
        }
        public override void Display()
        {
            Console.Write('[');
            keyValuePair.Key.Display();
            Console.Write(", ");
            keyValuePair.Value.Display();
            Console.Write(']');
        }
        public Expression this[string Name]
        {
            get
            {
                switch (Name)
                {
                    case "key":
                        return keyValuePair.Key;
                    case "value":
                        return keyValuePair.Value;
                    default:
                        throw new ArgumentException();
                }
            }

            set
            {
                throw new ArgumentException("key-value pair cannot be assgined to -- it's read-only");
            }
        }

        public override Expression Eval(Scope scope)
        {

            return this;
        }
    }
}
