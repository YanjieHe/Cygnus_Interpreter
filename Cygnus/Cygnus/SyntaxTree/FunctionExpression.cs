using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SymbolTable;
using Cygnus.Libraries;
namespace Cygnus.SyntaxTree
{
    public class FunctionExpression : Expression
    {
        public static FunctionTable functionTable = new FunctionTable();
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
        public ParameterExpression[] Arguments { get; private set; }
        public BlockExpression Body { get; private set; }
        public string Name { get; private set; }
        public Scope funcScope { get; private set; }
        public FunctionExpression(string Name, BlockExpression Body, Scope scope, ParameterExpression[] Arguments)
        {
            this.Name = Name;
            this.Body = Body;
            this.Arguments = Arguments;
            this.funcScope = scope;
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Function;
            }
        }
        public override string ToString()
        {
            return "(Function)";
        }
        public override Expression Eval(Scope scope)
        {
            return Body.Eval(scope);
        }
        public FunctionExpression Update(Expression[] Values, Scope scope)
        {
            Scope newScope = new Scope(funcScope.Parent);
            for (int i = 0; i < Arguments.Length; i++)
                newScope.variableTable[Arguments[i].Name] = Values[i].Eval(scope);
            return new FunctionExpression(Name, Body, newScope, Arguments);
        }
    }
}
