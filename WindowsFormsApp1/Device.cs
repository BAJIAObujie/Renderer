using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    class Device
    {
        //-----渲染模式-----
        public bool IsTriangleWireFrame;
        public bool IsRectWireFrame;
        public bool IsShaded;

        public bool IsPointLighting;
        public bool IsTexture;

        public bool IsEye;
        public bool IsCube;
        public bool IsPointLight;

        //-----输入顶点-----
        public Mesh mesh;
        private Matrix View;
        private Matrix Projection;
        //private Matrix Transform;
        public Camera MyCamera;
        public Light MyLight;

        //-----纹理部分-----
        public string TexturePath;
        public string ObjPath;
        public int TextureWidth;
        public int TextureHeight;
        public MyColor[,] TextureImage;

        //-----帧缓存部分-----
        public Bitmap MyBitMap;
        private int BitMapWidth;
        private int BitMapHeight;
        private double[,] DepthBuffer;

        //-------------------------------函数部分----------------------------------
        public Device()
        {
            InitializedInfo();
            UpdateMatrix();
            UpdateMyBitmap();
        }
        private void InitializedInfo()
        {
            IsShaded = true;
            IsEye = true;
            IsTexture = true;
            IsPointLighting = true;
            

            this.BitMapWidth = 720;
            this.BitMapHeight = 480;

            this.MyCamera = new Camera();
            this.TexturePath = @"C:\Users\ZL0032\Desktop\Document\Journey.jpg";
            this.ObjPath = @"C: \Users\ZL0032\Desktop\girl\girl.obj";
            
            //------------初始化顶点、面数据-----------
            LoadObj(true);//加载测试用例cube为true 加载obj模型为false

            //--------------光源信息---------------
            MyLight = new Light(new Vector(100, 0, 0, 1), new MyColor(1, 1, 1));
            //MyLight = new Light(new Vector(0, 0, 100, 1), new MyColor(1, 1, 1));
            Light.EnvironmentLight = new MyColor(0.5, 0.5, 0.5);

            DepthBuffer = new double[BitMapHeight, BitMapWidth];
            ClearDepthBuffer();

            //this.IsRectWireFrame = true;
            //this.IsTriangleWireFrame = true;
            //this.IsPointLighting = true;
            
            LoadTexture();
            this.MyBitMap = new Bitmap(this.BitMapWidth, this.BitMapHeight);
        }

        //------顶点部分---------
        private void UpdateMatrix()
        {
            Vector up = new Vector(0, 1, 0, 1);  //垂直向上的法向量
            this.View = MyStaticMethod.GetViewMatrix(this.MyCamera.eye, this.MyCamera.lookat, up);
            this.Projection = MyStaticMethod.GetPerspectiveMatrix(this.MyCamera.Fovy, this.MyCamera.Aspect, 1, 100);
            //this.Transform = View * Projection;

            //顺序不能乱 如果先去除隐藏面，然后根据显示的面来计算顶点法向量，这样有一些点的法向量会缺失，一些点法向量不完整
            //比如0点 因为隐藏面全消除的关系，法向量为0，而1点的法向量，也因为只添加上1357面的法向量的缘故，法向量为1 0 0 应该是0.33 -0.33 -0.33
            //所以必须先正确计算出顶点的法向量 再消除隐藏面


            MyStaticMethod.GetNormalFromFaces(mesh);    //获得每一个面的法向量
            //MyStaticMethod.HidenFacesDemolish(mesh, at - eye); 移动至最后一行
            MyStaticMethod.GetVerticesNormal(mesh);     //每一个面法向量添加到相应的顶点 并归一化
            MyStaticMethod.HidenFacesDemolish(mesh, this.MyCamera.lookat - this.MyCamera.eye);
            MyStaticMethod.PolygonToTriangle(mesh);   //把面分割成两个三角形并且赋值相应的法向量
            
        }
        private void UpdateMyBitmap()
        {
            if (IsShaded)
            {
                DrawModel(this.mesh.Vectors, this.mesh.Faces, this.mesh.Normals, this.mesh.UVs);
            }
            else if (IsRectWireFrame || IsTriangleWireFrame)
            {
                DrawWireFrame(this.mesh.Vectors, this.mesh.Faces);
            }
            
        }
        /// <summary>
        /// 正常着色模式
        /// </summary>
        /// <param name="vertexlist"></param>
        /// <param name="faces"></param>
        private void DrawModel(List<Vector> Vectors, List<Polygon> Faces, List<Vector> Normals, List<UV> UVs)
        {
            for (int i = 0; i < Faces.Count; i++)
            //for (int i = 0; i < 2; i++)
                {
                if(Faces[i].Count == 4)
                {
                    Vertex v0 = new Vertex();
                    v0.point = Vectors[Faces[i].p0index.IndexVertex];
                    v0.WorldPos = Vectors[Faces[i].p0index.IndexVertex];
                    v0.uv = UVs[Faces[i].p0index.IndexTexture];
                    v0.normal = Normals[Faces[i].p0index.IndexNormal];
                    //v0.color

                    Vertex v1 = new Vertex();
                    v1.point = Vectors[Faces[i].p1index.IndexVertex];
                    v1.WorldPos = Vectors[Faces[i].p1index.IndexVertex];
                    v1.uv = UVs[Faces[i].p1index.IndexTexture];
                    v1.normal = Normals[Faces[i].p1index.IndexNormal];

                    Vertex v2 = new Vertex();
                    v2.point = Vectors[Faces[i].p2index.IndexVertex];
                    v2.WorldPos = Vectors[Faces[i].p2index.IndexVertex];
                    v2.uv = UVs[Faces[i].p2index.IndexTexture];
                    v2.normal = Normals[Faces[i].p2index.IndexNormal];

                    Vertex v3 = new Vertex();
                    v3.point = Vectors[Faces[i].p3index.IndexVertex];
                    v3.WorldPos = Vectors[Faces[i].p3index.IndexVertex];
                    v3.uv = UVs[Faces[i].p3index.IndexTexture];
                    v3.normal = Normals[Faces[i].p3index.IndexNormal];
                    DrawPrimitive(v0, v1, v2);
                    DrawPrimitive(v0, v2, v3);//改成结构体了 对值传递后应该不会对原值进行修改

                }
                else if (Faces[i].Count == 3)
                {
                    Vertex v0 = new Vertex();
                    v0.point = Vectors[Faces[i].p0index.IndexVertex];
                    v0.WorldPos = Vectors[Faces[i].p0index.IndexVertex];
                    v0.uv = UVs[Faces[i].p0index.IndexTexture];
                    v0.normal = Normals[Faces[i].p0index.IndexNormal];
                    //v0.color

                    Vertex v1 = new Vertex();
                    v1.point = Vectors[Faces[i].p1index.IndexVertex];
                    v1.WorldPos = Vectors[Faces[i].p1index.IndexVertex];
                    v1.uv = UVs[Faces[i].p1index.IndexTexture];
                    v1.normal = Normals[Faces[i].p1index.IndexNormal];

                    Vertex v2 = new Vertex();
                    v2.point = Vectors[Faces[i].p2index.IndexVertex];
                    v2.WorldPos = Vectors[Faces[i].p2index.IndexVertex];
                    v2.uv = UVs[Faces[i].p2index.IndexTexture];
                    v2.normal = Normals[Faces[i].p2index.IndexNormal];

                    DrawPrimitive(v0, v1, v2);
                }
            }
        }

        /// <summary>
        /// 三角形 四边形线框模式 obj模型可能同时存在有四个顶点的面和三个顶点的面
        /// </summary>
        /// <param name="vertexlist"></param>
        /// <param name="showntrianglefaces"></param>
        private void DrawWireFrame(List<Vector> Vectors, List<Polygon> Faces)
        {
            for (int i = 0; i < Faces.Count; i++)
            {
                if (Faces[i].Count == 3)
                {
                    DrawPrimitive(Vectors[Faces[i].p0index.IndexVertex],
                    Vectors[Faces[i].p1index.IndexVertex],
                    Vectors[Faces[i].p2index.IndexVertex]);
                }
                else if (Faces[i].Count == 4)
                {
                    if (IsTriangleWireFrame)
                    {
                        DrawPrimitive(Vectors[Faces[i].p0index.IndexVertex],
                                      Vectors[Faces[i].p1index.IndexVertex],
                                      Vectors[Faces[i].p2index.IndexVertex]);
                        DrawPrimitive(Vectors[Faces[i].p0index.IndexVertex],
                                      Vectors[Faces[i].p2index.IndexVertex],
                                      Vectors[Faces[i].p3index.IndexVertex]);
                    }
                    else if (IsRectWireFrame)
                    {
                        DrawPrimitive(Vectors[Faces[i].p0index.IndexVertex],
                                      Vectors[Faces[i].p1index.IndexVertex],
                                      Vectors[Faces[i].p2index.IndexVertex],
                                      Vectors[Faces[i].p3index.IndexVertex]);
                    }
                }
                else throw new Exception("Current count of face excel four");
            }
        }

        //第一个 着色模式 第二三个 线框模式
        private void DrawPrimitive(Vertex v1, Vertex v2, Vertex v3)
        {
            Vector v4 = v1.point * View;
            Vector v5 = v2.point * View;
            Vector v6 = v3.point * View;

            Vector v7 = v4 * Projection;
            Vector v8 = v5 * Projection;
            Vector v9 = v6 * Projection;

            Vector p1 = MyStaticMethod.TransformHomogenize(this.BitMapWidth, this.BitMapHeight, v7);
            Vector p2 = MyStaticMethod.TransformHomogenize(this.BitMapWidth, this.BitMapHeight, v8);
            Vector p3 = MyStaticMethod.TransformHomogenize(this.BitMapWidth, this.BitMapHeight, v9);

            //纹理渲染模式 IsShaded true
            Vertex v11 = new Vertex(v1);
            v11.point = new Vector(p1);
            v11.point.t = v7.t;

            Vertex v22 = new Vertex(v2);
            v22.point = new Vector(p2);
            v22.point.t = v8.t;

            Vertex v33 = new Vertex(v3);
            v33.point = new Vector(p3);
            v33.point.t = v9.t;

            DrawTriangle_Shaded(v11, v22, v33);

        }
        private void DrawPrimitive(Vector v1, Vector v2, Vector v3)
        {
            Vector v5 = v1 * View;
            Vector v6 = v2 * View;
            Vector v7 = v3 * View;

            Vector v9 = v5 * Projection;
            Vector v10 = v6 * Projection;
            Vector v11 = v7 * Projection;

            Vector p1 = MyStaticMethod.TransformHomogenize(this.BitMapWidth, this.BitMapHeight, v9);
            Vector p2 = MyStaticMethod.TransformHomogenize(this.BitMapWidth, this.BitMapHeight, v10);
            Vector p3 = MyStaticMethod.TransformHomogenize(this.BitMapWidth, this.BitMapHeight, v11);

            //三角形的线框模式
            DrawTriangle_WireFrame(p1, p2, p3);
        }
        private void DrawPrimitive(Vector v1, Vector v2, Vector v3, Vector v4)
        {
            Vector v5 = v1 * View;
            Vector v6 = v2 * View;
            Vector v7 = v3 * View;
            Vector v8 = v4 * View;

            Vector v9 = v5 * Projection;
            Vector v10 = v6 * Projection;
            Vector v11 = v7 * Projection;
            Vector v12 = v8 * Projection;

            Vector p1 = MyStaticMethod.TransformHomogenize(this.BitMapWidth, this.BitMapHeight, v9);
            Vector p2 = MyStaticMethod.TransformHomogenize(this.BitMapWidth, this.BitMapHeight, v10);
            Vector p3 = MyStaticMethod.TransformHomogenize(this.BitMapWidth, this.BitMapHeight, v11);
            Vector p4 = MyStaticMethod.TransformHomogenize(this.BitMapWidth, this.BitMapHeight, v12);

            //四边形的线框模式
            DrawRect_WireFrame(p1, p2, p3, p4);
        }
        
        //视椎体裁剪功能未完成
        private bool FrustumCullOff(Vertex v1, Vertex v2, Vertex v3)
        {

            int State_1x, State_2x, State_3x;
            int State_1y, State_2y, State_3y;
            int State_1z, State_2z, State_3z;

            //------------判断x轴-------------
            if (v1.point.x > 1) State_1x = 2;
            else if (v1.point.x < -1) State_1x = 1;
            else State_1x = 0;

            if (v2.point.x > 1) State_2x = 2;
            else if (v2.point.x < -1) State_2x = 1;
            else State_2x = 0;

            if (v3.point.x > 1) State_3x = 2;
            else if (v3.point.x < -1) State_3x = 1;
            else State_3x = 0;

            if ((State_1x == 2) && (State_2x == 2) && (State_3x == 2)) return false;
            if ((State_1x == 1) && (State_2x == 1) && (State_3x == 1)) return false;

            //------------判断y轴-------------
            if (v1.point.y > 1) State_1y = 2;
            else if (v1.point.y < -1) State_1y = 1;
            else State_1y = 0;

            if (v2.point.y > 1) State_2y = 2;
            else if (v2.point.y < -1) State_2y = 1;
            else State_2y = 0;

            if (v3.point.y > 1) State_3y = 2;
            else if (v3.point.y < -1) State_3y = 1;
            else State_3y = 0;

            if ((State_1y == 2) && (State_2y == 2) && (State_3y == 2)) return false;//完全不可见状态
            if ((State_1y == 1) && (State_2y == 1) && (State_3y == 1)) return false;

            //------------判断z轴-------------
            int num_verts_in = 0;
            if (v1.point.z > 1) State_1z = 2;
            else if (v1.point.z < 0) State_1z = 1;
            else
            {
                State_1z = 0; num_verts_in++;
            }

            if (v2.point.z > 1) State_2z = 2;
            else if (v2.point.z < 0) State_2z = 1;
            else
            {
                State_2z = 0; num_verts_in++;
            }

            if (v3.point.z > 1) State_3z = 2;
            else if (v3.point.z < 0) State_3z = 1;
            else
            {
                State_3z = 0; num_verts_in++;
            }

            if ((State_1z == 2) && (State_2z == 2) && (State_3z == 2)) return false;
            if ((State_1z == 1) && (State_2z == 1) && (State_3z == 1)) return false;

            return true;
            // 判断是否有顶点在近裁剪面外侧
            if (State_1z == 1 || State_2z == 1 || State_3z == 1)
            {

                //考虑的情况不完善
                if (num_verts_in == 1)//两点在外侧
                {
                    if (State_1z == 0) { }
                    else if (State_2z == 0)
                    {
                        Vertex temp = v1;
                        v1 = v2;
                        v2 = v3;
                        v3 = temp;
                    }
                    else
                    {
                        Vertex temp = v1;
                        v1 = v3;
                        v3 = v2;
                        v2 = temp;
                    }
                    double t = (0 - v1.point.z) / (v2.point.z - v1.point.z);
                    v2 = Vertex.Interp(v1, v2, t);   //这样计算完以后的v2.point.z应该也为0
                    /*
                    double xi = v1.point.x + v.x * t1;
                    double yi = v1.point.y + v.y * t1;
                    v2.point.x = xi;
                    v2.point.y = yi;
                    v2.point.z = 0;
                    */
                    t = (0 - v1.point.z) / (v3.point.z - v1.point.z);
                    v3 = Vertex.Interp(v1, v3, t);
                }
                else if (num_verts_in == 2)
                {

                }


            }

        }

        private void DrawTriangle_WireFrame(Vector v1, Vector v2, Vector v3)
        {
            LineDDA((int)v1.x, (int)v1.y, (int)v2.x, (int)v2.y);
            LineDDA((int)v2.x, (int)v2.y, (int)v3.x, (int)v3.y);
            LineDDA((int)v3.x, (int)v3.y, (int)v1.x, (int)v1.y);
        }
        private void DrawRect_WireFrame(Vector v1, Vector v2, Vector v3, Vector v4)
        {
            LineDDA((int)v1.x, (int)v1.y, (int)v2.x, (int)v2.y);
            LineDDA((int)v2.x, (int)v2.y, (int)v3.x, (int)v3.y);
            LineDDA((int)v3.x, (int)v3.y, (int)v4.x, (int)v4.y);
            LineDDA((int)v4.x, (int)v4.y, (int)v1.x, (int)v1.y);
        }

        /// <summary>
        /// DDA画线算法 还缺少一个裁剪算法
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="color"></param>
        private void LineDDA(int x1, int y1, int x2, int y2)
        {
            float dx, dy, n, k; float xinc, yinc, x, y;
            dx = x2 - x1;
            dy = y2 - y1;
            if (Math.Abs(dx) > Math.Abs(dy))
                n = Math.Abs(dx);
            else
                n = Math.Abs(dy);
            xinc = (float)dx / n;
            yinc = (float)dy / n;
            x = (float)x1; y = (float)y1;

            int height = MyBitMap.Height;
            int width = MyBitMap.Width;
            for (k = 1; k <= n; k++)
            {
                MyBitMap.SetPixel((int)(x + 0.5), (int)(y + 0.5), Color.Black);
                x += xinc;
                y += yinc;
            }
        }

        private void DrawTriangle_Shaded(Vertex v1, Vertex v2, Vertex v3)
        {
            List<RenderTriangle> NTriangle;
            //-----------------下面函数可能要修改，因为vertex是类类型，指针传递，内容只有一份，这里应该是要复制一份的
            RenderTriangle.InitializedRenderTriangle(v1, v2, v3, out NTriangle);
            //渲染光栅化后的小三角形
            for (int i = 0; i < NTriangle.Count; i++)
            {
                StartRenderTrapezoid(NTriangle[i]);
            }
        }
        private void StartRenderTrapezoid(RenderTriangle r)
        {
            int top, bottom;
            top = (int)(r.topline + 0.5);
            bottom = (int)(r.downline + 0.5);
            for (int row = top; row < bottom; row++)
            {
                if (row >= 0 && row < this.BitMapHeight)
                {
                    Vertex left, right;
                    DoubleLinedInterp(r, (double)row + 0.5f, out left, out right);
                    //DoubleLinedInterp(r, (double)row, out left, out right);
                    ScanLine s = CalculateStepPerRow(left, right, row);
                    StartRenderLine(s);
                }
                if (row >= this.BitMapHeight) break;
            }
        }
        private ScanLine CalculateStepPerRow(Vertex left, Vertex right, int row)
        {
            ScanLine ret = new ScanLine();
            double width = right.point.x - left.point.x;
            ret.x = (int)(left.point.x + 0.5);
            ret.y = row;
            ret.width = (int)(right.point.x + 0.5) - ret.x;
            ret.leftvertex = left;
            if (left.point.x >= right.point.x) ret.width = 0;

            //计算步长
            double rate = 1.0 / width;
            ret.step.point.x = (right.point.x - left.point.x) * rate;
            ret.step.point.y = (right.point.y - left.point.y) * rate;
            ret.step.point.z = (right.point.z - left.point.z) * rate;
            ret.step.point.t = (right.point.t - left.point.t) * rate;
            ret.step.uv.u = (right.uv.u - left.uv.u) * rate;
            ret.step.uv.v = (right.uv.v - left.uv.v) * rate;
            ret.step.color.r = (right.color.r - left.color.r) * rate;
            ret.step.color.g = (right.color.g - left.color.g) * rate;
            ret.step.color.b = (right.color.b - left.color.b) * rate;
            ret.step.normal.x = (right.normal.x - left.normal.x) * rate;
            ret.step.normal.y = (right.normal.y - left.normal.y) * rate;
            ret.step.normal.z = (right.normal.z - left.normal.z) * rate;
            ret.step.WorldPos.x = (right.WorldPos.x - left.WorldPos.x) * rate;
            ret.step.WorldPos.y = (right.WorldPos.y - left.WorldPos.y) * rate;
            ret.step.WorldPos.z = (right.WorldPos.z - left.WorldPos.z) * rate;

            return ret;
        }
        private void DoubleLinedInterp(RenderTriangle r, double y, out Vertex left, out Vertex right)
        {
            double s1 = r.downleft.point.y - r.upleft.point.y;
            double s2 = r.downright.point.y - r.upright.point.y;
            double t1 = (y - r.upleft.point.y) / s1;
            double t2 = (y - r.upright.point.y) / s2;
            left = Vertex.Interp(r.upleft, r.downleft, t1);
            right = Vertex.Interp(r.upright, r.downright, t2);
        }

        private void StartRenderLine(ScanLine scanline)
        {
            int x = scanline.x;
            int width = scanline.width;
            if ((x + width) < 0) return;
            for (; width > 0; x++, width--)//逐像素计算，直到扫描完width长度的点
            {
                if (x >= 0 && x < BitMapWidth)
                {
                    //深度缓存
                    if (scanline.leftvertex.point.t < DepthBuffer[scanline.y, x])
                    {
                        DepthBuffer[scanline.y, x] = scanline.leftvertex.point.t;

                        double u = scanline.leftvertex.uv.u;
                        double v = scanline.leftvertex.uv.v;

                        MyColor mycolor = ReadTexture(u, v); //根据uv读取颜色   非立方体纹理映射
                        //MyColor mycolor = CubeMap(scanline.leftvertex.WorldPos, this.mesh.CubeCenter);

                        a2v FS = new a2v(scanline.leftvertex.WorldPos,
                                                               scanline.leftvertex.normal,
                                                               this.MyCamera.eye);
                        mycolor = Light.LightMode_BlinnPhong(FS, mycolor, IsTexture, IsPointLighting, MyLight);

                        Color color = mycolor.ConvertToColor();
                        MyBitMap.SetPixel(x, scanline.y, color);  //设置对应像素点颜色
                    }

                }
                scanline.leftvertex.NextStep(scanline.step);
                if (x > BitMapWidth) break;

            }

        }

        //------纹理部分------
        public void LoadTexture()
        {
            Image img = Image.FromFile(TexturePath);
            Bitmap bm = new Bitmap(img);
            this.TextureHeight = bm.Height;
            this.TextureWidth = bm.Width;
            this.TextureImage = new MyColor[TextureHeight, TextureWidth];
            for (int y = 0; y < bm.Height; y++)
            {
                for (int x = 0; x < bm.Width; x++)
                {
                    this.TextureImage[y, x] = new MyColor();
                    this.TextureImage[y, x].r = bm.GetPixel(x, y).R;
                    this.TextureImage[y, x].g = bm.GetPixel(x, y).G;
                    this.TextureImage[y, x].b = bm.GetPixel(x, y).B;
                    //getpixel是从左下角开始算先X后Y 纹理图是从右上角开始算，先行后列
                }
            }
        }
        public void LoadObj(bool IsTest)
        {
            //加载Obj模型 还在改善
            if (IsTest)
            {
                this.mesh = new Mesh(IsTest);
            }
            //加载测试用例cube模型
            else
            {
                OBJReader o = new OBJReader(this.ObjPath);
                this.mesh = o.mesh;
                MyStaticMethod.RotateYAroundPoint(this.mesh, new Vector(0, 0, 0, 1), -1.57);
            }
            
        }
        public MyColor ReadTexture(double u, double v)
        {
            u = u * (TextureWidth - 1);
            v = v * (TextureHeight - 1);
            int x = (int)(u + 0.5);
            int y = (int)(v + 0.5);
            x = MyStaticMethod.CMID(x, 0, TextureWidth - 1);
            y = MyStaticMethod.CMID(y, 0, TextureHeight - 1);
            return TextureImage[TextureHeight - 1 - y, x];
        }
        private MyColor CubeMap(Vector WorldPos, Vector CubeCenter)
        {

            Vector vec = WorldPos - CubeCenter;
            double u, v;
            vec.NormalizedVector();
            //positive x
            if ((vec.x > vec.y) && (vec.x > vec.z))
            {
                u = 0.5 - (vec.z / (2 * vec.x));
                v = 0.5 - (vec.y / (2 * vec.x));
            }

            //negative x
            else if ((vec.x < vec.y) && (vec.x < vec.z))
            {
                u = 0.5 - (vec.z / (2 * vec.x));
                v = 0.5 + (vec.y / (2 * vec.x));
            }

            //positive y
            else if ((vec.y > vec.x) && (vec.y > vec.z))
            {
                u = 0.5 + (vec.x / (2 * vec.y));
                v = 0.5 + (vec.z / (2 * vec.y));
            }

            //negative y
            else if ((vec.y < vec.x) && (vec.y < vec.z))
            {
                u = 0.5 - (vec.x / (2 * vec.y));
                v = 0.5 + (vec.z / (2 * vec.y));
            }

            //positive z
            else if ((vec.z > vec.x) && (vec.z > vec.y))
            {
                u = 0.5 + (vec.x / (2 * vec.z));
                v = 0.5 - (vec.y / (2 * vec.z));
            }

            //negative z
            else if ((vec.z < vec.x) && (vec.z < vec.y))
            {
                u = 0.5 + (vec.x / (2 * vec.z));
                v = 0.5 + (vec.y / (2 * vec.z));
            }
            else
            {
                u = v = 1;
            }
            return ReadTexture(u, v);

        }

        //------帧缓存--------
        private void ClearDepthBuffer()
        {
            for (int y = 0; y < this.BitMapHeight; y++)
            {
                for (int x = 0; x < this.BitMapWidth; x++)
                {
                    DepthBuffer[y, x] = 1000000;
                }
            }
        }
        private void ClearFrameBuffer()
        {
            for (int x = 0; x < this.BitMapWidth; x++)
            {
                for (int y = 0; y < this.BitMapHeight; y++)
                {
                    MyBitMap.SetPixel(x, y, Color.Gray);
                }
            }

        }
        public Bitmap GetBigMap_UpdateMatrix()
        {
            ClearFrameBuffer();
            ClearDepthBuffer();
            UpdateMatrix();
            return MyBitMap;
        }
        public Bitmap GetBigMap_UpdateMatNBitmap()
        {
            ClearFrameBuffer();
            ClearDepthBuffer();
            UpdateMatrix();
            UpdateMyBitmap();
            return MyBitMap;
        }
    }
}
