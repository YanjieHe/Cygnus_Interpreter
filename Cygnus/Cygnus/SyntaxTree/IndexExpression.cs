using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SymbolTable;
using Expr = System.Linq.Expressions;
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
        public Expression Collection { get; private set; }
        public Expression Index { get; private set; }
        public IndexExpression(Expression Collection, Expression Index, IndexType indexType)
        {
            this.Collection = Collection;
            this.Index = Index;
            this.indexType = indexType;
        }
        public void SetValue(Expression value, Scope scope)
        {
            switch (indexType)
            {
                case IndexType.Bracket:
                    GetCollection(Collection, scope)[Index.Eval(scope), scope] = value.GetValue(scope);
                    break;
                case IndexType.Dot:
                    Collection.GetValue<TableExpression>(ExpressionType.Table, scope)
                        .Assign((Index as ParameterExpression).Name, value.GetValue(scope));
                    break;
                default:
                    throw new NotSupportedException();
            }
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
                case ExpressionType.Index:
                    return GetCollection(Expr.Eval(scope), scope);
                default:
                    Console.WriteLine(Expr);
                    throw new ArgumentException("Cannot get element by index");
            }
        }

        public override Expression Eval(Scope scope)
        {
            switch (indexType)
            {
                case IndexType.Bracket:
                    return GetCollection(Collection, scope)[Index.Eval(scope), scope];
                case IndexType.Dot:
                    return Collection.GetValue<TableExpression>(ExpressionType.Table, scope).Find((Index as ParameterExpression).Name);
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
