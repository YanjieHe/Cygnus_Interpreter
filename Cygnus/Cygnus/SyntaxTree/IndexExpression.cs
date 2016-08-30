using System;
namespace Cygnus.SyntaxTree
{
    public class IndexExpression : Expression, IAssignable
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
        public IIndexable GetByIndex(Expression obj, Scope scope)
        {
            var Expr = obj.GetValue(scope);
            if (Expr is IIndexable)
                return Expr as IIndexable;
            else
                throw new ArgumentException(Expr.ToString() + " Cannot get element by index");
        }
        public ITable GetByDot(Expression obj, Scope scope)
        {
            var Expr = obj.GetValue(scope);
            if (Expr is ITable)
                return Expr as ITable;
            else
                throw new ArgumentException(Expr.ToString() + " Cannot get element by index");
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
        public void Assgin(Expression value, Scope scope)
        {
            switch (indexType)
            {
                case IndexType.Bracket:
                    GetByIndex(ListExpr, scope)[Index.Eval(scope), scope] = value.GetValue(scope);
                    break;
                case IndexType.Dot:
                    GetByDot(ListExpr, scope)[(Index as ParameterExpression).Name] = value.GetValue(scope);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
    public enum IndexType
    {
        Bracket, Dot
    }
}
