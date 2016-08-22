using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SyntaxTree;
using Cygnus.LexicalAnalyzer;
using Cygnus.Errors;
namespace Cygnus.SyntaxAnalyzer.Statements
{
    public abstract class Statement
    {
        public BlockExpression Block { get; protected set; }
        public Scope scope { get; protected set; }
        public Lexeme[] array { get; private set; }
        public Statement(BlockExpression Block, Scope scope, Lexeme[] array)
        {
            this.Block = Block;
            this.scope = scope;
            this.array = array;
        }
        public void FindEnd(int StartPosition, ref int EndPosition, int end, ref Stack<TokenType> stack)
        {
            bool success = false;
            for (int i = StartPosition; i <= end; i++)
            {
                switch (array[i].tokenType)
                {
                    case TokenType.Then:
                    case TokenType.Do:
                    case TokenType.Begin:
                        stack.Push(array[i].tokenType);
                        break;
                    case TokenType.End:
                        var token = stack.Pop();
                        if (stack.Count == 0)
                        {
                            EndPosition = i;
                            success = true;
                            break;
                        }
                        break;
                }
                if (success) break;
            }
            if (!success) throw new SyntaxException("Missing 'end'");
        }
        public IEnumerable<Lexeme> Subset(int start, int end)
        {
            for (int i = start; i <= end; i++)
                yield return array[i];
        }
        public static bool IsTerminator(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.Define:
                case TokenType.Begin:
                case TokenType.Repeat:
                case TokenType.Until:
                case TokenType.Return:
                case TokenType.Do:
                case TokenType.End:
                case TokenType.If:
                case TokenType.Then:
                case TokenType.Else:
                case TokenType.ElseIf:
                case TokenType.For:
                case TokenType.While:
                case TokenType.Break:
                case TokenType.In:
                case TokenType.EndOfLine:
                    return true;
                default:
                    return false;
            }
        }
    }
}
