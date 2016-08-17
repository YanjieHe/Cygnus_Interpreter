using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SyntaxTree;
using Cygnus.SymbolTable;
namespace Cygnus.Libraries
{
    public static class ConvertFunctions
    {
        public static Expression ToInt(Expression[] args,Scope scope)
        {
            return Expression.Constant(Convert.ToInt32(args[0].GetObjectValue(scope)), ConstantType.Integer);
        }
        public static Expression ToDouble(Expression[] args, Scope scope)
        {
            return Expression.Constant(Convert.ToDouble(args[0].GetObjectValue(scope)), ConstantType.Double);
        }
        public static Expression Str(Expression[] args, Scope scope)
        {
            return Expression.Constant(Convert.ToString(args[0].GetObjectValue(scope)), ConstantType.String);
        }
        public static Expression ToArray(Expression[] args, Scope scope)
        {
            return new ArrayExpression(args[0].GetIEnumrableList(scope).ToArray());
        }
        public static Expression ToList(Expression[] args, Scope scope)
        {
            return new ListExpression(args[0].GetIEnumrableList(scope).ToList());
        }
    }
}
