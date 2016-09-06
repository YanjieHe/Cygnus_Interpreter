using System.Linq;
using System;
using Cygnus.Errors;
using Cygnus.SymbolTable;
using Cygnus.Libraries;
namespace Cygnus.Expressions
{
    public class Scope
    {
        public Scope Parent { get; private set; }
        private VariableTable variableTable;
        private FunctionTable functionTable;
        public static ClassTable classtable = new ClassTable();
        public Scope()
        {
            Parent = null;
            variableTable = new VariableTable();
            functionTable = new FunctionTable();
        }
        public Scope(Scope Parent)
        {
            this.Parent = Parent;
            variableTable = new VariableTable();
            functionTable = new FunctionTable();
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
        public void SetVariable(string Name, Expression Value)
        {
            variableTable[Name] = Value;
        }
        public void SetFunction(string Name, FunctionExpression func)
        {
            functionTable[Name] = func;
        }
        public FunctionExpression GetFunction(string Name)
        {
            return functionTable[Name];
        }
        public bool TryGetFunction(string Name, out FunctionExpression func)
        {
            Scope current = this;
            while (current != null)
            {
                if (current.functionTable.TryGetValue(Name, out func))
                {
                    return true;
                }
                current = current.Parent;
            }
            func = null;
            return false;
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
            throw new NotDefinedException(Name);
        }
        public bool TryGetVariable(string Name, out Expression value)
        {
            Scope current = this;
            while (current != null)
            {
                if (current.variableTable.TryGetValue(Name, out value))
                {
                    return true;
                }
                current = current.Parent;
            }
            value = null;
            return false;
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
        public Scope Clone()
        {
            var newScope = new Scope(Parent);
            foreach (var variable in variableTable)
            {
                newScope.variableTable[variable.Key] = variable.Value.Eval(this);
            }
            foreach (var function in functionTable)
            {
                newScope.functionTable[function.Key] = function.Value;
            }
            return newScope;
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
        public static readonly BuiltInMethodTable
          builtInMethodTable = new BuiltInMethodTable()
          {
              /***************     Basic functions     ***************/
              ["print"] = BuiltInFunctions.Print,
              ["list"] = BuiltInFunctions.InitList,
              //["table"] = BuiltInFunctions.InitTable,
              //["vector"] = BuiltInFunctions.InitVector,
              //["matrix"] = BuiltInFunctions.InitMatrix,
              //["setparent"] = BuiltInFunctions.SetParent,
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
              //["map"] = HigherOrderFunctions.Map,
              //["filter"] = HigherOrderFunctions.Filter,
              //["reduce"] = HigherOrderFunctions.Reduce,

              /***************     Matrix functions     ***************/
              //["zeros"] = MatrixFunctions.Mat_Zeros,
              //["ones"] = MatrixFunctions.Mat_Ones,
              //["inv"] = MatrixFunctions.Mat_Inverse,
              //["dot"] = MatrixFunctions.Mat_Dot,
              //["trans"] = MatrixFunctions.Mat_Transpose,

          };
    }
}
