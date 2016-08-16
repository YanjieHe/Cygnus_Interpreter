namespace Cygnus.SyntaxTree
{
    public class ParameterExpression : Expression
    {
        public string Name { get; private set; }
        public BlockExpression block;
        public ParameterExpression(string Name, BlockExpression block)
        {
            this.Name = Name;
            this.block = block;
        }
        public Expression Value
        {
            get
            {

                return block.Find(Name);
            }
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Parameter;
            }
        }
        public Expression Assgin(Expression value)
        {
            return block.Assgin(Name, value);
        }
        public void SetBlock(BlockExpression block)
        {
            this.block = block;
        }
        public override Expression Eval()
        {
            return this;
        }
        public override string ToString()
        {
            return string.Format("(Var: {0})", Name);
        }
    }
}
