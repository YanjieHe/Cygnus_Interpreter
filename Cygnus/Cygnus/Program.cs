using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.Executors;
namespace Cygnus
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.Write("Please input the code path: ");
            //new ExecuteFromFile(@"D:\myCode\CygnusCode\linkedlist.cys").Run();
            //  Console.ReadKey();
            new ExecuteInConsole().Run();
        }
    }
}
