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
