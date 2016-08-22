using System;
using System.Linq;
using Cygnus.SyntaxTree;
namespace Cygnus.Libraries
{
    public static class ConvertFunctions
    {
        public static Expression ToInt(Expression[] args, Scope scope)
        {
            return Convert.ToInt32(args[0].GetObjectValue(scope));
        }
        public static Expression ToDouble(Expression[] args, Scope scope)
        {
            return Convert.ToDouble(args[0].GetObjectValue(scope));
        }
        public static Expression Str(Expression[] args, Scope scope)
        {
            return Convert.ToString(args[0].GetObjectValue(scope));
        }
        public static Expression ToArray(Expression[] args, Scope scope)
        {
            return Expression.Array(args[0].GetIEnumrableList(scope).ToArray());
        }
        public static Expression ToList(Expression[] args, Scope scope)
        {
            return Expression.List(args[0].GetIEnumrableList(scope).ToList());
        }
    }
}
