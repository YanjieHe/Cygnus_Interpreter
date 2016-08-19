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
    public static class BuiltInFunctions
    {
        public static Expression Print(Expression[] args, Scope scope)
        {
            var obj = args.Single().GetObjectValue(scope);
            if (obj is Expression[])
                PrintList(((Expression[])obj).Select(j => j.GetObjectValue(scope)));
            else if (obj is List<Expression>)
                PrintList(((List<Expression>)obj).Select(j => j.GetObjectValue(scope)));
            else if (obj is Dictionary<ConstantExpression, Expression>)
                PrintList(((Dictionary<ConstantExpression, Expression>)obj)
                    .Select(j => (object)new KeyValuePair<object, object>(j.Key.GetObjectValue(scope), j.Value.GetObjectValue(scope))));
            else if (obj != null)
                Console.WriteLine(obj);
            return Expression.Void();
        }
        private static void PrintList(IEnumerable<object> objs)
        {
            Console.Write("{ ");
            using (var obj = objs.GetEnumerator())
                if (obj.MoveNext())
                    while (true)
                    {
                        Console.Write(obj.Current);
                        if (obj.MoveNext())
                            Console.Write(", ");
                        else break;
                    }
            Console.WriteLine(" }");
        }
        public static Expression InitArray(Expression[] args, Scope scope)
        {
            int n = (int)args.Single().GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value;
            var arr = new Expression[n];
            for (int i = 0; i < n; i++)
                arr[i] = Expression.Null();
            return new ArrayExpression(arr);
        }
        public static Expression InitList(Expression[] args, Scope scope)
        {
            var arr = args.Single();
            if (arr.IsVoid(scope))
                return new ListExpression(new Expression[0]);
            else
                return new ListExpression(arr.GetValue<ArrayExpression>(ExpressionType.Array, scope).Values);
        }
        public static Expression InitDictionary(Expression[] args, Scope scope)
        {
            var arr = args.Single();
            if (arr.IsVoid(scope))
                return new DictionaryExpression(new Dictionary<ConstantExpression, Expression>());
            else
                return new DictionaryExpression(
                        arr.GetValue<ArrayExpression>(ExpressionType.Array, scope).Values
                        .Map(i => i.GetValue<ArrayExpression>(ExpressionType.Array, scope).Values));
        }
        public static Expression InitTable(Expression[] args, Scope scope)
        {
            return new TableExpression(args.Cast<ParameterExpression>()
                .Select(i => new KeyValuePair<string, Expression>(i.Name, Expression.Null())).ToArray());
        }
        public static Expression Length(Expression[] args, Scope scope)
        {
            return (args.Single().GetValue(scope) as ICollectionExpression).Length;
        }
        public static Expression Dispose(Expression[] args, Scope scope)
        {
            var resource = args.Single().GetValue(scope);
            (resource as IDisposable).Dispose();
            return Expression.Void();
        }
        public static Expression Import(Expression[] args, Scope scope)
        {
            new CSharpAssembly(args[0].GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value.ToString(),
                args[1].GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value.ToString()).Import();
            return new ConstantExpression(null, ConstantType.Void);
        }
        public static Expression SetParent(Expression[] args, Scope scope)
        {
            var table = args[0].GetValue<TableExpression>(ExpressionType.Table, scope);
            var parent_table = args[1].GetValue<TableExpression>(ExpressionType.Table, scope);
            table.Parent = parent_table;
            return Expression.Void();
        }
        public static Expression Range(Expression[] args, Scope scope)
        {
            if (args.Length == 1)
            {
                return
                    Expression.IEnumerable(
                    Enumerable.Range(0,
                    (int)args[0].GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value)
                    .Select(i => Expression.Constant(i, ConstantType.Integer)));
            }
            else if (args.Length == 2 || args.Length == 3)
            {
                int start = (int)args[0].GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value;
                int end = (int)args[1].GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value;
                if (args.Length == 2)
                    return
                         Expression.IEnumerable(
                             Enumerable.Range(start, end - start)
                             .Select(i => Expression.Constant(i, ConstantType.Integer)));
                else if (args.Length == 3)
                {
                    int step = (int)args[2].GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value;
                    return
                        Expression.IEnumerable(
                            GetRange(start, end, step)
                            .Select(i => Expression.Constant(i, ConstantType.Integer)));
                }
                else throw new ArgumentException();
            }
            else throw new ArgumentException();
        }
        private static IEnumerable<int> GetRange(int start, int end, int step)
        {
            if (step == 0) throw new ArgumentException();
            if (step > 0)
                for (int i = start; i < end; i += step)
                    yield return i;
            else
                for (int i = start; i > end; i += step)
                    yield return i;
        }
        public static Expression ExecuteFile(Expression[] args, Scope scope)
        {
            var FilePath = args[0].GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value as string;
            var encoding = Encoding.Default;
            using (var lex = new Lexical(FilePath, encoding, TokenDefinition.tokenDefinitions))
            {
                lex.Tokenize();
                var lex_array = Lexeme.Generate(lex.tokenList);
                var ast = new AST();
                BlockExpression Root = ast.Parse(lex_array, scope);
                Expression Result = Root.Eval(scope).GetValue(scope);
                return Result;
            }
        }
        public static Expression Scan(Expression[] args, Scope scope)
        {
            if (args.Length != 0 && args.Length != 1) throw new ArgumentException();
            if (args.Length == 1)
                Console.Write(args.Single().GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value);
            return Console.ReadLine();
        }
        public static Expression Throw(Expression[] args, Scope scope)
        {
            throw new Exception(args.Single().GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value as string);
        }
        public static Expression Delete(Expression[] args, Scope scope)
        {
            foreach (var item in args.Cast<ParameterExpression>().Select(i => i.Name))
            {
                if (!scope.Delete(item))
                {
                    if (FunctionExpression.functionTable.ContainsKey(item))
                        FunctionExpression.functionTable.Remove(item);
                    else throw new NotDefinedException(item);
                }
            }
            return new ConstantExpression(null, ConstantType.Void);
        }
        public static Expression IsNull(Expression[] args, Scope scope)
        {
            var value = args.Single().GetValue(scope);
            if (value.NodeType != ExpressionType.Constant) return (ConstantExpression)false;
            else
                return ((value as ConstantExpression).constantType == ConstantType.Null);
        }
    }
}
