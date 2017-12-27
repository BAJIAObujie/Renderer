using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        //-------类似全局变量---------
        private Bitmap MyBitMap;
        private Mesh mesh;
        private Matrix View;
        private Matrix Projection;
        private Matrix Transform;
        private bool IsTriangleWireFrame;
        private bool IsPointLighting;
        private bool IsRectWireFrame;
        private string TexturePath;
        private bool IsTexture;
        private bool IsEye;
        private bool IsCube;
        private bool IsPointLight;


        private Vector eye;
        private Vector lookat;

        

        //-------Device---------
        private int BitMapWidth;
        private int BitMapHeight;
        private double Aspect;
        private double Fovy;

        private int TextureImageWidth;
        private int TextureImageHeight;
        private MyColor[,] TextureImage;


        //------------Form构造函数---------
        public Form1()
        {
            InitializeComponent();
            InitializedInfo();
            UpdateMatrix();
            UpdateMyPictureBox();

            
            
        }
        
        private void UpdateMyPictureBox()
        {
            if (IsRectWireFrame) DrawAllRect(mesh.vertices, mesh.faces); //矩形线框模式
            else DrawAllRect(mesh.vertices, mesh.trianglefaces); //三角形线框以及普通模式
            this.pictureBox1.Image = MyBitMap;
        }

        private void InitializedInfo()
        {
            //------------初始化Device数据-----------
            this.BitMapWidth = 720;
            this.BitMapHeight = 480;
            this.Aspect = (double)this.BitMapWidth / this.BitMapHeight;
            this.Fovy = 3.1415926 * 0.5;

            //------------初始化顶点、面数据-----------
            this.mesh = new Mesh();

            //------------摄像机位置  旋转角度  基矩阵---------------------
            this.eye = new Vector(350, 350, -50, 1); //摄像机位置
            //this.eye = new Vector(450, 150, 150, 1); //摄像机位置
            this.lookat = new Vector(0, 0, 300, 1); //默认看向的世界坐标

            //this.IsWireFrame = true;
            this.IsTriangleWireFrame = false;
            this.IsPointLighting = true;

            //------------加载纹理---------------------
            OpenFileDialog o = new OpenFileDialog();
            o.ShowDialog();
            this.TexturePath = o.FileName;
            LoadTexture();


            this.pictureBox1.Height = (int)(this.Height * 0.8);
            this.pictureBox1.Width = (int)(this.Width * 0.8);
            this.pictureBox1.Location = new Point( 150 + (this.Width - this.BitMapWidth) / 4,
                                                   (this.Height - this.BitMapHeight) / 4);
            //------------画图-------------------
            this.MyBitMap = new Bitmap(this.BitMapWidth, this.BitMapHeight);
        }
        private void UpdateMatrix()
        {
            Vector up = new Vector(0, 1, 0, 1);  //垂直向上的法向量
            this.View = MyStaticMethod.GetViewMatrix(this.eye, this.lookat, up);
            this.Projection = MyStaticMethod.GetPerspectiveMatrix(this.Fovy, this.Aspect, 1, 100);
            this.Transform = View * Projection;
            //顺序不能乱 如果先去除隐藏面，然后根据显示的面来计算顶点法向量，这样有一些点的法向量会缺失，一些点法向量不完整
            //比如0点 因为隐藏面全消除的关系，法向量为0，而1点的法向量，也因为只添加上1357面的法向量的缘故，法向量为1 0 0 应该是0.33 -0.33 -0.33
            //所以必须先正确计算出顶点的法向量 再消除隐藏面
            MyStaticMethod.GetNormalFromFaces(mesh);    //获得每一个面的法向量
            //MyStaticMethod.HidenFacesDemolish(mesh, at - eye); 移动至最后一行
            MyStaticMethod.GetVerticesNormal(mesh);     //每一个面法向量添加到相应的顶点 并归一化
            MyStaticMethod.HidenFacesDemolish(mesh, this.lookat - this.eye);
            MyStaticMethod.TranslateToTriangle(mesh);   //把面分割成两个三角形并且赋值相应的法向量
        }
        /// <summary>
        /// 四边形线框模式
        /// </summary>
        /// <param name="vertexlist"></param>
        /// <param name="faces"></param>
        private void DrawAllRect(List<Vertex> vertexlist, List<Polygon> faces)
        {
            for (int i = 0; i < faces.Count; i++)
            //for (int i = 0; i < 6; i++)
            {
                DrawPrimitive(vertexlist[faces[i].p0index],
                    vertexlist[faces[i].p1index],
                    vertexlist[faces[i].p2index],
                    vertexlist[faces[i].p3index]);
            }
        }
        /// <summary>
        /// 三角形线框模式以及普通渲染模式
        /// </summary>
        /// <param name="vertexlist"></param>
        /// <param name="showntrianglefaces"></param>
        private void DrawAllRect(List<Vertex> vertexlist, List<Triangle> showntrianglefaces)
        {
            for(int i = 0; i < showntrianglefaces.Count; i++)
            //for (int i = 0; i < 6; i++)
            {
                //之前用静态方法改变了顶点列表里的内容，所以画第二个三角形的时候，重复用到的数据p0index数据出错，注意是引用类型
                DrawPrimitive(vertexlist[showntrianglefaces[i].p0index],
                    vertexlist[showntrianglefaces[i].p1index],
                    vertexlist[showntrianglefaces[i].p2index]);
                /*
                DrawPrimitive(vertexlist[polygonlist[i].p0index], 
                    vertexlist[polygonlist[i].p2index],
                    vertexlist[polygonlist[i].p3index]);
                    */
            }
        }

        private void DrawPrimitive(Vertex v1,Vertex v2,Vertex v3)
        {
            Vector v4 = v1.point * View;
            Vector v5 = v2.point * View;
            Vector v6 = v3.point * View;

            Vector v7 = v4 * Projection;
            Vector v8 = v5 * Projection;
            Vector v9 = v6 * Projection;

            v7.ChangeTtoOne();
            v8.ChangeTtoOne();
            v9.ChangeTtoOne();

            Vector p1 = MyStaticMethod.TransformHomogenize(this.BitMapWidth, this.BitMapHeight, v7);
            Vector p2 = MyStaticMethod.TransformHomogenize(this.BitMapWidth, this.BitMapHeight, v8);
            Vector p3 = MyStaticMethod.TransformHomogenize(this.BitMapWidth, this.BitMapHeight, v9);

            //线框模式或者纹理渲染模式
            if (IsTriangleWireFrame) DrawTriangle_WireFrame(p1, p2, p3);
            else
            {
                //保存了世界坐标系下的法向量
                Vertex v11 = new Vertex(v1);
                v11.point = new Vector(p1);
                //v11.point.t = v7.t;

                Vertex v22 = new Vertex(v2);
                v22.point = new Vector(p2);

                Vertex v33 = new Vertex(v3);
                v33.point = new Vector(p3);

                DrawTriangle_Texture(v11,v22,v33);
            }
            
        }
        /*
        private void FrustumCullOff(Verx v1, Vertex v2, Vertex v3)
        {
            int state;


            if()



        }
        */



        private void DrawPrimitive(Vertex v1, Vertex v2, Vertex v3, Vertex v4)
        {
            Vector v5 = v1.point * View;
            Vector v6 = v2.point * View;
            Vector v7 = v3.point * View;
            Vector v8 = v4.point * View;

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
        private void DrawTriangle_WireFrame(Vector v1, Vector v2, Vector v3)
        {
            int a1 = (int)v1.x;
            int b1 = (int)v1.y;
            int a2 = (int)v2.x;
            int b2 = (int)v2.y;
            int a3 = (int)v3.x;
            int b3 = (int)v3.y;
            LineDDA(a1, b1, a2, b2);
            LineDDA(a2, b2, a3, b3);
            LineDDA(a3, b3, a1, b1);
        }
        private void DrawRect_WireFrame(Vector v1, Vector v2, Vector v3, Vector v4)
        {
            int a1 = (int)v1.x;
            int b1 = (int)v1.y;
            int a2 = (int)v2.x;
            int b2 = (int)v2.y;
            int a3 = (int)v3.x;
            int b3 = (int)v3.y;
            int a4 = (int)v4.x;
            int b4 = (int)v4.y;
            LineDDA(a1, b1, a2, b2);
            LineDDA(a2, b2, a3, b3);
            LineDDA(a3, b3, a4, b4);
            LineDDA(a4, b4, a1, b1);
        }
        /// <summary>
        /// DDA画线算法
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

        private void DrawTriangle_Texture(Vertex v1, Vertex v2, Vertex v3)
        {
            List<RenderTriangle> NTriangle;
            //-----------------下面函数可能要修改，因为vertex是类类型，指针传递，内容只有一份，这里应该是要复制一份的
            RenderTriangle.InitializedRenderTriangle(v1, v2, v3,out NTriangle);
            //渲染光栅化后的小三角形
            for(int i= 0; i < NTriangle.Count; i++)
            {
                StartRenderTrapezoid(NTriangle[i]);
            }
        }
        private void StartRenderTrapezoid(RenderTriangle r)
        {
            int top, bottom;
            top = (int)(r.topline + 0.5);
            bottom = (int)(r.downline + 0.5);
            for(int row = top; row < bottom; row++)
            {
                if(row >= 0 && row < this.BitMapHeight)
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
        private ScanLine CalculateStepPerRow(Vertex left,Vertex right, int row)
        {
            ScanLine ret = new ScanLine();
            double width = right.point.x - left.point.x;
            ret.x = (int)(left.point.x + 0.5);
            ret.y = row;
            ret.width = (int)(right.point.x + 0.5) - ret.x;
            ret.leftvertex = left;
            if (left.point.x >= right.point.x) ret.width = 0;

            //计算步长
            double rate = (double)1 / width;
            ret.step.point.x = (right.point.x - left.point.x) * rate;
            ret.step.point.y = (right.point.y - left.point.y) * rate;
            ret.step.point.z = (right.point.z - left.point.z) * rate;
            ret.step.point.t = (right.point.x - left.point.t) * rate;
            ret.step.u = (right.u - left.u) * rate;
            ret.step.v = (right.v - left.v) * rate;
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
        private void DoubleLinedInterp(RenderTriangle r,double y,out Vertex left, out Vertex right)
        {
            double s1 = r.downleft.point.y - r.upleft.point.y;
            double s2 = r.downright.point.y - r.upright.point.y;
            double t1 = (y - r.upleft.point.y) / s1;
            double t2 = (y - r.upright.point.y) / s2;
            left =  Vertex.Interp(r.upleft, r.downleft, t1);
            right = Vertex.Interp(r.upright, r.downright, t2);
        }

        private void StartRenderLine(ScanLine scanline)
        {
            //尚未加入深度缓存
            int x = scanline.x;
            int width = scanline.width;
            if ((x + width) < 0) return;
            for (; width>0; x++, width--)//逐像素计算，直到扫描完width长度的点
            {
                if(x >= 0 && x < BitMapWidth)
                {
                    //double u = scanline.leftvertex.u * scanline.width;
                    //double v = scanline.leftvertex.v * scanline.width;
                    double u = scanline.leftvertex.u;
                    double v = scanline.leftvertex.v;

                    MyColor mycolor = ReadTexture(u, v); //根据uv读取颜色

                    FragmentShader FS = new FragmentShader(scanline.leftvertex.WorldPos,
                                                           scanline.leftvertex.normal,
                                                           this.eye);
                    mycolor = MyStaticMethod.LightMode_BlinnPhong(FS, mycolor, IsTexture, IsPointLighting);


                    Color color = mycolor.ConvertToColor();
                    MyBitMap.SetPixel(x, scanline.y, color);  //设置对应像素点颜色
                }
                scanline.leftvertex.NextStep(scanline.step);
                if (x > BitMapWidth) break;
                
            }
            
        }

        

        private void LoadTexture()
        {
            //Image img = Image.FromFile(@"C:\Users\ZL0032\Desktop\Document\Journey.jpg");
            Image img = Image.FromFile(TexturePath);
            Bitmap bm = new Bitmap(img);
            this.TextureImageHeight = bm.Height;
            this.TextureImageWidth = bm.Width;
            this.TextureImage = new MyColor[TextureImageHeight, TextureImageWidth];
            for(int y = 0; y < bm.Height; y++)
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
        private MyColor ReadTexture(double u, double v)
        {
            u = u * (TextureImageWidth - 1);
            v = v * (TextureImageHeight - 1);
            int x = (int)(u + 0.5);
            int y = (int)(v + 0.5);
            x = MyStaticMethod.CMID(x, 0, TextureImageWidth - 1);
            y = MyStaticMethod.CMID(y, 0, TextureImageHeight - 1);
            return TextureImage[TextureImageHeight - 1 - y, x];
        }
        private void ClearPictureBox()
        {
            for (int x = 0; x < this.MyBitMap.Width; x++)
            {
                for (int y = 0; y < this.MyBitMap.Height; y++)
                {
                    this.MyBitMap.SetPixel(x, y, Color.Gray);
                }
            }
        }
        class ScanLine
        {
            public Vertex leftvertex,step;
            public int x, y, width;
            public ScanLine()
            {
                this.leftvertex = new Vertex();
                this.step = new Vertex();
                this.x = 0;
                this.y = 0;
                this.width = 0;
            }
        }
        class RenderTriangle
        {
            public Vertex upleft, downleft, upright, downright;
            public double topline, downline;
            public RenderTriangle(Vertex upleft, Vertex downleft, Vertex upright, Vertex downright, 
                double topline,double downline)
            {
                this.upleft = upleft;
                this.downleft = downleft;
                this.upright = upright;
                this.downright = downright;
                this.topline = topline;
                this.downline = downline;
            }

            public static void InitializedRenderTriangle(Vertex v1, Vertex v2, Vertex v3,out List<RenderTriangle> list)
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
                if(x <= v3.point.x)   //计算其延长线，如果顶点v3在延长线左边，则v1 连上v3为最长边，如果在右边。。。
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


        //-----------------UI控件逻辑-------------------
        private void ReadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.ShowDialog();
            this.TexturePath = o.FileName;
            UpdateMyPictureBox();
        }

        private void Button_CameraMove_Click(object sender, EventArgs e)
        {
            
        }

        private void CheckBox_IsTriangleWireFrame_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_IsTriangleWireFrame.Checked == true) this.IsTriangleWireFrame = true;
            else this.IsTriangleWireFrame = false;
            ClearPictureBox();
            UpdateMyPictureBox();
        }

        private void CheckBox_IsRectWireFrame_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_IsRectWireFrame.Checked == true) this.IsRectWireFrame = true;
            else this.IsRectWireFrame = false;
            ClearPictureBox();
            UpdateMyPictureBox();
        }

        private void CheckBox_IsTexture_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_IsTexture.Checked == true) this.IsTexture = true;
            else this.IsTexture = false;
            ClearPictureBox();
            UpdateMyPictureBox();
        }

        private void CheckBox_IsLighting_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_IsLighting.Checked == true) this.IsPointLighting = true;
            else this.IsPointLighting = false;
            ClearPictureBox();
            UpdateMyPictureBox();
        }

        private void EnvironmentLightScroll_Scroll(object sender, EventArgs e)
        {
            MyStaticMethod.EnvironmentLight.r = ((double)EnvironmentLightScroll.Value) / 10;
            MyStaticMethod.EnvironmentLight.g = ((double)EnvironmentLightScroll.Value) / 10;
            MyStaticMethod.EnvironmentLight.b = ((double)EnvironmentLightScroll.Value) / 10;
            ClearPictureBox();
            UpdateMyPictureBox();
        }

        private void CheckBox_IsEye_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_IsEye.Checked == true)
            {
                IsEye = true;

                IsPointLight = false;
                CheckBox_IsPointLight.Checked = false;
                IsCube = false;
                CheckBox_IsLookat.Checked = false;
            }
            else
            {
                IsEye = false;
            }
                
        }

        private void CheckBox_IsPointLight_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_IsPointLight.Checked == true) {
                IsPointLight = true;

                IsEye = false;
                CheckBox_IsEye.Checked = false;
                IsCube = false;
                CheckBox_IsLookat.Checked = false;
                
            } 
            else IsPointLight = false;
        }

        private void CheckBox_IsLookat_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_IsLookat.Checked == true)
            {
                IsCube = true;

                IsPointLight = false;
                CheckBox_IsPointLight.Checked = false;
                IsEye = false;
                CheckBox_IsEye.Checked = false;
            }
                
            else IsCube = false;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'w':
                    MoveEvent_Up();
                    break;
                case 's':
                    MoveEvent_Down();
                    break;
                case 'a':
                    MoveEvent_Left();
                    break;
                case 'd':
                    MoveEvent_Right();
                    break;
            }
        }
        private void MoveEvent_Up()
        {
            if (IsEye) this.eye += new Vector(0, 25, 0, 1);
            else if (IsCube)    mesh.Move(new Vector(0, 25, 0, 1));  
            else if (IsPointLight)   MyStaticMethod.PointLight.LightPos += new Vector(0, 25, 0, 1);
            ClearPictureBox();
            UpdateMatrix();
            UpdateMyPictureBox();
        }
        private void MoveEvent_Down()
        {
            if (IsEye) this.eye -= new Vector(0, 25, 0, 1);
            else if (IsCube) mesh.Move(new Vector(0, -25, 0, 1));
            else if (IsPointLight) MyStaticMethod.PointLight.LightPos -= new Vector(0, 25, 0, 1);
            ClearPictureBox();
            UpdateMatrix();
            UpdateMyPictureBox();
        }
        private void MoveEvent_Left()
        {
            if (IsEye) this.eye += new Vector(0, 0, -25, 1);
            else if (IsCube) mesh.Move(new Vector(0, 0, -25, 1));
            else if (IsPointLight) MyStaticMethod.PointLight.LightPos += new Vector(0, 0, -25, 1);
            ClearPictureBox();
            UpdateMatrix();
            UpdateMyPictureBox();
        }
        private void MoveEvent_Right()
        {
            if (IsEye) this.eye += new Vector(0, 0, 25, 1);
            else if (IsCube) mesh.Move(new Vector(0, 0, 25, 1));
            else if (IsPointLight) MyStaticMethod.PointLight.LightPos += new Vector(0, 0, 25, 1);
            ClearPictureBox();
            UpdateMatrix();
            UpdateMyPictureBox();
        }

        
    }
}
