using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.DataStructures
{
    public class CygnusVoid : CygnusObject
    {
        public override CygnusType type
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public CygnusVoid() { }
    }
}
