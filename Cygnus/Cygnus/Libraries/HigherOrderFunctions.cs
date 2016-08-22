using System;
using System.Linq;
using Cygnus.SyntaxTree;
namespace Cygnus.Libraries
{
    public static class HigherOrderFunctions
    {
        public static Expression Map(Expression[] args, Scope scope)
        {
            if (args.Length != 2) throw new ArgumentException();
            var list = args[0].GetIEnumrableList(scope);
            var func = args[1].GetValue<CallExpression>(ExpressionType.Call, scope);
            return Expression.IEnumerable(list.Select(i => Expression.Call(func.Name, i).Eval(scope)));
        }
        public static Expression Filter(Expression[] args, Scope scope)
        {
            if (args.Length != 2) throw new ArgumentException();
            var list = args[0].GetIEnumrableList(scope);
            var func = args[1].GetValue<CallExpression>(ExpressionType.Call, scope);
            return Expression.IEnumerable(list
                .Where(i => Expression.Call(func.Name, i).Eval(scope)
                .As<bool>(scope)));
        }
        public static Expression Reduce(Expression[] args, Scope scope)
        {
            if (args.Length == 2)
            {
                var list = args[0].GetIEnumrableList(scope);
                var func = args[1].GetValue<CallExpression>(ExpressionType.Call, scope);
                return list.Aggregate((x, y) => Expression.Call(func.Name, x, y).Eval(scope));
            }
            else if (args.Length == 3)
            {
                var list = args[0].GetIEnumrableList(scope);
                var seed = args[1].GetValue(scope);
                var func = args[2].GetValue<CallExpression>(ExpressionType.Call, scope);
                return list.Aggregate(seed, (x, y) => Expression.Call(func.Name, x, y).Eval(scope));
            }
            else throw new ArgumentException();
        }
    }
}
