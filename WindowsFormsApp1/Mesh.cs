using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Mesh
    {
        public List<Vector> Vectors;  //顶点位置的集合
        public List<Polygon> Faces;   //面的集合
        public List<Polygon> ShownFaces; //去除消隐的面
        public List<Triangle> TriangleFaces;  //要显示的三角形的集合

        public List<UV> UVs;
        public List<Vector> Normals;

        //public Vector CubeCenter;
        /// <summary>
        /// 调用此方法则默认生成一个cube
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="faces"></param>
        public Mesh()
        {
            this.Vectors = new List<Vector>(8)
            {
                new Vector(0.0,      0.0,      0.0,        1.0),    // x0
                new Vector(100.0,    0.0,      0.0 ,       1.0),    // x1   
                new Vector(0.0,      0.0,      100.0 ,     1.0),    // x2 
                new Vector(100.0,    0.0,      100.0,      1.0),    // x3   
                new Vector(0.0,      100.0,    0.0,        1.0),    // x4
                new Vector(100.0,    100.0,    0.0,        1.0),    // x5   
                new Vector(0.0,      100.0,    100.0,      1.0),    // x6 
                new Vector(100.0,    100.0,    100.0,      1.0)     // x7   

            };
            this.Faces = new List<Polygon>(1)
            {
             //改为以左手为准，左手坐标系
             /*
                new Polygon(1,5,7,3),
                new Polygon(3,7,6,2),
                new Polygon(5,4,6,7),
                new Polygon(1,0,4,5),
                new Polygon(0,1,3,2),
                new Polygon(0,2,6,4),
                */
            };
            Scale(1.5);
            //CubeCenter = (this.Vertices[0].point + this.Vertices[7].point)/2;
            Move(new Vector(-50, -50, -50, 1));
        }
        public void Scale(double mul)
        {
            for (int i = 0; i < this.Vectors.Count; i++)
            {
                this.Vectors[i].Scale(mul);
            } 
        }
        public void Move(Vector m)
        {
            for (int i = 0; i < this.Vectors.Count; i++)
            {
                this.Vectors[i] += m;
            }
        }
        //--------------OBJReader-----------------
        /// <summary>
        /// 调用此方法则是调用外部obj模型文件
        /// </summary>
        /// <param name="IsOBJReader"></param>
        public Mesh(bool IsOBJReader)
        {
            //模块写得太差劲了！
        }



    }





    

    
}
