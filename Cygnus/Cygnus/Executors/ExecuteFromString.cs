using System;
using Cygnus.LexicalAnalyzer;
using Cygnus.SyntaxAnalyzer;
using Cygnus.SyntaxTree;
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
                using (var lex = new Lexical(Code, TokenDefinition.tokenDefinitions))
                {
                    lex.Tokenize();
                    var lex_array = Lexeme.Generate(lex.tokenList);
                    var ast = new AST();
                    BlockExpression Root = ast.Parse(lex_array, GlobalScope);
                    // ast.Display(Root);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Expression Result = Root.Eval(GlobalScope).GetValue(GlobalScope);
                    // Console.WriteLine(Result);
                    return Result;
                }
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
