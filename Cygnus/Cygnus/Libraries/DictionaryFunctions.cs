using Cygnus.SyntaxTree;
using Cygnus.Errors;
using Cygnus.Extensions;
namespace Cygnus.Libraries
{
    public static class DictionaryFunctions
    {
        public static Expression Has_Key(Expression[] args, Scope scope)
        {
            (args.Length == 2).OrThrows<ParameterException>();
            return args[0].AsDictionary(scope).Dict.ContainsKey(args[1].AsConstant(scope));
        }
    }
}
