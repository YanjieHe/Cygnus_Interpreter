using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Expr = System.Linq.Expressions;
using Cygnus.SyntaxTree;
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
        //import('D:\myCode\CSharpCode\DllForTest\DllForTest\bin\Debug\DllForTest.dll','DllForTest.addclass')
        public void Import()
        {
            Assembly ass = Assembly.LoadFile(FilePath);  //load dll file
            Type tp = ass.GetType(Name);  //Namespace + class name
            foreach (var item in tp.GetMethods())
            {
                if (!ExceptMethods.Contains(item.Name))
                {
                    var func = CSharpWrapper.WrapFunc(CreateDelegate(item)
                        , item.GetParameters()
                        .Select(i => i.ParameterType).ToArray(), item.ReturnType);
                    FunctionExpression.builtInMethodTable[item.Name] = func;
                }
            }
        }
        private Delegate CreateDelegate(MethodInfo method)
        {
            var parameters = method.GetParameters().Select(i => i.ParameterType).Select(i => Expr.Expression.Parameter(i)).ToArray();
            var ReturnParameter = Expr.Expression.Parameter(method.ReturnParameter.ParameterType);
            var func = Expr.Expression.Lambda(Expr.Expression.Call(method, parameters), parameters).Compile();
            return func;
        }
        private static readonly string[] ExceptMethods = new string[]
        {
           "ToString","Equals","GetHashCode","GetType"
        };
    }
}
