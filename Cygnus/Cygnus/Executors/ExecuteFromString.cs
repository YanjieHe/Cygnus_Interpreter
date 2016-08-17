using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.LexicalAnalyzer;
using Cygnus.SyntaxAnalyzer;
using Cygnus.SyntaxTree;
using Cygnus.SymbolTable;
namespace Cygnus.Executors
{
    public class ExecuteFromString
    {
        string Code;
        public ExecuteFromString(string Code)
        {
            this.Code = Code;
        }
        public void Run()
        {
            try
            {
                using (var lex = new Lexical(Code, TokenDefinition.tokenDefinitions))
                {
                    lex.Tokenize();
                    var lex_array = Lexeme.Generate(lex.tokenList);
                    var ast = new AST();
                    Scope GlobalScope = new Scope();
                    BlockExpression Root = ast.Parse(lex_array, GlobalScope);
                    // ast.Display(Root);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Expression Result = Root.Eval(GlobalScope).GetValue(GlobalScope);
                   // Console.WriteLine(Result);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
