using System.Linq;
using Cygnus.SyntaxTree;
using Cygnus.Errors;
namespace Cygnus.SymbolTable
{
    public class Scope
    {
        public Scope Parent { get; private set; }
        public VariableTable variableTable;
        public Scope()
        {
            Parent = null;
            variableTable = new VariableTable();
        }
        public Scope(Scope Parent)
        {
            this.Parent = Parent;
            variableTable = new VariableTable();
        }
        public void Append(string Name, Expression Value)
        {
            variableTable.Add(Name, Value);
        }
        public Expression Find(string Name)
        {
            Scope current = this;
            while (current != null)
            {
                Expression Value = null;
                if (current.variableTable.TryGetValue(Name, out Value))
                    return Value;
                current = current.Parent;
            }
            if (FunctionExpression.functionTable.ContainsKey(Name)
                || FunctionExpression.builtInMethodTable.ContainsKey(Name))
            {
                return new CallExpression(Name, null);
            }
            throw new NotDefinedException(Name);
        }
        public Expression Assgin(string Name, Expression Value)
        {
            Scope current = this;
            while (current != null)
            {
                if (current.variableTable.ContainsKey(Name))
                {
                    current.variableTable[Name] = Value;
                    return Value;
                }
                current = current.Parent;
            }
            variableTable[Name] = Value;
            return Value;
        }
        public bool TryGetValue(string Name,out Expression Value)
        {
            Value = null;
            Scope current = this;
            while (current != null)
            {
                if (current.variableTable.TryGetValue(Name, out Value))
                    return true;
                current = current.Parent;
            }
            if (FunctionExpression.functionTable.ContainsKey(Name)
                || FunctionExpression.builtInMethodTable.ContainsKey(Name))
            {
                Value =  new CallExpression(Name, null);
                return true;
            }
            return false;
        }
        public bool Delete(string Name)
        {
            Scope current = this;
            while (current != null)
            {
                if (current.variableTable.ContainsKey(Name))
                {
                    current.variableTable.Remove(Name);
                    return true;
                }
                current = current.Parent;
            }
            return false;
        }
        public Scope SpawnScopeWith(ParameterExpression[] parameters, Expression[] values)
        {
            Scope scope = new Scope(this);
            for (int i = 0; i < values.Length; i++)
                scope.variableTable.Add(parameters[i].Name, values[i]);
            return scope;
        }
        public override string ToString()
        {
            return "Scope = {\r\n" + string.Join("\r\n", variableTable.Select(i => i)) + "\r\n}";
        }

    }
}
