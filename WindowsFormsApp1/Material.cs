using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Material
    {
        //class类中有其他自定义类的时候，要注意赋值的时候，不能使引用传递,不然仍然是同一份对象
        public MyColor Ambient;
        public MyColor Diffuse;
        public MyColor Specular;
        public Material()
        {
            Ambient = new MyColor();
            Diffuse = new MyColor();
            Specular = new MyColor();
        }
        /// <summary>
        /// range from zero to one
        /// </summary>
        /// <param name="ambient"></param>
        /// <param name="diffuse"></param>
        /// <param name="specular"></param>
        public Material(MyColor ambient, MyColor diffuse, MyColor specular)
        {
            Ambient = new MyColor();
            Diffuse = new MyColor();
            Specular = new MyColor();
            this.Ambient.r = ambient.r;
            this.Ambient.g = ambient.g;
            this.Ambient.b = ambient.b;

            this.Diffuse.r = diffuse.r;
            this.Diffuse.g = diffuse.g;
            this.Diffuse.b = diffuse.b;

            this.Specular.r = specular.r;
            this.Specular.g = specular.g;
            this.Specular.b = specular.b;
        }

    }
}
