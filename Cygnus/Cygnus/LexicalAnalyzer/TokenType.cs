﻿namespace Cygnus.LexicalAnalyzer
{
    public enum TokenType : byte
    {
        /* Basic Data Types */
        String, Char,
        Double, Integer,
        True, False,

        Null, Void,

        /* Comparison Operators */
        Equals,
        Greater_Than_Or_Equals,
        Less_Than_Or_Equals,
        Greater_Than,
        Less_Than,
        Not_Equal_To,
        /* Arithmetic Operators*/
        UnaryPlus, UnaryMinus,
        Add, Subtract, Multiply, Divide, Power,

        /* Concatenation Operators */
        Comma, Dot,

        /* Brackets */
        LeftParenthesis, RightParenthesis,
        LeftBracket, RightBracket,
        LeftBrace, RightBrace,
        /* Logical Operators */
        And, Or, Not,

        /* Control Statement */
        Define, Begin,
        Repeat, Until, Return,
        Do, End,
        If, Then, Else, ElseIf,
        For, In, While, Break, Continue,
        Try, Catch, Finally,
        Pass,

        /* Others */
        Assign, Call, Symbol,
        Space, Variable, EndOfLine,
        Comments, No_Arg,
    };
}