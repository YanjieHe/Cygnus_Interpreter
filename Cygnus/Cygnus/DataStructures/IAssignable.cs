using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.Expressions;
namespace Cygnus.DataStructures
{
    public interface IAssignable
    {
        void Assgin(Expression value, Scope scope);
    }
}
