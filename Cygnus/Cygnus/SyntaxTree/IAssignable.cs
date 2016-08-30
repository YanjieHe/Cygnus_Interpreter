using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.SyntaxTree
{
    public interface IAssignable
    {
        void Assgin(Expression value,Scope scope);
    }
}
