using System;
using System.Linq;
using System.Reflection;
using Cygnus.SyntaxTree;
using Cygnus.SymbolTable;
namespace Cygnus.AssemblyImporter
{
    public class CSharpAssembly
    {
        string FilePath;
        string Name;
        public CSharpAssembly(string FilePath, string Name)
        {
            this.FilePath = FilePath;
            this.Name = Name;
        }
        //import('DllForTest.dll','DllForTest.addclass')
        public void Import()
        {
            Assembly assembly = Assembly.LoadFile(FilePath);  //load dll file
            Type type = assembly.GetType(Name);  //Namespace + class name
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (!ExceptMethods.Contains(method.Name))
                {
                    var func = GetFunc(method);
                    Scope.builtInMethodTable[method.Name] = func;
                }
            }
        }
        public static Func<Expression[], Scope, Expression> GetFunc(MethodInfo method)
        {
            return Delegate.CreateDelegate(typeof(Func<Expression[], Scope, Expression>), method)
                as Func<Expression[], Scope, Expression>;
        }
        private static readonly string[] ExceptMethods = new string[]
        {
           "ToString","Equals","GetHashCode","GetType"
        };
    }
}
