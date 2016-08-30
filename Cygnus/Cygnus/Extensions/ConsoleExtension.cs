using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.Extensions
{
    public static class ConsoleExtension
    {
        public static void Join<T>(string separator, IEnumerable<T> values)
        {
            using (var x = values.GetEnumerator())
            {
                if (x.MoveNext())
                    Console.Write(x.Current);
                while (x.MoveNext())
                {
                    Console.Write(separator);
                    Console.Write(x.Current);
                }
            }
        }
        public static void Concat<T>(IEnumerable<T> values)
        {
            foreach (var item in values)
                Console.Write(item);
        }
        public static void Repeat<T>(T value, int count)
        {
            for (int i = 0; i < count; i++)
                Console.Write(value);
        }
    }
}
