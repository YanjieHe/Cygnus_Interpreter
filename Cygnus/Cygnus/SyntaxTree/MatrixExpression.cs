using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Cygnus.SyntaxTree
{
    public class MatrixExpression : Expression, IComputable,IIndexable
    {
        public Matrix<double> Data { get; private set; }

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

        public Expression this[Expression index, Scope scope]
        {
            get
            {
                int row = index.As<int>(scope);
                return new MatrixRowExpression(Data, row);
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public MatrixExpression(Matrix<double> Data)
        {
            this.Data = Data;
        }
        public MatrixExpression(params double[][] rows)
        {
            Data = DenseMatrix.OfRowArrays(rows);
        }
        public override string ToString()
        {
            return Data.ToString();
        }
        public override Expression Eval(Scope scope)
        {
            return this;
        }

        public Expression Add(Expression Other)
        {
            if (Other.NodeType == ExpressionType.Matrix)
            {
                return new MatrixExpression(Data + ((Other as MatrixExpression).Data));
            }
            else if (Other.NodeType == ExpressionType.Constant)
            {
                return new MatrixExpression(Data + ((Other as ConstantExpression).GetDouble()));
            }
            else throw new ArgumentException();
        }

        public Expression Subtract(Expression Other)
        {
            if (Other.NodeType == ExpressionType.Matrix)
            {
                return new MatrixExpression(Data - ((Other as MatrixExpression).Data));
            }
            else if (Other.NodeType == ExpressionType.Constant)
            {
                return new MatrixExpression(Data - ((Other as ConstantExpression).GetDouble()));
            }
            else throw new ArgumentException();
        }
        public Expression Multiply(Expression Other)
        {
            if (Other.NodeType == ExpressionType.Matrix)
            {
                return new MatrixExpression(Data * ((Other as MatrixExpression).Data));
            }
            else if (Other.NodeType == ExpressionType.Constant)
            {
                return new MatrixExpression(Data * ((Other as ConstantExpression).GetDouble()));
            }
            else throw new ArgumentException();
        }

        public Expression Divide(Expression Other)
        {
            if (Other.NodeType == ExpressionType.Matrix)
            {
                return new MatrixExpression(Data * ((Other as MatrixExpression).Data).Inverse());
            }
            else if (Other.NodeType == ExpressionType.Constant)
            {
                return new MatrixExpression(Data / ((Other as ConstantExpression).GetDouble()));
            }
            else throw new ArgumentException();
        }

        public Expression Power(Expression Other)
        {
            if (Other.NodeType == ExpressionType.Constant)
            {
                return new MatrixExpression(
                    Data.Power(
                        (int)((Other as ConstantExpression).Value)));
            }
            else throw new ArgumentException();
        }
    }
}
