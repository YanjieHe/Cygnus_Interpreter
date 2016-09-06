using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.DataStructures
{
    public class CygnusIEnumerable : CygnusObject,IEnumerable<CygnusObject>
    {
        public override CygnusType type
        {
            get
            {
                return new CygnusType("IEnumerable", 10);
            }
        }
        public IEnumerable<CygnusObject> Collection { get; private set; }
        public CygnusIEnumerable(IEnumerable<CygnusObject> Collection)
        {
            this.Collection = Collection;
        }

        public IEnumerator<CygnusObject> GetEnumerator()
        {
            foreach (var item in Collection)
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
