using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.SyntaxTree
{
    public interface ICollectionExpression 
    {
        Expression this[Expression index] { get; set; }
        ConstantExpression Length { get; }
    }
}
