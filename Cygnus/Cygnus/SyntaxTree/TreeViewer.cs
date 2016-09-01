using System;

namespace Cygnus.SyntaxTree
{
    public class ASTViewer
    {
        public static void PrintTree(Expression Node, int depth = 0)
        {
            if (Node == null) return;
            PrintNode(Node, depth);
            switch (Node.NodeType)
            {
                case ExpressionType.Block:
                    foreach (var item in ((BlockExpression)Node).Children)
                        PrintTree(item, depth + 1); break;
                case ExpressionType.Binary:
                    var left = ((BinaryExpression)Node).Left;
                    var right = ((BinaryExpression)Node).Right;
                    PrintTree(left, depth + 1);
                    PrintTree(right, depth + 1); break;
                case ExpressionType.IfThen:
                    {
                        var test = ((IfThenExpression)Node).Test;
                        var IfTrue = ((IfThenExpression)Node).IfTrue;
                        PrintTree(test, depth + 1);
                        PrintTree(IfTrue, depth + 1);
                    }
                    break;
                case ExpressionType.IfThenElse:
                    {
                        var test = ((IfThenElseExpression)Node).Test;
                        var IfTrue = ((IfThenElseExpression)Node).IfTrue;
                        var IfFalse = ((IfThenElseExpression)Node).IfFalse;
                        PrintTree(test, depth + 1);
                        PrintTree(IfTrue, depth + 1);
                        PrintTree(IfFalse, depth + 1);
                    }
                    break;
                case ExpressionType.While:
                    {
                        var condition = ((WhileExpression)Node).Condition;
                        var body = ((WhileExpression)Node).Body;
                        PrintTree(condition, depth + 1);
                        PrintTree(body, depth + 1);
                    }
                    break;
                case ExpressionType.Function:
                    foreach (var item in ((FunctionExpression)Node).Arguments)
                        PrintTree(item, depth + 1);
                    PrintTree(((FunctionExpression)Node).Body, depth + 1);
                    break;
                case ExpressionType.Index:
                    {
                        PrintTree(((IndexExpression)Node).ListExpr, depth + 1);
                        PrintTree(((IndexExpression)Node).Index, depth + 1);
                    }
                    break;
                case ExpressionType.Call:
                    foreach (var item in ((CallExpression)Node).Arguments)
                        PrintTree(item, depth + 1);
                    break;
                case ExpressionType.Return:
                    PrintTree(((GotoExpression)Node).Value, depth + 1);
                    break;
                case ExpressionType.Array:
                    {
                        foreach (var item in ((ArrayExpression)Node).Values)
                            PrintTree(item, depth + 1);
                    }
                    break;
                case ExpressionType.ForEach:
                    {
                        PrintTree(((ForEachExpression)Node).Collection, depth + 1);
                        PrintTree(((ForEachExpression)Node).Body, depth + 1);
                    }
                    break;
            }
        }
        private static void PrintNode(Expression Node, int depth)
        {
            int n = depth * 3;
            if (n != 0)
                Console.WriteLine(new string(' ', n - 3) + "|--" + Node);
            else
                Console.WriteLine(Node);
        }
    }
}
