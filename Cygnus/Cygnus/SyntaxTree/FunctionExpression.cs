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
              /***************     Basic functions     ***************/
              ["print"] = BuiltInFunctions.Print,
              ["array"] = BuiltInFunctions.InitArray,
              ["list"] = BuiltInFunctions.InitList,
              ["dict"] = BuiltInFunctions.InitDictionary,
              ["table"] = BuiltInFunctions.InitTable,
              ["setparent"] = BuiltInFunctions.SetParent,
              ["length"] = BuiltInFunctions.Length,
              ["dispose"] = BuiltInFunctions.Dispose,
              ["import"] = BuiltInFunctions.Import,
              ["execfile"] = BuiltInFunctions.ExecuteFile,
              ["throw"] = BuiltInFunctions.Throw,
              ["delete"] = BuiltInFunctions.Delete,
              ["isnull"] = BuiltInFunctions.IsNull,
              ["scan"] = BuiltInFunctions.Scan,
              ["range"] = BuiltInFunctions.Range,

              /***************     List functions     ***************/
              ["append"] = ListFunctions.Append,
              ["remove"] = ListFunctions.Remove,
              ["remove_at"] = ListFunctions.RemoveAt,
              ["insert"] = ListFunctions.Insert,

              /***************     Dictionary functions     ***************/
              ["has_key"] = DictionaryFunctions.Has_Key,

              /***************     Convert Type functions     ***************/
              ["int"] = ConvertFunctions.ToInt,
              ["double"] = ConvertFunctions.ToDouble,
              ["str"] = ConvertFunctions.Str,
              ["to_array"] = ConvertFunctions.ToArray,
              ["to_list"] = ConvertFunctions.ToList,

              /***************     Math functions     ***************/
              ["exp"] = MathFunctions.Exp,
              ["sqrt"] = MathFunctions.Sqrt,
              ["abs"] = MathFunctions.Abs,
              ["log"] = MathFunctions.Log,
              ["mod"] = MathFunctions.Mod,
              /***************     String functions     ***************/
              ["strcat"] = StringFunctions.StrConcat,
              ["strjoin"] = StringFunctions.StrJoin,

              /***************     Higher-order functions     ***************/
              ["map"] = HigherOrderFunctions.Map,
              ["filter"] = HigherOrderFunctions.Filter,

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
