using System;

namespace Cygnus.DataStructures
{
    public class CygnusType : IEquatable<CygnusType>
    {
        public string TypeName { get; private set; }
        public int Width { get; private set; }
        public CygnusType(string TypeName, int Width)
        {
            this.TypeName = TypeName;
            this.Width = Width;
        }
        public readonly static CygnusType
            Null = new CygnusType("null", 0),
            Void = new CygnusType("void", 0),
            Integer = new CygnusType("int", 4),
            Double = new CygnusType("double", 8),
            Boolean = new CygnusType("boolean", 1),
            String = new CygnusType("string", int.MaxValue),
            Array = new CygnusType("array", 10),
            List = new CygnusType("list", 10),
            IEnumerable = new CygnusType("IEnumerable", 10);

        public override string ToString()
        {
            return string.Format("(Type: {0}, Width = {1})", TypeName, Width);
        }

        public bool Equals(CygnusType other)
        {
            return (Width == other.Width) && (TypeName == other.TypeName);
        }
    }
}
