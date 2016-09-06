using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SymbolTable;

namespace Cygnus.Expressions
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
        public override string ToString()
        {
            return "(While)";
        }
        public override Expression Eval(Scope scope)
        {
            Expression Result = null;
            while (Condition.As<bool>(scope))
            {
                Result = Body.Eval(scope);
                switch (Result.NodeType)
                {
                    case ExpressionType.Break:
                        goto EndWhile;
                    case ExpressionType.Continue:
                        continue;
                    case ExpressionType.Return:
                        return Result;
                }
            }
        EndWhile:
            return Void();
        }
    }
}
