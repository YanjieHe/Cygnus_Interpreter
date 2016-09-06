using System;
using Cygnus.LexicalAnalyzer;
using Cygnus.Expressions;
namespace Cygnus.Executors
{
    public class ExecuteFromString : InterpreterExecutor
    {
        public string Code { get; private set; }
        public ExecuteFromString(string Code, Scope GlobalScope) : base(GlobalScope)
        {
            this.Code = Code;
        }
        public ExecuteFromString(string Code) : base()
        {
            this.Code = Code;
        }
        public override Expression Run()
        {
            try
            {
                return Execute(new Lexical(Code));
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                return Expression.Void();
            }
        }
    }
}
