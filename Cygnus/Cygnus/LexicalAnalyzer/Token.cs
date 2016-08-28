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
}
