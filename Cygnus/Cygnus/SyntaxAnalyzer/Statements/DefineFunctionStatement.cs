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
    public class DefineFunctionStatement : Statement
    {
        public DefineFunctionStatement(BlockExpression Block, Scope scope, Lexeme[] array) : base(Block, scope, array) { }
        public void Parse(int start, int end, ref int EndIndex)
        {
            int Def_Position = -1, Begin_Position = -1, End_Position = -1;
            var stack = new Stack<TokenType>();
            if (array[start].tokenType == TokenType.Define)
            {
                Def_Position = start;
                for (int i = Def_Position + 1; i <= end; i++)
                {
                    if (array[i].tokenType == TokenType.Begin)
                    {
                        Begin_Position = i;
                        break;
                    }
                }
                if (Begin_Position < 0)
                    throw new SyntaxException("Missing 'begin'");
                FindEnd(Begin_Position, ref End_Position, end, ref stack);
                var body = new BlockExpression(Block);
                var parameters = Subset(Def_Position + 1, Begin_Position - 1);
                var funcTuple = (array[Def_Position + 1].Content as FuncTuple);
                int argsCount = parameters.Count(j => j.tokenType == TokenType.Variable);
                var arguments = new ParameterExpression[argsCount];
                int k = 0;
                foreach (var item in parameters.Where(j => j.tokenType == TokenType.Variable))
                {
                    arguments[k] = new ParameterExpression(item.Content as string);
                    k++;
                }
                var funcScope = new Scope(scope);
                var FUNCTION = new FunctionExpression(funcTuple.Name, body, funcScope, arguments);
                Scope.functionTable[FUNCTION.Name] = FUNCTION;
                new BlockStatement(body, scope, array).ParseBlock(Begin_Position + 1, End_Position - 1);
                Block.Append(new ConstantExpression(null, ConstantType.Void));
                EndIndex = End_Position;
            }
            else throw new ArgumentException();
        }
    }
}
