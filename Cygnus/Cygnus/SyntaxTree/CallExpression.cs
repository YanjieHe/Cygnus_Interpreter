using Cygnus.Errors;
namespace Cygnus.SyntaxTree
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
            if (Scope.functionTable.ContainsKey(Name))
            {
                var func = Scope.functionTable[Name].Update(Arguments, scope);
                return func.Eval(func.funcScope);
            }
            else if (Scope.builtInMethodTable.ContainsKey(Name))
            {
                return Scope.builtInMethodTable[Name](Arguments, scope);
            }
            Expression funcExpr;
            if (scope.TryGetValue(Name, out funcExpr))
            {
                if (funcExpr.NodeType == ExpressionType.Call)
                    return new CallExpression((funcExpr as CallExpression).Name, Arguments).Eval(scope);
            }
            throw new NotDefinedException(Name);
        }
    }
}
