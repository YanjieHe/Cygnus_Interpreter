using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SymbolTable;

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
        public Expression Collection { get; private set; }
        public Expression Index { get; private set; }
        public IndexExpression(Expression Collection, Expression Index)
        {
            this.Collection = Collection;
            this.Index = Index;
        }
        public void SetValue(Expression value, Scope scope)
        {
            GetCollection(Collection, scope)[Index.Eval(scope), scope] = value.GetValue(scope);
        }
        public ICollectionExpression GetCollection(Expression obj, Scope scope)
        {
            var Expr = obj.Eval(scope);
            switch (Expr.NodeType)
            {
                case ExpressionType.Array:
                case ExpressionType.List:
                case ExpressionType.Dictionary:
                    return Expr as ICollectionExpression;
                case ExpressionType.Parameter:
                    return GetCollection((Expr as ParameterExpression).Eval(scope), scope);
                case ExpressionType.Call:
                case ExpressionType.Binary:
                case ExpressionType.Unary:
                    return GetCollection(Expr.Eval(scope), scope);
                case ExpressionType.Return:
                    return GetCollection((Expr as ReturnExpression).expression.Eval(scope), scope);
                default:
                    throw new ArgumentException("Cannot get element by index");
            }
        }

        public override Expression Eval(Scope scope)
        {
            return GetCollection(Collection, scope)[Index.Eval(scope), scope];
        }
    }
}
