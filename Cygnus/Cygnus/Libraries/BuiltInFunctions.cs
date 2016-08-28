using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cygnus.SyntaxTree;
using Cygnus.Extensions;
using Cygnus.AssemblyImporter;
using Cygnus.LexicalAnalyzer;
using Cygnus.SyntaxAnalyzer;
using Cygnus.Errors;
using Cygnus.Executors;
using System.IO;
namespace Cygnus.Libraries
{
    public static class BuiltInFunctions
    {
        public static Expression Print(Expression[] args, Scope scope)
        {
            var obj = args.Single().GetValue(scope);
            obj.Display();
            Console.WriteLine();
            return Expression.Void();
        }
        public static Expression InitArray(Expression[] args, Scope scope)
        {
            int n = args.Single().As<int>(scope);
            var arr = new Expression[n];
            for (int i = 0; i < n; i++)
                arr[i] = Expression.Null();
            return new ArrayExpression(arr);
        }
        public static Expression InitList(Expression[] args, Scope scope)
        {
            if (args.Length == 1 && args.Single().IsVoid(scope))
                return Expression.List(new List<Expression>());
            else
                return Expression.List(args.Select(i => i.GetValue(scope)).ToList());
        }
        public static Expression InitDictionary(Expression[] args, Scope scope)
        {
            if (args.Length == 1 && args.Single().IsVoid(scope))
                return new DictionaryExpression(new Dictionary<ConstantExpression, Expression>());
            else
                return new DictionaryExpression(args.Map(i =>
                {
                    var kvparr = i.AsArray(scope).Values;
                    if (kvparr.Length != 2)
                        throw new ArgumentException("The length of key-value pair must be 2");
                    else
                        return new KeyValuePair<ConstantExpression, Expression>
                        (kvparr[0].AsConstant(scope), kvparr[1].GetValue(scope));
                }
                ));
        }
        public static Expression InitTable(Expression[] args, Scope scope)
        {
            return new TableExpression(args.Cast<ParameterExpression>()
                .Select(i => new KeyValuePair<string, Expression>(i.Name, Expression.Null())).ToArray());
        }
        public static Expression InitMatrix(Expression[] args, Scope scope)
        {
            var rows = new double[args.Length][];
            for (int i = 0; i < args.Length; i++)
            {
                rows[i] = args[i].AsArray(scope).Values.Map(j => j.AsConstant(scope).GetDouble());
            }
            return new MatrixExpression(rows);
        }
        public static Expression Length(Expression[] args, Scope scope)
        {
            return (args.Single().GetValue(scope) as IIndexable).Length;
        }
        public static Expression Import(Expression[] args, Scope scope)
        {
            (args.Length == 2).OrThrows<ParameterException>();
            var path = GetFilePath(args[0].AsString(scope));
            var info = args[1].AsString(scope);//Namespace.Class
            new CSharpAssembly(path, info).Import();
            return Expression.Void();
        }
        private static string GetFilePath(string path)
        {
            if (!Path.IsPathRooted(path))
            {
                var file = path;
                path = SearchFile(Directory.GetCurrentDirectory(), file);
                if (path == null)
                    throw new DirectoryNotFoundException(file);
            }
            return path;
        }
        private static string SearchFile(string path, string file)
        {
            foreach (var f in Directory.EnumerateFiles(path))
                if (Path.GetFileName(f) == file)
                    return f;
            foreach (var folder in Directory.EnumerateDirectories(path))
            {
                var result = SearchFile(folder, file);
                if (result != null) return result;
            }
            return null;
        }
        public static Expression SetParent(Expression[] args, Scope scope)
        {
            (args.Length == 2).OrThrows<ParameterException>();
            var table = args[0].AsTable(scope);
            var parent_table = args[1].GetValue<TableExpression>(ExpressionType.Table, scope);
            table.Parent = parent_table;
            return Expression.Void();
        }
        public static Expression Range(Expression[] args, Scope scope)
        {
            switch (args.Length)
            {
                case 1:
                    return
               Expression.IEnumerable(
               Enumerable.Range(0, args[0].As<int>(scope))
               .Select(i => Expression.Constant(i, ConstantType.Integer)));
                case 2:
                    {
                        int start = args[0].As<int>(scope);
                        int end = args[1].As<int>(scope);
                        return
                       Expression.IEnumerable(
                           Enumerable.Range(start, end - start)
                           .Select(i => Expression.Constant(i, ConstantType.Integer)));
                    }
                case 3:
                    {
                        int start = args[0].As<int>(scope);
                        int end = args[1].As<int>(scope);
                        int step = args[2].As<int>(scope);
                        return
                            Expression.IEnumerable(
                                GetRange(start, end, step)
                                .Select(i => Expression.Constant(i, ConstantType.Integer)));
                    }
                default:
                    throw new ParameterException();
            }
        }
        private static IEnumerable<int> GetRange(int start, int end, int step)
        {
            if (step == 0) throw new ParameterException("The step cannot be zero");
            if (step > 0)
                for (int i = start; i < end; i += step)
                    yield return i;
            else
                for (int i = start; i > end; i += step)
                    yield return i;
        }
        public static Expression ExecuteFile(Expression[] args, Scope scope)
        {
            (args.Length == 1 || args.Length == 2).OrThrows<ParameterException>();
            var FilePath = GetFilePath(args[0].AsString(scope));
            var encoding = args.Length == 2 ? GetEncoding(args[1].AsString(scope)) : Encoding.Default;
            return new ExecuteFromFile(FilePath, encoding, scope).Run();
        }
        private static Encoding GetEncoding(string encoding)
        {
            switch (encoding)
            {
                case "default":
                    return Encoding.Default;
                case "utf-8":
                case "utf8":
                    return Encoding.UTF8;
                case "unicode":
                    return Encoding.Unicode;
                case "ascii":
                    return Encoding.ASCII;
                default:
                    return Encoding.GetEncoding(encoding);
            }
        }
        public static Expression Scan(Expression[] args, Scope scope)
        {
            (args.Length == 0 || args.Length == 1).OrThrows<ParameterException>();
            if (args.Length == 1)
                Console.Write(args.Single().AsString(scope));
            return Console.ReadLine();
        }
        public static Expression Throw(Expression[] args, Scope scope)
        {
            throw new Exception(args.Single().AsString(scope));
        }
        public static Expression Delete(Expression[] args, Scope scope)
        {
            foreach (var item in args.Cast<ParameterExpression>().Select(i => i.Name))
            {
                if (!scope.Delete(item))
                {
                    if (Scope.functionTable.ContainsKey(item))
                        Scope.functionTable.Remove(item);
                    else throw new NotDefinedException(item);
                }
            }
            return Expression.Void();
        }
        public static Expression Exit(Expression[] args, Scope scope)
        {
            Environment.Exit(0);
            return Expression.Void();
        }
    }
}
