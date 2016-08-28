using System;
namespace Cygnus.SyntaxTree
{
    public class IndexExpression : Expression
    {
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Index;
            }
        }
        public IndexType indexType { get; private set; }
        public Expression ListExpr { get; private set; }
        public Expression Index { get; private set; }
        public IndexExpression(Expression ListExpr, Expression Index, IndexType indexType)
        {
            this.ListExpr = ListExpr;
            this.Index = Index;
            this.indexType = indexType;
        }
        public void SetValue(Expression value, Scope scope)
        {
            switch (indexType)
            {
                case IndexType.Bracket:
                    GetByIndex(ListExpr, scope)[Index.Eval(scope), scope] = value.GetValue(scope);
                    break;
                case IndexType.Dot:
                    ListExpr.GetValue<TableExpression>(ExpressionType.Table, scope)
                        .Assign((Index as ParameterExpression).Name, value.GetValue(scope));
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
        public IIndexable GetByIndex(Expression obj, Scope scope)
        {
            var Expr = obj.Eval(scope);
            switch (Expr.NodeType)
            {
                case ExpressionType.Array:
                case ExpressionType.List:
                case ExpressionType.Dictionary:
                case ExpressionType.Matrix:
                case ExpressionType.MatrixRow:
                    return Expr as IIndexable;
                case ExpressionType.Parameter:
                case ExpressionType.Call:
                case ExpressionType.Binary:
                case ExpressionType.Unary:
                case ExpressionType.Index:
                    return GetByIndex(Expr.Eval(scope), scope);
                case ExpressionType.Return:
                    return GetByIndex((Expr as ReturnExpression).expression.Eval(scope), scope);
                default:
                    throw new ArgumentException(Expr.ToString() + " Cannot get element by index");
            }
        }
        public ITable GetByDot(Expression obj, Scope scope)
        {
            var Expr = obj.Eval(scope);
            switch (Expr.NodeType)
            {
                case ExpressionType.Table:
                case ExpressionType.KeyValuePair:
                    return Expr as ITable;
                case ExpressionType.Parameter:
                case ExpressionType.Call:
                case ExpressionType.Binary:
                case ExpressionType.Unary:
                case ExpressionType.Index:
                    return GetByDot(Expr.Eval(scope), scope);
                case ExpressionType.Return:
                    return GetByDot((Expr as ReturnExpression).expression.Eval(scope), scope);
                default:
                    throw new ArgumentException(Expr.ToString() + " Cannot get element by index");
            }
        }
        public override Expression Eval(Scope scope)
        {
            switch (indexType)
            {
                case IndexType.Bracket:
                    return GetByIndex(ListExpr, scope)[Index.Eval(scope), scope];
                case IndexType.Dot:
                    return GetByDot(ListExpr, scope)[(Index as ParameterExpression).Name];
                default:
                    throw new NotSupportedException();
            }
        }
        public override string ToString()
        {
            return string.Format("(Index  Type: {0})", indexType);
        }
    }
    public enum IndexType
    {
        Bracket, Dot
    }
}
