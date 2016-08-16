using System;
using System.Linq;
using Cygnus.SymbolTable;
using System.Collections;
using Cygnus.AssemblyImporter;
using System.Collections.Generic;
using Cygnus.Libraries;
namespace Cygnus.SyntaxTree
{
    public class MethodCallExpression : Expression
    {
        public static readonly BuiltInMethodTable
            builtInMethodTable = new BuiltInMethodTable()
            {
                ["print"] = BuiltInFunctions.Print,
                ["array"] = BuiltInFunctions.InitArray,
                ["length"] = BuiltInFunctions.Length,

                ["list"] = BuiltInFunctions.InitList,
                ["append"] = ListFunctions.Append,
                ["remove"] = ListFunctions.Remove,
                ["remove_at"] = ListFunctions.RemoveAt,
                ["insert"] = ListFunctions.Insert,

                ["dict"] = BuiltInFunctions.InitDictionary,
                ["has_key"] = DictionaryFunctions.Has_Key,

                ["import"] = BuiltInFunctions.Import,
                ["range"] = BuiltInFunctions.Range,
                ["int"] = ConvertFunctions.ToInt,
                ["double"] = ConvertFunctions.ToDouble,
                ["str"] = ConvertFunctions.Str,
                ["to_array"] = ConvertFunctions.ToArray,
                ["to_list"] = ConvertFunctions.ToList,

            };
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.MethodCall;
            }
        }
        public Expression[] arguments { get; private set; }
        public string Name { get; private set; }
        public MethodCallExpression(string Name, Expression[] arguments)
        {
            this.Name = Name;
            this.arguments = arguments;
        }

        public override Expression Eval()
        {
            return builtInMethodTable[Name](arguments);
        }

        public MethodCallExpression Update(Expression[] arguments)
        {
            this.arguments = arguments;
            return this;
        }
        public override string ToString()
        {
            return "(Method Call: " + Name + ")";
        }
    }
}
