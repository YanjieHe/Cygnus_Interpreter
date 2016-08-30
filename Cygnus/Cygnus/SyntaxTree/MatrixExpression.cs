using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Cygnus.SyntaxTree
{
    public class MatrixExpression : ConstantExpression, IIndexable, ITable
    {
        public MatrixExpression(Matrix<double> Data) : base(Data, ConstantType.Matrix) { }
        public MatrixExpression(params double[][] rows) : base(ConstantType.Matrix)
        {
            Value = DenseMatrix.OfRowArrays(rows);
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Matrix;
            }
        }
        public ConstantExpression Length
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Expression this[string Name]
        {
            get
            {
                switch (Name)
                {
                    case "rowcount":
                        return (Value as Matrix<double>).RowCount;
                    case "colcount":
                        return (Value as Matrix<double>).ColumnCount;
                    default:
                        throw new NotImplementedException();
                }
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Expression this[Expression index, Scope scope]
        {
            get
            {
                int row = index.As<int>(scope);
                return new MatrixRowExpression(Value as Matrix<double>, row);
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public override Expression Eval(Scope scope)
        {
            return this;
        }
    }
}
