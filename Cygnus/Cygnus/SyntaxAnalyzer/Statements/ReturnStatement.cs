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
    public class ReturnStatement : Statement
    {
        public ReturnStatement(BlockExpression Block, Scope scope, Lexeme[] array) : base(Block, scope, array)
        {
        }
        public void Parse(int start, int end, ref int EndIndex)
        {
            int Return_Position = -1, End_Position = -1;
            if (array[start].tokenType == TokenType.Return)
            {
                Return_Position = start;
                for (int i = start + 1; i <= end; i++)
                {
                    if (IsTerminator(array[i].tokenType))
                    {
                        End_Position = i;
                        break;
                    }
                }
                if (End_Position < 0)
                    End_Position = end;
                var expression = new ExpressionStatement(Block, scope, array).ParseExpr(Return_Position + 1, End_Position);
                Block.Append(new ReturnExpression(expression));
                EndIndex = End_Position;
            }
            else throw new ArgumentException();
        }
    }
}
