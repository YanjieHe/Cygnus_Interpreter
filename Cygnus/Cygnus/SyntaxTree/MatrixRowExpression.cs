using System;
using MathNet.Numerics.LinearAlgebra;
namespace Cygnus.SyntaxTree
{
    public sealed class MatrixRowExpression : Expression, IIndexable
    {
        public Matrix<double> Data { get; private set; }
        public int RowIndex { get; private set; }
        public MatrixRowExpression(Matrix<double> Data, int RowIndex)
        {
            this.Data = Data;
            this.RowIndex = RowIndex;
        }
        public Expression this[Expression index, Scope scope]
        {
            get
            {
                int col = index.As<int>(scope);
                return Data[RowIndex, col];
            }

            set
            {
                int col = index.As<int>(scope);
                Data[RowIndex, col] = value.AsConstant(scope).GetDouble();
            }
        }
        public ConstantExpression Length
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.MatrixRow;
            }
        }
        public override Expression Eval(Scope scope)
        {
            return this;
        }
    }
}
