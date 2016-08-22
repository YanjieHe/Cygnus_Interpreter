using System.Linq;
using Cygnus.SyntaxTree;
namespace Cygnus.Libraries
{
    public static class StringFunctions
    {
        public static Expression StrConcat(Expression[] args, Scope scope)
        {
            return string.Concat(args.Select(i => i.GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value.ToString()));
        }
        public static Expression StrJoin(Expression[] args, Scope scope)
        {
            return string.Join(args.First().AsString(scope),
                args.Skip(1).Select(i => i.GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value.ToString()));
        }
        public static Expression StrSplit(Expression[] args, Scope scope)
        {
            return
                new ArrayExpression(args.First().AsString(scope)
                .Split(
                args.Skip(1)
                .Select(i => i.AsString(scope).Single()).ToArray())
                .Select(i => (Expression)i).ToArray());
        }
        public static Expression StrFormat(Expression[] args, Scope scope)
        {
            return string.Format(args.First().AsString(scope),
                args.Skip(1).Select(i => i.GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value).ToArray());
        }
        public static Expression StrLen(Expression[] args,Scope scope)
        {
            return args.Single().AsString(scope).Length;
        }
    }
}
