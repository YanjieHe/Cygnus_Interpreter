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
        public static TResult[] Map<TSource, TResult>(this TSource[] source, Func<TSource, TResult> selector)
        {
            int n = source.Length;
            var array = new TResult[n];
            for (int i = 0; i < n; i++)
                array[i] = selector(source[i]);
            return array;
        }
    }
}
