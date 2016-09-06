using System;
using System.Collections.Generic;
using System.Linq;
using Cygnus.Extensions;
using Cygnus.DataStructures;
using Cygnus.LexicalAnalyzer;
namespace Cygnus.Expressions
{
    public abstract class Expression : IEquatable<Expression>, IDisplayable
    {
        public abstract ExpressionType NodeType { get; }
        public abstract Expression Eval(Scope scope);
        public static implicit operator Expression(int value)
        {
            return new ConstantExpression(value);
        }
        public static implicit operator Expression(double value)
        {
            return new ConstantExpression(value);
        }
        public static implicit operator Expression(string value)
        {
            return new ConstantExpression(value);
        }
        public static implicit operator Expression(bool value)
        {
            return new ConstantExpression(value);
        }
        public static implicit operator Expression(CygnusObject value)
        {
            return new ConstantExpression(value);
        }
        //public static implicit operator Expression(Vector<double> Data)
        //{
        //    return new VectorExpression(Data);
        //}
        //public static implicit operator Expression(Matrix<double> Data)
        //{
        //    return new MatrixExpression(Data);
        //}
        //public static ConstantExpression Constant(object obj, ConstantType constantType)
        //{
        //    return new ConstantExpression(obj, constantType);
        //}
        public static NewArrayExpression NewArray(params Expression[] array)
        {
            return new NewArrayExpression(array);
        }
        //public static TableExpression Table(params KeyValuePair<string, Expression>[] kvps)
        //{
        //    throw new NotImplementedException();
        //    //return new TableExpression(kvps);
        //}
        public static CallExpression Call(string Name, params Expression[] Arguments)
        {
            return new CallExpression(Name, Arguments);
        }
        public static ConstantExpression IEnumerable(IEnumerable<CygnusObject> Collection)
        {
            return new ConstantExpression(new CygnusIEnumerable(Collection));
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
            return new ConstantExpression(new CygnusVoid());
        }
        public static ConstantExpression Null()
        {
            throw new NotImplementedException();
            //return new ConstantExpression(null, ConstantType.Null);
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
            return new BinaryExpression(Operator.Assign, Left, Right);
        }
        public static BinaryExpression LessThan(Expression Left, Expression Right)
        {
            return new BinaryExpression(Operator.Less, Left, Right);
        }
        public static BinaryExpression GreaterThan(Expression Left, Expression Right)
        {
            return new BinaryExpression(Operator.Greater, Left, Right);
        }
        public static BinaryExpression LessOrEquals(Expression Left, Expression Right)
        {
            return new BinaryExpression(Operator.LessOrEquals, Left, Right);
        }
        public static BinaryExpression GreaterOrEquals(Expression Left, Expression Right)
        {
            return new BinaryExpression(Operator.GreaterOrEquals, Left, Right);
        }
        public static BinaryExpression Add(Expression Left, Expression Right)
        {
            return new BinaryExpression(Operator.Add, Left, Right);
        }
        public static BinaryExpression Subtract(Expression Left, Expression Right)
        {
            return new BinaryExpression(Operator.Subtract, Left, Right);
        }
        public static BinaryExpression Multiply(Expression Left, Expression Right)
        {
            return new BinaryExpression(Operator.Multiply, Left, Right);
        }
        public static BinaryExpression Divide(Expression Left, Expression Right)
        {
            return new BinaryExpression(Operator.Divide, Left, Right);
        }
        public static BinaryExpression Power(Expression Left, Expression Right)
        {
            return new BinaryExpression(Operator.Power, Left, Right);
        }
        public static UnaryExpression UnaryPlus(Expression Value)
        {
            return new UnaryExpression(Operator.UnaryPlus, Value);
        }
        public static UnaryExpression UnaryMinus(Expression Value)
        {
            return new UnaryExpression(Operator.UnaryMinus, Value);
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
            return Void();
        }
        public static BinaryExpression And(Expression Left, Expression Right)
        {
            return new BinaryExpression(Operator.And, Left, Right);
        }
        public static BinaryExpression Or(Expression Left, Expression Right)
        {
            return new BinaryExpression(Operator.Or, Left, Right);
        }
        public static UnaryExpression Not(Expression Value)
        {
            return new UnaryExpression(Operator.Not, Value);
        }
        public T As<T>(Scope scope) where T : struct
        {
            dynamic x = GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value;
            return (T)x;
        }
        public bool AsBool(Scope scope)
        {
            return (bool)GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value;
        }
        public string AsString(Scope scope)
        {
            return (string)GetValue<ConstantExpression>(ExpressionType.Constant, scope).Value;
        }
        public ConstantExpression AsConstant(Scope scope)
        {
            return GetValue<ConstantExpression>(ExpressionType.Constant, scope);
        }
        //public TableExpression AsTable(Scope scope)
        //{
        //    return GetValue<TableExpression>(ExpressionType.Table, scope);
        //}
        public CallExpression AsCall(Scope scope)
        {
            return GetValue<CallExpression>(ExpressionType.Call, scope);
        }
        //public MatrixExpression AsMatrix(Scope scope)
        //{
        //    return GetValue<ConstantExpression>(ExpressionType.Constant, scope) as MatrixExpression;
        //}
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
                case ExpressionType.NewArray:
                case ExpressionType.Dot:
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
                case ExpressionType.NewArray:
                case ExpressionType.Dot:
                    return Eval(scope).GetValue<T>(expressionType, scope);
                default:
                    throw new NotSupportedException(string.Format("expected {0} get {1}", expressionType, ToString()));
            }
        }
        public bool IsVoid(Scope scope)
        {
            //var value = GetValue(scope);
            //if (value is ConstantExpression)
            //    return (value as ConstantExpression).type == ConstantType.Void;
            //else return false;
            throw new Exception();
        }
        public IEnumerable<Expression> GetIEnumrableList(Scope scope)
        {
            var value = AsConstant(scope).Value;
            (value is IEnumerable<CygnusObject>).OrThrows<NotSupportedException>(NodeType.ToString());
            return (value as IEnumerable<CygnusObject>).Select(i => new ConstantExpression(i));
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
        Matrix, MatrixRow, Computable, Goto, Collection, NewArray,
        Member,Class,Dot,
    }
}
