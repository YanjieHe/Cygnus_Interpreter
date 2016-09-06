using System;
using System.Text;
using Cygnus.LexicalAnalyzer;
using Cygnus.Expressions;

namespace Cygnus.Executors
{
    public class ExecuteFromFile : InterpreterExecutor
    {
        string FilePath;
        Encoding encoding;
        public ExecuteFromFile(string FilePath) : base()
        {
            Initialize(FilePath, Encoding.Default);
        }
        public ExecuteFromFile(string FilePath, Scope GlobalScope) : base(GlobalScope)
        {
            Initialize(FilePath, Encoding.Default);
        }
        public ExecuteFromFile(string FilePath, Encoding encoding) : base()
        {
            Initialize(FilePath, encoding);
        }
        public ExecuteFromFile(string FilePath, Encoding encoding, Scope GlobalScope) : base(GlobalScope)
        {
            Initialize(FilePath, encoding);
        }
        private void Initialize(string FilePath, Encoding encoding)
        {
            this.FilePath = FilePath;
            this.encoding = encoding;
        }
        public override Expression Run()
        {
            try
            {
                return Execute(new Lexical(FilePath, encoding));
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                return Expression.Void();
            }
        }
    }
}
