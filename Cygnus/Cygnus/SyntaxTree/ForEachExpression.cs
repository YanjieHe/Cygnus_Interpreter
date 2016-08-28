using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SymbolTable;

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
        public Expression list { get; private set; }
        public BlockExpression Block { get; private set; }
        public ParameterExpression Iterator { get; private set; }
        public ForEachExpression(IEnumerable<Expression> list, BlockExpression Block, ParameterExpression Iterator)
        {
            this.list = new IEnumerableExpression(list);
            this.Block = Block;
            this.Iterator = Iterator;
        }
        public ForEachExpression(Expression Expr_list, BlockExpression Block, ParameterExpression Iterator)
        {
            this.list = Expr_list;
            this.Block = Block;
            this.Iterator = Iterator;
        }
        public override string ToString()
        {
            return "(ForEach)";
        }
        public override Expression Eval(Scope scope)
        {
            Expression Result = null;
            IEnumerable<Expression> Iter_List = list.Eval(scope).GetIEnumrableList(scope);
            foreach (var item in Iter_List)
            {
                Iterator.Assgin(item, scope);
                Result = Block.Eval(scope);
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
