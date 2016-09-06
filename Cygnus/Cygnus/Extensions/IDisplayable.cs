using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.Expressions;
namespace Cygnus.Extensions
{
    public interface IDisplayable
    {
        void Display(Scope scope);
    }
}
