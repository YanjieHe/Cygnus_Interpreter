using System.Collections.Generic;
using Cygnus.SymbolTable;
using System;
namespace Cygnus.SyntaxTree
{
    public class BlockExpression : Expression
    {
        public VariableTable variableTable { get; set; }
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
            variableTable = new VariableTable();
        }
        public void Append(Expression line)
        {
            Children.AddLast(line);
        }
        public void SetParent(BlockExpression Parent)
        {
            this.Parent = Parent;
        }
        public Expression Find(string Name)
        {
            BlockExpression current = this;
            while (current != null)
            {
                Expression result = null;
                if (current.variableTable.TryGetValue(Name, out result))
                    return result;
                current = current.Parent;
            }
            throw new ArgumentException("Use of unassgined variable '" + Name + "'");
        }
        public Expression Assgin(string Name, Expression value)
        {
            BlockExpression current = this;
            while (current != null)
            {
                if (current.variableTable.ContainsKey(Name))
                {
                    current.variableTable[Name] = value;
                    return value;
                }
                current = current.Parent;
            }
            variableTable.Add(Name, value);
            return value;
        }
        public bool TryGetValue(string Name, out Expression value)
        {
            value = null;
            BlockExpression current = this;
            while (current != null)
            {
                if (current.variableTable.TryGetValue(Name, out value))
                    return true;
                current = current.Parent;
            }
            return false;
        }
        public override Expression Eval()
        {
            Expression Result = null;
            foreach (var line in Children)
            {
                Result = line.Eval();
                if (Result.NodeType == ExpressionType.Break)
                    break;
                else if (Result.NodeType == ExpressionType.Return)
                    return Result;
            }
            if (Result == null) return new DefaultExpression(ConstantType.Void);
            else return Result;
        }
        public override string ToString()
        {
            return "(Block)";
        }
    }
}
