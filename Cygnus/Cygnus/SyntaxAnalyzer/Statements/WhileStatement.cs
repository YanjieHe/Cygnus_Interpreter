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
    public class WhileStatement : Statement
    {
        public WhileStatement(BlockExpression Block, Scope scope, Lexeme[] array) : base(Block, scope, array) { }
        public void Parse(int start, int end, ref int EndIndex)
        {
            int While_Position = -1, Do_Position = -1, End_Position = -1;
            var stack = new Stack<TokenType>();
            if (array[start].tokenType == TokenType.While)
            {
                While_Position = start;
                for (int i = start + 1; i <= end; i++)
                {
                    if (array[i].tokenType == TokenType.Do)
                    {
                        Do_Position = i;
                        break;
                    }
                }
                if (Do_Position < 0)
                    throw new SyntaxException("Missing 'do'");
                FindEnd(Do_Position, ref End_Position, end, ref stack);
                var condition = new ExpressionStatement(Block, scope, array).ParseExpr(While_Position + 1, Do_Position - 1);
                var body = new BlockExpression(Block);
                new BlockStatement(body, scope, array).ParseBlock(Do_Position + 1, End_Position - 1);
                Block.Append(new WhileExpression(condition, body));
                EndIndex = End_Position;
            }
            else throw new ArgumentException();
        }
    }
}
