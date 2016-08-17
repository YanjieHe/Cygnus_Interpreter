using Cygnus.SymbolTable;
namespace Cygnus.SyntaxTree
{
    public interface ICollectionExpression
    {
        Expression this[Expression index, Scope scope] { get; set; }
        ConstantExpression Length { get; }
    }
}
