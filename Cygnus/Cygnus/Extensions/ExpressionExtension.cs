using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SyntaxTree;
using Cygnus.LexicalAnalyzer;
namespace Cygnus.Extensions
{
    public static class ExpressionExtension
    {
        public static TResult[] Map<TSource, TResult>(this ICollection<TSource> source, Func<TSource, TResult> selector)
        {
            int n = source.Count;
            var array = new TResult[n];
            int i = 0;
            foreach (var item in source)
            {
                array[i] = selector(item);
                i++;
            }
            return array;
        }
        public static void DisplayList<T>(this IEnumerable<T> objs, Scope scope) where T : Expression
        {
            Console.Write("{ ");
            using (var obj = objs.GetEnumerator())
                if (obj.MoveNext())
                    while (true)
                    {
                        obj.Current.Display(scope);
                        if (obj.MoveNext())
                            Console.Write(", ");
                        else break;
                    }
            Console.Write(" }");
        }
        public static void OrThrows(this bool Condition, string ErrorText = null)
        {
            if (!Condition)
                throw new Exception(ErrorText ?? "Error");
        }
        public static void OrThrows<T>(this bool Condition) where T : Exception, new()
        {
            if (!Condition)
                throw new T();
        }
        public static void OrThrows<T>(this bool Condition, string ErrorText) where T : Exception, new()
        {
            if (!Condition)
                throw (T)Activator.CreateInstance(typeof(T), ErrorText);
        }
        public static void OrThrows<T>(this bool Condition, string ErrorText, params object[] args) where T : Exception, new()
        {
            if (!Condition)
                throw (T)Activator.CreateInstance(typeof(T), ErrorText, args);
        }
        public static bool IsCompareOp(this Operator Op)
        {
            switch (Op)
            {
                case Operator.Less:
                case Operator.Greater:
                case Operator.LessOrEquals:
                case Operator.GreaterOrEquals:
                    return true;
                default:
                    return false;
            }
        }
        public static bool In<T>(this T obj, params T[] array)
        {
            return array.Contains(obj);
        }
        public static bool IsCompareOp(this TokenType Op)
        {
            switch (Op)
            {
                case TokenType.Less:
                case TokenType.Greater:
                case TokenType.LessOrEquals:
                case TokenType.GreaterOrEquals:
                    return true;
                default:
                    return false;
            }
        }
    }
}
