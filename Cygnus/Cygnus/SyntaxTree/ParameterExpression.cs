using System;
using Cygnus.SymbolTable;

namespace Cygnus.SyntaxTree
{
    public class ParameterExpression : Expression
    {
        public string Name { get; private set; }
        public ParameterExpression(string Name)
        {
            this.Name = Name;
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Parameter;
            }
        }
        public Expression Assgin(Expression value, Scope scope)
        {
            return scope.Assgin(Name, value);
        }
        public override string ToString()
        {
            return string.Format("(Var: {0})", Name);
        }
        public override Expression Eval(Scope scope)
        {
            return scope.Find(Name);
        }
    }
}
