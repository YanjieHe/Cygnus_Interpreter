using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.Expressions;
using Cygnus.Extensions;
namespace Cygnus.DataStructures
{
    public abstract class CygnusObject : IDisplayable
    {
        public abstract CygnusType type { get; }

        public virtual void Display(Scope scope)
        {
            Console.Write(type);
        }

        public virtual CygnusObject FromObject(CygnusObject obj)
        {
            throw new NotImplementedException();
        }
        public static implicit operator CygnusObject(int Value)
        {
            return new CygnusInteger(Value);
        }
        public static implicit operator CygnusObject(double Value)
        {
            return new CygnusDouble(Value);
        }
        public static implicit operator CygnusObject(bool Value)
        {
            return new CygnusBoolean(Value);
        }
        public static implicit operator CygnusObject(string Value)
        {
            return new CygnusString(Value);
        }
        public static explicit operator int(CygnusObject obj)
        {
            return (obj as CygnusInteger).Value;
        }
        public static explicit operator double(CygnusObject obj)
        {
            return (obj as CygnusDouble).Value;
        }
        public static explicit operator bool(CygnusObject obj)
        {
            return (obj as CygnusBoolean).Value;
        }
        public static explicit operator string(CygnusObject obj)
        {
            return (obj as CygnusString).Value;
        }
    }
}
