using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Polygon
    {
        public int p0index, p1index, p2index, p3index;
        public Vector normal;
        
        /// <summary>
        /// 默认多边形构造方法
        /// </summary>
        public Polygon()
        {
            this.p0index = 0;
            this.p1index = 0;
            this.p2index = 0;
            this.p3index = 0;
            this.normal = new Vector();
        }
        public Polygon(int p0index, int p1index, int p2index, int p3index)
        {
            this.p0index = p0index;
            this.p1index = p1index;
            this.p2index = p2index;
            this.p3index = p3index;
            this.normal = new Vector();
        }

    }
}
