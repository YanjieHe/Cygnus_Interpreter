using System;
using System.Collections;
using System.Collections.Generic;

namespace Cygnus.SyntaxTree
{
    public class ArrayExpression : IListExpression<Expression[]>
    {
        public ArrayExpression(Expression[] Values) : base(Values)
        {
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Array;
            }
        }
        public override string ToString()
        {
            return "(Array)";
        }
    }
}
