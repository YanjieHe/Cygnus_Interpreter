using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Cygnus.Errors;
using Cygnus.Extensions;
using Cygnus.DataStructures;
namespace Cygnus.Expressions
{
    public class ConstantExpression : Expression, IDotAccessible
    {
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Constant;
            }
        }
        public CygnusObject Value { get; protected set; }
        public CygnusType type
        {
            get
            {
                return Value.type;
            }
        }
        public ConstantExpression(CygnusObject Value)
        {
            this.Value = Value;
        }
        public override void Display(Scope scope)
        {
            Value.Display(scope);
        }
        public double GetDouble()
        {
            return type == CygnusType.Integer ? (Value as CygnusInteger).Value : (Value as CygnusDouble).Value;
        }
        public override string ToString()
        {
            return string.Format("(Constant: {0}  Type: {1})", Value, type);
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals(obj);
        }
        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public override Expression Eval(Scope scope)
        {
            return new ConstantExpression(Value);
        }
        public Expression GetByDot(string field, bool IsMethod)
        {
            return (Value as IDotAccessible).GetByDot(field, IsMethod);
        }

        public void SetByDot(CygnusObject obj, string field, bool IsMethod)
        {
            (Value as IDotAccessible).SetByDot(obj, field, IsMethod);
        }
    }
}
