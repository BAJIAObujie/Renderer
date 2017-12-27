using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Vertex
    {
        public Vector point;
        public double u, v;
        public Vector normal;
        public MyColor color;
        public Vector WorldPos;

        /// <summary>
        /// 向量 uv 颜色全部为0，除了向量的t
        /// </summary>
        public Vertex()
        {
            this.point = new Vector();
            this.u = 0;
            this.v = 0;
            this.normal = new Vector();
            this.color = new MyColor();
            this.WorldPos = new Vector();
        }
        public Vertex(double x,double y,double z,double t,double u,double v)
        {
            this.point = new Vector(x,y,z,t);
            this.u = u;
            this.v = v;
            this.normal = new Vector();
            this.color = new MyColor();
            this.WorldPos = new Vector(x, y, z, t);
        }
        /*
        public Vertex(Vector point,double u,double v)
        {
            this.point = new Vector(point.x, point.y, point.z, point.t);
            this.u = u;
            this.v = v;
            this.normal = new Vector();
            this.color = new MyColor();
            this.WorldPos = new Vector(point.x, point.y, point.z, point.t);
        }
        */

        /// <summary>
        /// normal is worldnormal
        /// </summary>
        /// <param name="copy"></param>
        public Vertex(Vertex copy)
        {
            this.point = new Vector(copy.point.x, copy.point.y, copy.point.z, copy.point.t);
            this.u = copy.u;
            this.v = copy.v;
            this.normal = new Vector(copy.normal.x, copy.normal.y, copy.normal.z, copy.normal.t);
            this.color = new MyColor(copy.color.r, copy.color.g, copy.color.b);
            this.WorldPos = new Vector(copy.WorldPos.x, copy.WorldPos.y, copy.WorldPos.z, copy.WorldPos.t);
        }
        public static Vertex Interp(Vertex v1, Vertex v2, double t)
        {
            Vertex ret = new Vertex();
            ret.point = Vector.Interp(v1.point, v2.point, t);
            ret.u = v1.u + (v2.u - v1.u) * t;
            ret.v = v1.v + (v2.v - v1.v) * t;
            ret.normal = Vector.Interp(v1.normal, v2.normal, t);
            ret.color = MyColor.Interp(v1.color, v2.color, t);
            ret.WorldPos = Vector.Interp(v1.WorldPos, v2.WorldPos, t);
            return ret;
        }

        /// <summary>
        /// 当前顶点值加上step
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public Vertex NextStep(Vertex step)
        {
            this.point += step.point;
            this.u += step.u;
            this.v += step.v;
            this.normal += step.normal;
            this.color += step.color;
            this.WorldPos += step.WorldPos;
            return this;
        }











    }
}
