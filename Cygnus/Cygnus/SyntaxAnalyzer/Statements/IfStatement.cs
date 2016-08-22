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
    public class IfStatement : Statement
    {
        public IfStatement(BlockExpression Block, Scope scope, Lexeme[] array) : base(Block, scope, array) { }
        public void Parse(int start, int end, ref int EndIndex)
        {
            int IF_Position = -1, Then_Position = -1, Else_Position = -1, End_Position = -1;
            var stack = new Stack<TokenType>();
            var Else_Stack = new Stack<int>();
            if (array[start].tokenType == TokenType.If)
            {
                IF_Position = start;
                for (int i = start + 1; i <= end; i++)
                {
                    if (array[i].tokenType == TokenType.Then)
                    {
                        Then_Position = i;
                        break;
                    }
                }
                if (Then_Position < 0)
                    throw new SyntaxException("Missing 'then'");
                bool success = false;
                for (int i = Then_Position; i <= end; i++)
                {
                    switch (array[i].tokenType)
                    {
                        case TokenType.Then:
                        case TokenType.Do:
                        case TokenType.Begin:
                            stack.Push(array[i].tokenType);
                            break;
                        case TokenType.Else:
                            Else_Stack.Push(i);
                            stack.Push(array[i].tokenType); break;
                        case TokenType.End:
                            var token = stack.Pop();
                            if (stack.Count == 0)
                            {
                                End_Position = i;
                                success = true;
                                break;
                            }
                            else if (stack.Count == 1 && token == TokenType.Else)
                            {
                                End_Position = i;
                                success = true;
                                break;
                            }
                            if (token == TokenType.Else)
                            {
                                Else_Stack.Pop();
                                stack.Pop();
                            }
                            break;
                    }
                    if (success) break;
                }
                if (!success) throw new SyntaxException("Missing 'end'");
                if (Else_Stack.Count == 0)
                {
                    var test = new ExpressionStatement(Block, scope, array).ParseExpr(IF_Position + 1, Then_Position - 1);
                    var IfTrue = new BlockExpression(Block);
                    new BlockStatement(IfTrue, scope, array).ParseBlock(Then_Position + 1, End_Position - 1);
                    Block.Append(new IfThenExpression(test, IfTrue));
                }
                else if (Else_Stack.Count == 1)
                {
                    Else_Position = Else_Stack.Pop();
                    var test = new ExpressionStatement(Block, scope, array).ParseExpr(IF_Position + 1, Then_Position - 1);
                    var IfTrue = new BlockExpression(Block);
                    var IfFalse = new BlockExpression(Block);
                    new BlockStatement(IfTrue, scope, array).ParseBlock(Then_Position + 1, Else_Position - 1);
                    new BlockStatement(IfFalse, scope, array).ParseBlock(Else_Position + 1, End_Position - 1);
                    Block.Append(new IfThenElseExpression(test, IfTrue, IfFalse));
                }
                else throw new Exception();
                EndIndex = End_Position;
            }
            else throw new ArgumentException();
        }
    }
}
