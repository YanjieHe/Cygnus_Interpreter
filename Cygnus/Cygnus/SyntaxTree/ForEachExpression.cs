using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Iterator.SetBlock(Block);
        }
        public ForEachExpression(Expression Expr_list, BlockExpression Block, ParameterExpression Iterator)
        {
            this.list = Expr_list;
            this.Block = Block;
            this.Iterator = Iterator;
            Iterator.SetBlock(Block);
        }
        public override Expression Eval()
        {
            Expression Result = null;
            IEnumerable<Expression> Iter_List = list.Eval().GetIEnumrableList();
            foreach (var item in Iter_List)
            {
                Iterator.Assgin(item);
                Result = Block.Eval();
                if (Result.NodeType == ExpressionType.Break) break;
                else if (Result.NodeType == ExpressionType.Return) return Result;
            }
            return new DefaultExpression(ConstantType.Void);
        }
        public override string ToString()
        {
            return "(ForEach)";
        }
    }
}
