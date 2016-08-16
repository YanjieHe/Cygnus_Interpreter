using System.Text.RegularExpressions;
namespace Cygnus.LexicalAnalyzer
{
    public sealed class TokenMatcher
    {
        public readonly Regex Matcher;
        public TokenMatcher(string regex)
        {
            Matcher = new Regex("^" + regex);
        }
        public static implicit operator TokenMatcher(string s)
        {
            return new TokenMatcher(s);
        }
        public override string ToString()
        {
            return Matcher.ToString();
        }
    }
}
