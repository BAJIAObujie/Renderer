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
        /// 测试用例 正方形
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="faces"></param>
        public Mesh()
        {

            this.UVs = new List<UV>(4)
            {
                new UV(0,0),
                new UV(1,0),
                new UV(0,1),
                new UV(1,1)
            };

            this.Normals = new List<Vector>(8)
            {
                /*
                new Vector(-1, -1, -1, 1).NormalizedVector(),
                new Vector(1, -1, -1, 1).NormalizedVector(),
                new Vector(-1, -1, 1, 1).NormalizedVector(),
                new Vector(1, -1, 1, 1).NormalizedVector(),
                new Vector(-1, 1, -1, 1).NormalizedVector(),
                new Vector(1, 1, -1, 1).NormalizedVector(),
                new Vector(-1, 1, 1, 1).NormalizedVector(),
                new Vector(1, 1, 1, 1).NormalizedVector(),
                */
                new Vector(1, 0, 0, 1).NormalizedVector(),
                new Vector(0, 1, 0, 1).NormalizedVector(),
                new Vector(0, 0, 1, 1).NormalizedVector(),
                new Vector(-1, 0, 0, 1).NormalizedVector(),
                new Vector(0, -1, 0, 1).NormalizedVector(),
                new Vector(0, 0, -1, 1).NormalizedVector(),
            };

            this.Vectors = new List<Vector>(8)
            {
                new Vector(-50.0,      -50.0,      -50.0,        1.0),    // x0
                new Vector(50.0,    -50.0,      -50.0 ,       1.0),    // x1   
                new Vector(-50.0,      -50.0,      50.0 ,     1.0),    // x2 
                new Vector(50.0,    -50.0,     50.0,      1.0),    // x3   
                new Vector(-50.0,      50.0,    -50.0,        1.0),    // x4
                new Vector(50.0,    50.0,    -50.0,        1.0),    // x5   
                new Vector(-50.0,      50.0,    50.0,      1.0),    // x6 
                new Vector(50.0,    50.0,    50.0,      1.0)     // x7   

            };
            this.Faces = new List<Polygon>(1)
            {
                new Polygon(new FaceUnit(1,0,0),new FaceUnit(5,1,0),new FaceUnit(7,2,0),new FaceUnit(3,3,0)),
                new Polygon(new FaceUnit(3,0,2),new FaceUnit(7,1,2),new FaceUnit(6,2,2),new FaceUnit(2,3,2)),
                new Polygon(new FaceUnit(5,0,1),new FaceUnit(4,1,1),new FaceUnit(6,2,1),new FaceUnit(7,3,1)),
                new Polygon(new FaceUnit(1,0,5),new FaceUnit(0,1,5),new FaceUnit(4,2,5),new FaceUnit(5,3,5)),
                new Polygon(new FaceUnit(0,0,4),new FaceUnit(1,1,4),new FaceUnit(3,2,4),new FaceUnit(2,3,4)),
                new Polygon(new FaceUnit(0,0,3),new FaceUnit(2,1,3),new FaceUnit(6,2,3),new FaceUnit(4,3,3)),

                /*
                new Polygon(3,7,6,2),
                new Polygon(5,4,6,7),
                new Polygon(1,0,4,5),
                new Polygon(0,1,3,2),
                new Polygon(0,2,6,4),
                */
                
            };
            //CubeCenter = (this.Vertices[0].point + this.Vertices[7].point)/2;
            Scale(1);
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
            
        }



    }





    

    
}
