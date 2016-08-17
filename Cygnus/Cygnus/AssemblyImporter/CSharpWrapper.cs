using System;
using Expr = System.Linq.Expressions;
using Cygnus.SyntaxTree;
using Cygnus.SymbolTable;
namespace Cygnus.AssemblyImporter
{
    public static class CSharpWrapper
    {
        public static Func<Expression[], Scope, Expression> WrapFunc(Delegate func, Type[] ParameterTypes, Type ReturnType)
        {
            Expr.Expression<Func<Expression[], Scope, Expression>> lambda
                = (args, scope) =>
               GetReturnValue(
                   func.DynamicInvoke(
                       WrapParameters(args, scope, ParameterTypes)), ReturnType);
            return lambda.Compile();
        }
        public static object[] WrapParameters(Expression[] expressions, Scope scope, Type[] types)
        {
            int n = expressions.Length;
            var objs = new object[n];
            for (int i = 0; i < n; i++)
                objs[i] = WrapParameter(expressions[i], scope, types[i]);
            return objs;
        }
        public static object WrapParameter(Expression expression, Scope scope, Type type)
        {
            switch (type.Name)
            {
                case "Int32":
                    return (int)expression.GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value;
                case "Double":
                    return (double)expression.GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value;
                case "Char":
                    return (char)expression.GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value;
                case "String":
                    return (string)expression.GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value;
                case "Boolean":
                    return (bool)expression.GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value;
                default:
                    throw new NotSupportedException();
            }
        }
        public static Expression GetReturnValue(object obj, Type type)
        {
            switch (type.Name)
            {
                case "Int32":
                    return new ConstantExpression((int)obj, ConstantType.Integer);
                case "Double":
                    return new ConstantExpression((double)obj, ConstantType.Double);
                case "Char":
                    return new ConstantExpression((char)obj, ConstantType.Char);
                case "String":
                    return new ConstantExpression((string)obj, ConstantType.String);
                case "Boolean":
                    return new ConstantExpression((bool)obj, ConstantType.Boolean);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
