using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SyntaxTree;

namespace Cygnus.Libraries
{
    public static class ConvertFunctions
    {
        public static Expression ToInt(Expression[] args)
        {
            return Expression.Constant(Convert.ToInt32(args[0].GetObjectValue()), ConstantType.Integer);
        }
        public static Expression ToDouble(Expression[] args)
        {
            return Expression.Constant(Convert.ToDouble(args[0].GetObjectValue()), ConstantType.Double);
        }
        public static Expression Str(Expression[] args)
        {
            return Expression.Constant(Convert.ToString(args[0].GetObjectValue()), ConstantType.String);
        }
        public static Expression ToArray(Expression[] args)
        {
            return new ArrayExpression(args[0].GetIEnumrableList().ToArray());
        }
        public static Expression ToList(Expression[] args)
        {
            return new ListExpression(args[0].GetIEnumrableList().ToList());
        }
    }
}
