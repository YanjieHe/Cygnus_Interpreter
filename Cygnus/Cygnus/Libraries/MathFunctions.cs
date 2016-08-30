using System;
using System.Linq;
using Cygnus.SyntaxTree;
namespace Cygnus.Libraries
{
    public static class MathFunctions
    {
        public static Expression Exp(Expression[] args, Scope scope)
        {
            var d = args.Single().AsConstant(scope).GetDouble();
            return Math.Exp(d);
        }
        public static Expression Sqrt(Expression[] args, Scope scope)
        {
            var d = args.Single().AsConstant(scope).GetDouble();
            return Math.Sqrt(d);
        }
        public static Expression Abs(Expression[] args, Scope scope)
        {
            var d = args.Single().AsConstant(scope);
            if (d.type == ConstantType.Integer)
                return Math.Abs((int)d.Value);
            else if (d.type == ConstantType.Double)
                return Math.Abs((double)d.Value);
            else throw new ArgumentException();
        }
        public static Expression Log(Expression[] args, Scope scope)
        {
            if (args.Length == 1)
            {
                var d = args[0].AsConstant(scope).GetDouble();
                return Math.Log(d);
            }
            else if (args.Length == 2)
            {
                var a = args[0].AsConstant(scope).GetDouble();
                var newBase = args[1].AsConstant(scope).GetDouble();
                return Math.Log(a, newBase);
            }
            else throw new ArgumentException();
        }
        public static Expression Mod(Expression[] args, Scope scope)
        {
            if (args.Length == 2)
            {
                var a = args[0].As<int>(scope);
                var b = args[1].As<int>(scope);
                return (a % b);
            }
            else throw new ArgumentException();
        }
    }
}
