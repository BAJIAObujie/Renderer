using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Light
    {
        public Vector LightPos;
        public MyColor LightColor;
        public Light()
        {
            this.LightPos = new Vector();
            this.LightColor = new MyColor();
        }
        public Light(Vector LightPos, MyColor LightColor)
        {
            this.LightPos = new Vector();
            this.LightColor = new MyColor();
            this.LightPos.x = LightPos.x;
            this.LightPos.y = LightPos.y;
            this.LightPos.z = LightPos.z;
            this.LightPos.t = LightPos.t;
            this.LightColor.r = LightColor.r;
            this.LightColor.g = LightColor.g;
            this.LightColor.b = LightColor.b;
        }

        //-----
        //public static Light PointLight;
        public static MyColor EnvironmentLight;

        /// <summary>
        /// FragmentShader是结构体 值类型，MyColor是类类型，引用类型
        /// </summary>
        /// <param name="FS"></param>
        /// <param name="mycolor"></param>
        /// <returns></returns>
        public static MyColor LightMode_BlinnPhong(a2v FS, MyColor mycolor, bool IsTexture, bool IsPointLighting, Light PointLight)
        {
            //----------------------所有颜色统一在0-1内计算----------------返回mycolor时，在回到0-255
            //材质 光照 暂时写在这边 完成光照模型后，写在外部
            //MyColor Ambient = new MyColor(0, 0, 0);  //环境光 可以不和下面两个一样
            MyColor Ambient = new MyColor(0.3, 0.3, 0.3);
            //MyColor Diffuse = new MyColor(0, 0, 0);
            MyColor Diffuse = new MyColor(0.3, 0.3, 0.3);
            //MyColor Specular = new MyColor(0, 0, 0);
            MyColor Specular = new MyColor(0.8, 0.8, 0.8);
            Material material = new Material(Ambient, Diffuse, Specular);

            //PointLight = new Light(new Vector(5, 10, 0, 1), new MyColor(1, 1, 1));

            //读取纹理的颜色  先假设为mycolor
            MyColor texture = new MyColor();
            if (IsTexture)
            {
                texture = new MyColor(mycolor).Scale(1.0 / 255);
            }
            else
            {
                texture.r = 0.5;
                texture.g = 0.5;
                texture.b = 0.5;
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
                double linear = 0.0009;
                double quadratic = 0.000032;
                double num = constant + linear * distance + quadratic * (distance * distance);
                double attenuation = 0;
                if (num != 0) attenuation = 1.0 / num;
                attenuation = MyStaticMethod.MaxNumber(attenuation, 0);

                MyColor LightAmbient = texture * ((PointLight.LightColor + EnvironmentLight) * material.Ambient);

                //漫反射系数计算
                double diff = MyStaticMethod.MaxNumber(Vector.DotMultiply(normal, LightDir), 0);
                MyColor LightDiffuse = texture * (PointLight.LightColor * material.Diffuse) * diff;

                //反射计算 blinn-phong
                Vector H = (LightDir + ViewDir).NormalizedVector();
                double shininess = 256;//镜面反射系数
                double reflect = Math.Pow(MyStaticMethod.MaxNumber(Vector.DotMultiply(H, normal), 0), shininess);
                MyColor LightSpecular = texture * PointLight.LightColor * material.Specular * reflect;

                //MyColor ret = LightAmbient + (LightDiffuse + LightSpecular);
                MyColor ret = LightAmbient + (LightDiffuse + LightSpecular) * attenuation;
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
