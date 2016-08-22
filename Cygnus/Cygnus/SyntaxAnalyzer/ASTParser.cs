using System;
using System.Collections.Generic;
using System.Linq;
using Cygnus.LexicalAnalyzer;
using Cygnus.Errors;
using Cygnus.SyntaxTree;
using Cygnus.SyntaxAnalyzer.Statements;
namespace Cygnus.SyntaxAnalyzer
{
    public class ASTParser
    {
        Lexeme[] array;
        public Scope scope;
        public BlockExpression program { get; private set; }
        public ASTParser(Lexeme[] array, Scope scope)
        {
            this.array = array;
            this.scope = scope;
            program = new BlockExpression();
        }
        public ASTParser(Lexeme[] array, BlockExpression program)
        {
            this.array = array;
            this.program = program;
        }
        public void Parse(int start, int end)
        {
            new BlockStatement(program, scope, array).ParseBlock(0, array.Length - 1);
        }
    }
}
