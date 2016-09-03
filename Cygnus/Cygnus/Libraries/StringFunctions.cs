using System.Linq;
using Cygnus.SyntaxTree;
using System;
using Cygnus.Errors;
using Cygnus.Extensions;
namespace Cygnus.Libraries
{
    public static class StringFunctions
    {
        public static Expression StrConcat(Expression[] args, Scope scope)
        {
            return string.Concat(args.Select(i => i.AsConstant(scope).Value.ToString()));
        }
        public static Expression StrJoin(Expression[] args, Scope scope)
        {
            return string.Join(args.First().AsString(scope),
                args.Skip(1).Select(i => i.AsConstant(scope).Value.ToString()));
        }
        //public static Expression StrSplit(Expression[] args, Scope scope)
        //{
        //    return
        //        new ArrayExpression(
        //            args.First().AsString(scope)
        //            .Split(args.Skip(1)
        //            .Select(i => i.AsString(scope).Single()).ToArray())
        //            .Select(i => (Expression)i).ToArray());
        //}
        public static Expression StrFormat(Expression[] args, Scope scope)
        {
            return string.Format(args.First().AsString(scope),
                args.Skip(1).Select(i => i.AsConstant(scope).Value).ToArray());
        }
        public static Expression StrLen(Expression[] args, Scope scope)
        {
            return args.Single().AsString(scope).Length;
        }
        public static Expression StrFind(Expression[] args, Scope scope)
        {
            (args.Length == 2).OrThrows<ParameterException>("Wrong number of parameters for the function");
            var str = args[0].AsString(scope);
            var value = args[1].AsString(scope);
            return str.IndexOf(value);
        }
        public static Expression StrReplace(Expression[] args, Scope scope)
        {
            (args.Length == 3).OrThrows<ParameterException>("Wrong number of parameters for the function");
            var str = args[0].AsString(scope);
            var oldvalue = args[1].AsString(scope);
            var newvalue = args[2].AsString(scope);
            return str.Replace(oldvalue, newvalue);
        }
    }
}
