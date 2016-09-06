using System;
using System.IO;
using Cygnus.LexicalAnalyzer;
using Cygnus.SyntaxAnalyzer;
using Cygnus.Expressions;
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

        protected Expression Execute(Lexical lex)
        {
            lex.Tokenize();
            var lex_array = Lexeme.Generate(lex.tokenList);
            var ast = new AST();
            BlockExpression Root = ast.Parse(lex_array, GlobalScope);
            //ast.Display(Root);
            Console.ForegroundColor = ConsoleColor.Green;
            Expression Result = Root.Eval(GlobalScope).GetValue(GlobalScope);
            // Console.WriteLine(Result);
            return Result;
        }
    }
}
