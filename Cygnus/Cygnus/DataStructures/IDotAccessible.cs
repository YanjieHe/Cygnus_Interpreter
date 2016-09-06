using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.Expressions;
namespace Cygnus.DataStructures
{
    public interface IDotAccessible
    {
        Expression GetByDot(string field, bool IsMethod);
        void SetByDot(CygnusObject obj, string field, bool IsMethod);

    }
}
