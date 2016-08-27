using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.SyntaxTree
{
    public interface IComputable
    {
        Expression Add(Expression Other);
        Expression Subtract(Expression Other);
        Expression Multiply(Expression Other);
        Expression Divide(Expression Other);
        Expression Power(Expression Other);
    }
}
