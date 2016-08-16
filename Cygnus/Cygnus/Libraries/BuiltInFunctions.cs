using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SyntaxTree;
using Cygnus.Extensions;
using Cygnus.AssemblyImporter;

namespace Cygnus.Libraries
{
    public static class BuiltInFunctions
    {
        public static Expression Print(Expression[] args)
        {
            var obj = args.Single().GetObjectValue();
            if (obj is Expression[])
                PrintList(((Expression[])obj).Select(j => j.GetObjectValue()));
            else if (obj is List<Expression>)
                PrintList(((List<Expression>)obj).Select(j => j.GetObjectValue()));
            else if (obj is Dictionary<ConstantExpression, Expression>)
                PrintList(((Dictionary<ConstantExpression, Expression>)obj)
                    .Select(j => (object)new KeyValuePair<object, object>(j.Key.GetObjectValue(), j.Value.GetObjectValue())));
            else if (obj != null)
                Console.WriteLine(obj);
            return Expression.Void();
        }
        private static void PrintList(IEnumerable<object> objs)
        {
            Console.Write("[ ");
            using (var obj = objs.GetEnumerator())
                if (obj.MoveNext())
                    while (true)
                    {
                        Console.Write(obj.Current);
                        if (obj.MoveNext())
                            Console.Write(", ");
                        else break;
                    }
            Console.WriteLine(" ]");
        }
        public static Expression InitArray(Expression[] args)
        {
            int n = (int)args.Single().GetValue<ConstantExpression>(ExpressionType.Constant).Value;
            var arr = new Expression[n];
            for (int i = 0; i < n; i++)
                arr[i] = Expression.Void();
            return new ArrayExpression(arr);
        }
        public static Expression InitList(Expression[] args)
        {
            var arr = args.Single();
            if (arr.IsVoid())
                return new ListExpression(new Expression[0]);
            else
                return new ListExpression(arr.GetValue<ArrayExpression>(ExpressionType.Array).Values);
        }
        public static Expression InitDictionary(Expression[] args)
        {
            var arr = args.Single();
            if (arr.IsVoid())
                return new DictionaryExpression(null);
            else
                return new DictionaryExpression(
                        arr.GetValue<ArrayExpression>(ExpressionType.Array).Values
                        .Map(i => i.GetValue<ArrayExpression>(ExpressionType.Array).Values));
        }
        public static Expression Length(Expression[] args)
        {
            return (args.Single().GetValue() as ICollectionExpression).Length;
        }
        public static Expression Import(Expression[] args)
        {
            new CSharpAssembly(args[0].GetValue<ConstantExpression>(ExpressionType.Constant).Value.ToString(),
                args[1].GetValue<ConstantExpression>(ExpressionType.Constant).Value.ToString()).Import();
            return new DefaultExpression(ConstantType.Void);
        }
        public static Expression Range(Expression[] args)
        {
            if (args.Length == 1)
            {
                return
                    Expression.IEnumerable(
                    Enumerable.Range(0,
                    (int)args[0].GetValue<ConstantExpression>(ExpressionType.Constant).Value)
                    .Select(i => Expression.Constant(i, ConstantType.Integer)));
            }
            else if (args.Length == 2 || args.Length == 3)
            {
                int start = (int)args[0].GetValue<ConstantExpression>(ExpressionType.Constant).Value;
                int end = (int)args[1].GetValue<ConstantExpression>(ExpressionType.Constant).Value;
                if (args.Length == 2)
                    return
                         Expression.IEnumerable(
                             Enumerable.Range(start, end - start)
                             .Select(i => Expression.Constant(i, ConstantType.Integer)));
                else if (args.Length == 3)
                {
                    int step = (int)args[2].GetValue<ConstantExpression>(ExpressionType.Constant).Value;
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
    }
}
