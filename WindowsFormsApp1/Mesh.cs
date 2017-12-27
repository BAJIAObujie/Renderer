using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Mesh
    {
        public List<Vertex> vertices;  //顶点集合    一般来说顶点的个数大于面的面数
        public List<Polygon> faces;   //面的集合
        public List<Polygon> shownfaces; //去除消隐的面
        public List<Triangle> trianglefaces;  //要显示的三角形的集合
        

        /// <summary>
        /// 在调用之前必须先初始化第二个参数 faces 先得出所有面的信息 根据你的模型来设定三角面
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="faces"></param>
        public Mesh()
        {
            this.vertices = new List<Vertex>(8)
            {
                new Vertex(0.0,      0.0,      0.0,        1.0    ,1         ,1       ),    // x0
                new Vertex(100.0,    0.0,      0.0 ,       1.0    ,0         ,0       ),    // x1   
                new Vertex(0.0,      0.0,      100.0 ,     1.0    ,0         ,0       ),    // x2 
                new Vertex(100.0,    0.0,      100.0,      1.0    ,1         ,0       ),    // x3   
                new Vertex(0.0,      100.0,    0.0,        1.0    ,0         ,0       ),    // x4
                new Vertex(100.0,    100.0,    0.0,        1.0    ,0         ,1       ),    // x5   
                new Vertex(0.0,      100.0,    100.0,      1.0    ,1         ,0       ),    // x6 
                new Vertex(100.0,    100.0,    100.0,      1.0    ,1         ,1       )     // x7   

            };
            this.faces = new List<Polygon>(1)
            {
             //改为以左手为准，左手坐标系
                new Polygon(1,5,7,3),
                new Polygon(3,7,6,2),
                new Polygon(5,4,6,7),
                new Polygon(1,0,4,5),
                new Polygon(0,1,3,2),
                new Polygon(0,2,6,4),
            };
            init();
        }
        private void init()
        {
            int mul = 2;
            for (int i = 0; i < this.vertices.Count; i++)
            {
                this.vertices[i].point.x *= mul;
                this.vertices[i].point.y *= mul;
                this.vertices[i].point.z *= mul;
                this.vertices[i].WorldPos.x *= mul;
                this.vertices[i].WorldPos.y *= mul;
                this.vertices[i].WorldPos.z *= mul;
            }
            
            

        }
        public void Move(Vector m)
        {
            for (int i = 0; i < this.vertices.Count; i++)
            {
                this.vertices[i].point.y += m.y;
                this.vertices[i].point.z += m.z;
                this.vertices[i].WorldPos.y += m.y;
                this.vertices[i].WorldPos.z += m.z;
            }
        }



    }





    

    
}
