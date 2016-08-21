using System.Collections.Generic;
using Cygnus.Errors;
namespace Cygnus.SyntaxTree
{
    public class TableExpression : Expression
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
        public TableExpression(KeyValuePair<string, Expression>[] properties)
        {
            Properties = new Dictionary<string, Expression>(properties.Length);
            foreach (var kvp in properties)
                Properties.Add(kvp.Key, kvp.Value);
        }
        public override Expression Eval(Scope scope)
        {
            foreach (var key in Properties.Keys)
                if (Properties[key].NodeType == ExpressionType.Call)
                    Properties[key] = Properties[key].Eval(scope);
            return this;
        }
        public override string ToString()
        {
            return "Table";
        }
    }
}
