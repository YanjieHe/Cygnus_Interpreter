using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra;

namespace Cygnus.SyntaxTree
{
    public class VectorExpression : ConstantExpression, IIndexable
    {
        public VectorExpression(Vector<double> Value) : base(Value, ConstantType.Vector) { }
        public VectorExpression(double[] Value) : base(ConstantType.Vector)
        {
            this.Value = DenseVector.OfArray(Value);
        }

        public Expression this[Expression index, Scope scope]
        {
            get
            {
                int i = index.As<int>(scope);
                return (Value as Vector<double>)[i];
            }

            set
            {
                int i = index.As<int>(scope);
                (Value as Vector<double>)[i] = value.AsConstant(scope).GetDouble();
            }
        }

        public ConstantExpression Length
        {
            get
            {
                return Constant((Value as Vector<double>).Count, ConstantType.Integer);
            }
        }

        public override Expression Eval(Scope scope)
        {
            return this;
        }

    }
}
