using System;
using System.Collections.Generic;
using System.Linq;
using Cygnus.LexicalAnalyzer;
using Cygnus.Errors;
using Cygnus.SyntaxTree;
using Cygnus.Extensions;
namespace Cygnus.SyntaxAnalyzer
{
    public class ASTParser
    {
        Lexeme[] array;
        public Scope scope;
        public Lexeme Current;
        public int index;
        public BlockExpression program { get; private set; }
        public ASTParser(Lexeme[] array, Scope scope)
        {
            this.array = array;
            this.scope = scope;
            program = new BlockExpression();
            Init();
        }
        public ASTParser(Lexeme[] array, BlockExpression program)
        {
            this.array = array;
            this.program = program;
            Init();
        }
        public void Init()
        {
            this.index = 0;
            Current = array[index];
        }
        public BlockExpression Parse()
        {
            program.Append(ParseBlock(program, i => false));
            return program;
        }
        public void MoveNext()
        {
            index++;
            Current = array[index];
        }
        public void PutBack()
        {
            index--;
            Current = array[index];
        }
        public bool CanMove()
        {
            return index != array.Length - 1;
        }
        public Expression ParseBlock(BlockExpression Parent, Predicate<TokenType> Stop)
        {
            var Block = new BlockExpression(Parent);
            while (index < array.Length - 1 && !Stop(Current.tokenType))
            {
                //    Console.WriteLine(Current);
                switch (Current.tokenType)
                {
                    case TokenType.Define:
                        Block.Append(ParseDefFunc(Block));
                        break;
                    case TokenType.Repeat:
                        break;
                    case TokenType.Until:
                        break;
                    case TokenType.Return:
                        Block.Append(ParseReturn());
                        break;
                    case TokenType.If:
                    case TokenType.ElseIf:
                        Block.Append(ParseIf(Block));
                        break;
                    case TokenType.For:
                        Block.Append(ParseForEach(Block));
                        break;
                    case TokenType.While:
                        Block.Append(ParseWhile(Block));
                        break;
                    case TokenType.Try:
                        break;
                    case TokenType.Catch:
                        break;
                    case TokenType.Finally:
                        break;
                    case TokenType.EndOfLine:
                        if (CanMove()) MoveNext();
                        else return Block;
                        break;
                    case TokenType.Comments:
                        break;
                    default:
                        Block.Append(ParseExpression());
                        if (CanMove()) MoveNext();
                        else return Block;
                        break;
                }
            }
            return Block;
        }

        public Expression ParseIf(BlockExpression Parent)
        {
            if (Current.tokenType == TokenType.If || Current.tokenType == TokenType.ElseIf)
            {
                MoveNext();
                Expression Test = ParseExpression();
                while (Current.tokenType == TokenType.EndOfLine)
                    MoveNext();
                (Current.tokenType == TokenType.Then).OrThrows<SyntaxException>("Expecting 'Then'");
                MoveNext();
                Expression IfTrue = ParseBlock(Parent, i => i.In(TokenType.Else, TokenType.ElseIf, TokenType.End));
                if (Current.tokenType == TokenType.End)
                {
                    MoveNext();
                    return Expression.IfThen(Test, IfTrue);
                }
                else if (Current.tokenType == TokenType.Else)
                {
                    MoveNext();
                    Expression IfFalse = ParseBlock(Parent, i => i == TokenType.End);
                    MoveNext();
                    return Expression.IfThenElse(Test, IfTrue, IfFalse);
                }
                else if (Current.tokenType == TokenType.ElseIf)
                {
                    Expression IfFalse = ParseBlock(Parent, i => i == TokenType.End);
                    MoveNext();
                    return Expression.IfThenElse(Test, IfTrue, IfFalse);
                }
            }
            throw new Exception();
        }
        public Expression ParseWhile(BlockExpression Parent)
        {
            if (Current.tokenType == TokenType.While)
            {
                MoveNext();
                Expression Condition = ParseExpression();
                while (Current.tokenType == TokenType.EndOfLine)
                    MoveNext();
                (Current.tokenType == TokenType.Do).OrThrows<SyntaxException>("Expecting 'Do'");
                MoveNext();
                Expression Body = ParseBlock(Parent, i => i == TokenType.End);
                MoveNext();
                return Expression.While(Condition, Body);
            }
            throw new Exception();
        }
        public Expression ParseForEach(BlockExpression Parent)
        {
            if (Current.tokenType == TokenType.For)
            {
                MoveNext();
                (Current.tokenType == TokenType.Variable).OrThrows<SyntaxException>("Expecting Iteration Variable");
                var Iter_Variable = Expression.Variable(Current.Content as string);
                MoveNext();
                (Current.tokenType == TokenType.In).OrThrows<SyntaxException>("Expecting 'In'");
                MoveNext();
                Expression Collection = ParseExpression();
                while (Current.tokenType == TokenType.EndOfLine)
                    MoveNext();
                (Current.tokenType == TokenType.Do).OrThrows<SyntaxException>("Expecting 'Do'");
                MoveNext();
                Expression Body = ParseBlock(Parent, i => i == TokenType.End);
                MoveNext();
                return Expression.ForEach(Iter_Variable, Collection, Body);
            }
            throw new Exception();
        }
        public Expression ParseDefFunc(BlockExpression Parent)
        {
            if (Current.tokenType == TokenType.Define)
            {
                MoveNext();
                (Current.tokenType == TokenType.Call).OrThrows<SyntaxException>("Expecting function name");
                var Name = Current.Content as string;
                MoveNext();
                var argslist = new List<ParameterExpression>();
                while (Current.tokenType != TokenType.Begin)
                {
                    if (Current.tokenType == TokenType.Variable)
                        argslist.Add(Expression.Parameter(Current.Content as string));
                    MoveNext();
                }
                MoveNext();
                Expression Body = ParseBlock(Parent, i => i == TokenType.End);
                MoveNext();
                var FUNCTION = Expression.Function(Name, Body, new Scope(scope), argslist.ToArray());
                Scope.functionTable[Name] = FUNCTION;
                return Expression.Void();
            }
            throw new Exception();
        }
        public Expression ParseReturn()
        {
            if (Current.tokenType == TokenType.Return)
            {
                MoveNext();
                return Expression.Return(ParseExpression());
            }
            throw new Exception();
        }
        public Expression ParseExpression()
        {
            Expression val = ParseAssign();
            return val;
        }
        public Expression ParseAssign()
        {
            Expression value = null;
            value = ParseOr();
            if (Current.tokenType == TokenType.Assign)
            {
                MoveNext();
                value = Expression.Assign(value, ParseExpression());
            }
            return value;
        }
        public Expression ParseOr()
        {
            Expression value;
            value = ParseAnd();
            while (Current.tokenType == TokenType.Or)
            {
                MoveNext();
                value = Expression.Or(value, ParseAnd());
            }
            return value;
        }
        public Expression ParseAnd()
        {
            Expression value;
            value = ParseEquals();
            while (Current.tokenType == TokenType.And)
            {
                MoveNext();
                value = Expression.And(value, ParseEquals());
            }
            return value;
        }
        public Expression ParseEquals()
        {
            Expression value;

            value = ParseCompare();

            while (Current.tokenType.In(TokenType.Equal, TokenType.NotEqual))
            {
                TokenType op = Current.tokenType;
                MoveNext();
                if (op == TokenType.Equal)
                    value = new BinaryExpression(Operator.Equal, value, ParseCompare());
                else if (op == TokenType.NotEqual)
                    value = new BinaryExpression(Operator.NotEqual, value, ParseCompare());
            }
            return value;
        }
        public Expression ParseCompare()
        {
            Expression value;

            value = Parse_Add_Subtract();

            while (Current.tokenType.IsCompareOp())
            {
                TokenType op = Current.tokenType;
                MoveNext();

                switch (op)
                {
                    case TokenType.Less:
                        value = Expression.LessThan(value, Parse_Add_Subtract()); break;
                    case TokenType.Greater:
                        value = Expression.GreaterThan(value, Parse_Add_Subtract()); break;
                    case TokenType.LessOrEquals:
                        value = Expression.LessOrEquals(value, Parse_Add_Subtract()); break;
                    case TokenType.GreaterOrEquals:
                        value = Expression.GreaterOrEquals(value, Parse_Add_Subtract()); break;
                    default:
                        break;
                }
            }
            return value;
        }
        public Expression Parse_Add_Subtract()
        {
            Expression value;
            value = Parse_Mul_Div();
            while (Current.tokenType.In(TokenType.Add, TokenType.Subtract))
            {
                TokenType op = Current.tokenType;
                MoveNext();


                if (op == TokenType.Add)
                    value = Expression.Add(value, Parse_Mul_Div());
                else if (op == TokenType.Subtract)
                    value = Expression.Subtract(value, Parse_Mul_Div());
            }
            return value;
        }
        public Expression Parse_Mul_Div()
        {
            Expression value;

            value = ParsePower();
            while (Current.tokenType.In(TokenType.Multiply, TokenType.Divide))
            {
                TokenType op = Current.tokenType;
                MoveNext();

                if (op == TokenType.Multiply)
                    value = Expression.Multiply(value, ParsePower());
                else if (op == TokenType.Divide)
                    value = Expression.Divide(value, ParsePower());
            }
            return value;
        }
        public Expression ParsePower()
        {
            Expression value;

            value = ParseUnary();
            while (Current.tokenType == TokenType.Power)
            {
                TokenType op = Current.tokenType;
                MoveNext();

                value = Expression.Power(value, ParseUnary());
            }
            return value;
        }
        public Expression ParseUnary()
        {
            Expression value = null;
            while (Current.tokenType.In(TokenType.Add, TokenType.Subtract, TokenType.Not))
            {
                TokenType op = Current.tokenType;
                MoveNext();

                if (op == TokenType.Add)
                    value = Expression.UnaryPlus(ParseExpression());
                else if (op == TokenType.Subtract)
                    value = Expression.UnaryMinus(ParseExpression());
                else if (op == TokenType.Not)
                    value = Expression.Not(ParseExpression());
            }
            return value ?? ParseIndex();
        }
        public Expression ParseIndex()
        {
            Expression value = ParseCall();

            while (Current.tokenType.In(TokenType.Dot, TokenType.LeftBracket))
            {
                TokenType op = Current.tokenType;
                MoveNext();

                if (op == TokenType.Dot)
                {
                    value = Expression.Property(value, Current.Content as string);
                    MoveNext();
                }
                else if (op == TokenType.LeftBracket)
                {
                    value = Expression.MakeIndex(value, ParseExpression());
                    if (Current.tokenType == TokenType.RightBracket)
                    {
                        MoveNext();
                    }
                    else
                    {
                        throw new Exception("[]括号不匹配\n");
                    }
                }
            }
            return value;
        }
        public Expression ParseCall()
        {
            Expression value = null;
            if (Current.tokenType == TokenType.Call)
            {
                var Name = Current.Content as string;
                MoveNext();
                List<Expression> argsList = new List<Expression>();
                while (Current.tokenType != TokenType.RightParenthesis)
                {
                    argsList.Add(ParseExpression());
                    if (Current.tokenType == TokenType.Comma)
                    {
                        MoveNext();
                    }
                }
                MoveNext();
                value = Expression.Call(Name, argsList.ToArray());
            }
            return value ?? ParseInitTable();
        }
        public Expression ParseInitTable()
        {
            Expression value = null;
            if (Current.tokenType == TokenType.LeftBrace)
            {
                var name = Current.Content;
                MoveNext();
                List<Expression> argsList = new List<Expression>();
                while (Current.tokenType != TokenType.RightBrace)
                {
                    argsList.Add(ParseExpression());
                    if (Current.tokenType == TokenType.Comma)
                    {
                        MoveNext();
                    }
                }
                MoveNext();
                value = new TableExpression(argsList.ToArray());
            }
            return value ?? ParseFactor();
        }
        public Expression ParseFactor()
        {
            Expression value;
            if (Current.tokenType == TokenType.LeftParenthesis)
            {
                MoveNext();
                value = ParseExpression();
                (Current.tokenType == TokenType.RightParenthesis).OrThrows<SyntaxException>("Mismatch '(' ')'");
                goto Finish;
            }
            else
            {
                switch (Current.tokenType)
                {
                    case TokenType.Integer:
                        value = (int)Current.Content; goto Finish;
                    case TokenType.Double:
                        value = (double)Current.Content; goto Finish;
                    case TokenType.True:
                        value = true; goto Finish;
                    case TokenType.False:
                        value = false; goto Finish;
                    case TokenType.Null:
                        value = Expression.Null(); goto Finish;
                    case TokenType.Void:
                        value = Expression.Void(); goto Finish;
                    case TokenType.Break:
                        value = Expression.Break(); goto Finish;
                    case TokenType.Continue:
                        value = Expression.Continue(); goto Finish;
                    case TokenType.Pass:
                        value = Expression.Pass(); goto Finish;
                    case TokenType.String:
                        value = Current.Content as string; goto Finish;
                    case TokenType.Variable:
                        value = Expression.Variable(Current.Content as string); goto Finish;
                    default:
                        // 既不是数字也不是'(', 也不是标识符
                        throw new Exception("unkown charactor: \n" + Current);
                }
            }
        Finish:
            MoveNext();
            return value;
        }
    }
}
