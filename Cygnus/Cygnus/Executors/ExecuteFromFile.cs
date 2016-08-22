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
    public class ExecuteFromFile : InterpreterExecutor
    {
        string FilePath;
        Encoding encoding;
        public ExecuteFromFile(string FilePath) : base()
        {
            Initialize(FilePath, Encoding.Default);
        }
        public ExecuteFromFile(string FilePath, Scope GlobalScope) : base(GlobalScope)
        {
            Initialize(FilePath, Encoding.Default);
        }
        public ExecuteFromFile(string FilePath, Encoding encoding) : base()
        {
            Initialize(FilePath, encoding);
        }
        public ExecuteFromFile(string FilePath, Encoding encoding, Scope GlobalScope) : base(GlobalScope)
        {
            Initialize(FilePath, encoding);
        }
        private void Initialize(string FilePath, Encoding encoding)
        {
            this.FilePath = FilePath;
            this.encoding = encoding;
        }
        public override Expression Run()
        {
            try
            {
                using (var lex = new Lexical(FilePath, encoding, TokenDefinition.tokenDefinitions))
                {
                    lex.Tokenize();
                    var lex_array = Lexeme.Generate(lex.tokenList);
                    var ast = new AST();
                    BlockExpression Root = ast.Parse(lex_array, GlobalScope);
                    //                     ast.Display(Root);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Expression Result = Root.Eval(GlobalScope).GetValue(GlobalScope);
                    //Console.WriteLine(Result);
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
