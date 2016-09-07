using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InnerType = System.Double;
using ThisType = Cygnus.DataStructures.CygnusDouble;
using Cygnus.Expressions;
namespace Cygnus.DataStructures
{
    public class CygnusDouble : CygnusObject,IComputable,IComparable
    {
        public override CygnusType type
        {
            get
            {
                return CygnusType.Double;
            }
        }
        public InnerType Value;
        public CygnusDouble(InnerType Value)
        {
            this.Value = Value;
        }
        public override bool Equals(CygnusObject other)
        {
            if (other == null) return false;
            else return Value.Equals((other as ThisType).Value);
        }
        public override void Display(Scope scope)
        {
            Console.Write(Value);
        }
        public override CygnusObject FromObject(CygnusObject obj)
        {
            if (obj is CygnusInteger)
            {
                return new CygnusDouble((obj as CygnusInteger).Value);
            }
            else if (obj is CygnusDouble)
            {
                return (CygnusDouble)obj;
            }
            else throw new NotSupportedException();
        }
        public override string ToString()
        {
            return this.Value.ToString();
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
            return Math.Pow(Value, (Other as ThisType).Value);
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
