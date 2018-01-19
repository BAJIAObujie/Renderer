using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Polygon
    {
        public int Count;
        public Vector normal;
        public FaceUnit p0index, p1index, p2index, p3index;
        
        /// <summary>
        /// 默认多边形构造方法
        /// </summary>
        /// 
        
        public Polygon()
        {
            this.Count = 0;
            this.normal = new Vector();
            this.p0index = new FaceUnit();
            this.p1index = new FaceUnit();
            this.p2index = new FaceUnit();
            this.p3index = new FaceUnit();
        }
        public Polygon(int index0, int index1, int index2, int index3)
        {
            Count = 4;
            this.p0index.IndexVertex = index0;
        }
        
        public Polygon(FaceUnit p0index, FaceUnit p1index, FaceUnit p2index)
        {
            this.p0index = p0index;
            this.p1index = p1index;
            this.p2index = p2index;
            this.p3index = new FaceUnit();
            this.Count = 3;
            this.normal = new Vector();
        }
        public Polygon(FaceUnit p0index, FaceUnit p1index, FaceUnit p2index, FaceUnit p3index)
        {
            this.p0index = p0index;
            this.p1index = p1index;
            this.p2index = p2index;
            this.p3index = p3index;
            this.Count = 4;
            this.normal = new Vector();
        }

        public void SetNormal(Vector Normal)
        {
            this.normal = Normal;
        }

    }


    struct FaceUnit
    {
        public int IndexVertex;
        public int IndexTexture;
        public int IndexNormal;
        public FaceUnit(int Vertex,int Texture,int Normal)
        {
            this.IndexVertex = Vertex;
            this.IndexTexture = Texture;
            this.IndexNormal = Normal;
        }
    }
    struct UV
    {
        public double u;
        public double v;
        public UV(double u,double v)
        {
            this.u = u;
            this.v = v;
        }
    }
}
