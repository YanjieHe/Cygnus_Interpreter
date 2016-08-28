using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.LexicalAnalyzer
{
    public enum Operator : byte
    {
        /* Arithmetic Operators */
        UnaryPlus, UnaryMinus,
        Add, Subtract, Multiply, Divide, Power,
        /* Logical Operators */
        And, Or, Not,
        /* comparison Operators */
        Less, Greater, LessOrEquals, GreaterOrEquals,
        Equals, NotEqualTo,
        /* Brackets */
        LeftParenthesis, RightParenthesis,
        LeftBrace, RightBrace,
        LeftBracket, RightBracket,

        /* Concatenation Operators */
        Comma, Dot,

        /* Others */
        Call, Assgin,
    }
}
