using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.Expressions
{
    public enum ExpressionType
    {
        Constant,
        Add, Subtract, Multiply, Divide, Power,
        Less, Greater, LessOrEquals, GreaterOrEquals,
        Equal, NotEqual,
        Assign, And, Or, Not,
        UnaryPlus, UnaryMinus,
        Block, Unary, Binary,
        IfThen, IfThenElse, While, Break,
        Parameter, Function, Array, List, Dictionary,
        Index, Return, ForEach, IEnumerable, Call,
        Table, IList, KeyValuePair, Continue, CSharpObject,
        Matrix, MatrixRow, Computable, Goto, Collection, NewArray,
        Member, Class, Dot, ClassInit,
    }
}
