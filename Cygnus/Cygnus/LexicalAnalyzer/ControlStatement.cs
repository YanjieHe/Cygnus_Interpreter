using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.LexicalAnalyzer
{
    public enum ControlStatement
    {
        If, Then, Else, ElseIf, End, Break,
        While, Do,
        Terminator,
        Define, Begin,
        For, In,
        Return, Continue, Pass
    }
}
