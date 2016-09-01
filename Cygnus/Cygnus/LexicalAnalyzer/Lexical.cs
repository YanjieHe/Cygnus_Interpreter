using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cygnus.Errors;
namespace Cygnus.LexicalAnalyzer
{
    public sealed class Lexical
    {
        TokenDefinition[] tokenDefinitions;
        public LinkedList<Token> tokenList;
        public Stack<TokenType> BracketStack;
        public string Code { get; private set; }
        public Lexical(string Code)
        {
            BracketStack = new Stack<TokenType>();
            Initialize(Code);
        }
        public Lexical(string Code, Stack<TokenType> BracketStack)
        {
            this.BracketStack = BracketStack;
            Initialize(Code);
        }
        public Lexical(string FilePath, Encoding encoding)
        {
            BracketStack = new Stack<TokenType>();
            Initialize(File.ReadAllText(FilePath, encoding));
        }
        void Initialize(string Code)
        {
            this.Code = Code;
            tokenDefinitions = TokenDefinition.tokenDefinitions;
            tokenList = new LinkedList<Token>();
        }
        public void Tokenize()
        {
            int index = 0;
            do
            {
                bool success = false;
                foreach (var item in tokenDefinitions)
                {
                    var match = item.Match(Code, index);
                    if (match.Success)
                    {
                        if ((item.tokenType != TokenType.EndOfLine)
                            ||
                            (item.tokenType == TokenType.EndOfLine && BracketStack.Count == 0))
                            Eat(match.Value, item.tokenType);
                        index += match.Length;
                        success = true;
                        break;
                    }
                }
                if (!success)
                {
                    if (index == Code.Length) break;
                    else throw new LexicalException("Unrecognizable input: {0}", Code.Substring(index));
                }
            } while (index < Code.Length);
            Append(new Token("\r\n", TokenType.EndOfLine));
        }
        private void Eat(string content, TokenType tokenType)
        {
            switch (tokenType)
            {
                //case TokenType.Add:
                //    if (BackTrack(tokenList.Last))
                //        tokenType = TokenType.UnaryPlus; break;
                //case TokenType.Subtract:
                //    if (BackTrack(tokenList.Last))
                //        tokenType = TokenType.UnaryMinus; break;
                case TokenType.Space:
                case TokenType.Comments: return;
                case TokenType.Symbol:
                    CheckKeywords(content, ref tokenType);
                    break;
                case TokenType.Call:
                    var substr = content.TrimEnd(' ', '(');
                    CheckKeywords(substr, ref tokenType);
                    if (tokenType == TokenType.Variable)
                        Append(new Token(substr, TokenType.Call));
                    else
                    {
                        Append(new Token(substr, tokenType));
                        Append(new Token("(", TokenType.LeftParenthesis));
                    }
                    return;
            }
            var token = new Token(content, tokenType);
            Append(token);
        }
        private void Append(Token token)
        {
            if (token.tokenType == TokenType.LeftBracket || token.tokenType == TokenType.LeftBrace || token.tokenType == TokenType.LeftParenthesis || token.tokenType == TokenType.Call)
            {
                BracketStack.Push(token.tokenType);
            }
            else if (token.tokenType == TokenType.RightBracket || token.tokenType == TokenType.RightBrace || token.tokenType == TokenType.RightParenthesis)
            {
                switch (token.tokenType)
                {
                    case TokenType.RightBracket:
                        if (BracketStack.Peek() == TokenType.LeftBracket)
                        {
                            BracketStack.Pop();
                            break;
                        }
                        else throw new SyntaxException("Mismatch for brackets");
                    case TokenType.RightBrace:
                        if (BracketStack.Peek() == TokenType.LeftBrace)
                        {
                            BracketStack.Pop();
                            break;
                        }
                        else throw new SyntaxException("Mismatch for braces");
                    case TokenType.RightParenthesis:
                        if (BracketStack.Peek() == TokenType.LeftParenthesis || BracketStack.Peek() == TokenType.Call)
                        {
                            BracketStack.Pop();
                            break;
                        }
                        else throw new SyntaxException("Mismatch for parenthesises");
                    default:
                        throw new SyntaxException("Mismatch for {0}", token.tokenType);
                }
            }
            tokenList.AddLast(token);
        }
        public void Display()
        {
            foreach (var item in tokenList) Console.WriteLine(item);
        }
        public void CheckKeywords(string word, ref TokenType tokenType)
        {
            switch (word)
            {
                case "and":
                    tokenType = TokenType.And; break;
                case "or":
                    tokenType = TokenType.Or; break;
                case "not":
                    tokenType = TokenType.Not; break;
                case "true":
                    tokenType = TokenType.True; break;
                case "false":
                    tokenType = TokenType.False; break;
                case "def":
                    tokenType = TokenType.Define; break;
                case "repeat":
                    tokenType = TokenType.Repeat; break;
                case "until":
                    tokenType = TokenType.Until; break;
                case "return":
                    tokenType = TokenType.Return; break;
                case "end":
                    tokenType = TokenType.End; break;
                case "if":
                    tokenType = TokenType.If; break;
                case "else":
                    tokenType = TokenType.Else; break;
                case "elif":
                    tokenType = TokenType.ElseIf; break;
                case "for":
                    tokenType = TokenType.For; break;
                case "while":
                    tokenType = TokenType.While; break;
                case "break":
                    tokenType = TokenType.Break; break;
                case "continue":
                    tokenType = TokenType.Continue; break;
                case "begin":
                    tokenType = TokenType.Begin; break;
                case "in":
                    tokenType = TokenType.In; break;
                case "null":
                    tokenType = TokenType.Null; break;
                case "void":
                    tokenType = TokenType.Void; break;
                case "do":
                    tokenType = TokenType.Do; break;
                case "then":
                    tokenType = TokenType.Then; break;
                case "pass":
                    tokenType = TokenType.Pass; break;
                case "try":
                    tokenType = TokenType.Try; break;
                case "catch":
                    tokenType = TokenType.Catch; break;
                default:
                    tokenType = TokenType.Variable; break;
            }
        }
    }
}
