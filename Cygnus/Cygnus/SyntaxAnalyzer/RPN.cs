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
                        {
                            var current = (Operator)item.Content;
                            while (op.Count > 0)
                            {
                                int cmp = Priority(current) - Priority(GetOp(op.Peek()));
                                if (cmp > 0) break;
                                else if (cmp == 0)
                                {
                                    Operands.Add(op.Pop());
                                    break;
                                }
                                else Operands.Add(op.Pop());
                            }
                            op.Push(item);
                        }
                        break;
                    case TokenType.EndOfLine://终止符按双目运算符考虑
                        continue;
                    case TokenType.Function:
                        fstack.Push(item);
                        op.Push(item); break;
                    case TokenType.Comma:
                        var comma = item;
                        while (op.Count > 0)
                        {
                            var current = GetOp(op.Peek());
                            if (current == Operator.Comma
                                || current == Operator.LeftBrace)//Left brace stands for initializing an array
                            {
                                Operands.Add(item);
                                break;
                            }
                            else if (current == Operator.Function)
                            {
                                Operands.Add(item);
                                break;
                            }
                            else Operands.Add(op.Pop());
                        }
                        break;
                    case TokenType.LeftParenthesis:
                        op.Push(item);
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
                                else if (GetOp(current) == Operator.Function)
                                {
                                    var func = fstack.Pop();
                                    Operands.Add(func);//  将函数压入栈
                                    success = true;
                                    break;
                                }
                                else Operands.Add(current);
                            }
                            if (!success)
                                throw new SyntaxException("There are mismatched parentheses.");
                        }
                        break;
                    case TokenType.LeftBrace:
                        op.Push(item); break;
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
                    case TokenType.LeftBracket:
                        op.Push(item); break;
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
            return obj.tokenType == TokenType.Function
                ? Operator.Function
                : obj.tokenType == TokenType.LeftBrace
                ? Operator.LeftBrace
                : (Operator)obj.Content;
        }
        public void Display()
        {
            foreach (var item in Operands) Console.Write(item + "  ");
        }
        /*
        0:#        1:=        2:(        3:,        4:or        5:and
        6:< <= > >=        7: == !=        8:+-        9:* /        10:正负,not
        11:!%^        12:)        */
        private static int Priority(Operator op)
        {
            switch (op)
            {
                //case Operator.Terminator: return 0;
                case Operator.LeftBracket: return 1;//Index
                case Operator.Return:
                case Operator.Function:
                case Operator.LeftParenthesis: return 2;
                case Operator.Assgin: return 3;
                case Operator.Comma: return 3;
                case Operator.Or: return 4;
                case Operator.And: return 5;
                case Operator.Less:
                case Operator.Greater:
                case Operator.LessOrEquals:
                case Operator.GreaterOrEquals: return 6;
                case Operator.Equals:
                case Operator.NotEqualTo: return 7;
                case Operator.Add:
                case Operator.Subtract: return 8;
                case Operator.Multiply:
                case Operator.Divide: return 9;
                case Operator.Not:
                case Operator.UnaryPlus:
                case Operator.UnaryMinus:
                    return 10;
                case Operator.Power: return 11;
                case Operator.RightBracket: return 12;
                case Operator.RightParenthesis: return 13;
                default:
                    throw new NotSupportedException("not supported operator '" + op + "'");
            }
        }
        /*
        BL:转向某标号(为单目后缀运算符)        BT：条件为真时转移
        BF：条件为假时转移        BR：无条件转移        */
    }
}
