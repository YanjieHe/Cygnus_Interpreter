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
    public class ForEachStatement : Statement
    {
        public ForEachStatement(BlockExpression Block, Scope scope, Lexeme[] array) : base(Block, scope, array) { }
        public  void Parse(int start, int end, ref int EndIndex)
        {
            int For_Position = -1, In_Position = -1, Do_Position = -1, End_Position = -1;
            var stack = new Stack<TokenType>();
            if (array[start].tokenType == TokenType.For)
            {
                For_Position = start;
                if (array[For_Position + 1].tokenType != TokenType.Variable)
                    throw new ArgumentException();
                if (array[For_Position + 2].tokenType == TokenType.In)
                    In_Position = For_Position + 2;
                else throw new ArgumentException();

                for (int i = In_Position; i <= end; i++)
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
                var Iter_List = new ExpressionStatement(Block, scope, array).ParseExpr(In_Position + 1, Do_Position - 1);
                var body = new BlockExpression(Block);
                new BlockStatement(body, scope, array).ParseBlock(Do_Position + 1, End_Position - 1);
                var Iter_Variable = new ParameterExpression(array[For_Position + 1].Content as string);
                Block.Append(new ForEachExpression(Iter_List, body, Iter_Variable));
                EndIndex = End_Position;
            }
            else throw new ArgumentException();
        }
    }
}
