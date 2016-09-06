using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cygnus.Expressions;

namespace Cygnus.Libraries
{
    public static class MatrixFunctions
    {
        //public static Expression Mat_Zeros(Expression[] args, Scope scope)
        //{
        //    switch (args.Length)
        //    {
        //        case 1:
        //            int n = args.Single().As<int>(scope);
        //            return DenseMatrix.OfArray(new double[n, n]);
        //        case 2:
        //            int row = args[0].As<int>(scope);
        //            int col = args[1].As<int>(scope);
        //            return DenseMatrix.OfArray(new double[row, col]);
        //        default:
        //            throw new ArgumentException();
        //    }
        //}
        //public static Expression Mat_Ones(Expression[] args, Scope scope)
        //{
        //    switch (args.Length)
        //    {
        //        case 1:
        //            int n = args.Single().As<int>(scope);
        //            return DenseMatrix.Create(n, n, 1.0);
        //        case 2:
        //            int row = args[0].As<int>(scope);
        //            int col = args[1].As<int>(scope);
        //            return DenseMatrix.Create(row, col, 1.0);
        //        default:
        //            throw new ArgumentException();
        //    }
        //}
        //public static Expression Mat_Inverse(Expression[] args, Scope scope)
        //{
        //    //return (args.Single().AsMatrix(scope).Value as Matrix<double>).Inverse();
        //}
        //public static Expression Mat_Dot(Expression[] args, Scope scope)
        //{
        //    var a = args[0].AsMatrix(scope).Value as Matrix<double>;
        //    var b = args[1].AsMatrix(scope).Value as Matrix<double>;
        //    return a.PointwiseMultiply(b);
        //}
        //public static Expression Mat_Transpose(Expression[] args, Scope scope)
        //{
        //    return (args.Single().AsMatrix(scope).Value as Matrix<double>).Transpose();
        //}
    }
}
