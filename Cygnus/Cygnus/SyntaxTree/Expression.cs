using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SymbolTable;
namespace Cygnus.SyntaxTree
{
    public abstract class Expression : IEquatable<Expression>
    {
        public abstract ExpressionType NodeType { get; }
        public abstract Expression Eval(Scope scope);
        public static ConstantExpression Constant(object obj, ConstantType constantType)
        {
            return new ConstantExpression(obj, constantType);
        }
        public static IEnumerableExpression IEnumerable(IEnumerable<Expression> list)
        {
            return new IEnumerableExpression(list);
        }
        public static ConstantExpression Void()
        {
            return new ConstantExpression(null, ConstantType.Void);
        }
        public static ConstantExpression Null()
        {
            return new ConstantExpression(null, ConstantType.Null);
        }
        public Expression GetValue(Scope scope)
        {
            switch (NodeType)
            {
                case ExpressionType.Parameter:
                    return (this as ParameterExpression).Eval(scope).GetValue(scope);
                case ExpressionType.Return:
                    return (this as ReturnExpression).expression.Eval(scope).GetValue(scope);
                case ExpressionType.Index:
                    return (this as IndexExpression).Eval(scope).GetValue(scope);
                case ExpressionType.Default:
                case ExpressionType.Block:
                case ExpressionType.Binary:
                case ExpressionType.Unary:
                    return this.Eval(scope).GetValue(scope);
                default:
                    return this;
            }
        }
        public T GetValue<T>(ExpressionType expressionType, Scope scope) where T : Expression
        {
            if (NodeType == expressionType) return this as T;
            switch (NodeType)
            {
                case ExpressionType.Call:
                    return this.Eval(scope).GetValue<T>(expressionType, scope);
                case ExpressionType.Parameter:
                    return (this as ParameterExpression).Eval(scope).GetValue<T>(expressionType, scope);
                case ExpressionType.Return:
                    return (this as ReturnExpression).expression.Eval(scope).GetValue<T>(expressionType, scope);
                case ExpressionType.Default:
                case ExpressionType.Block:
                case ExpressionType.Binary:
                case ExpressionType.Unary:
                    return Eval(scope).GetValue<T>(expressionType, scope);
                default:
                    throw new NotSupportedException(ToString());
            }
        }
        public object GetObjectValue(Scope scope)
        {
            switch (NodeType)
            {
                case ExpressionType.Constant:
                    return (this as ConstantExpression).Value;
                case ExpressionType.Parameter:
                    return (this as ParameterExpression).Eval(scope).GetObjectValue(scope);
                case ExpressionType.Binary:
                case ExpressionType.Unary:
                    return this.Eval(scope).GetObjectValue(scope);
                case ExpressionType.Index:
                    return (this as IndexExpression).Eval(scope).GetObjectValue(scope);
                case ExpressionType.Array:
                    return (this as ArrayExpression).Values.ToArray();
                case ExpressionType.List:
                    return (this as ListExpression).Values.ToList();
                case ExpressionType.Dictionary:
                    return (this as DictionaryExpression).Dict;
                case ExpressionType.Return:
                    return (this as ReturnExpression).expression.Eval(scope).GetObjectValue(scope);
                case ExpressionType.Call:
                    return (this as CallExpression).Eval(scope).GetObjectValue(scope);
                default:
                    throw new NotSupportedException(this.ToString());
            }
        }
        public bool IsVoid(Scope scope)
        {
            switch (NodeType)
            {
                case ExpressionType.Constant:
                    return (this as ConstantExpression).constantType == ConstantType.Void;
                case ExpressionType.Parameter:
                    return (this as ParameterExpression).Eval(scope).IsVoid(scope);
                case ExpressionType.Return:
                    return (this as ReturnExpression).expression.IsVoid(scope);
                case ExpressionType.Block:
                case ExpressionType.Binary:
                case ExpressionType.Unary:
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
                case ExpressionType.IEnumerableList:
                    return this as IEnumerable<Expression>;
                case ExpressionType.Parameter:
                    return (this as ParameterExpression).Eval(scope).GetIEnumrableList(scope);
                case ExpressionType.Return:
                    return (this as ReturnExpression).expression.Eval(scope).GetIEnumrableList(scope);
                case ExpressionType.Call:
                    return Eval(scope).GetIEnumrableList(scope);
                default:
                    throw new NotSupportedException(NodeType.ToString());
            }
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
                    case ExpressionType.IEnumerableList:
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
        IfThen, IfThenElse, Default, While, Break,
        Parameter, Function, Array, List, Dictionary,
        Index, Return, ForEach, IEnumerableList, Call
    }
}
