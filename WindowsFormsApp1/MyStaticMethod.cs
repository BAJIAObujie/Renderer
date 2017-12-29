using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace WindowsFormsApp1
{
    class MyStaticMethod
    {
        /// <summary>
        /// 模拟多边形转化成三角形 但是多边形只能取四边形
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public static void TranslateToTriangle(Mesh mesh)
        {
            mesh.trianglefaces = new List<Triangle>();
            for (int i = 0; i < mesh.shownfaces.Count; i++)
            {
                mesh.trianglefaces.Add(new Triangle(mesh.shownfaces[i].p0index,
                    mesh.shownfaces[i].p1index,
                    mesh.shownfaces[i].p2index,
                    mesh.shownfaces[i].normal
                    ));
                mesh.trianglefaces.Add(new Triangle(mesh.shownfaces[i].p0index,
                    mesh.shownfaces[i].p2index,
                    mesh.shownfaces[i].p3index,
                    mesh.shownfaces[i].normal
                    ));
            }
        }

        
        public static void HidenFacesDemolish(Mesh mesh,Vector camera)
        {
            mesh.shownfaces = new List<Polygon>();
            for(int i = 0; i < mesh.faces.Count; i++)
            {
                if(Vector.DotMultiply(mesh.faces[i].normal, camera) <= -double.Epsilon)
                {
                    mesh.shownfaces.Add(mesh.faces[i]);
                }
            }

        }
        

        //这个不行
        /// <summary>
        /// 顶点坐标系变换算法 vector = offset 
        /// </summary>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <returns></returns>
        public static void CoordinateChange(Mesh mesh, Matrix mat,Matrix offset)
        {
            //必要数据预先计算
            // ny平方+nz平方的和的开方
            double x2 = mat[2, 1] * mat[2, 1] + mat[2, 2] * mat[2, 2];
            double ny2nz2 = Math.Sqrt(x2);

            //三矩阵旋转变换
            Matrix mat1 = new Matrix();//第一个矩阵
            mat1[0, 0] = 1;
            mat1[1, 1] = mat[2, 2] / ny2nz2;
            mat1[1, 2] = mat[2, 1] / ny2nz2;
            mat1[2, 1] = -mat1[1, 2];
            mat1[2, 2] = mat1[1, 1];

            Matrix mat2 = new Matrix();//第二个矩阵
            mat2[2, 2] = mat2[0, 0] = ny2nz2;
            mat2[0, 2] = mat[2, 0];
            mat2[2, 0] = -mat[2, 0];
            mat2[1, 1] = 1;

            Matrix mat3 = new Matrix();//第三个矩阵
            mat3[1, 1] = mat3[0, 0] = mat[0, 0] / ny2nz2;
            mat3[1, 0] = (mat[0, 1] * mat[2, 2] - mat[0, 2] * mat[2, 1]) / ny2nz2;
            mat3[0, 1] = -mat[1, 0];
            mat3[2, 2] = 1;
            //Matrix mat4 = offset * mat1 * mat2 * mat3;
            for (int i = 0; i < mesh.vertices.Count; i++)
            {
                mesh.vertices[i].point = mesh.vertices[i].point * offset * mat1 * mat2 * mat3;
                //mesh.vertices[i] = mesh.vertices[i] * mat4;
            }

        }


        public static void RotateYAroundPoint(Mesh mesh, Vector point, double theta)
        {
            double AngleSin = Math.Sin(theta);
            double AngleCos = Math.Cos(theta);
            Matrix rotate = new Matrix();
            rotate[1, 1] = 1;
            rotate[0, 0] = rotate[2, 2] = AngleCos;
            rotate[0, 2] = rotate[2, 0] = AngleSin;
            rotate[0, 2] = -rotate[0, 2];
            for (int i = 0; i < mesh.vertices.Count; i++)
            {
                Vector vec = mesh.vertices[i].point - point;
                mesh.vertices[i].point = vec * rotate;
            }
        }
        /// <summary>
        /// 从多边形面中获得法向量
        /// </summary>
        /// <returns></returns>
        public static void GetNormalFromFaces(Mesh mesh)
        {
            for (int i = 0; i < mesh.faces.Count; i++)
            {
                Vector vector1 = mesh.vertices[mesh.faces[i].p1index].point
                    - mesh.vertices[mesh.faces[i].p0index].point;
                Vector vector2 = mesh.vertices[mesh.faces[i].p2index].point
                    - mesh.vertices[mesh.faces[i].p1index].point;
                mesh.faces[i].normal = Vector.CrossMultiply(vector1, vector2).NormalizedVector();
            }
        }

        public static void GetVerticesNormal(Mesh mesh)
        {
            GetVerticesNormal_1(mesh);
            GetVerticesNormal_2(mesh);
        }
        private static void GetVerticesNormal_1(Mesh mesh)
        {
            for (int i = 0; i < mesh.faces.Count; i++)
            {
                mesh.vertices[mesh.faces[i].p0index].normal += mesh.faces[i].normal;
                mesh.vertices[mesh.faces[i].p1index].normal += mesh.faces[i].normal;
                mesh.vertices[mesh.faces[i].p2index].normal += mesh.faces[i].normal;
                mesh.vertices[mesh.faces[i].p3index].normal += mesh.faces[i].normal;
            }
        }
        private static void GetVerticesNormal_2(Mesh mesh)
        {
            for (int i = 0; i < mesh.vertices.Count; i++)
            {
                mesh.vertices[i].normal.NormalizedVector();
            }
        }

        //大于max返回max，小于min返回min，大于min小于max返回原值
        public static int CMID(int x,int min,int max)
        {
            { return (x < min) ? min : ((x > max) ? max : x); }
        }

        /// <summary>
        /// 从世界坐标变换到摄像机坐标
        /// </summary>
        /// <param name="eye"></param>
        /// <param name="at"></param>
        /// <param name="up"></param>
        /// <returns></returns>
        public static Matrix GetViewMatrix(Vector eye, Vector at, Vector up)
        {
            Vector Xaxis, Yaxis, Zaxis;

            Zaxis = at - eye;
            Zaxis.NormalizedVector();
            Xaxis = Vector.CrossMultiply(up, Zaxis); 
            Xaxis.NormalizedVector();
            Yaxis = Vector.CrossMultiply(Zaxis, Xaxis);
        
            Matrix mat = new Matrix();
            mat[0, 0] = Xaxis.x;
            mat[1, 0] = Xaxis.y;
            mat[2, 0] = Xaxis.z;
            mat[3, 0] = -Vector.DotMultiply(Xaxis, eye);
            
            mat[0, 1] = Yaxis.x;
            mat[1, 1] = Yaxis.y;
            mat[2, 1] = Yaxis.z;
            mat[3, 1] = -Vector.DotMultiply(Yaxis, eye);
            
            mat[0, 2] = Zaxis.x;
            mat[1, 2] = Zaxis.y;
            mat[2, 2] = Zaxis.z;
            mat[3, 2] = -Vector.DotMultiply(Zaxis, eye);
            
            mat[0, 3] = 0;
            mat[1, 3] = 0;
            mat[2, 3] = 0;
            mat[3, 3] = 1;

            //-----------------------得到旋转矩阵------------------
            
            return mat;
        }
        
        /// <summary>
        /// 从摄像机坐标变换到投影坐标
        /// </summary>
        /// <param name="fovy"></param>
        /// <param name="aspect"></param>
        /// <param name="zn"></param>
        /// <param name="zf"></param>
        /// <returns></returns>
        public static Matrix GetPerspectiveMatrix(double fovy,double aspect,double zn,double zf)
        {
            double fax = 1.0 / Math.Tan(fovy * 0.5);
            Matrix mat = new Matrix();mat[3, 3] = 0;    //初始化一个全零的矩阵
            mat[0, 0] = (double)(fax / aspect);
            mat[1, 1] = (double)fax;
            mat[2, 2] = zf / (zf - zn);
            mat[3, 2] = -zn * zf / (zf - zn);
            mat[2, 3] = 1;
            return mat;
        }

        /// <summary>
        /// 归一化，得到屏幕坐标
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Vector TransformHomogenize(int Width,int Height, Vector p)
        {
            Vector ret = new Vector();//0 0 0 1
            double rhw = 1.0 / p.t;
            //此时p.t为投影变换后的z值，p实际上为（xz',yz',zz',z'） 同时除以t之后才是正确的投影变换 不要'的是投影空间，四方体里的,带‘的是
            ret.x = (p.x * rhw + 1.0) * Width * 0.5;
            ret.y = (1.0 - p.y * rhw) * Height * 0.5;
            ret.z = p.z * rhw; 
            ret.t = 1.0;//可以不要这一句 重复
            return ret;
        }

        

        
        public static double MaxNumber(double one,double two)
        {
            return (one > two ? one : two); 
        }
        public static double MinNumber(double one, double two)
        {
            return (one < two ? one : two);
        }


        public static void SwapVertex(ref Vertex v1,ref Vertex v2)
        {
            Vertex temp = v1;
            v1 = v2;
            v2 = temp;
        }

        public static Light PointLight = new Light(new Vector(350, 350, -50, 1), new MyColor(1, 1, 1));
        public static MyColor EnvironmentLight = new MyColor(0.2, 0.2, 0.2);

        /// <summary>
        /// FragmentShader是结构体 值类型，MyColor是类类型，引用类型
        /// </summary>
        /// <param name="FS"></param>
        /// <param name="mycolor"></param>
        /// <returns></returns>
        public static MyColor LightMode_BlinnPhong(FragmentShader FS, MyColor mycolor, bool IsTexture, bool IsPointLighting)
        {
            //----------------------所有颜色统一在0-1内计算----------------返回mycolor时，在回到0-255
            //材质 光照 暂时写在这边 完成光照模型后，写在外部

            //MyColor Ambient = new MyColor(0, 0, 0);  //环境光 可以不和下面两个一样
            MyColor Ambient = new MyColor(0.1, 0.1, 0.1);
            //MyColor Diffuse = new MyColor(0, 0, 0);
            MyColor Diffuse = new MyColor(0.2, 0.2, 0.2);
            //MyColor Specular = new MyColor(0, 0, 0);
            MyColor Specular = new MyColor(0.5, 0.5, 0.5);
            Material material = new Material(Ambient, Diffuse, Specular);

           

            //读取纹理的颜色  先假设为mycolor
            MyColor texture = new MyColor();
            if (IsTexture)
            {
                texture = new MyColor(mycolor).Scale(1.0 / 255);
            }
            else
            {
                texture.r = 1.0;
                texture.g = 1.0;
                texture.b = 1.0;
            }
            if (IsPointLighting)
            {
                Vector normal = new Vector(FS.WorldNormal);
                Vector ViewDir = FS.eye - FS.WorldPos;
                ViewDir.NormalizedVector();

                Vector LightDir = PointLight.LightPos - FS.WorldPos;
                double distance = LightDir.Length();//光源与渲染点的距离
                LightDir.NormalizedVector();

                //----衰减计算----
                double constant = 1;
                double linear = 0.009;
                double quadratic = 0.00032;
                double num = constant + linear * distance + quadratic * (distance * distance);
                double attenuation = 0;
                if (num != 0) attenuation = 1.0 / num;
                attenuation = MinNumber(attenuation, 0);

                MyColor LightA = texture * ((PointLight.LightColor + EnvironmentLight) * material.Ambient);

                //漫反射系数计算
                double diff = MaxNumber(Vector.DotMultiply(normal, LightDir), 0);
                MyColor LightD = texture * (PointLight.LightColor * material.Diffuse) * diff;

                //反射计算 blinn-phong
                Vector H = (LightDir + ViewDir).NormalizedVector();
                double shininess = 2;//镜面反射系数
                double reflect = Math.Pow(MaxNumber(Vector.DotMultiply(H, normal), 0), shininess);
                MyColor LightS = texture * PointLight.LightColor * material.Specular * reflect;

                MyColor ret = LightA + (LightD + LightS);
                //MyColor ret = LightA + (LightD + LightS) * attenuation;
                ret.Scale(255.0);
                return ret;
            }
            else
            {
                MyColor ret = texture.Scale(255.0);
                return ret;
            }



        }





    }
    }

