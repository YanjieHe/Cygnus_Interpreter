using System.Linq;
using Cygnus.SyntaxTree;
using Cygnus.Errors;
using Cygnus.SymbolTable;
using Cygnus.Libraries;
namespace Cygnus.SyntaxTree
{
    public class Scope
    {
        public Scope Parent { get; private set; }
        private VariableTable variableTable;
        public Scope()
        {
            Parent = null;
            variableTable = new VariableTable();
        }
        public Scope(Scope Parent)
        {
            this.Parent = Parent;
            variableTable = new VariableTable();
        }
        public Expression this[string Name]
        {
            set { variableTable[Name] = value; }
        }
        public void AddVariable(string Name, Expression Value)
        {
            variableTable.Add(Name, Value);
        }
        public Expression Find(string Name)
        {
            Scope current = this;
            while (current != null)
            {
                Expression Value = null;
                if (current.variableTable.TryGetValue(Name, out Value))
                    return Value;
                current = current.Parent;
            }
            if (functionTable.ContainsKey(Name) || builtInMethodTable.ContainsKey(Name))
            {
                return new CallExpression(Name, null);
            }
            throw new NotDefinedException(Name);
        }
        public Expression Assgin(string Name, Expression Value)
        {
            Scope current = this;
            while (current != null)
            {
                if (current.variableTable.ContainsKey(Name))
                {
                    current.variableTable[Name] = Value;
                    return Value;
                }
                current = current.Parent;
            }
            variableTable[Name] = Value;
            return Value;
        }
        public bool TryGetValue(string Name, out Expression Value)
        {
            Value = null;
            Scope current = this;
            while (current != null)
            {
                if (current.variableTable.TryGetValue(Name, out Value))
                    return true;
                current = current.Parent;
            }
            if (functionTable.ContainsKey(Name) || builtInMethodTable.ContainsKey(Name))
            {
                Value = new CallExpression(Name, null);
                return true;
            }
            return false;
        }
        public bool Delete(string Name)
        {
            Scope current = this;
            while (current != null)
            {
                if (current.variableTable.ContainsKey(Name))
                {
                    current.variableTable.Remove(Name);
                    return true;
                }
                current = current.Parent;
            }
            return false;
        }
        public override string ToString()
        {
            return "Scope = {\r\n" + string.Join("\r\n", variableTable.Select(i => i)) + "\r\n}";
        }
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
              ["import"] = BuiltInFunctions.Import,
              ["execfile"] = BuiltInFunctions.ExecuteFile,
              ["throw"] = BuiltInFunctions.Throw,
              ["delete"] = BuiltInFunctions.Delete,
              ["scan"] = BuiltInFunctions.Scan,
              ["range"] = BuiltInFunctions.Range,
              ["exit"] = BuiltInFunctions.Exit,
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
              ["strsplit"] = StringFunctions.StrSplit,
              ["strformat"] = StringFunctions.StrFormat,
              ["strlen"] = StringFunctions.StrLen,
              ["strfind"] = StringFunctions.StrFind,
              ["strreplace"] = StringFunctions.StrReplace,
              /***************     Higher-order functions     ***************/
              ["map"] = HigherOrderFunctions.Map,
              ["filter"] = HigherOrderFunctions.Filter,
              ["reduce"] = HigherOrderFunctions.Reduce,

          };
    }
}
