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
    public class ExpressionStatement : Statement
    {
        public ExpressionStatement(BlockExpression Block, Scope scope, Lexeme[] array) : base(Block, scope, array) { }
        public  void Parse(int start, int end, ref int EndIndex)
        {
            throw new NotImplementedException();
        }
        public void ParseLine( ref int start, int end)
        {
            if (start != end)
            {
                Block.Append(ParseExpr( start, end));
                start = end;
            }
        }
        public Expression ParseExpr(int start, int end)
        {
            CountArguments(start, end);
            return ParseRPNExpr(ConvertToRPN(Subset(start, end)));
        }
        private IEnumerable<Lexeme> ConvertToRPN(IEnumerable<Lexeme> sequence)
        {
            return new RPN(sequence).Analyze().Operands;
        }
        private Expression ParseRPNExpr(IEnumerable<Lexeme> ReversePolishNotation)
        {
            var stack = new Stack<Expression>();
            foreach (var item in ReversePolishNotation)
            {
                switch (item.tokenType)
                {
                    case TokenType.String:
                        stack.Push(new ConstantExpression(item.Content, ConstantType.String));
                        break;
                    case TokenType.Char:
                        stack.Push(new ConstantExpression(item.Content, ConstantType.Char));
                        break;
                    case TokenType.Double:
                        stack.Push(new ConstantExpression(item.Content, ConstantType.Double));
                        break;
                    case TokenType.Integer:
                        stack.Push(new ConstantExpression(item.Content, ConstantType.Integer));
                        break;
                    case TokenType.UnaryPlus:
                    case TokenType.UnaryMinus:
                    case TokenType.Not:
                        {
                            var value = stack.Pop();
                            stack.Push(new UnaryExpression((Operator)item.Content, value));
                        }
                        break;
                    case TokenType.Add:
                    case TokenType.Subtract:
                    case TokenType.Multiply:
                    case TokenType.Divide:
                    case TokenType.Power:
                    case TokenType.And:
                    case TokenType.Or:
                    case TokenType.Equals:
                    case TokenType.Greater_Than_Or_Equals:
                    case TokenType.Less_Than_Or_Equals:
                    case TokenType.Greater_Than:
                    case TokenType.Less_Than:
                    case TokenType.Not_Equal_To:
                    case TokenType.Assign:
                        {
                            var right = stack.Pop();
                            var left = stack.Pop();
                            stack.Push(new BinaryExpression((Operator)item.Content, left, right));
                        }
                        break;
                    case TokenType.Comma: continue;
                    case TokenType.Dot:
                        {
                            var index = stack.Pop();
                            var collection = stack.Pop();
                            stack.Push(new IndexExpression(collection, index, IndexType.Dot));
                        }
                        break;
                    case TokenType.LeftBracket:
                        {
                            var index = stack.Pop();
                            var collection = stack.Pop();
                            stack.Push(new IndexExpression(collection, index, IndexType.Bracket));
                        }
                        break;
                    case TokenType.LeftBrace:
                        {
                            var tuple = item.Content as FuncTuple;
                            Expression[] arguments = new Expression[tuple.argsCount];
                            for (int i = tuple.argsCount - 1; i >= 0; i--)
                                arguments[i] = stack.Pop().GetValue(scope);
                            stack.Push(new ArrayExpression(arguments));
                        }
                        break;
                    case TokenType.True:
                        stack.Push(new ConstantExpression(true, ConstantType.Boolean));
                        break;
                    case TokenType.False:
                        stack.Push(new ConstantExpression(false, ConstantType.Boolean));
                        break;
                    case TokenType.Repeat:
                        break;
                    case TokenType.Until:
                        break;
                    case TokenType.Return:
                        {
                            var value = stack.Pop();
                            stack.Push(new ReturnExpression(value));
                        }
                        break;
                    case TokenType.Call:
                        {
                            var tuple = item.Content as FuncTuple;
                            Expression[] arguments = new Expression[tuple.argsCount];
                            for (int i = tuple.argsCount - 1; i >= 0; i--)
                                arguments[i] = stack.Pop();
                            stack.Push(new CallExpression(tuple.Name, arguments));
                        }
                        break;
                    case TokenType.Null:
                        stack.Push(new ConstantExpression(null, ConstantType.Null));
                        break;
                    case TokenType.Variable:
                        stack.Push(new ParameterExpression(item.Content as string));
                        break;
                    case TokenType.Void:
                        stack.Push(new ConstantExpression(null, ConstantType.Void));
                        break;
                    default:
                        throw new SyntaxException("Wrong element for expression: '{0}'", item);
                }
            }
            return stack.Pop();
        }
        public void CountArguments(int start, int end)
        {
            Stack<Lexeme> stack = new Stack<Lexeme>();
            Stack<int> args_stack = new Stack<int>();
            for (int i = start; i <= end; i++)
            {
                switch (array[i].tokenType)
                {
                    case TokenType.Call:
                    case TokenType.LeftBrace:
                    case TokenType.LeftParenthesis:
                        stack.Push(array[i]);
                        args_stack.Push(1);
                        break;
                    case TokenType.No_Arg:
                        {
                            args_stack.Pop();
                            args_stack.Push(0);
                            break;
                        }
                    case TokenType.Comma:
                        {
                            int n = args_stack.Pop();
                            args_stack.Push(n + 1);
                        }
                        break;
                    case TokenType.RightBrace:
                        var leftbrace = stack.Pop();
                        if (leftbrace.tokenType == TokenType.LeftBrace)
                            ((FuncTuple)leftbrace.Content).argsCount = args_stack.Pop();
                        else throw new ArgumentException();
                        break;
                    case TokenType.RightParenthesis:
                        if (stack.Peek().tokenType == TokenType.LeftParenthesis)
                            stack.Pop();
                        else if (stack.Peek().tokenType == TokenType.Call)
                            ((FuncTuple)stack.Pop().Content).argsCount = args_stack.Pop();
                        break;
                }
            }
        }
       
    }
}
