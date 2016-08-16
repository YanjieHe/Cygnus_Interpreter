using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cygnus.SyntaxTree
{
    public class FunctionCallExpression : Expression
    {
        public string Name { get; private set; }
        public Expression[] Arguments { get; private set; }
        public FunctionCallExpression(string Name, Expression[] Arguments)
        {
            this.Name = Name;
            this.Arguments = Arguments;
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.FunctionCall;
            }
        }
        public override Expression Eval()
        {
            var func = FunctionExpression.functionTable[Name].Update(Arguments);
            return func.Eval();
        }
        public override string ToString()
        {
            return "(Function Call: " + Name + ")";
        }
    }
}
