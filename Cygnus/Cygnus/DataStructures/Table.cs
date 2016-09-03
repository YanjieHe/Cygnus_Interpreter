using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.Extensions;
namespace Cygnus.DataStructures
{
    public class Table<T> : IEnumerable<KeyValuePair<object, T>> where T : class
    {
        T[] array;
        Dictionary<object, T> dict;
        const int InitialSize = 1;
        public Table()
        {
            array = new T[InitialSize];
            dict = new Dictionary<object, T>(0);
        }
        public Table(params T[] objs)
        {
            array = objs;
            dict = new Dictionary<object, T>(0);
        }
        public Table(T[] array, Dictionary<object, T> dict)
        {
            this.array = array;
            this.dict = dict;
        }
        public int Count()
        {
            return array.TakeWhile(i => i != null).Count();
        }
        public void Insert(T value)
        {
            int i = 0;
            while (true)
            {
                for (; i < array.Length; i++)
                {
                    if (array[i] == null)
                    {
                        array[i] = value;
                        return;
                    }
                }
                Expand();
            }
        }
        public void Insert(int index, T value)
        {
            if (array[array.Length - 1] != null)
                Expand();
            Array.Copy(array, index, array, index + 1, array.Length - index - 1);
            array[index] = value;
        }
        public void Remove()
        {
            for (int i = array.Length - 1; i >= 0; i--)
            {
                if (array[i] != null)
                {
                    array[i] = null;
                    return;
                }
            }
        }
        public void Remove(int index)
        {
            Array.Copy(array, index + 1, array, index, array.Length - index - 1);
            array[array.Length - 1] = null;
        }
        public void Delete(int index)
        {
            if (index >= 0 && index < array.Length)
            {
                array[index] = null;
            }
            else dict.Remove(index);
        }
        public T this[int index]
        {
            get
            {
                if (index >= 0 && index < array.Length)
                {
                    return array[index];
                }
                else
                {
                    T value;
                    if (dict.TryGetValue(index, out value))
                    {
                        return value;
                    }
                    else return null;
                }
            }
            set
            {
                int n = array.Length;
                if (index >= 0)
                {
                    if (index < n)
                    {
                        array[index] = value;
                        return;
                    }
                    else if (index < n)
                    {
                        array[index] = value;
                        return;
                    }
                    else if (index < (n + n))
                    {
                        //If the index is not larger than the double size of the array capcity, then expand the array
                        Expand();
                        array[index] = value;
                        return;
                    }
                    else dict[index] = value;
                }
                else dict[index] = value;
            }
        }
        public T this[object index]
        {
            get
            {
                T value;
                if (dict.TryGetValue(index, out value))
                {
                    return value;
                }
                else return null;
            }
            set { dict[index] = value; }
        }
        public void Expand()
        {
            T[] newarray = new T[array.Length << 1];
            Array.Copy(array, newarray, array.Length);
            foreach (var key in dict.Select(i => i.Key as int?).Where(i => i != null).Cast<int>())
            {
                if (key >= 0 && key < newarray.Length)
                {
                    newarray[key] = dict[key];
                    dict.Remove(key);
                }
            }
            array = newarray;
        }
        public IEnumerator<KeyValuePair<object, T>> GetEnumerator()
        {
            foreach (var item in array.TakeWhile(i => i != null).Select((x, index) => new KeyValuePair<object, T>(index, x)).Concat(dict))
            {
                yield return item;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this.AsEnumerable();
        }
    }
}
