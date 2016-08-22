using System;
using System.IO;
using Cygnus.SyntaxTree;
namespace Cygnus.Executors
{
    public abstract class InterpreterExecutor
    {
        public Scope GlobalScope { get; protected set; }
        public void SetConsole(TextWriter textWriter)
        {
            Console.SetOut(textWriter);
        }
        public InterpreterExecutor(Scope GlobalScope)
        {
            this.GlobalScope = GlobalScope;
        }
        public InterpreterExecutor()
        {
            this.GlobalScope = new Scope();
        }
        public abstract Expression Run();
    }
}
