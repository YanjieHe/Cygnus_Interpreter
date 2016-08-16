using System;

namespace Cygnus.Errors
{
    public class InterpreterException : Exception
    {
        public InterpreterException(Exception ex, string message)
            : base(message, ex)
        {

        }

        public InterpreterException(Exception ex)
            : base(ex.Message, ex)
        {

        }

        public InterpreterException(string message)
            : base(message)
        {

        }

        public InterpreterException(string format, params object[] args)
            : base(string.Format(format, args))
        {

        }
    }
}
