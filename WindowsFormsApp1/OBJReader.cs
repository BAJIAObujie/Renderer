using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsFormsApp1
{
    /// <summary>
    /// 更新原本的Cube为范例模型。大规模重构整体框架,因为加入了obj读取器所以修改了很多函数
    /// </summary>
    class OBJReader
    {
        public int VertexCount;
        public int FaceCount;
        public int NormalCount;
        public int UVCount;
        public Mesh mesh;

        List<string> Array;

        public OBJReader(string path)
        {
            VertexCount = 0;
            FaceCount = 0;
            NormalCount = 0;
            UVCount = 0;
            mesh = new Mesh(true);
            Array = new List<string>();

            if (!File.Exists(path)) throw new Exception("path is wrong");
            else
            {
                StreamReader sr = new StreamReader(path);
                string temp;
                while (sr.Peek() != -1)
                {
                    temp = sr.ReadLine();
                    
                    if (temp.IndexOf('v') == 0 && temp.IndexOf(' ') == 1)
                    {
                        VertexCount++;
                    }
                    else if (temp.IndexOf('f') == 0 && temp.IndexOf(' ') == 1)
                    {
                        FaceCount++;
                    }
                    else if (temp.IndexOf('v') == 0 && temp.IndexOf('n') == 1 && temp.IndexOf(' ') == 2)
                    {
                        NormalCount++;
                    }
                    else if (temp.IndexOf('v') == 0 && temp.IndexOf('t') == 1 && temp.IndexOf(' ') == 2)
                    {
                        UVCount++;
                    }
                    else
                    {
                        continue;
                    }
                    Array.Add(temp);
                }
            }

            mesh.Vectors = new List<Vector>(VertexCount);
            mesh.Faces = new List<Polygon>(FaceCount);
            mesh.Normals = new List<Vector>(NormalCount);
            mesh.UVs = new List<UV>(UVCount);

            foreach (string temp in Array)
            {
                if (temp.IndexOf('v') == 0 && temp.IndexOf(' ') == 1)
                {
                    string[] tempArray = temp.Split(new char[] { 'v', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                    Vector v = new Vector();
                    v.x = Double.Parse(tempArray[0]);
                    v.y = Double.Parse(tempArray[1]);
                    v.z = -Double.Parse(tempArray[2]);
                    mesh.Vectors.Add(v);
                }
                else if (temp.IndexOf('f') == 0 && temp.IndexOf(' ') == 1)
                {
                    string[] tempArray = temp.Split(new char[] { 'f', ' '}, System.StringSplitOptions.RemoveEmptyEntries);
                    if(tempArray.Length == 3)
                    {
                        string[] VertexInfo0 = tempArray[0].Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);
                        string[] VertexInfo1 = tempArray[1].Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);
                        string[] VertexInfo2 = tempArray[2].Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);
                        int IndexVertex0 = Int32.Parse(VertexInfo0[0]) - 1;  //obj格式的索引是从1开始。
                        int IndexTexture0 = Int32.Parse(VertexInfo0[1]) - 1;
                        int IndexNormal0 = Int32.Parse(VertexInfo0[2]) - 1;

                        int IndexVertex1 = Int32.Parse(VertexInfo1[0]) - 1;
                        int IndexTexture1 = Int32.Parse(VertexInfo1[1]) - 1;
                        int IndexNormal1 = Int32.Parse(VertexInfo1[2]) - 1;

                        int IndexVertex2 = Int32.Parse(VertexInfo2[0]) - 1;
                        int IndexTexture2 = Int32.Parse(VertexInfo2[1]) - 1;
                        int IndexNormal2 = Int32.Parse(VertexInfo2[2]) - 1;

                        mesh.Faces.Add(new Polygon( new FaceUnit(IndexVertex0, IndexTexture0, IndexNormal0),
                                                    new FaceUnit(IndexVertex1, IndexTexture1, IndexNormal1),
                                                    new FaceUnit(IndexVertex2, IndexTexture2, IndexNormal2)));

                    }
                    else if(tempArray.Length == 4)
                    {
                        string[] VertexInfo0 = tempArray[0].Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);
                        string[] VertexInfo1 = tempArray[1].Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);
                        string[] VertexInfo2 = tempArray[2].Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);
                        string[] VertexInfo3 = tempArray[3].Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);
                        int IndexVertex0 = Int32.Parse(VertexInfo0[0]) - 1;  //obj格式的索引是从1开始。
                        int IndexTexture0 = Int32.Parse(VertexInfo0[1]) - 1;
                        int IndexNormal0 = Int32.Parse(VertexInfo0[2]) - 1;

                        int IndexVertex1 = Int32.Parse(VertexInfo1[0]) - 1;
                        int IndexTexture1 = Int32.Parse(VertexInfo1[1]) - 1;
                        int IndexNormal1 = Int32.Parse(VertexInfo1[2]) - 1;

                        int IndexVertex2 = Int32.Parse(VertexInfo2[0]) - 1;
                        int IndexTexture2 = Int32.Parse(VertexInfo2[1]) - 1;
                        int IndexNormal2 = Int32.Parse(VertexInfo2[2]) - 1;

                        int IndexVertex3 = Int32.Parse(VertexInfo3[0]) - 1;
                        int IndexTexture3 = Int32.Parse(VertexInfo3[1]) - 1;
                        int IndexNormal3 = Int32.Parse(VertexInfo3[2]) - 1;

                        mesh.Faces.Add(new Polygon(new FaceUnit(IndexVertex0, IndexTexture0, IndexNormal0),
                                                    new FaceUnit(IndexVertex1, IndexTexture1, IndexNormal1),
                                                    new FaceUnit(IndexVertex2, IndexTexture2, IndexNormal2),
                                                    new FaceUnit(IndexVertex3, IndexTexture3, IndexNormal3)));
                    }
                }
                else if (temp.IndexOf('v') == 0 && temp.IndexOf('n') == 1 && temp.IndexOf(' ') == 2)
                {
                    string[] tempArray = temp.Split(new char[] { 'v', 'n' , ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                    Vector n = new Vector();
                    n.x = Double.Parse(tempArray[0]);
                    n.y = Double.Parse(tempArray[1]);
                    n.z = -Double.Parse(tempArray[2]);
                    mesh.Normals.Add(n);
                }
                else if (temp.IndexOf('v') == 0 && temp.IndexOf('t') == 1 && temp.IndexOf(' ') == 2)
                {
                    string[] tempArray = temp.Split(new char[] { 'v', 't', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                    UV uv;
                    uv.u = Double.Parse(tempArray[0]);
                    uv.v = Double.Parse(tempArray[1]);
                    mesh.UVs.Add(uv);
                }
            }

            PostOperative();
        }
        public void PostOperative()
        {
            this.mesh.Scale(1);
        }
    }
}
