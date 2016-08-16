using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.SyntaxTree
{
    public class WhileExpression : Expression
    {
        public Expression Condition { get; private set; }
        public Expression Body { get; private set; }
        public WhileExpression(Expression Condition, Expression Body)
        {
            this.Condition = Condition;
            this.Body = Body;
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.While;
            }
        }

        public override Expression Eval()
        {
            Expression Result = null;
            while ((bool)((ConstantExpression)Condition.Eval()).Value)
            {
                Result = Body.Eval();
                if (Result.NodeType == ExpressionType.Break)
                    break;
                else if (Result.NodeType == ExpressionType.Return)
                    return Result;
            }
            return new DefaultExpression(ConstantType.Void);
        }
        public override string ToString()
        {
            return "(While)";
        }
    }
}
