using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class RenderTriangle
    {
        public Vertex upleft, downleft, upright, downright;
        public double topline, downline;
        public RenderTriangle(Vertex upleft, Vertex downleft, Vertex upright, Vertex downright,
            double topline, double downline)
        {
            this.upleft = upleft;
            this.downleft = downleft;
            this.upright = upright;
            this.downright = downright;
            this.topline = topline;
            this.downline = downline;
        }

        public static void InitializedRenderTriangle(Vertex v1, Vertex v2, Vertex v3, out List<RenderTriangle> list)
        {
            //Y轴，v3 v2 v1，所以当v1=v2的时候，只有一个三角形，底边在下方。v2=v3时，底边在上.
            list = new List<RenderTriangle>(2);
            if (v1.point.y > v2.point.y) MyStaticMethod.SwapVertex(ref v1, ref v2);
            if (v1.point.y > v3.point.y) MyStaticMethod.SwapVertex(ref v1, ref v3);
            if (v2.point.y > v3.point.y) MyStaticMethod.SwapVertex(ref v2, ref v3);
            if (v1.point.y == v2.point.y && v1.point.y == v3.point.y) return;
            if (v1.point.x == v2.point.x && v1.point.x == v3.point.x) return;

            //三角形尖角向下
            if (v1.point.y == v2.point.y)
            {
                if (v1.point.x > v2.point.x) MyStaticMethod.SwapVertex(ref v1, ref v2);
                RenderTriangle n1 = new RenderTriangle(v1, v3, v2, v3, v1.point.y, v3.point.y);
                list.Add(n1);
                return;
            }
            //三角形尖角向上
            if (v2.point.y == v3.point.y)
            {
                if (v2.point.x > v3.point.x) MyStaticMethod.SwapVertex(ref v2, ref v3);
                list.Add(new RenderTriangle(v1, v2, v1, v3, v1.point.y, v2.point.y));
                return;
            }

            //剩下的情况必定是在Y轴上 v3 > v2 > v1   注意d3d的规则，图像上面的y轴比较小,从上向下排列是v1 2 3
            double k = (v3.point.y - v1.point.y) / (v2.point.y - v1.point.y);
            double x = v1.point.x + (v2.point.x - v1.point.x) * k;
            if (x <= v3.point.x)   //计算其延长线，如果顶点v3在延长线左边，则v1 连上v3为最长边，如果在右边。。。
            {
                list.Add(new RenderTriangle(v1, v2, v1, v3, v1.point.y, v2.point.y));
                list.Add(new RenderTriangle(v2, v3, v1, v3, v2.point.y, v3.point.y));
            }
            else
            {
                list.Add(new RenderTriangle(v1, v3, v1, v2, v1.point.y, v2.point.y));
                list.Add(new RenderTriangle(v1, v3, v2, v3, v2.point.y, v3.point.y));
            }
        }
    }
}
