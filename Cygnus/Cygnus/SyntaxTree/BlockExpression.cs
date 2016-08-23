using System.Collections.Generic;
using Cygnus.SymbolTable;
using System;
namespace Cygnus.SyntaxTree
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
                if (Result.NodeType == ExpressionType.Break)
                    break;
                else if (Result.NodeType == ExpressionType.Continue || Result.NodeType == ExpressionType.Return)
                    return Result;
            }
            if (Result == null)
                return Void();
            else return Result;
        }
    }
}
