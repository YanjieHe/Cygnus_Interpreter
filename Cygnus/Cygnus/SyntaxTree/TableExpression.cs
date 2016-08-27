using System.Collections.Generic;
using Cygnus.Errors;
namespace Cygnus.SyntaxTree
{
    public class TableExpression : Expression, ITable
    {
        public TableExpression Parent { get; set; }
        public Dictionary<string, Expression> Properties { get; private set; }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Table;
            }
        }
        public Expression this[string Name]
        {
            get
            {
                return Find(Name);
            }

            set
            {
                Assign(Name, value);
            }
        }

        public Expression Find(string key)
        {
            TableExpression current = this;
            while (current != null)
            {
                Expression Value = null;
                if (current.Properties.TryGetValue(key, out Value))
                    return Value;
                current = current.Parent;
            }
            throw new NotDefinedException(key);
        }
        public Expression Assign(string key, Expression value)
        {
            TableExpression current = this;
            while (current != null)
            {
                if (current.Properties.ContainsKey(key))
                    return current.Properties[key] = value;
                current = current.Parent;
            }
            throw new NotDefinedException(key);
        }
        public TableExpression(params KeyValuePair<string, Expression>[] properties)
        {
            Properties = new Dictionary<string, Expression>(properties.Length);
            foreach (var kvp in properties)
                Properties.Add(kvp.Key, kvp.Value);
        }
        public TableExpression Append(string Name,Expression property)
        {
            Properties.Add(Name, property);
            return this;
        }
        public override Expression Eval(Scope scope)
        {
            return this;
        }
        public override string ToString()
        {
            return "Table";
        }
    }
}
