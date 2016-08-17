using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SyntaxTree;
namespace Cygnus.SymbolTable
{
    public class Scope
    {
        public Scope Parent { get; private set; }
        public VariableTable variableTable;
        public Scope()
        {
            this.Parent = null;
            this.variableTable = new VariableTable();
        }
        public Scope(Scope parent)
        {
            this.Parent = parent;
            this.variableTable = new VariableTable();
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
            throw new Exception(Name + " is not defined.");
        }
        public Expression Assgin(string Name, Expression Value)
        {
            variableTable[Name] = Value;
            return Value;
        }
        public Scope SpawnScopeWith(ParameterExpression[] parameters, Expression[] values)
        {
            Scope scope = new Scope(this);
            for (int i = 0; i < values.Length; i++)
                scope.variableTable.Add(parameters[i].Name, values[i]);
            return scope;
        }
        public Expression FindInTop(string Name)
        {
            if (variableTable.ContainsKey(Name))
                return variableTable[Name];
            else return null;
        }
        public override string ToString()
        {
            return "Scope = {\r\n" + string.Join("\r\n", variableTable.Select(i => i)) + "\r\n}";
        }

    }
}
