using System.Linq;
using Cygnus.SyntaxTree;
namespace Cygnus.Libraries
{
    public static class ListFunctions
    {
        public static Expression Append(Expression[] args, Scope scope)
        {
            args[0].AsList(scope).Values.AddRange(args.Skip(1));
            return Expression.Void();
        }
        public static Expression Remove(Expression[] args, Scope scope)
        {
            return args[0].AsList(scope).Values.Remove(args[1]);
        }
        public static Expression Insert(Expression[] args, Scope scope)
        {
            args[0].AsList(scope).Values.Insert(args[1].As<int>(scope), args[2]);
            return Expression.Void();
        }
        public static Expression RemoveAt(Expression[] args, Scope scope)
        {
            args[0].AsList(scope).Values.RemoveAt(args[1].As<int>(scope));
            return Expression.Void();
        }
    }
}
