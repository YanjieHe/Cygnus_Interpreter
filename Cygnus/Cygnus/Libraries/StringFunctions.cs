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
using System.Data;
namespace Cygnus.Libraries
{
    public static class StringFunctions
    {
        public static Expression StrConcat(Expression[] args, Scope scope)
        {
            return string.Concat(args.Select(i => i.GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value.ToString()));
        }
        public static Expression StrJoin(Expression[] args, Scope scope)
        {
            return string.Join(args.First().GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value.ToString(),
                args.Skip(1).Select(i => i.GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value.ToString()));
        }
    }
}
