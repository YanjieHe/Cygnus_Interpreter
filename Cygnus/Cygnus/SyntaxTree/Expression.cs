using System;
using System.Collections.Generic;
using System.Linq;
using Cygnus.Extensions;
using MathNet.Numerics.LinearAlgebra;
namespace Cygnus.SyntaxTree
{
    public abstract class Expression : IEquatable<Expression>
    {
        public abstract ExpressionType NodeType { get; }
        public abstract Expression Eval(Scope scope);
        public static implicit operator Expression(int value)
        {
            return new ConstantExpression(value, ConstantType.Integer);
        }
        public static implicit operator Expression(double value)
        {
            return new ConstantExpression(value, ConstantType.Double);
        }
        public static implicit operator Expression(string value)
        {
            return new ConstantExpression(value, ConstantType.String);
        }
        public static implicit operator Expression(bool value)
        {
            return new ConstantExpression(value, ConstantType.Boolean);
        }
        public static implicit operator Expression(Vector<double> Data)
        {
            return new VectorExpression(Data);
        }
        public static implicit operator Expression(Matrix<double> Data)
        {
            return new MatrixExpression(Data);
        }
        public static ConstantExpression Constant(object obj, ConstantType constantType)
        {
            return new ConstantExpression(obj, constantType);
        }
        public static TableExpression Table(params KeyValuePair<string, Expression>[] kvps)
        {
            throw new NotImplementedException();
            //return new TableExpression(kvps);
        }
        public static CallExpression Call(string Name, params Expression[] Arguments)
        {
            return new CallExpression(Name, Arguments);
        }
        public static IEnumerableExpression IEnumerable(IEnumerable<Expression> list)
        {
            return new IEnumerableExpression(list);
        }
        public static CSharpObjectExpression CSharpObject(object obj, Type type)
        {
            return new CSharpObjectExpression(obj, type);
        }
        public static CSharpObjectExpression CSharpObject(object obj)
        {
            return new CSharpObjectExpression(obj);
        }
        public static ConstantExpression Void()
        {
            return new ConstantExpression(null, ConstantType.Void);
        }
        public static ConstantExpression Null()
        {
            return new ConstantExpression(null, ConstantType.Null);
        }
        public static ParameterExpression Variable(string Name)
        {
            return new ParameterExpression(Name);
        }
        public static ParameterExpression Parameter(string Name)
        {
            return new ParameterExpression(Name);
        }
        public static GotoExpression Return(Expression expression)
        {
            return new GotoExpression(GotoExpressionKind.Return, expression);
        }
        public static FunctionExpression Function(string Name, Expression Body, Scope scope, ParameterExpression[] arguments)
        {
            return new FunctionExpression(Name, Body, scope, arguments);
        }
        public static BinaryExpression Assign(Expression Left, Expression Right)
        {
            return new BinaryExpression(LexicalAnalyzer.Operator.Assign, Left, Right);
        }
        public static BinaryExpression LessThan(Expression Left, Expression Right)
        {
            return new BinaryExpression(LexicalAnalyzer.Operator.Less, Left, Right);
        }
        public static BinaryExpression GreaterThan(Expression Left, Expression Right)
        {
            return new BinaryExpression(LexicalAnalyzer.Operator.Greater, Left, Right);
        }
        public static BinaryExpression LessOrEquals(Expression Left, Expression Right)
        {
            return new BinaryExpression(LexicalAnalyzer.Operator.LessOrEquals, Left, Right);
        }
        public static BinaryExpression GreaterOrEquals(Expression Left, Expression Right)
        {
            return new BinaryExpression(LexicalAnalyzer.Operator.GreaterOrEquals, Left, Right);
        }
        public static BinaryExpression Add(Expression Left, Expression Right)
        {
            return new BinaryExpression(LexicalAnalyzer.Operator.Add, Left, Right);
        }
        public static BinaryExpression Subtract(Expression Left, Expression Right)
        {
            return new BinaryExpression(LexicalAnalyzer.Operator.Subtract, Left, Right);
        }
        public static BinaryExpression Multiply(Expression Left, Expression Right)
        {
            return new BinaryExpression(LexicalAnalyzer.Operator.Multiply, Left, Right);
        }
        public static BinaryExpression Divide(Expression Left, Expression Right)
        {
            return new BinaryExpression(LexicalAnalyzer.Operator.Divide, Left, Right);
        }
        public static BinaryExpression Power(Expression Left, Expression Right)
        {
            return new BinaryExpression(LexicalAnalyzer.Operator.Power, Left, Right);
        }
        public static UnaryExpression UnaryPlus(Expression Value)
        {
            return new UnaryExpression(LexicalAnalyzer.Operator.UnaryPlus, Value);
        }
        public static UnaryExpression UnaryMinus(Expression Value)
        {
            return new UnaryExpression(LexicalAnalyzer.Operator.UnaryMinus, Value);
        }
        public static IndexExpression Property(Expression Member, string property)
        {
            return new IndexExpression(Member, new ParameterExpression(property), IndexType.Dot);
        }
        public static IndexExpression MakeIndex(Expression array, Expression index)
        {
            return new IndexExpression(array, index, IndexType.Bracket);
        }
        public static IfThenExpression IfThen(Expression Test, Expression IfTrue)
        {
            return new IfThenExpression(Test, IfTrue);
        }
        public static IfThenElseExpression IfThenElse(Expression Test, Expression IfTrue, Expression IfFalse)
        {
            return new IfThenElseExpression(Test, IfTrue, IfFalse);
        }
        public static GotoExpression Break()
        {
            return new GotoExpression(GotoExpressionKind.Break);
        }
        public static WhileExpression While(Expression Condition, Expression Body)
        {
            return new WhileExpression(Condition, Body);
        }
        public static ForEachExpression ForEach(ParameterExpression Item, Expression Collection, Expression Body)
        {
            return new ForEachExpression(Item, Collection, Body);
        }
        public static GotoExpression Continue()
        {
            return new GotoExpression(GotoExpressionKind.Continue);
        }
        public static ConstantExpression Pass()
        {
            return Expression.Void();
        }
        public static BinaryExpression And(Expression Left, Expression Right)
        {
            return new BinaryExpression(LexicalAnalyzer.Operator.And, Left, Right);
        }
        public static BinaryExpression Or(Expression Left, Expression Right)
        {
            return new BinaryExpression(LexicalAnalyzer.Operator.Or, Left, Right);
        }
        public static UnaryExpression Not(Expression Value)
        {
            return new UnaryExpression(LexicalAnalyzer.Operator.Not, Value);
        }
        public T As<T>(Scope scope) where T : struct
        {
            return (T)GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value;
        }
        public string AsString(Scope scope)
        {
            return GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value as string;
        }
        public ConstantExpression AsConstant(Scope scope)
        {
            return GetValue<ConstantExpression>(ExpressionType.Constant, scope);
        }
        public TableExpression AsTable(Scope scope)
        {
            return GetValue<TableExpression>(ExpressionType.Table, scope);
        }
        public CallExpression AsCall(Scope scope)
        {
            return GetValue<CallExpression>(ExpressionType.Call, scope);
        }
        public MatrixExpression AsMatrix(Scope scope)
        {
            return GetValue<ConstantExpression>(ExpressionType.Constant, scope) as MatrixExpression;
        }
        public Expression GetValue(Scope scope)
        {
            switch (NodeType)
            {
                case ExpressionType.Return:
                    return (this as GotoExpression).Value.Eval(scope).GetValue(scope);
                case ExpressionType.Call:
                case ExpressionType.Parameter:
                case ExpressionType.Block:
                case ExpressionType.Binary:
                case ExpressionType.Unary:
                case ExpressionType.Index:
                    return Eval(scope).GetValue(scope);
                default:
                    return this;
            }
        }
        public T GetValue<T>(ExpressionType expressionType, Scope scope) where T : Expression
        {
            if (NodeType == expressionType) return this as T;
            switch (NodeType)
            {
                case ExpressionType.Return:
                    return (this as GotoExpression).Value.Eval(scope).GetValue<T>(expressionType, scope);
                case ExpressionType.Parameter:
                case ExpressionType.Call:
                case ExpressionType.Block:
                case ExpressionType.Binary:
                case ExpressionType.Unary:
                case ExpressionType.Index:
                    return Eval(scope).GetValue<T>(expressionType, scope);
                default:
                    throw new NotSupportedException(string.Format("expected {0} get {1}", expressionType, ToString()));
            }
        }
        public bool IsVoid(Scope scope)
        {
            var value = GetValue(scope);
            if (value is ConstantExpression)
                return (value as ConstantExpression).type == ConstantType.Void;
            else return false;
        }
        public IEnumerable<Expression> GetIEnumrableList(Scope scope)
        {
            var value = GetValue(scope);
            (value is IEnumerable<Expression>).OrThrows<NotSupportedException>(NodeType.ToString());
            return value as IEnumerable<Expression>;
        }
        public virtual void Display(Scope scope)
        {
            Console.Write(Eval(scope).ToString());
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals(obj as Expression);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public bool Equals(Expression other)
        {
            if (NodeType != other.NodeType) return false;
            else
            {
                switch (NodeType)
                {
                    case ExpressionType.Constant:
                        return (this as ConstantExpression).Equals(other as ConstantExpression);
                    case ExpressionType.IEnumerable:
                        return (this as IEnumerableExpression).Equals(other as IEnumerableExpression);
                    default:
                        throw new NotSupportedException("'equals' method has not been defined between '" + this.ToString() + "' and '" + other.ToString() + "'");
                }
            }
        }
    }
    public enum ExpressionType
    {
        Constant = 1,
        Block, Unary, Binary,
        IfThen, IfThenElse, While, Break,
        Parameter, Function, Array, List, Dictionary,
        Index, Return, ForEach, IEnumerable, Call,
        Table, IList, KeyValuePair, Continue, CSharpObject,
        Matrix, MatrixRow, Computable, Goto,Collection,
    }
}
