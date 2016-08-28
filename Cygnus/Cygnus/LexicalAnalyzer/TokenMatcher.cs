using System.Text.RegularExpressions;
namespace Cygnus.LexicalAnalyzer
{
    public sealed class TokenMatcher
    {
        public readonly Regex Matcher;
        public TokenMatcher(string regex)
        {
            Matcher = new Regex(@"\G" + regex);
        }
        public TokenMatcher(string regex, RegexOptions option)
        {
            Matcher = new Regex(@"\G" + regex, option);
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
