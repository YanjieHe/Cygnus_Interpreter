using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SyntaxTree;
using Cygnus.SymbolTable;
namespace Cygnus.Libraries
{
    public static class MathFunctions
    {
        public static Expression Exp(Expression[] args, Scope scope)
        {
            var d = GetDouble(args.Single().GetValue<ConstantExpression>(ExpressionType.Constant, scope));
            return (ConstantExpression)Math.Exp(d);
        }
        public static Expression Sqrt(Expression[] args, Scope scope)
        {
            var d = GetDouble(args.Single().GetValue<ConstantExpression>(ExpressionType.Constant, scope));
            return (ConstantExpression)Math.Sqrt(d);
        }
        public static Expression Abs(Expression[] args, Scope scope)
        {
            var d = args.Single().GetValue<ConstantExpression>(ExpressionType.Constant, scope);
            if (d.constantType == ConstantType.Integer)
            {
                return (ConstantExpression)Math.Abs((int)d.Value);
            }
            else if (d.constantType == ConstantType.Double)
                return (ConstantExpression)Math.Abs((double)d.Value);
            else throw new ArgumentException();
        }
        public static Expression Log(Expression[] args, Scope scope)
        {
            if (args.Length == 1)
            {
                var d = GetDouble(args[0].GetValue<ConstantExpression>(ExpressionType.Constant, scope));
                return (ConstantExpression)Math.Log(d);
            }
            else if (args.Length == 2)
            {
                var a = GetDouble(args[0].GetValue<ConstantExpression>(ExpressionType.Constant, scope));
                var newBase = GetDouble(args[1].GetValue<ConstantExpression>(ExpressionType.Constant, scope));
                return (ConstantExpression)Math.Log(a, newBase);
            }
            else throw new ArgumentException();
        }
        public static Expression Mod(Expression[] args, Scope scope)
        {
            if (args.Length == 2)
            {
                var a = (int)args[0].GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value;
                var b = (int)args[1].GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value;
                return (ConstantExpression)(a % b);
            }
            else throw new ArgumentException();
        }
        public static double GetDouble(ConstantExpression Expr)
        {
            switch (Expr.constantType)
            {
                case ConstantType.Integer: return (int)Expr.Value;
                case ConstantType.Double: return (double)Expr.Value;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
