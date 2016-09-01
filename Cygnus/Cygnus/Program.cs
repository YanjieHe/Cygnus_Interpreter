using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.Executors;
using System.IO;
using Cygnus.SyntaxTree;
namespace Cygnus
{
    class Program
    {
        static void Main(string[] args)
        {
            Settings.WorkSpace.CurrentWorkSpace = @"D:\myCode\CygnusCode";
            //Console.Write("Please input the code path: ");
            //new ExecuteFromFile(@"D:\myCode\CygnusCode\linkedlist.cys").Run();
            //  Console.ReadKey();
            new ExecuteInConsole().Run();
            //            new ExecuteFromString(@"
            //            if 2 > 3 then
            //                print('haha')
            //            else
            //                if false then
            //                    print('kuu')
            //                else
            //                    print('foo')
            //                ende
            //                print('kaka')
            //            end
            //").Run();
            //            new ExecuteFromString(@"
            //        def max(x,y) begin
            //            if x > y then
            //                return x
            //            else
            //                return y
            //            end
            //        end
            //").Run();
            Console.ReadKey();
        }
    }
}
