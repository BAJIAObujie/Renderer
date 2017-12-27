using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Triangle
    {
        public Vector normal;
        public int p0index, p1index, p2index;

        /// <summary>
        /// 默认三角面构造方法 000 点集null
        /// </summary>
        public Triangle()
        {
            this.p0index = 0;
            this.p1index = 0;
            this.p2index = 0;
            this.normal = new Vector();
        }

        /// <summary>
        /// 三角面生成方法
        /// </summary>
        /// <param name="p0index"></param>
        /// <param name="p1index"></param>
        /// <param name="p2index"></param>
        public Triangle(int p0index, int p1index, int p2index)
        {
            this.p0index = p0index;
            this.p1index = p1index;
            this.p2index = p2index;
            this.normal = new Vector();
        }

        public Triangle(int p0index, int p1index, int p2index,Vector normal)
        {
            this.p0index = p0index;
            this.p1index = p1index;
            this.p2index = p2index;
            this.normal = new Vector(normal.x, normal.y, normal.z, normal.t);
        }
    }
}
