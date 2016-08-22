using System.Collections.Generic;

namespace Cygnus.SyntaxTree
{
    public class ListExpression : IListExpression<List<Expression>>
    {
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.List;
            }
        }
        public ListExpression(List<Expression> Values) : base(Values) { }
        public override string ToString()
        {
            return "(List)";
        }
    }
}
