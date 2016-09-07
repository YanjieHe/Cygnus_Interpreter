using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InnerType = System.Boolean;
using ThisType = Cygnus.DataStructures.CygnusBoolean;
using Cygnus.Expressions;
namespace Cygnus.DataStructures
{
    public class CygnusBoolean : CygnusObject, IComputable
    {
        public override CygnusType type
        {
            get
            {
                return CygnusType.Boolean;
            }
        }
        public InnerType Value;
        public CygnusBoolean(InnerType Value)
        {
            this.Value = Value;
        }
        public override bool Equals(CygnusObject other)
        {
            if (other == null) return false;
            else return Value.Equals((other as ThisType).Value);
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
            throw new NotImplementedException();
        }

        public CygnusObject Subtract(CygnusObject Other)
        {
            throw new NotImplementedException();
        }

        public CygnusObject Multiply(CygnusObject Other)
        {
            throw new NotImplementedException();
        }

        public CygnusObject Divide(CygnusObject Other)
        {
            throw new NotImplementedException();
        }

        public CygnusObject Power(CygnusObject Other)
        {
            throw new NotImplementedException();
        }

        public CygnusObject UnaryPlus()
        {
            throw new NotImplementedException();
        }

        public CygnusObject Negate()
        {
            return !Value;
        }
    }
}
