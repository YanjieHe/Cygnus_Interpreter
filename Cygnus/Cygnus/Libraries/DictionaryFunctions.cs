﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.SyntaxTree;
namespace Cygnus.Libraries
{
    public static class DictionaryFunctions
    {
        public static Expression Has_Key(Expression[] args)
        {
            return Expression.Constant(args[0].GetValue<DictionaryExpression>(ExpressionType.Dictionary)
                   .Dict.ContainsKey(args[1].GetValue<ConstantExpression>(ExpressionType.Constant)),
                   ConstantType.Boolean);
        }
    }
}
