using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.Expressions;
using InnerType = System.Int32;
using ThisType = Cygnus.DataStructures.CygnusInteger;

namespace Cygnus.DataStructures
{
    public class CygnusInteger : CygnusObject, IComputable, IComparable
    {
        public override CygnusType type
        {
            get
            {
                return CygnusType.Integer;
            }
        }
        public InnerType Value;
        public CygnusInteger(InnerType Value)
        {
            this.Value = Value;
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }
        public override void Display(Scope scope)
        {
            Console.Write(Value);
        }
        public CygnusObject Add(CygnusObject Other)
        {
            return Value + (Other as ThisType).Value;
        }

        public CygnusObject Subtract(CygnusObject Other)
        {
            return Value - (Other as ThisType).Value;
        }

        public CygnusObject Multiply(CygnusObject Other)
        {
            return Value * (Other as ThisType).Value;
        }

        public CygnusObject Divide(CygnusObject Other)
        {
            return Value / (Other as ThisType).Value;
        }

        public CygnusObject Power(CygnusObject Other)
        {
            return (ThisType)Math.Pow(Value, (Other as ThisType).Value);
        }

        public CygnusObject UnaryPlus()
        {
            return +Value;
        }

        public CygnusObject Negate()
        {
            return -Value;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo((obj as ThisType).Value);
        }
    }
}
