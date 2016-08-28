using System;
using System.Collections.Generic;
using System.Linq;
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
        public static implicit operator Expression(Matrix<double> Data)
        {
            return new MatrixExpression(Data);
        }
        public static ConstantExpression Constant(object obj, ConstantType constantType)
        {
            return new ConstantExpression(obj, constantType);
        }
        public static ConstantExpression Constant(object obj)
        {
            return new ConstantExpression(obj);
        }
        public static ArrayExpression Array(Expression[] array)
        {
            return new ArrayExpression(array);
        }
        public static ListExpression List(List<Expression> list)
        {
            return new ListExpression(list);
        }
        public static DictionaryExpression Dictionary(Dictionary<ConstantExpression, Expression> dict)
        {
            return new DictionaryExpression(dict);
        }
        public static TableExpression Table(KeyValuePair<string, Expression> kvps)
        {
            return new TableExpression(kvps);
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
        public ArrayExpression AsArray(Scope scope)
        {
            return GetValue<ArrayExpression>(ExpressionType.Array, scope);
        }
        public ListExpression AsList(Scope scope)
        {
            return GetValue<ListExpression>(ExpressionType.List, scope);
        }
        public DictionaryExpression AsDictionary(Scope scope)
        {
            return GetValue<DictionaryExpression>(ExpressionType.Dictionary, scope);
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
            return GetValue<MatrixExpression>(ExpressionType.Matrix, scope);
        }
        public Expression GetValue(Scope scope)
        {
            switch (NodeType)
            {
                case ExpressionType.Return:
                    return (this as ReturnExpression).expression.Eval(scope).GetValue(scope);
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
                    return (this as ReturnExpression).expression.Eval(scope).GetValue<T>(expressionType, scope);
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
            switch (NodeType)
            {
                case ExpressionType.Constant:
                    return (this as ConstantExpression).constantType == ConstantType.Void;
                case ExpressionType.Return:
                    return (this as ReturnExpression).expression.IsVoid(scope);
                case ExpressionType.Parameter:
                case ExpressionType.Block:
                case ExpressionType.Binary:
                case ExpressionType.Unary:
                case ExpressionType.Call:
                    return Eval(scope).IsVoid(scope);
                default:
                    return false;
            }
        }
        public IEnumerable<Expression> GetIEnumrableList(Scope scope)
        {
            switch (NodeType)
            {
                case ExpressionType.Array:
                case ExpressionType.List:
                case ExpressionType.Dictionary:
                case ExpressionType.IEnumerable:
                case ExpressionType.Constant:
                    return this as IEnumerable<Expression>;
                case ExpressionType.Return:
                    return (this as ReturnExpression).expression.Eval(scope).GetIEnumrableList(scope);
                case ExpressionType.Parameter:
                case ExpressionType.Call:
                    return Eval(scope).GetIEnumrableList(scope);
                default:
                    throw new NotSupportedException(NodeType.ToString());
            }
        }
        public virtual void Display()
        {
            Console.Write(ToString());
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
                    case ExpressionType.Array:
                        return (this as ArrayExpression).Equals(other as ArrayExpression);
                    case ExpressionType.List:
                        return (this as ListExpression).Equals(other as ListExpression);
                    case ExpressionType.Dictionary:
                        return (this as DictionaryExpression).Equals(other as DictionaryExpression);
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
        Constant, Block, Unary, Binary,
        IfThen, IfThenElse, While, Break,
        Parameter, Function, Array, List, Dictionary,
        Index, Return, ForEach, IEnumerable, Call,
        Table, IList, KeyValuePair, Continue, CSharpObject,
        Matrix, MatrixRow,
    }
}
