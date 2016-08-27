namespace Cygnus.LexicalAnalyzer
{
    public sealed class Token
    {
        public readonly string Content;
        public TokenType tokenType { get; set; }
        public Token(string content, TokenType _token)
        {
            Content = content;
            tokenType = _token;
        }
        public override string ToString()
        {
            return string.Format("TokenContent: {0}  Token:{1}", Content, tokenType);
        }
    }
    public enum TokenType : byte
    {
        String, Char,
        Equals,
        Greater_Than_Or_Equals,
        Less_Than_Or_Equals,
        Greater_Than,
        Less_Than,
        Not_Equal_To,
        Assign,
        Double, Integer,
        UnaryPlus, UnaryMinus,
        Add, Subtract, Multiply, Divide, Power,
        Comma, Dot,
        LeftParenthesis, RightParenthesis,
        LeftBracket, RightBracket,
        LeftBrace, RightBrace,
        And, Or, Not,
        True, False,
        Define, Begin,
        Repeat, Until, Return,
        Do, End,
        If, Then, Else, ElseIf,
        For, While, Break,
        Call,
        In,
        Null,
        Void,
        Symbol,
        Space,
        Variable,
        EndOfLine,
        Comments,
        No_Arg,
        Continue,
        Pass,
        Try, Catch, Finally
    };
}
