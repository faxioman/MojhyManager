using System;
using System.Collections.Generic;
using System.Text;

namespace Mojhy.Utils.DrawingsExt
{
    public struct Point3DF
    {
        Mamanze.Math.Matrix
          matrix;

        #region public Point3DF(System.Single X, System.Single Y, System.Single Z) : this()
        public Point3DF(System.Single X, System.Single Y, System.Single Z)
        {
            matrix = new Mamanze.Math.Matrix(1, 4);
            matrix[0, 0] = X;
            matrix[0, 1] = Y;
            matrix[0, 2] = Z;
            matrix[0, 3] = 1;
        }
        #endregion

        #region public Point3DF(Mamanze.Math.Matrix Matrix)
        public Point3DF(Mamanze.Math.Matrix Matrix)
        {
            if (Matrix.Rows != 1 || Matrix.Columns != 4)
                throw new System.Exception("Matrix must be a 1 by 4 matrix.");
            matrix = Matrix;
        }
        #endregion

        #region public System.Single X
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
        #endregion

        #region public System.Single Y
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
        #endregion

        #region public System.Single Z
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
        #endregion

        #region public Mamanze.Math.Matrix Matrix
        public Mamanze.Math.Matrix Matrix
        {
            get
            {
                return matrix;
            }
        }
        #endregion

        #region public override System.String ToString()
        public override System.String ToString()
        {
            return System.String.Format("({0},{1},{2})", X, Y, Z);
        }
        #endregion

    }
}
