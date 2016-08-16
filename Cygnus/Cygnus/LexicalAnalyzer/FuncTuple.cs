using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.LexicalAnalyzer
{
    public sealed class FuncTuple
    {
        public string Name;
        public int argsCount;
        public FuncTuple(string Name,int argsCount)
        {
            this.Name = Name;
            this.argsCount = argsCount;
        }
    }
}
