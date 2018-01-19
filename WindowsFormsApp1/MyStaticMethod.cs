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
        public static void PolygonToTriangle(Mesh mesh)
        {
            mesh.TriangleFaces = new List<Triangle>(mesh.Faces.Count * 2);
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                if(mesh.Faces[i].Count == 3)
                {
                    //还没有确定obj格式读取的法线是怎么回事.
                    /*
                    mesh.TriangleFaces.Add(new Triangle(mesh.Faces[i].p0index,
                    mesh.Faces[i].p1index,
                    mesh.Faces[i].p2index,
                    mesh.Faces[i].normal
                    ));*/
                    mesh.TriangleFaces.Add(new Triangle(mesh.Faces[i].p0index,
                    mesh.Faces[i].p1index,
                    mesh.Faces[i].p2index
                    ));
                }
                else if(mesh.Faces[i].Count == 4)
                {
                    mesh.TriangleFaces.Add(new Triangle(mesh.Faces[i].p0index,
                    mesh.Faces[i].p1index,
                    mesh.Faces[i].p2index
                    ));
                    mesh.TriangleFaces.Add(new Triangle(mesh.Faces[i].p0index,
                    mesh.Faces[i].p2index,
                    mesh.Faces[i].p3index
                    ));
                }
                else {
                    throw new Exception("this model contain polygons whose Count excel 4");
                }
            }
        }

        public static void HidenFacesDemolish(Mesh mesh,Vector camera)
        {
            mesh.ShownFaces = new List<Polygon>(mesh.Faces.Count);
            for(int i = 0; i < mesh.Faces.Count; i++)
            {
                if(Vector.DotMultiply(mesh.Faces[i].normal, camera) <= -double.Epsilon)
                {
                    mesh.ShownFaces.Add(mesh.Faces[i]);
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
            for (int i = 0; i < mesh.Vectors.Count; i++)
            {
                mesh.Vectors[i] = mesh.Vectors[i] * offset * mat1 * mat2 * mat3;
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
            for (int i = 0; i < mesh.Vectors.Count; i++)
            {
                Vector vec = mesh.Vectors[i] - point;
                mesh.Vectors[i] = vec * rotate;

                
            }
            for(int i = 0; i < mesh.Normals.Count; i++)
            {
                mesh.Normals[i] = (mesh.Normals[i] * rotate).NormalizedVector();
            }
        }
        /// <summary>
        /// 从多边形面中获得法向量
        /// </summary>
        /// <returns></returns>
        public static void GetNormalFromFaces(Mesh mesh)
        {
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                Vector vector1 = mesh.Vectors[mesh.Faces[i].p1index.IndexVertex]
                    - mesh.Vectors[mesh.Faces[i].p0index.IndexVertex];
                Vector vector2 = mesh.Vectors[mesh.Faces[i].p2index.IndexVertex]
                    - mesh.Vectors[mesh.Faces[i].p1index.IndexVertex];
                mesh.Faces[i].SetNormal(Vector.CrossMultiply(vector1, vector2).NormalizedVector()) ;
            }
        }

        public static void GetVerticesNormal(Mesh mesh)
        {
            GetVerticesNormal_1(mesh);
            GetVerticesNormal_2(mesh);
        }
        private static void GetVerticesNormal_1(Mesh mesh)
        {
            /*  //之前是用输入顶点三维位置和面索引，直接组成一个顶点 然后计算法向量等 逐个给顶点增加信息。现在使用obj格式读取，法线等信息都有。似乎渲染流水线也没有这个步骤了
            for (int i = 0; i < mesh.Faces.Count; i++)
            {
                if (mesh.Faces[i].Count == 3)
                {
                    mesh.Vectors[mesh.Faces[i].p0index.IndexVertex].normal += mesh.Faces[i].normal;
                    mesh.Vectors[mesh.Faces[i].p1index].normal += mesh.Faces[i].normal;
                    mesh.Vectors[mesh.Faces[i].p2index].normal += mesh.Faces[i].normal;
                }
                else if (mesh.Faces[i].Count == 4)
                {
                    mesh.Vectors[mesh.Faces[i].p0index].normal += mesh.Faces[i].normal;
                    mesh.Vectors[mesh.Faces[i].p1index].normal += mesh.Faces[i].normal;
                    mesh.Vectors[mesh.Faces[i].p2index].normal += mesh.Faces[i].normal;
                    mesh.Vectors[mesh.Faces[i].p3index].normal += mesh.Faces[i].normal;
                }
                else
                {
                    //面的点数超过4
                }
                
            }
            */
        }
        private static void GetVerticesNormal_2(Mesh mesh)
        {
            for (int i = 0; i < mesh.Normals.Count; i++)
            {
                mesh.Normals[i].NormalizedVector();
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



        }

    }
    

