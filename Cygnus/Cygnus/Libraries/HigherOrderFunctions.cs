using System;
using System.Linq;
using Cygnus.SyntaxTree;
using Cygnus.Errors;
using Cygnus.Extensions;
namespace Cygnus.Libraries
{
    public static class HigherOrderFunctions
    {
        public static Expression Map(Expression[] args, Scope scope)
        {
            (args.Length == 2).OrThrows<ParameterException>();
            var list = args[0].GetIEnumrableList(scope);
            var func = args[1].AsCall(scope);
            return Expression.IEnumerable(list.Select(i => Expression.Call(func.Name, i).Eval(scope)));
        }
        public static Expression Filter(Expression[] args, Scope scope)
        {
            (args.Length == 2).OrThrows<ParameterException>();
            var list = args[0].GetIEnumrableList(scope);
            var func = args[1].AsCall(scope);
            return Expression.IEnumerable(list
                .Where(i => Expression.Call(func.Name, i).Eval(scope)
                .As<bool>(scope)));
        }
        public static Expression Reduce(Expression[] args, Scope scope)
        {
            switch (args.Length)
            {
                case 2:
                    {
                        var list = args[0].GetIEnumrableList(scope);
                        var func = args[1].GetValue<CallExpression>(ExpressionType.Call, scope);
                        return list.Aggregate((x, y) => Expression.Call(func.Name, x, y).Eval(scope));
                    }
                case 3:
                    {
                        var list = args[0].GetIEnumrableList(scope);
                        var seed = args[1].GetValue(scope);
                        var func = args[2].GetValue<CallExpression>(ExpressionType.Call, scope);
                        return list.Aggregate(seed, (x, y) => Expression.Call(func.Name, x, y).Eval(scope));
                    }
                default:
                    throw new ParameterException();
            }
        }
    }
}
