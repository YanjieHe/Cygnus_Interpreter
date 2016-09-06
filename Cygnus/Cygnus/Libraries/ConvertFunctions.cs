using System;
using System.Linq;
using Cygnus.Expressions;
namespace Cygnus.Libraries
{
    public static class ConvertFunctions
    {
        public static Expression ToInt(Expression[] args, Scope scope)
        {
            return Convert.ToInt32(args.Single().AsConstant(scope).Value);
        }
        public static Expression ToDouble(Expression[] args, Scope scope)
        {
            return Convert.ToDouble(args.Single().AsConstant(scope).Value);
        }
        public static Expression Str(Expression[] args, Scope scope)
        {
            return Convert.ToString(args.Single().AsConstant(scope).Value);
        }
    }
}
