using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.LexicalAnalyzer;
using Cygnus.SyntaxAnalyzer;
using Cygnus.SyntaxTree;
namespace Cygnus.Executors
{
    public class ExecuteFromFile
    {
        string FilePath;
        Encoding encoding;
        public ExecuteFromFile(string FilePath)
        {
            this.FilePath = FilePath;
            this.encoding = Encoding.Default;
        }
        public ExecuteFromFile(string FilePath, Encoding encoding)
        {
            this.FilePath = FilePath;
            this.encoding = encoding;
        }
        public void Run()
        {
            try
            {
                using (var lex = new Lexical(FilePath, encoding, TokenDefinition.tokenDefinitions))
                {
                    lex.Tokenize();
                    var array = Lexeme.Generate(lex.tokenList);
                    var ast = new AST();
                    BlockExpression Root = ast.Parse(array);
                   // ast.Display(Root);
                    Expression Result = Root.Eval();
                    Console.ForegroundColor = ConsoleColor.Green;
                    if (Result.NodeType == ExpressionType.Default)
                    {
                        if (((DefaultExpression)Result).defaultType != ConstantType.Void)
                            Console.WriteLine(Result);
                    }
                    else if (Result.NodeType == ExpressionType.Parameter)
                    {
                        Console.WriteLine(((ParameterExpression)Result).Value);
                    }
                    else if (Result.NodeType == ExpressionType.Return)
                    {
                        Console.WriteLine((Result as ReturnExpression).expression.Eval());
                    }
                    else Console.WriteLine(Result);
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
