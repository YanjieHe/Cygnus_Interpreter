using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.SyntaxTree
{
    public abstract class Expression : IEquatable<Expression>
    {
        public abstract ExpressionType NodeType { get; }
        public abstract Expression Eval();
        public T GetValue<T>(ExpressionType type) where T : Expression
        {
            if (NodeType == type) return this as T;
            switch (NodeType)
            {
                case ExpressionType.FunctionCall:
                    return this.Eval().GetValue<T>(type);
                case ExpressionType.Parameter:
                    return (this as ParameterExpression).Value.GetValue<T>(type);
                case ExpressionType.Return:
                    return (this as ReturnExpression).expression.Eval().GetValue<T>(type);
                case ExpressionType.Default:
                case ExpressionType.Block:
                case ExpressionType.Binary:
                case ExpressionType.Unary:
                    return this.Eval().GetValue<T>(type);
                default:
                    throw new NotSupportedException(ToString());
            }
        }
        public Expression GetValue()
        {
            switch (NodeType)
            {
                case ExpressionType.Parameter:
                    return (this as ParameterExpression).Value.GetValue();
                case ExpressionType.Return:
                    return (this as ReturnExpression).expression.Eval().GetValue();
                case ExpressionType.Default:
                case ExpressionType.Block:
                case ExpressionType.Binary:
                case ExpressionType.Unary:
                    return this.Eval().GetValue();
                default:
                    return this;
            }
        }
        public object GetObjectValue()
        {
            switch (NodeType)
            {
                case ExpressionType.Constant:
                    return (this as ConstantExpression).Value;
                case ExpressionType.Parameter:
                    return (this as ParameterExpression).Value.GetObjectValue();
                case ExpressionType.Default:
                    {
                        var Expr = this as DefaultExpression;
                        if (Expr.defaultType == ConstantType.Void) return null;
                        else return Expr.Eval().GetObjectValue();
                    }
                case ExpressionType.Binary:
                case ExpressionType.Unary:
                    return this.Eval().GetObjectValue();
                case ExpressionType.Index:
                    return (this as IndexExpression).Eval().GetObjectValue();
                case ExpressionType.Array:
                    return (this as ArrayExpression).Values.ToArray();
                case ExpressionType.List:
                    return (this as ListExpression).Values.ToList();
                case ExpressionType.Dictionary:
                    return (this as DictionaryExpression).Dict;
                case ExpressionType.Return:
                    return (this as ReturnExpression).expression.Eval().GetObjectValue();
                case ExpressionType.MethodCall:
                    return (this as MethodCallExpression).Eval().GetObjectValue();
                case ExpressionType.FunctionCall:
                    return (this as FunctionCallExpression).Eval().GetObjectValue();
                default:
                    throw new NotSupportedException(this.ToString());
            }
        }
        public bool IsVoid()
        {
            switch (NodeType)
            {
                case ExpressionType.Default:
                    return (this as DefaultExpression).defaultType == ConstantType.Void;
                case ExpressionType.Parameter:
                    return (this as ParameterExpression).Value.IsVoid();
                case ExpressionType.Return:
                    return (this as ReturnExpression).expression.IsVoid();
                case ExpressionType.Block:
                case ExpressionType.Binary:
                case ExpressionType.Unary:
                    return Eval().IsVoid();
                default:
                    return false;
            }
        }
        public IEnumerable<Expression> GetIEnumrableList()
        {
            switch (NodeType)
            {
                case ExpressionType.Array:
                case ExpressionType.List:
                case ExpressionType.Dictionary:
                case ExpressionType.IEnumerableList:
                    return this as IEnumerable<Expression>;
                case ExpressionType.Parameter:
                    return (this as ParameterExpression).Value.GetIEnumrableList();
                case ExpressionType.Return:
                    return (this as ReturnExpression).expression.Eval().GetIEnumrableList();
                case ExpressionType.MethodCall:
                    return Eval().GetIEnumrableList();
                default:
                    throw new NotSupportedException(NodeType.ToString());
            }
        }
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
        public static ConstantExpression Constant(object obj, ConstantType constantType)
        {
            return new ConstantExpression(obj, constantType);
        }
        public static IEnumerableExpression IEnumerable(IEnumerable<Expression> list)
        {
            return new IEnumerableExpression(list);
        }
        public static DefaultExpression Void()
        {
            return new DefaultExpression(ConstantType.Void);
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return this.Equals(obj as Expression);
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
                    case ExpressionType.Default:
                        return (this as DefaultExpression).Equals(other as DefaultExpression);
                    case ExpressionType.Parameter:
                        return this.Eval().Equals(other.Eval());
                    case ExpressionType.Array:
                        return (this as ArrayExpression).Equals(other as ArrayExpression);
                    case ExpressionType.List:
                        return (this as ListExpression).Equals(other as ListExpression);
                    case ExpressionType.Dictionary:
                        return (this as DictionaryExpression).Equals(other as DictionaryExpression);
                    case ExpressionType.Return:
                        return (this as ReturnExpression).expression.Eval().Equals((other as ReturnExpression).expression.Eval());
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
        Parameter, Function, MethodCall, Array, List, Dictionary,
        Index, Return, ForEach, IEnumerableList,FunctionCall
    }
}
