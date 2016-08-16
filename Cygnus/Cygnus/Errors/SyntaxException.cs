namespace Cygnus.Errors
{
    public class SyntaxException : InterpreterException
    {
        public SyntaxException(string format, params object[] args)
			: base("[Syntax Exception]: " + format, args)
		{

        }
        public SyntaxException(string message)
			: base("[Syntax Exception]: " + message)
		{

        }
    }
}
