using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Matrix
    {
        private double[,] MyMatrix;

        /// <summary>
        /// 初始化矩阵函数,初始化为0
        /// </summary>
        public Matrix()
        {
            MyMatrix = new double[4, 4]
            {
                {0,0,0,0 },
                {0,0,0,0 },
                {0,0,0,0 },
                {0,0,0,1 }
            };
        }
        public Matrix(Matrix mat)
        {
            MyMatrix = new double[4, 4]
            {
                {0,0,0,0 },
                {0,0,0,0 },
                {0,0,0,0 },
                {0,0,0,1 }
            };
            for (int i = 0; i<4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    MyMatrix[i,j] = mat[i,j];
                }
            }
        }
        /// <summary>
        /// 索引器重载
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public double this[int row, int column]
        {
            get
            {
                return MyMatrix[row,column];
            }
            set
            {
                MyMatrix[row, column] = value;
            }
        }

        /// <summary>
        /// 矩阵和矩阵的乘积 矩阵MVP变换 注意顺序
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns> 
        public static Matrix operator *(Matrix lhs, Matrix rhs)
        {
            Matrix ret = new Matrix();
            for (int i = 0; i < 4; i++) //次数
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        ret[i, j] += (lhs[i, k] * rhs[k, i]);
                    }
                }
            }
            return ret;
        }

        public Matrix Normalize()
        {
            if(MyMatrix[3,3] != 1)
            {
                for (int i = 0; i < 4; i++) //次数
                {
                    for (int j = 0; j < 4; j++)
                    {
                        MyMatrix[i, j] /= MyMatrix[3, 3];

                    }
                }
            }

            return this;
        }
        
        
        /// <summary>
        /// 归零
        /// </summary>
        /// <returns></returns>
        public Matrix SetZero()
        {
            for(int i = 0; i < 4; i++)
            {
                for (int j = 0; i < 4; i++)
                {
                    MyMatrix[i, j] = 0;
                }
            }
            return this;
        }

        public Matrix Translate(double x,double y,double z)
        {
            MyMatrix[3, 0] = x;
            MyMatrix[3, 1] = y;
            MyMatrix[3, 2] = z;
            return this;
        }

        public Matrix Scale(double x, double y, double z)
        {
            MyMatrix[0, 0] = x;
            MyMatrix[1, 1] = y;
            MyMatrix[2, 2] = z;
            return this;
        }




    }
}
