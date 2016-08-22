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
    public class BlockStatement : Statement
    {
        public BlockStatement(BlockExpression Block, Scope scope, Lexeme[] array) : base(Block, scope, array) { }

        public  void Parse(int start, int end, ref int EndIndex)
        {
            throw new NotImplementedException();
        }
        public void ParseBlock(int start, int end)
        {
            int ExprStart = start;
            int ExprEnd = ExprStart;
            for (int i = start; i <= end; i++)
            {
                switch (array[i].tokenType)
                {
                    case TokenType.If:
                        new ExpressionStatement(Block, scope, array).ParseLine(ref ExprStart, ExprEnd);
                        new IfStatement(Block, scope, array).Parse(i, end, ref i);
                        ExprStart = i; ExprEnd = i;
                        break;
                    case TokenType.While:
                        new ExpressionStatement(Block, scope, array).ParseLine(ref ExprStart, ExprEnd);
                        new WhileStatement(Block, scope, array).Parse(i, end, ref i);
                        ExprStart = i; ExprEnd = i;
                        break;
                    case TokenType.For:
                        new ExpressionStatement(Block, scope, array).ParseLine(ref ExprStart, ExprEnd);
                        new ForEachStatement(Block, scope, array).Parse(i, end, ref i);
                        ExprStart = i; ExprEnd = i;
                        break;
                    case TokenType.Define:
                        new ExpressionStatement(Block, scope, array).ParseLine(ref ExprStart, ExprEnd);
                        new DefineFunctionStatement(Block, scope, array).Parse(i, end, ref i);
                        ExprStart = i; ExprEnd = i;
                        break;
                    case TokenType.EndOfLine:
                        new ExpressionStatement(Block, scope, array).ParseLine(ref ExprStart, ExprEnd);
                        ExprStart++; ExprEnd++;
                        break;
                    case TokenType.Break:
                        new ExpressionStatement(Block, scope, array).ParseLine(ref ExprStart, ExprEnd);
                        Block.Append(new BreakExpression());
                        break;
                    case TokenType.Return:
                        new ExpressionStatement(Block, scope, array).ParseLine(ref ExprStart, ExprEnd);
                        new ReturnStatement(Block, scope, array).Parse(i, end, ref i);
                        ExprStart = i; ExprEnd = i;
                        break;
                    default:
                        ExprEnd++;
                        break;
                }
            }
            new ExpressionStatement(Block, scope, array).ParseLine(ref ExprStart, ExprEnd);
        }
    }
}
