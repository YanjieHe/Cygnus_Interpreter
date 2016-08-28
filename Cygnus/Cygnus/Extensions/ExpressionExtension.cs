using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SyntaxTree;
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
        public static IEnumerable<T> Slice<T>(this T[] array, int start, int end)
        {
            for (int i = start; i <= end; i++)
                yield return array[i];
        }
        public static void DisplayList<T>(this IEnumerable<T> objs) where T : Expression
        {
            Console.Write("{ ");
            using (var obj = objs.GetEnumerator())
                if (obj.MoveNext())
                    while (true)
                    {
                        obj.Current.Display();
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
    }
}
