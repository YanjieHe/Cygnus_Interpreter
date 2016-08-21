using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cygnus.Errors;
namespace Cygnus.LexicalAnalyzer
{
    public sealed class Lexical : IDisposable
    {
        TextReader CodeReader;
        TokenDefinition[] tokenDefinitions;
        public LinkedList<Token> tokenList;
        public Stack<TokenType> BracketStack;
        public Lexical(string Code, TokenDefinition[] TokenDefinitions)
        {
            CodeReader = new StringReader(Code);
            BracketStack = new Stack<TokenType>();
            Initialize(TokenDefinitions);
        }
        public Lexical(string Code, TokenDefinition[] TokenDefinitions, Stack<TokenType> BracketStack)
        {
            CodeReader = new StringReader(Code);
            this.BracketStack = BracketStack;
            Initialize(TokenDefinitions);
        }
        public Lexical(string FilePath, Encoding encoding, TokenDefinition[] TokenDefinitions)
        {
            CodeReader = new StreamReader(FilePath, encoding);
            BracketStack = new Stack<TokenType>();
            Initialize(TokenDefinitions);
        }
        void Initialize(TokenDefinition[] TokenDefinitions)
        {
            tokenDefinitions = TokenDefinitions;
            tokenList = new LinkedList<Token>();
        }
        private IEnumerable<string> ReadCode()
        {
            while (CodeReader.Peek() != -1)
                yield return CodeReader.ReadLine();
        }
        public void Tokenize()
        {
            foreach (var line in ReadCode()) Scan(line);
        }
        private void Scan(string line)
        {
            do
            {
                bool success = false;
                foreach (var item in tokenDefinitions)
                {
                    string content;
                    int len = item.Match(line, out content);
                    if (len != -1)
                    {
                        line = Eat(line, content, item.tokenType, len);
                        success = true;
                        break;
                    }
                }
                if (!success)
                {
                    if (line.Length == 0) break;
                    else throw new LexicalException("Unrecognizable input: {0}", line);
                }
            } while (line.Length != 0);
            if (tokenList.Last != null && tokenList.Last.Value.tokenType != TokenType.EndOfLine && tokenList.Last.Value.tokenType != TokenType.Comments && BracketStack.Count == 0)
                tokenList.AddLast(new Token("\\n", TokenType.EndOfLine));
        }
        private string Eat(string line, string content, TokenType tokenType, int len)
        {
            switch (tokenType)
            {
                case TokenType.Add:
                    if (BackTrack(tokenList.Last))
                        tokenType = TokenType.UnaryPlus; break;
                case TokenType.Subtract:
                    if (BackTrack(tokenList.Last))
                        tokenType = TokenType.UnaryMinus; break;
                case TokenType.Space:
                    return line.Substring(len);
                case TokenType.Symbol:
                    CheckKeywords(content, ref tokenType);
                    break;
                case TokenType.Function:
                    var substr = content.TrimEnd(' ', '(');
                    CheckKeywords(substr, ref tokenType);
                    if (tokenType == TokenType.Variable)
                        Append(new Token(substr, TokenType.Function));
                    else
                    {
                        Append(new Token(substr, tokenType));
                        Append(new Token("(", TokenType.LeftParenthesis));
                    }
                    return line.Substring(len);
                case TokenType.RightParenthesis://To identify no-arg function
                    if (AppendVoid(content, TokenType.Function, TokenType.RightParenthesis))
                        return line.Substring(len);
                    break;
                case TokenType.RightBrace://To identify no-arg array
                    if (AppendVoid(content, TokenType.LeftBrace, TokenType.RightBrace))
                        return line.Substring(len);
                    break;
                case TokenType.Comments:
                    return line.Substring(len);
            }
            var token = new Token(content, tokenType);
            Append(token);
            return line.Substring(len);
        }
        private void Append(Token token)
        {
            if (token.tokenType == TokenType.LeftBracket || token.tokenType == TokenType.LeftBrace || token.tokenType == TokenType.LeftParenthesis || token.tokenType == TokenType.Function)
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
                        if (BracketStack.Peek() == TokenType.LeftParenthesis || BracketStack.Peek() == TokenType.Function)
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
        private bool AppendVoid(string content, TokenType leftPart, TokenType rightPart)
        {
            if (tokenList.Last.Value.tokenType == leftPart)
            {
                Append(new Token("void", TokenType.Void));
                Append(new Token(content, rightPart));
                return true;
            }
            else return false;
        }
        public void Display()
        {
            foreach (var item in tokenList) Console.WriteLine(item);
        }
        public bool BackTrack(LinkedListNode<Token> node)
        {
            if (node == null) return true;
            switch (node.Value.tokenType)
            {
                case TokenType.Double:
                case TokenType.Integer:
                case TokenType.RightParenthesis:
                case TokenType.RightBracket:
                case TokenType.Variable:
                case TokenType.String:
                    return false;
                default: return true;
            }
        }
        public void Dispose()
        {
            CodeReader.Dispose();
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
                case "begin":
                    tokenType = TokenType.Begin; break;
                //case "func":
                //    tokenType = TokenType.Function; break;
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
                default:
                    tokenType = TokenType.Variable; break;
            }
        }
    }
}
