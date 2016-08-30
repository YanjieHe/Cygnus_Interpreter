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
        public BlockExpression program { get; private set; }
        public ASTParser(Lexeme[] array, Scope scope)
        {
            this.array = array;
            this.scope = scope;
            program = new BlockExpression();
        }
        public ASTParser(Lexeme[] array, BlockExpression program)
        {
            this.array = array;
            this.program = program;
        }
        public void Parse(int start, int end)
        {
            ParseBlock(program, 0, array.Length - 1);
        }
        #region ParseBlock
        public void ParseBlock(BlockExpression Block, int start, int end)
        {
            int ExprStart = start;
            int ExprEnd = ExprStart;
            for (int i = start; i <= end; i++)
            {
                switch (array[i].tokenType)
                {
                    case TokenType.If:
                    case TokenType.ElseIf:
                        ParseLine(Block, ref ExprStart, ExprEnd);
                        ParseIf(Block, i, end, ref i);
                        ExprStart = i; ExprEnd = i;
                        break;
                    case TokenType.While:
                        ParseLine(Block, ref ExprStart, ExprEnd);
                        ParseWhile(Block, i, end, ref i);
                        ExprStart = i; ExprEnd = i;
                        break;
                    case TokenType.For:
                        ParseLine(Block, ref ExprStart, ExprEnd);
                        ParseForEach(Block, i, end, ref i);
                        ExprStart = i; ExprEnd = i;
                        break;
                    case TokenType.Define:
                        ParseLine(Block, ref ExprStart, ExprEnd);
                        ParseDefFunc(Block, i, end, ref i);
                        ExprStart = i; ExprEnd = i;
                        break;
                    case TokenType.EndOfLine:
                        ParseLine(Block, ref ExprStart, ExprEnd);
                        ExprStart++; ExprEnd++;
                        break;
                    case TokenType.Break:
                        ParseLine(Block, ref ExprStart, ExprEnd);
                        Block.Append(new BreakExpression());
                        break;
                    case TokenType.Continue:
                        ParseLine(Block, ref ExprStart, ExprEnd);
                        Block.Append(new ContinueExpression());
                        break;
                    case TokenType.Return:
                        ParseLine(Block, ref ExprStart, ExprEnd);
                        ParseReturn(Block, i, end, ref i);
                        ExprStart = i; ExprEnd = i;
                        break;
                    case TokenType.Pass:
                        ParseLine(Block, ref ExprStart, ExprEnd);
                        Block.Append(Expression.Void());
                        break;
                    default:
                        ExprEnd++;
                        break;
                }
            }
            ParseLine(Block, ref ExprStart, ExprEnd);
        }
        #endregion

        #region Parse If
        public void ParseIf(BlockExpression Block, int start, int end, ref int EndIndex)
        {
            int IF_Position = -1, Then_Position = -1, Else_Position = -1, End_Position = -1;
            var stack = new Stack<TokenType>();
            var Else_Stack = new Stack<int>();
            if (array[start].tokenType == TokenType.If || array[start].tokenType == TokenType.ElseIf)
            {
                IF_Position = start;
                stack.Push(TokenType.If);

                Then_Position = FindTokenType(IF_Position + 1, end, TokenType.Then);
                if (Then_Position < 0)
                    throw new SyntaxException("Missing 'then'");
                bool success = false;
                for (int i = Then_Position; i <= end; i++)
                {
                    switch (array[i].tokenType)
                    {
                        case TokenType.Do:
                        case TokenType.Begin:
                            stack.Push(array[i].tokenType);
                            break;
                        case TokenType.Else:
                        case TokenType.ElseIf:
                            Else_Stack.Push(i);
                            stack.Push(array[i].tokenType); break;
                        case TokenType.End:
                            var token = stack.Pop();
                            if (stack.Count == 0)
                            {
                                End_Position = i;
                                success = true;
                                break;
                            }
                            else if (stack.Count == 1 && token == TokenType.Else || token == TokenType.ElseIf)
                            {
                                End_Position = i;
                                success = true;
                                break;
                            }
                            if (token == TokenType.Else || token == TokenType.ElseIf)
                            {
                                Else_Stack.Pop();
                                stack.Pop();
                            }
                            break;
                    }
                    if (success) break;
                }
                if (!success) throw new SyntaxException("Missing 'end'");
                if (Else_Stack.Count == 0)
                {
                    var test = ParseExpression(Block, IF_Position + 1, Then_Position - 1);
                    var IfTrue = new BlockExpression(Block);
                    ParseBlock(IfTrue, Then_Position + 1, End_Position - 1);
                    Block.Append(new IfThenExpression(test, IfTrue));
                }
                else if (Else_Stack.Count == 1)
                {
                    Else_Position = Else_Stack.Pop();
                    var test = ParseExpression(Block, IF_Position + 1, Then_Position - 1);
                    var IfTrue = new BlockExpression(Block);
                    var IfFalse = new BlockExpression(Block);
                    ParseBlock(IfTrue, Then_Position + 1, Else_Position - 1);
                    if (array[Else_Position].tokenType == TokenType.ElseIf)
                        ParseBlock(IfFalse, Else_Position, End_Position);
                    else
                        ParseBlock(IfFalse, Else_Position + 1, End_Position - 1);
                    Block.Append(new IfThenElseExpression(test, IfTrue, IfFalse));
                }
                else throw new Exception();
                EndIndex = End_Position;
            }
            else throw new ArgumentException();
        }
        #endregion

        #region Parse While
        public void ParseWhile(BlockExpression Block, int start, int end, ref int EndIndex)
        {
            int While_Position = -1, Do_Position = -1, End_Position = -1;
            var stack = new Stack<TokenType>();
            if (array[start].tokenType == TokenType.While)
            {
                Do_Position = FindTokenType(While_Position + 1, end, TokenType.Do);

                if (Do_Position < 0)
                    throw new SyntaxException("Missing 'do'");
                FindEnd(Do_Position, ref End_Position, end, ref stack);

                var condition = ParseExpression(Block, While_Position + 1, Do_Position - 1);
                var body = new BlockExpression(Block);
                ParseBlock(body, Do_Position + 1, End_Position - 1);
                Block.Append(new WhileExpression(condition, body));

                EndIndex = End_Position;
            }
            else throw new ArgumentException();
        }
        #endregion

        #region Parse ForEach
        public void ParseForEach(BlockExpression Block, int start, int end, ref int EndIndex)
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

                Do_Position = FindTokenType(In_Position + 1, end, TokenType.Do);
                if (Do_Position < 0)
                    throw new SyntaxException("Missing 'do'");

                FindEnd(Do_Position, ref End_Position, end, ref stack);

                var Iter_List = ParseExpression(Block, In_Position + 1, Do_Position - 1);
                var body = new BlockExpression(Block);
                ParseBlock(body, Do_Position + 1, End_Position - 1);
                var Iter_Variable = new ParameterExpression(array[For_Position + 1].Content as string);
                Block.Append(new ForEachExpression(Iter_List, body, Iter_Variable));

                EndIndex = End_Position;
            }
            else throw new ArgumentException();
        }
        #endregion

        #region Parse Define Function
        public void ParseDefFunc(BlockExpression Block, int start, int end, ref int EndIndex)
        {
            int Def_Position = -1, Begin_Position = -1, End_Position = -1;
            var stack = new Stack<TokenType>();
            if (array[start].tokenType == TokenType.Define)
            {
                Def_Position = start;

                Begin_Position = FindTokenType(Def_Position + 1, end, TokenType.Begin);
                if (Begin_Position < 0)
                    throw new SyntaxException("Missing 'begin'");

                FindEnd(Begin_Position, ref End_Position, end, ref stack);
                var body = new BlockExpression(Block);
                var parameters = array.Slice(Def_Position + 1, Begin_Position - 1);
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
                ParseBlock(body, Begin_Position + 1, End_Position - 1);
                Block.Append(Expression.Void());
                EndIndex = End_Position;
            }
            else throw new ArgumentException();
        }
        #endregion

        #region Parse Expression
        public void ParseLine(BlockExpression Block, ref int start, int end)
        {
            if (start != end)
            {
                Block.Append(ParseExpression(Block, start, end));
                start = end;
            }
        }
        public Expression ParseExpression(BlockExpression Block, int start, int end)
        {
            CountArguments(start, end);
            return ParseReversePolishNotation(ConvertToRPN(array.Slice(start, end)));
        }
        private IEnumerable<Lexeme> ConvertToRPN(IEnumerable<Lexeme> sequence)
        {
            return new RPN(sequence).Analyze().Operands;
        }
        private Expression ParseReversePolishNotation(IEnumerable<Lexeme> ReversePolishNotation)
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
                    case TokenType.Null:
                        stack.Push(new ConstantExpression(null, ConstantType.Null));
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
                                arguments[i] = stack.Pop();
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
                        else throw new ArgumentException();
                        break;
                }
            }
        }

        #endregion

        #region Parse Return 
        public void ParseReturn(BlockExpression Block, int start, int end, ref int EndIndex)
        {
            int Return_Position = -1, End_Position = -1;
            if (array[start].tokenType == TokenType.Return)
            {
                Return_Position = start;
                for (int i = start + 1; i <= end; i++)
                {
                    if (IsTerminator(array[i].tokenType))
                    {
                        End_Position = i;
                        break;
                    }
                }
                if (End_Position < 0)
                    End_Position = end;
                var expression = ParseExpression(Block, Return_Position + 1, End_Position);
                Block.Append(new ReturnExpression(expression));
                EndIndex = End_Position;
            }
            else throw new ArgumentException();
        }
        #endregion

        public void FindEnd(int StartPosition, ref int EndPosition, int end, ref Stack<TokenType> stack)
        {
            bool success = false;
            for (int i = StartPosition; i <= end; i++)
            {
                switch (array[i].tokenType)
                {
                    case TokenType.If:
                    case TokenType.Do:
                    case TokenType.Begin:
                        stack.Push(array[i].tokenType);
                        break;
                    case TokenType.End:
                        var token = stack.Pop();
                        if (stack.Count == 0)
                        {
                            EndPosition = i;
                            success = true;
                            break;
                        }
                        break;
                }
                if (success) break;
            }
            if (!success) throw new SyntaxException("Missing 'end'");
        }
        public int FindTokenType(int start, int end, TokenType tokenType)
        {
            for (int i = start; i <= end; i++)
                if (array[i].tokenType == tokenType)
                    return i;
            return (-1);
        }
        public static bool IsTerminator(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.Define:
                case TokenType.Begin:
                case TokenType.Repeat:
                case TokenType.Until:
                case TokenType.Return:
                case TokenType.Do:
                case TokenType.End:
                case TokenType.If:
                case TokenType.Then:
                case TokenType.Else:
                case TokenType.ElseIf:
                case TokenType.For:
                case TokenType.While:
                case TokenType.Break:
                case TokenType.In:
                case TokenType.EndOfLine:
                    return true;
                default:
                    return false;
            }
        }
    }
}
