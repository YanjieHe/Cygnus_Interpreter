using System.Collections.Generic;
using Cygnus.SymbolTable;
using System;
namespace Cygnus.Expressions
{
    public class BlockExpression : Expression
    {
        public BlockExpression Parent { get; private set; }
        public LinkedList<Expression> Children { get; private set; }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Block;
            }
        }
        public BlockExpression(BlockExpression parent = null)
        {
            Parent = parent;
            Children = new LinkedList<Expression>();
        }
        public void Append(Expression line)
        {
            Children.AddLast(line);
        }
        public void SetParent(BlockExpression Parent)
        {
            this.Parent = Parent;
        }
        public override string ToString()
        {
            return "(Block)";
        }
        public override Expression Eval(Scope scope)
        {
            Expression Result = null;
            foreach (var line in Children)
            {
                Result = line.Eval(scope);
                switch (Result.NodeType)
                {
                    case ExpressionType.Break:
                    case ExpressionType.Continue:
                    case ExpressionType.Return:
                        return Result;
                }
            }
            return Result ?? Void();
        }
    }
}
