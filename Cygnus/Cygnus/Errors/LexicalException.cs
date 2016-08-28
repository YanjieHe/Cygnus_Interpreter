namespace Cygnus.Errors
{
    public class LexicalException : InterpreterException
    {
        public LexicalException() : base("[Lexical Exception]") { }
        public LexicalException(string format, params object[] args)
            : base("[Lexical Exception]: " + format, args)
        {

        }
        public LexicalException(string message)
            : base("[Lexical Exception]: " + message)
        {

        }
    }
}
