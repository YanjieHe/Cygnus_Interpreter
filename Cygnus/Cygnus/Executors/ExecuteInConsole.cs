using System;
using System.Collections.Generic;
using Cygnus.LexicalAnalyzer;
using Cygnus.SyntaxAnalyzer;
using Cygnus.SyntaxTree;
namespace Cygnus.Executors
{
    public class ExecuteInConsole : InterpreterExecutor
    {
        Stack<TokenType> stack;
        Stack<TokenType> BracketStack = new Stack<TokenType>();
        LinkedList<Token> currentList;
        public ExecuteInConsole() : base()
        {
            stack = new Stack<TokenType>();
            currentList = new LinkedList<Token>();
        }
        public ExecuteInConsole(Scope GlobalScope) : base(GlobalScope)
        {
            stack = new Stack<TokenType>();
            currentList = new LinkedList<Token>();
        }
        public override Expression Run()
        {
            while (true)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(">>> ");
                    string line = Console.ReadLine();
                Start:
                    var lex = new Lexical(line, BracketStack);
                    lex.Tokenize();
                    foreach (var item in lex.tokenList)
                        currentList.AddLast(item);
                    if (!Check(lex.tokenList))
                    {
                        Console.Write("... ");
                        line = Console.ReadLine();
                        goto Start;
                    }
                    var lex_array = Lexeme.Generate(currentList);
                    currentList.Clear();

                    var ast = new AST();
                    BlockExpression Root = ast.Parse(lex_array, GlobalScope);
                    //   ast.Display(Root);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Expression Result = Root.Eval(GlobalScope).GetValue(GlobalScope);
                    //Console.WriteLine(Result);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.ToString());
                    currentList.Clear();
                    stack.Clear();
                    BracketStack.Clear();
                }
            }
        }
        public bool Check(LinkedList<Token> list)
        {
            var current = list.First;
            while (current != null)
            {
                var item = current.Value;
                switch (item.tokenType)
                {
                    case TokenType.LeftBrace:
                    case TokenType.LeftParenthesis:
                    case TokenType.LeftBracket:
                    case TokenType.Call:
                    case TokenType.Do:
                    case TokenType.Begin:
                        stack.Push(item.tokenType); break;
                    case TokenType.If:
                        stack.Push(item.tokenType);
                        break;
                    case TokenType.RightBrace:
                        if (stack.Peek() != TokenType.LeftBrace) throw new ArgumentException();
                        else stack.Pop(); break;

                    case TokenType.RightBracket:
                        if (stack.Peek() != TokenType.LeftBracket) throw new ArgumentException();
                        else stack.Pop(); break;
                    case TokenType.RightParenthesis:
                        if (stack.Peek() != TokenType.LeftParenthesis
                            && stack.Peek() != TokenType.Call)
                            throw new ArgumentException();
                        else stack.Pop(); break;
                    case TokenType.End:
                        if (stack.Peek() != TokenType.Do
                            && stack.Peek() != TokenType.Begin
                            && stack.Peek() != TokenType.If
                            && stack.Peek() != TokenType.Else)
                            throw new ArgumentException();
                        else stack.Pop(); break;
                }
                current = current.Next;
            }
            return stack.Count == 0;
        }
    }
}
