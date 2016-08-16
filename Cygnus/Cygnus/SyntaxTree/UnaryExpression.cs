﻿using System;
using Cygnus.SyntaxAnalyzer;
using Cygnus.LexicalAnalyzer;

namespace Cygnus.SyntaxTree
{
    public class UnaryExpression : Expression
    {
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Unary;
            }
        }
        public Expression Value { get; private set; }
        public Operator Op { get; private set; }
        public UnaryExpression(Operator Op, Expression Value)
        {
            this.Op = Op;
            this.Value = Value;
        }
        public override Expression Eval()
        {
            var value = Value.Eval().GetValue<ConstantExpression>(ExpressionType.Constant);
            return UnaryOp(value, Op);
        }
        public ConstantExpression UnaryOp(ConstantExpression expr, Operator op)
        {
            switch (expr.constantType)
            {
                case ConstantType.Integer:
                    if (Op == Operator.UnaryPlus)
                        return new ConstantExpression(+(int)expr.Value, ConstantType.Integer);
                    else if (Op == Operator.UnaryMinus)
                        return new ConstantExpression(-(int)expr.Value, ConstantType.Integer);
                    else throw new NotSupportedException();
                case ConstantType.Double:
                    if (Op == Operator.UnaryPlus)
                        return new ConstantExpression(+(double)expr.Value, ConstantType.Integer);
                    else if (Op == Operator.UnaryMinus)
                        return new ConstantExpression(-(double)expr.Value, ConstantType.Integer);
                    else throw new NotSupportedException();
                case ConstantType.Boolean:
                    return new ConstantExpression(!(bool)expr.Value, ConstantType.Boolean);
                default:
                    throw new NotSupportedException("Not supported unary operator '" + Op + "'");
            }
        }
    }
}