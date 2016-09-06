using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Cygnus.Expressions;
using Cygnus.Extensions;
namespace Cygnus.DataStructures
{
    public class CygnusArray : CygnusObject, IEnumerable<CygnusObject>, IIndexable
    {
        public override CygnusType type
        {
            get
            {
                return new CygnusType("array", 10);
            }
        }

        public CygnusInteger Length
        {
            get
            {
                return new CygnusInteger(array.Length);
            }
        }

        public CygnusObject this[params CygnusObject[] indexes]
        {
            get
            {
                return array[(int)indexes.Single()];
            }

            set
            {
                array[(int)indexes.Single()] = value;
            }
        }

        public CygnusObject[] array;
        public CygnusArray(params CygnusObject[] array)
        {
            this.array = array;
        }

        public IEnumerator<CygnusObject> GetEnumerator()
        {
            foreach (var item in array)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this.AsEnumerable();
        }
        public override void Display(Scope scope)
        {
            array.DisplayList(scope);
        }
        public override string ToString()
        {
            return "{ " + string.Join(", ", array.Select(i => i.ToString())) + " }";
        }
    }
}
