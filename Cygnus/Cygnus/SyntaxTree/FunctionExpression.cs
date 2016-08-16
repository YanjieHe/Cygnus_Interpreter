using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SymbolTable;
namespace Cygnus.SyntaxTree
{
    public class FunctionExpression : Expression
    {
        public static FunctionTable functionTable = new FunctionTable();
        public ParameterExpression[] Arguments { get; private set; }
        public BlockExpression Body { get; private set; }
        public string Name { get; private set; }
        public FunctionExpression(string Name, BlockExpression Body, ParameterExpression[] Arguments)
        {
            this.Name = Name;
            this.Body = Body;
            this.Arguments = Arguments;
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Function;
            }
        }
        public override Expression Eval()
        {
            return Body.Eval();
        }
        public FunctionExpression Update(Expression[] arguments)
        {
            for (int i = 0; i < Arguments.Length; i++)
                Arguments[i].Assgin(arguments[i].Eval());
            return this;
        }
        public override string ToString()
        {
            return "(Function)";
        }
    }
}
