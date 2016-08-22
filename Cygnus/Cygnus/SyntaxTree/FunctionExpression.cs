namespace Cygnus.SyntaxTree
{
    public class FunctionExpression : Expression
    {
       
        public ParameterExpression[] Arguments { get; private set; }
        public BlockExpression Body { get; private set; }
        public string Name { get; private set; }
        public Scope funcScope { get; private set; }
        public FunctionExpression(string Name, BlockExpression Body, Scope scope, ParameterExpression[] Arguments)
        {
            this.Name = Name;
            this.Body = Body;
            this.Arguments = Arguments;
            this.funcScope = scope;
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Function;
            }
        }
        public override string ToString()
        {
            return "(Function)";
        }
        public override Expression Eval(Scope scope)
        {
            return Body.Eval(scope);
        }
        public FunctionExpression Update(Expression[] Values, Scope scope)
        {
            Scope newScope = new Scope(funcScope.Parent);
            for (int i = 0; i < Arguments.Length; i++)
                newScope[Arguments[i].Name] = Values[i].Eval(scope);
            return new FunctionExpression(Name, Body, newScope, Arguments);
        }
    }
}
