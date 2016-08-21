using System.Linq;
using Cygnus.SyntaxTree;
namespace Cygnus.Libraries
{
    public static class ListFunctions
    {
        public static Expression Append(Expression[] args, Scope scope)
        {
            args[0].GetValue<ListExpression>(ExpressionType.List, scope).Values.AddRange(args.Skip(1));
            return new ConstantExpression(null, ConstantType.Void);
        }
        public static Expression Remove(Expression[] args, Scope scope)
        {
            return Expression.Constant(args[0].GetValue<ListExpression>(ExpressionType.List, scope).Values.Remove(args[1]), ConstantType.Boolean);
        }
        public static Expression Insert(Expression[] args, Scope scope)
        {
            args[0].GetValue<ListExpression>(ExpressionType.List, scope).Values.Insert(
                (int)args[1].GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value,
                args[2]);
            return Expression.Void();
        }
        public static Expression RemoveAt(Expression[] args, Scope scope)
        {
            args[0].GetValue<ListExpression>(ExpressionType.List, scope).Values.RemoveAt(
                (int)args[1].GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value);
            return Expression.Void();
        }
    }
}
