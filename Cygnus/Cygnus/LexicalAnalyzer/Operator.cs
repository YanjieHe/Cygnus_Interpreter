using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.LexicalAnalyzer
{
    [Flags]
    public enum Operator
    {
        /* Arithmetic Operators */
        UnaryPlus = 0x1,
        UnaryMinus = 0x2,
        Add = 0x4,
        Subtract = 0x8,
        Multiply = 0x10,
        Divide = 0x20,
        Power = 0x40,
        /* Logical Operators */
        And = 0x80,
        Or = 0x100,
        Not = 0x200,
        /* comparison Operators */
        Less = 0x400,
        Greater = 0x800,
        LessOrEquals = 0x1000,
        GreaterOrEquals = 0x2000,
        Equal = 0x4000,
        NotEqual = 0x8000,
        /* Others */
        Call, Assign,
    }
}
