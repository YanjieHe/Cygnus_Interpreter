using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
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
            int n = list.Count;
            var array = new Lexeme[n];
            int i = 0;
            foreach (var item in list)
            {
                array[i] = new Lexeme(item.tokenType, GetContent(item));
                i++;
            }
            return array;
        }
        private static object GetContent(Token token)
        {
            switch (token.tokenType)
            {
                case TokenType.String:
                    // return Regex.Unescape(token.Content.Substring(1, token.Content.Length - 2));
                    return token.Content.Substring(1, token.Content.Length - 2);
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
                    break;

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
                    return Operator.Return;
                case TokenType.Do:
                    return ControlStmt.Do;
                case TokenType.End:
                    return ControlStmt.End;
                case TokenType.If:
                    return ControlStmt.If;
                case TokenType.Then:
                    return ControlStmt.Then;
                case TokenType.Else:
                    return ControlStmt.Else;
                case TokenType.ElseIf:
                    return ControlStmt.ElseIf;
                case TokenType.For:
                    return ControlStmt.For;
                case TokenType.While:
                    return ControlStmt.While;
                case TokenType.Break:
                    return ControlStmt.Break;
                case TokenType.EndOfLine:
                    return ControlStmt.Terminator;
                case TokenType.Function:
                    return new FuncTuple(token.Content, 0);
                case TokenType.In:
                    return ControlStmt.In;
                case TokenType.Null:
                    return null;
                case TokenType.Void:
                    return "void";
                case TokenType.Define:
                    return ControlStmt.Define;
                case TokenType.Begin:
                    return ControlStmt.Begin;
                case TokenType.Variable:
                    return token.Content;

                default:
                    throw new NotSupportedException();
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
    public enum Operator
    {
        UnaryPlus, UnaryMinus,
        Add, Subtract, Multiply, Divide, Power,
        And, Or, Not,
        Less, Greater, LessOrEquals, GreaterOrEquals,
        Equals, NotEqualTo,
        LeftParenthesis, RightParenthesis,
        LeftBrace, RightBrace,
        Function, Comma, Assgin,
        LeftBracket, RightBracket,
        Return
    }
    public enum ControlStmt
    {
        If, Then, Else, ElseIf, End, Break,
        While, Do,
        Terminator,
        Define, Begin,
        For, In,
    }
}
