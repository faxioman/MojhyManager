using System;
using System.Collections.Generic;
using System.Text;

namespace Mojhy.Utils.DrawingExt
{
    /// <summary>
    /// 3D PointF class
    /// </summary>
    public struct Point3DF
    {
        Matrix  matrix;
        public Point3DF(System.Single X, System.Single Y, System.Single Z)
        {
            matrix = new Matrix(1, 4);
            matrix[0, 0] = X;
            matrix[0, 1] = Y;
            matrix[0, 2] = Z;
            matrix[0, 3] = 1;
        }
        public Point3DF(Matrix Matrix)
        {
            if (Matrix.Rows != 1 || Matrix.Columns != 4)
                throw new System.Exception("Matrix must be a 1 by 4 matrix.");
            matrix = Matrix;
        }
        public System.Single X
        {
            get
            {
                return (System.Single)matrix[0, 0];
            }
            set
            {
                matrix[0, 0] = value;
            }
        }
        public System.Single Y
        {
            get
            {
                return (System.Single)matrix[0, 1];
            }
            set
            {
                matrix[0, 1] = value;
            }
        }
        public System.Single Z
        {
            get
            {
                return (System.Single)matrix[0, 2];
            }
            set
            {
                matrix[0, 2] = value;
            }
        }

        public Matrix Matrix
        {
            get
            {
                return matrix;
            }
        }
        public override System.String ToString()
        {
            return System.String.Format("({0},{1},{2})", X, Y, Z);
        }
    }
    /// <summary>
    /// Extended Matrix
    /// </summary>
    public class Matrix
    {
        System.Double[,] elements;

        public Matrix(System.Int32 Rows, System.Int32 Columns)
        {
            elements = new System.Double[Rows, Columns];
        }
        public System.Double this[System.Int32 Row, System.Int32 Column]
        {
            get
            {
                return elements[Row, Column];
            }
            set
            {
                elements[Row, Column] = value;
            }
        }
        public System.Int32 Rows
        {
            get
            {
                return elements.GetLength(0);
            }
        }
        public System.Int32 Columns
        {
            get
            {
                return elements.GetLength(1);
            }
        }
        public static Matrix operator *(Matrix Operand1, Matrix Operand2)
        {
            if (Operand1.Columns != Operand2.Rows)
                throw new System.Exception("The number of columns in the first Operand " +
                  "must be equal to the number of rows in the second Operand");

            Matrix result = new Matrix(Operand1.Rows, Operand2.Columns);

            for (System.Int32 i = 0; i < result.Rows; i++)
            {
                for (System.Int32 j = 0; j < result.Columns; j++)
                {
                    result[i, j] = Multiply(Operand1, i, Operand2, j);
                }
            }

            return result;
        }
        private static System.Double Multiply(Matrix Operand1, System.Int32 Row, Matrix Operand2, System.Int32 Column)
        {
            System.Double sum = 0;

            for (System.Int32 i = 0; i < Operand1.Columns; i++)
            {
                sum += Operand1[Row, i] * Operand2[i, Column];
            }

            return sum;
        }
    }
}
