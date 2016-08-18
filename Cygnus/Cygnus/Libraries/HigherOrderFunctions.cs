using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SyntaxTree;
using Cygnus.Extensions;
using Cygnus.AssemblyImporter;
using Cygnus.SymbolTable;
using Cygnus.LexicalAnalyzer;
using Cygnus.SyntaxAnalyzer;
using Cygnus.Errors;
namespace Cygnus.Libraries
{
    public static class HigherOrderFunctions
    {
        public static Expression Map(Expression[] args, Scope scope)
        {
            if (args.Length != 2) throw new ArgumentException();
            var func = args[0].GetValue<CallExpression>(ExpressionType.Call, scope);
            var list = args[1].GetIEnumrableList(scope);
            return Expression.IEnumerable(list.Select(i => new CallExpression(func.Name, new Expression[] { i }).Eval(scope)));
        }
        public static Expression Filter(Expression[] args, Scope scope)
        {
            if (args.Length != 2) throw new ArgumentException();
            var func = args[0].GetValue<CallExpression>(ExpressionType.Call, scope);
            var list = args[1].GetIEnumrableList(scope);
            return Expression.IEnumerable(list
                .Where(i => (bool)new CallExpression(func.Name, new Expression[] { i }).Eval(scope)
                .GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value));
        }
    }
}
