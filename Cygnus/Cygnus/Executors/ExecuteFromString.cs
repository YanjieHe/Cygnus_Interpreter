using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.LexicalAnalyzer;
using Cygnus.SyntaxAnalyzer;
using Cygnus.SyntaxTree;
using Cygnus.SymbolTable;
using System.IO;
namespace Cygnus.Executors
{
    public class ExecuteFromString
    {
        string Code;
        Scope GlobalScope;
        public ExecuteFromString(string Code)
        {
            this.Code = Code;
            GlobalScope = new Scope();
        }
        public ExecuteFromString(string Code, Scope GlobalScope)
        {
            this.Code = Code;
            this.GlobalScope = GlobalScope;
        }
        public ExecuteFromString(string Code, TextWriter tr)
        {
            this.Code = Code;
            Console.SetOut(tr);
            GlobalScope = new Scope();
        }
        public Expression Run()
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
