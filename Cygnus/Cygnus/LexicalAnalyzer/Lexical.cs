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
        public Lexical(string Code, TokenDefinition[] TokenDefinitions)
        {
            CodeReader = new StringReader(Code);
            Initialize(TokenDefinitions);
        }
        public Lexical(string FilePath, Encoding encoding, TokenDefinition[] TokenDefinitions)
        {
            CodeReader = new StreamReader(FilePath, encoding);
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
            if (tokenList.Last != null && tokenList.Last.Value.tokenType != TokenType.EndOfLine)
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
                        tokenList.AddLast(new Token(substr, TokenType.Function));
                    else
                    {
                        tokenList.AddLast(new Token(substr, tokenType));
                        tokenList.AddLast(new Token("(", TokenType.LeftParenthesis));
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
            }
            var token = new Token(content, tokenType);
            tokenList.AddLast(token);
            return line.Substring(len);
        }
        private bool AppendVoid(string content, TokenType leftPart, TokenType rightPart)
        {
            if (tokenList.Last.Value.tokenType == leftPart)
            {
                tokenList.AddLast(new Token("void", TokenType.Void));
                tokenList.AddLast(new Token(content, rightPart));
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
