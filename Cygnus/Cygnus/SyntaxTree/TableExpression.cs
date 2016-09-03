using System.Collections.Generic;
using Cygnus.Errors;
using Cygnus.DataStructures;
using System;
using System.Linq;
namespace Cygnus.SyntaxTree
{
    public class TableExpression : Expression, IIndexable
    {
        public TableExpression Parent { get; set; }
        public Table<Expression> table { get; private set; }
        public TableExpression(params Expression[]values)
        {
            table = new Table<Expression>(values);
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Table;
            }
        }

        public ConstantExpression Length
        {
            get
            {
                return Constant(table.Count(),ConstantType.Integer);
            }
        }

        public Expression this[Expression index, Scope scope]
        {
            get
            {
                var i = index.AsConstant(scope);
                switch (i.type)
                {
                    case ConstantType.Integer:
                        return table[(int)i.Value];
                    case ConstantType.Double:
                    case ConstantType.Boolean:
                    case ConstantType.Char:
                    case ConstantType.String:
                        return table[i.Value];
                    default:
                        throw new NotImplementedException();
                }
            }

            set
            {
                var i = index.AsConstant(scope);
                switch (i.type)
                {
                    case ConstantType.Integer:
                        table[(int)i.Value] = value.GetValue(scope); break;
                    case ConstantType.Double:
                    case ConstantType.Boolean:
                    case ConstantType.Char:
                    case ConstantType.String:
                        table[i.Value] = value.GetValue(scope); break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        public override Expression Eval(Scope scope)
        {
            return this;
        }
        public override string ToString()
        {
            return "Table";
        }
    }
}
