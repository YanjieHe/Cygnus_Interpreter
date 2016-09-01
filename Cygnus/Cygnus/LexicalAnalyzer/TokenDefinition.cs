using System.Text.RegularExpressions;
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
        public Match Match(string s, int startat)
        {
            return tokenMatcher.Matcher.Match(s, startat);
        }
        public static readonly
            TokenDefinition[] tokenDefinitions
            = new TokenDefinition[]
               {
                   new TokenDefinition(@"([""'])(?:\\\1|.)*?\1", TokenType.String),
                   new TokenDefinition(@"([\r\n]|[\n])+", TokenType.EndOfLine),
                   new TokenDefinition(new TokenMatcher( @"--\[\[(.*)\]\]",RegexOptions.Singleline), TokenType.Comments),
                   new TokenDefinition(@"%(.*)", TokenType.Comments),

                   new TokenDefinition(@"==", TokenType.Equals),
                   new TokenDefinition(@">=", TokenType.GreaterOrEquals),
                   new TokenDefinition(@"<=", TokenType.LessOrEquals),
                   new TokenDefinition(@">", TokenType.Greater),
                   new TokenDefinition(@"<", TokenType.Less),
                   new TokenDefinition(@"!=", TokenType.NotEqualTo),
                   new TokenDefinition(@"=", TokenType.Assign),

                   new TokenDefinition(@"[0-9]+\.[0-9]+", TokenType.Double),
                   new TokenDefinition(@"[0-9]+", TokenType.Integer),

                   new TokenDefinition(@"\+", TokenType.Add),
                   new TokenDefinition(@"\-", TokenType.Subtract),
                   new TokenDefinition(@"\*", TokenType.Multiply),
                   new TokenDefinition(@"\/", TokenType.Divide),
                   new TokenDefinition(@"\^", TokenType.Power),
                   new TokenDefinition(@"\,", TokenType.Comma),
                   new TokenDefinition(@"\.", TokenType.Dot),

                   new TokenDefinition(@"\(", TokenType.LeftParenthesis),
                   new TokenDefinition(@"\)", TokenType.RightParenthesis),
                   new TokenDefinition(@"\[", TokenType.LeftBracket),
                   new TokenDefinition(@"\]", TokenType.RightBracket),
                   new TokenDefinition(@"\{", TokenType.LeftBrace),
                   new TokenDefinition(@"\}", TokenType.RightBrace),

                   new TokenDefinition(@"\;", TokenType.EndOfLine),

                   new TokenDefinition(@"[_A-Za-z]+[_A-Za-z0-9]*[\s]*\(", TokenType.Call),
                   new TokenDefinition(@"[_A-Za-z]+[_A-Za-z0-9]*", TokenType.Symbol),
                   new TokenDefinition(@"[\s]+", TokenType.Space),
               };

    }
}
