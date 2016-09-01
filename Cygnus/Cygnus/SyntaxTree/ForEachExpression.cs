using System.Collections.Generic;

namespace Cygnus.SyntaxTree
{
    public class ForEachExpression : Expression
    {
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.ForEach;
            }
        }
        public Expression Collection { get; private set; }
        public Expression Body { get; private set; }
        public ParameterExpression Iterator { get; private set; }
        public ForEachExpression(ParameterExpression Iterator, IEnumerable<Expression> Collection, Expression Body)
        {
            this.Collection = new IEnumerableExpression(Collection);
            this.Body = Body;
            this.Iterator = Iterator;
        }
        public ForEachExpression(ParameterExpression Iterator, Expression Collection, Expression Body)
        {
            this.Collection = Collection;
            this.Body = Body;
            this.Iterator = Iterator;
        }
        public override string ToString()
        {
            return "(ForEach)";
        }
        public override Expression Eval(Scope scope)
        {
            Expression Result = null;
            IEnumerable<Expression> Collection = this.Collection.Eval(scope).GetIEnumrableList(scope);
            foreach (var item in Collection)
            {
                Iterator.Assgin(item, scope);
                Result = Body.Eval(scope);
                switch (Result.NodeType)
                {
                    case ExpressionType.Break:
                        goto EndForEach;
                    case ExpressionType.Continue:
                        continue;
                    case ExpressionType.Return:
                        return Result;
                }
            }
        EndForEach:
            return Void();
        }
    }
}
