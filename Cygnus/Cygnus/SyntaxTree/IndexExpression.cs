using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void SetValue(Expression value)
        {
            GetCollection(Collection)[Index.Eval()] = value.GetValue();
        }
        public override Expression Eval()
        {
            return GetCollection(Collection)[Index.Eval()];
        }
        public ICollectionExpression GetCollection(Expression obj)
        {
            var Expr = obj.Eval();
            switch (Expr.NodeType)
            {
                case ExpressionType.Array:
                case ExpressionType.List:
                case ExpressionType.Dictionary:
                    return Expr as ICollectionExpression;
                case ExpressionType.Parameter:
                    return GetCollection((Expr as ParameterExpression).Value);
                case ExpressionType.FunctionCall:
                case ExpressionType.MethodCall:
                case ExpressionType.Binary:
                case ExpressionType.Unary:
                    return GetCollection(Expr.Eval());
                case ExpressionType.Return:
                    return GetCollection((Expr as ReturnExpression).expression.Eval());
                default:
                    throw new ArgumentException("Cannot get element by index");
            }
        }
    }
}
