using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Mojhy.Utils.DrawingExt
{
    public class RectangleObject
    {
        private Rectangle l_rctInternalRectangle;
        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of this <see cref="T:PointObject"/>.
        /// </summary>
        /// <value>The x-coordinate of the upper-left corner.</value>
        public int X
        {
            get { return l_rctInternalRectangle.X; }
            set { l_rctInternalRectangle.X = value; }
        }
        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of this <see cref="T:PointObject"/>.
        /// </summary>
        /// <value>The y-coordinate of the upper-left corner.</value>
        public int Y
        {
            get { return l_rctInternalRectangle.Y; }
            set { l_rctInternalRectangle.Y = value; }
        }
        /// <summary>
        /// Gets or sets the width of this <see cref="T:PointObject"/>.
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get { return l_rctInternalRectangle.Width; }
            set { l_rctInternalRectangle.Width = value; }
        }
        /// <summary>
        /// Gets or sets the height of this <see cref="T:PointObject"/>.
        /// </summary>
        /// <value>The height.</value>
        public int Height
        {
            get { return l_rctInternalRectangle.Height; }
            set { l_rctInternalRectangle.Height = value; }
        }
        /// <summary>
        /// Gets the y-coordinate of the top edge of this <see cref="T:PointObject"/>.
        /// </summary>
        /// <value>The y-coordinate of the top edge as int.</value>
        public int Top
        {
            get { return l_rctInternalRectangle.Top; }
        }
        /// <summary>
        /// Gets the x-coordinate of the left edge of this <see cref="T:PointObject"/>.
        /// </summary>
        /// <value>The x-coordinate of the left edge as int.</value>
        public int Left
        {
            get { return l_rctInternalRectangle.Left; }
        }
        /// <summary>
        /// Gets the y-coordinate that is the sum of the Y and Height property values of this <see cref="T:PointObject"/>.
        /// </summary>
        /// <value>The y-coordinate of the bottom as int.</value>
        public int Bottom
        {
            get { return l_rctInternalRectangle.Bottom; }
        }
        /// <summary>
        /// Gets the x-coordinate that is the sum of X and Width property values of this <see cref="T:PointObject"/>.
        /// </summary>
        /// <value>The x-coordinate of the right as int.</value>
        public int Right
        {
            get { return l_rctInternalRectangle.Right; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:RectangleObject"/> class.
        /// </summary>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public RectangleObject(int x, int y, int width, int height){
            l_rctInternalRectangle = new Rectangle(x, y, width, height);
        }
        public bool Contains (PointObject ptObj)
        {
            if ((ptObj.X < this.X) || (ptObj.X > this.Right) || (ptObj.Y < this.Y) || (ptObj.Y > this.Bottom))
                return false;
            else
                return true;
        }
    }
    /// <summary>
    /// PointObject class
    /// </summary>
    public class PointObject
    {
        private Point l_ptInternalPoint;
        /// <summary>
        /// Gets or sets the x-coordinate of this <see cref="T:PointObject"/>.
        /// </summary>
        /// <value>The x-coordinate.</value>
        public int X
        {
            get { return l_ptInternalPoint.X; }
            set { l_ptInternalPoint.X = value; }
        }
        /// <summary>
        /// Gets or sets the y-coordinate of this <see cref="T:PointObject"/>.
        /// </summary>
        /// <value>The y-coordinate.</value>
        public int Y
        {
            get { return l_ptInternalPoint.Y; }
            set { l_ptInternalPoint.Y = value; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:PointObject"/> class.
        /// </summary>
        /// <param name="X">The x-coordinate.</param>
        /// <param name="Y">The y-coordinate.</param>
        public PointObject(int X, int Y)
        {
            l_ptInternalPoint = new Point(X, Y);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:PointObject"/> class.
        /// </summary>
        public PointObject()
        {
            l_ptInternalPoint = new Point(0, 0);
        }
    }
    /// <summary>
    /// 3D PointF class
    /// </summary>
    public class Point3D
    {
        Matrix  matrix;
        public Point3D(System.Int32 X, System.Int32 Y, System.Int32 Z)
        {
            matrix = new Matrix(1, 4);
            matrix[0, 0] = X;
            matrix[0, 1] = Y;
            matrix[0, 2] = Z;
            matrix[0, 3] = 1;
        }
        public Point3D(Matrix Matrix)
        {
            if (Matrix.Rows != 1 || Matrix.Columns != 4)
                throw new System.Exception("Matrix must be a 1 by 4 matrix.");
            matrix = Matrix;
        }
        public System.Int32 X
        {
            get
            {
                return (System.Int32)matrix[0, 0];
            }
            set
            {
                matrix[0, 0] = value;
            }
        }
        public System.Int32 Y
        {
            get
            {
                return (System.Int32)matrix[0, 1];
            }
            set
            {
                matrix[0, 1] = value;
            }
        }
        public System.Int32 Z
        {
            get
            {
                return (System.Int32)matrix[0, 2];
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
