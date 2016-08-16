using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SyntaxTree;
namespace Cygnus.Libraries
{
    public static class ListFunctions
    {
        public static Expression Append(Expression[] args)
        {
            args[0].GetValue<ListExpression>(ExpressionType.List).Values.AddRange(args.Skip(1));
            return new DefaultExpression(ConstantType.Void);
        }
        public static Expression Remove(Expression[] args)
        {
            return Expression.Constant(args[0].GetValue<ListExpression>(ExpressionType.List).Values.Remove(args[1]), ConstantType.Boolean);
        }
        public static Expression Insert(Expression[] args)
        {
            args[0].GetValue<ListExpression>(ExpressionType.List).Values.Insert(
                (int)args[1].GetValue<ConstantExpression>(ExpressionType.Constant).Value,
                args[2]);
            return Expression.Void();
        }
        public static Expression RemoveAt(Expression[] args)
        {
            args[0].GetValue<ListExpression>(ExpressionType.List).Values.RemoveAt(
                (int)args[1].GetValue<ConstantExpression>(ExpressionType.Constant).Value);
            return Expression.Void();
        }
    }
}
