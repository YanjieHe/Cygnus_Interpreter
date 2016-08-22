namespace Cygnus.SyntaxTree
{
    public interface IIndexable
    {
        Expression this[Expression index, Scope scope] { get; set; }
        ConstantExpression Length { get; }
    }
}
