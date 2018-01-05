using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Vector
    {
        public double x, y, z, t;
        
        public Vector()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
            this.t = 1;
        }
        
        public Vector(double x, double y, double z, double t)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.t = t;
        }

        public Vector(Vector copy)
        {
            this.x = copy.x;
            this.y = copy.y;
            this.z = copy.z;
            this.t = copy.t;
        }
        
        public static Vector operator +(Vector lhs, Vector rhs)
        {
            Vector ret = new Vector(lhs);
            ret.x += rhs.x;
            ret.y += rhs.y;
            ret.z += rhs.z;
            //ret.t = lhs.t + rhs.t;
            return ret;
        }
        public static Vector operator -(Vector lhs, Vector rhs)
        {
            Vector ret = new Vector(lhs);
            ret.x -= rhs.x;
            ret.y -= rhs.y;
            ret.z -= rhs.z;
            //ret.t = lhs.t - rhs.t;
            return ret;
        }
        public Vector NormalizedVector()
        {
            double length = Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
            if (length != 0)
            {
                double inv = 1.0 / length;
                this.x *= inv;
                this.y *= inv;
                this.z *= inv;
            }
            return this;
        }
        public static Vector CrossMultiply(Vector a, Vector b)
        {
            Vector ret = new Vector();
            ret.x = a.y * b.z - a.z * b.y;
            ret.y = a.z * b.x - a.x * b.z;
            ret.z = a.x * b.y - a.y * b.x;
            //ret.t = 0;
            return ret;
        }
        public double Length()
        {
            return Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
        }

        public Vector ChangeTtoOne()
        {
            this.x /= this.t;
            this.y /= this.t;
            this.z /= this.t;
            this.t = 1;
            return this;
        }

        /// <summary>
        /// 向量和矩阵的乘积 向量、顶点进行矩阵变换
        /// </summary>
        /// <param name="v"></param>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Vector operator *(Vector v, Matrix mat)
        {
            Vector ret = new Vector();
            ret.x = v.x * mat[0, 0] + v.y * mat[1, 0] + v.z * mat[2, 0] + v.t * mat[3, 0];
            ret.y = v.x * mat[0, 1] + v.y * mat[1, 1] + v.z * mat[2, 1] + v.t * mat[3, 1];
            ret.z = v.x * mat[0, 2] + v.y * mat[1, 2] + v.z * mat[2, 2] + v.t * mat[3, 2];
            ret.t = v.x * mat[0, 3] + v.y * mat[1, 3] + v.z * mat[2, 3] + v.t * mat[3, 3];
            return ret;
        }

        public static Vector operator /(Vector v, double t)
        {
            Vector ret = new Vector();
            ret.x = v.x / t;
            ret.y = v.y / t;
            ret.z = v.z / t;
            //ret.t = v.t / t;
            return ret;
        }

        public static double DotMultiply(Vector lhs, Vector rhs)
        {
            return (lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z);
        }

        public static Vector Interp(Vector v1, Vector v2, double t)
        {
            Vector ret = new Vector();
            ret.x = v1.x + (v2.x - v1.x) * t;
            ret.y = v1.y + (v2.y - v1.y) * t;
            ret.z = v1.z + (v2.z - v1.z) * t;
            ret.t = v1.t + (v2.t - v1.t) * t;
            return ret;
        }
        
        public void Scale(double t)
        {
            this.x *= t;
            this.y *= t;
            this.z *= t;
        }

        /// <summary>
        /// Inverse x y z
        /// </summary>
        /// <returns></returns>
        public Vector InverseSelf()
        {
            this.x = -this.x;
            this.y = -this.y;
            this.z = -this.z;
            return this;
        }
    }
}
