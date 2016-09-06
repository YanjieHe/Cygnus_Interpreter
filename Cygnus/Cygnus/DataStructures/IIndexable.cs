using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Cygnus.DataStructures
{
    public interface IIndexable
    {
        CygnusObject this[params CygnusObject[] indexes] { get; set; }
        CygnusInteger Length { get; }
    }
}
