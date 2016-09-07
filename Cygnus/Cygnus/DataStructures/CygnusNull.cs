using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.DataStructures
{
    public class CygnusNull : CygnusObject
    {
        public override CygnusType type
        {
            get
            {
                return CygnusType.Null;
            }
        }
        public CygnusNull() { }
    }
}
