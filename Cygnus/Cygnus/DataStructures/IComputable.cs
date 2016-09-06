using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.DataStructures
{
    interface IComputable 
    {
        CygnusObject Add(CygnusObject Other);
        CygnusObject Subtract(CygnusObject Other);
        CygnusObject Multiply(CygnusObject Other);
        CygnusObject Divide(CygnusObject Other);
        CygnusObject Power(CygnusObject Other);
        CygnusObject UnaryPlus();
        CygnusObject Negate();
    }
}
