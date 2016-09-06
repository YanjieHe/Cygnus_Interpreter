using Cygnus.Errors;
namespace Cygnus.Expressions
{
    public class CallExpression : Expression
    {
        public string Name { get; private set; }
        public Expression[] Arguments { get; private set; }
        public CallExpression(string Name, Expression[] Arguments)
        {
            this.Name = Name;
            this.Arguments = Arguments;
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Call;
            }
        }
        public override string ToString()
        {
            return "(Function Call: " + Name + ")";
        }
        public override Expression Eval(Scope scope)
        {
            FunctionExpression FUNCTION;
            if (scope.TryGetFunction(Name, out FUNCTION))
            {
                var func = FUNCTION.Update(Arguments, scope);
                return func.Eval(func.funcScope);
            }
            else if (Scope.builtInMethodTable.ContainsKey(Name))
            {
                return Scope.builtInMethodTable[Name](Arguments, scope).Eval(scope);
            }
            else throw new NotDefinedException(Name);
        }
    }
}
