using Cygnus.DataStructures;
namespace Cygnus.Expressions
{
    public class ParameterExpression : Expression, IAssignable
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
        public override string ToString()
        {
            return string.Format("(Var: {0})", Name);
        }
        public override Expression Eval(Scope scope)
        {
            return scope.GetVariable(Name);
        }
        public void Assgin(Expression value, Scope scope)
        {
            scope.Assgin(Name, value.GetValue(scope));
        }
    }
}
