namespace Cygnus.LexicalAnalyzer
{
    public sealed class TokenDefinition
    {
        public readonly TokenMatcher tokenMatcher;
        public readonly TokenType tokenType;

        public TokenDefinition(TokenMatcher _tokenMatcher, TokenType TokenType)
        {
            tokenMatcher = _tokenMatcher;
            tokenType = TokenType;
        }
        public int Match(string s, out string result)
        {
            result = string.Empty;
            if (s.Length == 0) return (-1);
            var m = tokenMatcher.Matcher.Match(s);
            if (m.Success)
            {
                result = m.Value;
                return m.Length;
            }
            else return (-1);
        }
        public static readonly TokenDefinition[] tokenDefinitions = new TokenDefinition[]
               {
                new TokenDefinition(@"([""'])(?:\\\1|.)*?\1", TokenType.String),

                new TokenDefinition(@"==", TokenType.Equals),
                new TokenDefinition(@">=", TokenType.Greater_Than_Or_Equals),
                new TokenDefinition(@"<=", TokenType.Less_Than_Or_Equals),
                new TokenDefinition(@">", TokenType.Greater_Than),
                new TokenDefinition(@"<", TokenType.Less_Than),
                new TokenDefinition(@"!=", TokenType.Not_Equal_To),
                new TokenDefinition(@"=", TokenType.Assign),

                new TokenDefinition(@"[0-9]+\.[0-9]+", TokenType.Double),
                new TokenDefinition(@"[0-9]+", TokenType.Integer),

                new TokenDefinition(@"^\+", TokenType.Add),
                new TokenDefinition(@"^\-", TokenType.Subtract),
                new TokenDefinition(@"^\*", TokenType.Multiply),
                new TokenDefinition(@"^\/", TokenType.Divide),
                new TokenDefinition(@"^\^", TokenType.Power),
                new TokenDefinition(@"^\,", TokenType.Comma),
                new TokenDefinition(@"^\.", TokenType.Dot),

                new TokenDefinition(@"^\(", TokenType.LeftParenthesis),
                new TokenDefinition(@"^\)", TokenType.RightParenthesis),
                new TokenDefinition(@"^\[", TokenType.LeftBracket),
                new TokenDefinition(@"^\]", TokenType.RightBracket),
                new TokenDefinition(@"^\{", TokenType.LeftBrace),
                new TokenDefinition(@"^\}", TokenType.RightBrace),

                new TokenDefinition(@"^\;", TokenType.EndOfLine),

                new TokenDefinition(@"[_A-Za-z]+[_A-Za-z0-9]*[\s]*\(", TokenType.Function),
                new TokenDefinition(@"[_A-Za-z]+[_A-Za-z0-9]*", TokenType.Symbol),
                new TokenDefinition(@"^[\s]+", TokenType.Space),
                new TokenDefinition(@"^%(.*)", TokenType.Comments),
               };

    }
}
