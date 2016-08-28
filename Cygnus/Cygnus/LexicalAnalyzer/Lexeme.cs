using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cygnus.Extensions;
namespace Cygnus.LexicalAnalyzer
{
    public sealed class Lexeme
    {
        public TokenType tokenType { get; private set; }
        public object Content { get; private set; }
        public Lexeme(TokenType tokenType, object Content)
        {
            this.tokenType = tokenType;
            this.Content = Content;
        }
        public static Lexeme[] Generate(LinkedList<Token> list)
        {
            return list.Map(i => new Lexeme(i.tokenType, GetContent(i)));
        }
        private static object GetContent(Token token)
        {
            switch (token.tokenType)
            {
                case TokenType.String:
                    return Regex.Unescape(token.Content.Substring(1, token.Content.Length - 2));
                case TokenType.Char:
                    return Regex.Unescape(token.Content.Substring(1, token.Content.Length - 2))[0];
                case TokenType.Add:
                case TokenType.Subtract:
                case TokenType.Multiply:
                case TokenType.Divide:
                case TokenType.Power:
                case TokenType.And:
                case TokenType.Or:
                case TokenType.Not:
                case TokenType.Equals:
                case TokenType.Greater_Than_Or_Equals:
                case TokenType.Less_Than_Or_Equals:
                case TokenType.Greater_Than:
                case TokenType.Less_Than:
                case TokenType.Not_Equal_To:
                case TokenType.Assign:
                    return ParseBinaryOperator(token);
                case TokenType.Double:
                    return double.Parse(token.Content);
                case TokenType.Integer:
                    return int.Parse(token.Content);
                case TokenType.UnaryPlus:
                case TokenType.UnaryMinus:
                    return ParseUnaryOperator(token);
                case TokenType.Comma:
                    return Operator.Comma;
                case TokenType.Dot:
                    return Operator.Dot;
                case TokenType.LeftParenthesis:
                    return Operator.LeftParenthesis;
                case TokenType.RightParenthesis:
                    return Operator.RightParenthesis;
                case TokenType.LeftBracket:
                    return Operator.LeftBracket;
                case TokenType.RightBracket:
                    return Operator.RightBracket;
                case TokenType.LeftBrace:
                    return new FuncTuple(token.Content, 0);
                case TokenType.RightBrace:
                    return Operator.RightBrace;
                case TokenType.True:
                    return true;
                case TokenType.False:
                    return false;
                case TokenType.Repeat:
                    break;
                case TokenType.Until:
                    break;
                case TokenType.Return:
                    return ControlStatement.Return;
                case TokenType.Continue:
                    return ControlStatement.Continue;
                case TokenType.Do:
                    return ControlStatement.Do;
                case TokenType.End:
                    return ControlStatement.End;
                case TokenType.If:
                    return ControlStatement.If;
                case TokenType.Then:
                    return ControlStatement.Then;
                case TokenType.Else:
                    return ControlStatement.Else;
                case TokenType.ElseIf:
                    return ControlStatement.ElseIf;
                case TokenType.For:
                    return ControlStatement.For;
                case TokenType.While:
                    return ControlStatement.While;
                case TokenType.Break:
                    return ControlStatement.Break;
                case TokenType.EndOfLine:
                    return ControlStatement.Terminator;
                case TokenType.Call:
                    return new FuncTuple(token.Content, 0);
                case TokenType.In:
                    return ControlStatement.In;
                case TokenType.Pass:
                    return ControlStatement.Pass;
                case TokenType.Null:
                    return null;
                case TokenType.Void:
                    return "void";
                case TokenType.No_Arg:
                    return "No_Arg";
                case TokenType.Define:
                    return ControlStatement.Define;
                case TokenType.Begin:
                    return ControlStatement.Begin;
                case TokenType.Variable:
                    return token.Content;
                default:
                    throw new NotSupportedException(token.ToString());
            }
            throw new NotSupportedException();
        }
        private static Operator ParseUnaryOperator(Token token)
        {
            switch (token.tokenType)
            {
                case TokenType.UnaryPlus: return Operator.UnaryPlus;
                case TokenType.UnaryMinus: return Operator.UnaryMinus;
                case TokenType.Not: return Operator.Not;
                default: throw new Exception();
            }
        }
        private static Operator ParseBinaryOperator(Token token)
        {
            switch (token.tokenType)
            {
                case TokenType.Add: return Operator.Add;
                case TokenType.Subtract: return Operator.Subtract;
                case TokenType.Multiply: return Operator.Multiply;
                case TokenType.Divide: return Operator.Divide;
                case TokenType.Power: return Operator.Power;
                case TokenType.And: return Operator.And;
                case TokenType.Or: return Operator.Or;
                case TokenType.Not: return Operator.Not;

                case TokenType.Less_Than: return Operator.Less;
                case TokenType.Greater_Than: return Operator.Greater;
                case TokenType.Less_Than_Or_Equals: return Operator.LessOrEquals;
                case TokenType.Greater_Than_Or_Equals: return Operator.GreaterOrEquals;
                case TokenType.Equals: return Operator.Equals;
                case TokenType.Not_Equal_To: return Operator.NotEqualTo;

                case TokenType.Assign: return Operator.Assgin;
                default: throw new Exception();
            }
        }
        public override string ToString()
        {
            return string.Concat("(", Content, ", ", tokenType, ")");
        }
    }
}
