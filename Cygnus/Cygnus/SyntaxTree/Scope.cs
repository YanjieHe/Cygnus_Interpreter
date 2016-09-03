using System.Linq;
using System;
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
        public Scope GlobalScope
        {
            get
            {
                var current = this;
                while (current.Parent != null)
                    current = current.Parent;
                return current;
            }
        }
        public Expression this[string Name]
        {
            set { variableTable[Name] = value; }
        }
        public void AddVariable(string Name, Expression Value)
        {
            variableTable.Add(Name, Value);
        }
        public void SetVariable(string Name, Expression Value)
        {
            variableTable[Name] = Value;
        }
        public Expression GetVariable(string Name)
        {
            Scope current = this;
            while (current != null)
            {
                Expression Value = null;
                if (current.variableTable.TryGetValue(Name, out Value)) return Value;
                current = current.Parent;
            }
            if (functionTable.ContainsKey(Name) || builtInMethodTable.ContainsKey(Name))
                return new CallExpression(Name, null);
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
        public bool Delete(string Name)
        {
            return Find(Name, table => table.Remove(Name));
        }
        private bool Find(string Name, Action<VariableTable> action)
        {
            Scope current = this;
            while (current != null)
            {
                if (current.variableTable.ContainsKey(Name))
                {
                    action(current.variableTable);
                    return true;
                }
                current = current.Parent;
            }
            return false;
        }
        public override string ToString()
        {
            return
                "===================================================\r\n" +
                "                       Scope                       \r\n" +
                "===================================================\r\n" +
                 string.Join(",  ", variableTable.Select(i => i.Key).Concat(functionTable.Select(i => i.Key)).Select(i => i)) + "\r\n" +
                 "===================================================";
        }
        public static FunctionTable functionTable = new FunctionTable();
        public static readonly BuiltInMethodTable
          builtInMethodTable = new BuiltInMethodTable()
          {
              /***************     Basic functions     ***************/
              ["print"] = BuiltInFunctions.Print,
              ["table"] = BuiltInFunctions.InitTable,
              ["vector"] = BuiltInFunctions.InitVector,
              ["matrix"] = BuiltInFunctions.InitMatrix,
              ["setparent"] = BuiltInFunctions.SetParent,
              ["length"] = BuiltInFunctions.Length,
              ["import"] = BuiltInFunctions.Import,
              ["execfile"] = BuiltInFunctions.ExecuteFile,
              ["throw"] = BuiltInFunctions.Throw,
              ["delete"] = BuiltInFunctions.Delete,
              ["scan"] = BuiltInFunctions.Scan,
              ["range"] = BuiltInFunctions.Range,
              ["scope"] = BuiltInFunctions.DisplayScope,
              ["getwd"] = BuiltInFunctions.GetWorkingDirectory,
              ["setwd"] = BuiltInFunctions.SetWorkingDirectory,
              ["exit"] = BuiltInFunctions.Exit,

              /***************     List functions     ***************/
              //["append"] = ListFunctions.Append,
              //["remove"] = ListFunctions.Remove,
              //["remove_at"] = ListFunctions.RemoveAt,
              //["insert"] = ListFunctions.Insert,

              /***************     Dictionary functions     ***************/
              //["has_key"] = DictionaryFunctions.Has_Key,

              /***************     Convert Type functions     ***************/
              ["int"] = ConvertFunctions.ToInt,
              ["double"] = ConvertFunctions.ToDouble,
              ["str"] = ConvertFunctions.Str,

              /***************     Math functions     ***************/
              ["exp"] = MathFunctions.Exp,
              ["sqrt"] = MathFunctions.Sqrt,
              ["abs"] = MathFunctions.Abs,
              ["log"] = MathFunctions.Log,
              ["mod"] = MathFunctions.Mod,

              /***************     String functions     ***************/
              ["strcat"] = StringFunctions.StrConcat,
              ["strjoin"] = StringFunctions.StrJoin,
              //["strsplit"] = StringFunctions.StrSplit,
              ["strformat"] = StringFunctions.StrFormat,
              ["strlen"] = StringFunctions.StrLen,
              ["strfind"] = StringFunctions.StrFind,
              ["strreplace"] = StringFunctions.StrReplace,

              /***************     Higher-order functions     ***************/
              ["map"] = HigherOrderFunctions.Map,
              ["filter"] = HigherOrderFunctions.Filter,
              ["reduce"] = HigherOrderFunctions.Reduce,

              /***************     Matrix functions     ***************/
              ["zeros"] = MatrixFunctions.Mat_Zeros,
              ["ones"] = MatrixFunctions.Mat_Ones,
              ["inv"] = MatrixFunctions.Mat_Inverse,
              ["dot"] = MatrixFunctions.Mat_Dot,
              ["trans"] = MatrixFunctions.Mat_Transpose,

          };
    }
}
