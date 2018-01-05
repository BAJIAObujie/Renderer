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
        public UV uv;
        public Vector normal;
        public MyColor color;
        public Vector WorldPos;

        /// <summary>
        /// 向量 uv 颜色全部为0，除了向量的t
        /// </summary>
        /// 
        
        public Vertex()
        {
            this.point = new Vector();
            this.uv.u = 0;
            this.uv.v = 0;
            this.normal = new Vector();
            this.color = new MyColor();
            this.WorldPos = new Vector();
        }
        
        public Vertex(double x,double y,double z,double t,UV uv)
        {
            this.point = new Vector(x,y,z,t);
            this.uv.u = uv.u;
            this.uv.v = uv.v;
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
            this.uv.u = copy.uv.u;
            this.uv.v = copy.uv.v;
            this.normal = new Vector(copy.normal.x, copy.normal.y, copy.normal.z, copy.normal.t);
            this.color = new MyColor(copy.color.r, copy.color.g, copy.color.b);
            this.WorldPos = new Vector(copy.WorldPos.x, copy.WorldPos.y, copy.WorldPos.z, copy.WorldPos.t);
        }
        public static Vertex Interp(Vertex v1, Vertex v2, double t)
        {
            Vertex ret = new Vertex();
            ret.point = Vector.Interp(v1.point, v2.point, t);
            //ret.point.t = v1.point.t + (v2.point.t - v1.point.t) * t;
            ret.uv.u = v1.uv.u + (v2.uv.u - v1.uv.u) * t;
            ret.uv.v = v1.uv.v + (v2.uv.v - v1.uv.v) * t;
            ret.normal = Vector.Interp(v1.normal, v2.normal, t);
            ret.color = MyColor.Interp(v1.color, v2.color, t);
            ret.WorldPos = Vector.Interp(v1.WorldPos, v2.WorldPos, t);
            return ret;
        }
        public Vertex Interp(Vertex offset, double t)
        {
            this.point = Vector.Interp(this.point, offset.point, t);
            this.uv.u = this.uv.u + offset.uv.u * t;
            this.uv.v = this.uv.v + offset.uv.v * t;
            this.normal = Vector.Interp(this.normal, offset.normal, t);
            this.color = MyColor.Interp(this.color, offset.color, t);
            this.WorldPos = Vector.Interp(this.WorldPos, offset.WorldPos, t);
            return this;
        }

        /// <summary>
        /// 当前顶点值加上step
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public Vertex NextStep(Vertex step)
        {
            this.point += step.point;
            this.point.t += step.point.t;
            this.uv.u += step.uv.u;
            this.uv.v += step.uv.v;
            this.normal += step.normal;
            this.color += step.color;
            this.WorldPos += step.WorldPos;
            return this;
        }











    }
}
