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
                case TokenType.Double:
                    return double.Parse(token.Content);
                case TokenType.Integer:
                    return int.Parse(token.Content);
                case TokenType.True: return true;
                case TokenType.False: return false;
                case TokenType.Variable: return token.Content;
                /* Binary Operators */
                case TokenType.Add:
                case TokenType.Subtract:
                case TokenType.Multiply:
                case TokenType.Divide:
                case TokenType.Power:
                case TokenType.And:
                case TokenType.Or:
                case TokenType.Not:
                case TokenType.Equal:
                case TokenType.GreaterOrEquals:
                case TokenType.LessOrEquals:
                case TokenType.Greater:
                case TokenType.Less:
                case TokenType.NotEqual:
                case TokenType.Assign:
                    return ParseBinaryOperator(token);
                /* Unary Operators */
                case TokenType.UnaryPlus:
                case TokenType.UnaryMinus:
                    return ParseUnaryOperator(token);

                case TokenType.Comma:
                case TokenType.Dot:
                case TokenType.LeftParenthesis:
                case TokenType.RightParenthesis:
                case TokenType.LeftBracket:
                case TokenType.RightBracket:
                case TokenType.LeftBrace:
                case TokenType.RightBrace:
                case TokenType.Repeat:
                case TokenType.Until:
                case TokenType.Return:
                case TokenType.Continue:
                case TokenType.Do:
                case TokenType.End:
                case TokenType.If:
                case TokenType.Then:
                case TokenType.Else:
                case TokenType.ElseIf:
                case TokenType.For:
                case TokenType.While:
                case TokenType.Break:
                case TokenType.EndOfLine:
                case TokenType.In:
                case TokenType.Pass:
                case TokenType.Define:
                case TokenType.Begin:
                case TokenType.Null:
                case TokenType.Void: return null;
                case TokenType.Call: return token.Content;
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

                case TokenType.Less: return Operator.Less;
                case TokenType.Greater: return Operator.Greater;
                case TokenType.LessOrEquals: return Operator.LessOrEquals;
                case TokenType.GreaterOrEquals: return Operator.GreaterOrEquals;
                case TokenType.Equal: return Operator.Equal;
                case TokenType.NotEqual: return Operator.NotEqual;

                case TokenType.Assign: return Operator.Assign;
                default: throw new Exception();
            }
        }
        public override string ToString()
        {
            return string.Concat("(", Content, ", ", tokenType, ")");
        }
    }
}
