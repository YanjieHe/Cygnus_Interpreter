using System;
using Cygnus.Expressions;
using Cygnus.LexicalAnalyzer;
using Cygnus.SymbolTable;
namespace Cygnus.SyntaxAnalyzer
{
    public class AST
    {
        public AST() { }
        public BlockExpression Parse(Lexeme[] array, Scope GlobalScope)
        {
            return new ASTParser(array, GlobalScope).Parse();
        }
        public void Display(BlockExpression Root)
        {
            Console.WriteLine("Abstract syntax tree: ");
            Console.WriteLine();
            ASTViewer.PrintTree(Root);
            Console.WriteLine();
            Console.WriteLine("End of the tree");
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("                    output                    ");
            Console.WriteLine("----------------------------------------------");
        }
    }
}
