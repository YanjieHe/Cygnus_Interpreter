using System;
using System.Collections.Generic;
using Cygnus.LexicalAnalyzer;
using Cygnus.Errors;
namespace Cygnus.SyntaxAnalyzer
{
    public sealed class RPN
    {
        IEnumerable<Lexeme> list;
        public List<Lexeme> Operands;
        public RPN(IEnumerable<Lexeme> _list)
        {
            list = _list;
        }
        public RPN Analyze()
        {
            Stack<Lexeme> op = new Stack<Lexeme>();
            Operands = new List<Lexeme>();
            Stack<Lexeme> fstack = new Stack<Lexeme>();
            foreach (var item in list)
            {
                switch (item.tokenType)
                {
                    case TokenType.Double:
                    case TokenType.Integer:
                    case TokenType.True:
                    case TokenType.False:
                    case TokenType.String:
                    case TokenType.Char:
                    case TokenType.Null:
                    case TokenType.Variable:
                    case TokenType.Void:
                        Operands.Add(item); break;

                    case TokenType.UnaryPlus:
                    case TokenType.UnaryMinus:
                    case TokenType.Not:
                    case TokenType.Return:
                        op.Push(item);
                        break;
                    case TokenType.Add:
                    case TokenType.Subtract:
                    case TokenType.Multiply:
                    case TokenType.Divide:
                    case TokenType.Power:

                    case TokenType.Less_Than:
                    case TokenType.Greater_Than:
                    case TokenType.Less_Than_Or_Equals:
                    case TokenType.Greater_Than_Or_Equals:
                    case TokenType.Equals:
                    case TokenType.Not_Equal_To:

                    case TokenType.And:
                    case TokenType.Or:
                    case TokenType.Assign:
                    case TokenType.Dot:
                    case TokenType.LeftBracket:
                        {
                            var current = (Operator)item.Content;
                            while (op.Count > 0)
                            {
                                int cmp = Priority(current) - Priority(GetOp(op.Peek()));
                                if (cmp > 0) break;
                                else if (cmp == 0)
                                {
                                    if (op.Peek().tokenType != TokenType.Assign)
                                        Operands.Add(op.Pop());
                                    break;
                                }
                                else Operands.Add(op.Pop());
                            }
                            op.Push(item);
                        }
                        break;
                    case TokenType.No_Arg:
                    case TokenType.EndOfLine://Omit end of line
                        continue;
                    case TokenType.Call:
                        fstack.Push(item);
                        op.Push(item); break;
                    case TokenType.Comma:
                        var comma = item;
                        while (op.Count > 0)
                        {
                            var current = GetOp(op.Peek());
                            if (current == Operator.Comma || current == Operator.LeftBrace)
                            //Left brace stands for initializing an array
                            {
                                Operands.Add(item);
                                break;
                            }
                            else if (current == Operator.Call)
                            {
                                Operands.Add(item);
                                break;
                            }
                            else Operands.Add(op.Pop());
                        }
                        break;
                    case TokenType.RightParenthesis:
                        {
                            bool success = false;
                            while (op.Count > 0)
                            {
                                var current = op.Pop();
                                if (GetOp(current) == Operator.LeftParenthesis)
                                {
                                    success = true;
                                    break;
                                }
                                else if (GetOp(current) == Operator.Call)
                                {
                                    var func = fstack.Pop();
                                    Operands.Add(func);//  Push the function into the function stack
                                    success = true;
                                    break;
                                }
                                else Operands.Add(current);
                            }
                            if (!success)
                                throw new SyntaxException("There are mismatched parentheses.");
                        }
                        break;
                    case TokenType.LeftParenthesis:
                    case TokenType.LeftBrace:
                        op.Push(item);
                        break;
                    case TokenType.RightBrace:
                        {
                            bool success = false;
                            while (op.Count > 0)
                            {
                                var current = op.Pop();
                                if (GetOp(current) == Operator.LeftBrace)
                                {
                                    Operands.Add(current);
                                    success = true;
                                    break;
                                }
                                else
                                    Operands.Add(current);
                            }
                            if (!success)
                                throw new SyntaxException("There are mismatched Braces.");
                        }
                        break;
                    case TokenType.RightBracket:
                        {
                            bool success = false;
                            while (op.Count > 0)
                            {
                                var current = op.Pop();
                                if (GetOp(current) == Operator.LeftBracket)
                                {
                                    Operands.Add(current);
                                    success = true;
                                    break;
                                }
                                else
                                    Operands.Add(current);
                            }
                            if (!success)
                                throw new SyntaxException("There are mismatched Brackets.");
                        }
                        break;
                    default:
                        throw new NotSupportedException(string.Format("{0} error", item));
                }
            }
            while (op.Count > 0) Operands.Add(op.Pop());
            return this;
        }
        public Operator GetOp(Lexeme obj)
        {
            switch (obj.tokenType)
            {
                case TokenType.Call:
                    return Operator.Call;
                case TokenType.LeftBrace:
                    return Operator.LeftBrace;
                case TokenType.LeftBracket:
                    return Operator.LeftBracket;
                default:
                    return (Operator)obj.Content;
            }
        }
        public void Display()
        {
            foreach (var item in Operands) Console.Write(item + "  ");
        }
        private static int Priority(Operator op)
        {
            switch (op)
            {
                case Operator.LeftParenthesis:
                case Operator.Call: return 0;
                case Operator.LeftBrace:
                case Operator.Comma: return 1;
                case Operator.Assgin: return 2;
                case Operator.Or: return 3;
                case Operator.And: return 4;
                case Operator.Equals:
                case Operator.NotEqualTo: return 5;
                case Operator.Greater:
                case Operator.Less:
                case Operator.GreaterOrEquals:
                case Operator.LessOrEquals: return 6;
                case Operator.Add:
                case Operator.Subtract: return 7;
                case Operator.Multiply:
                case Operator.Divide: return 8;
                case Operator.Power: return 9;
                case Operator.UnaryPlus:
                case Operator.UnaryMinus:
                case Operator.Not: return 10;
                case Operator.Dot:
                case Operator.RightBrace:
                case Operator.LeftBracket:
                case Operator.RightBracket:
                case Operator.RightParenthesis: return 11;
                default:
                    throw new NotSupportedException("not supported operator '" + op + "'");
            }
        }
        public static void DisplayStack<T>(Stack<T> stack)
        {
            Console.WriteLine("Stack: " + string.Join("  ", stack));
        }
        public static void DisplayOperands<T>(List<T> list)
        {
            Console.WriteLine("Operands: " + string.Join("  ", list));
        }
    }
}
