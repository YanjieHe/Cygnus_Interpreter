using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InnerType = System.String;
using ThisType = Cygnus.DataStructures.CygnusString;
using Cygnus.Expressions;
using System.Collections;

namespace Cygnus.DataStructures
{
    public class CygnusString : CygnusObject, IComputable, IComparable, IEnumerable<CygnusObject>
    {
        public override CygnusType type
        {
            get
            {
                return CygnusType.String;
            }
        }
        public InnerType Value;
        public CygnusString(InnerType Value)
        {
            this.Value = Value;
        }
        public override bool Equals(CygnusObject other)
        {
            if (other == null) return false;
            else return Value.Equals((other as ThisType).Value);
        }
        public override CygnusObject FromObject(CygnusObject obj)
        {
            return new CygnusString(obj.ToString());
        }
        public override string ToString()
        {
            return Value;
        }
        public override void Display(Scope scope)
        {
            Console.Write(Value);
        }
        public CygnusObject Add(CygnusObject Other)
        {
            return Value + (Other as CygnusString).Value;
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
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo((obj as ThisType).Value);
        }

        public IEnumerator<CygnusObject> GetEnumerator()
        {
            foreach (var item in Value)
            {
                yield return item.ToString();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this.AsEnumerable();
        }
    }
}
