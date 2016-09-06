using System;
using Cygnus.DataStructures;
namespace Cygnus.Expressions
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
        public Expression expression { get; private set; }
        public Expression Index { get; private set; }
        public IndexExpression(Expression expression, Expression Index, IndexType indexType)
        {
            this.expression = expression;
            this.Index = Index;
            this.indexType = indexType;
        }
        public IIndexable GetByIndex(Expression obj, Scope scope)
        {
            var Expr = obj.AsConstant(scope).Value;
            if (Expr is IIndexable)
                return Expr as IIndexable;
            else
                throw new ArgumentException(Expr.ToString() + " Cannot get element by index");
        }
        public override Expression Eval(Scope scope)
        {
            return new ConstantExpression(GetByIndex(expression, scope)[Index.AsConstant(scope).Value]);
        }
        public override string ToString()
        {
            return string.Format("(Index  Type: {0})", indexType);
        }
        public void Assgin(Expression value, Scope scope)
        {
            GetByIndex(expression, scope)[Index.AsConstant(scope).Value] = value.AsConstant(scope).Value;
        }
    }
    public enum IndexType
    {
        Bracket, Dot
    }
}
