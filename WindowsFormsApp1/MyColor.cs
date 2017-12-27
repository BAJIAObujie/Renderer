using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    class MyColor
    {
        public double r, g, b;
        public MyColor()
        {
            this.r = 0;
            this.g = 0;
            this.b = 0;
        }
        /// <summary>
        /// better between 0-1
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public MyColor(double r,double g,double b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
        public MyColor(MyColor copy)
        {
            this.r = copy.r;
            this.g = copy.g;
            this.b = copy.b;
        }
        public MyColor Scale(double t)
        {
            this.r *= t;
            this.g *= t;
            this.b *= t;
            return this;
        }
        
        

        public static MyColor Interp(MyColor m1, MyColor m2,double t)
        {
            MyColor ret = new MyColor();
            ret.r = m1.r + (m1.r - m2.r) * t;
            ret.g = m1.g + (m1.g - m2.g) * t;
            ret.b = m1.b + (m1.b - m2.b) * t;
            return ret;
        }
        public static MyColor operator +(MyColor lhs, MyColor rhs)
        {
            MyColor ret = new MyColor();
            ret.r = lhs.r + rhs.r;
            ret.g = lhs.g + rhs.g;
            ret.b = lhs.b + rhs.b;
            //ret.t = lhs.t + rhs.t;
            return ret;
        }
        public static MyColor operator *(MyColor c, double t)
        {
            MyColor ret = new MyColor();
            ret.r = c.r * t;
            ret.g = c.r * t;
            ret.b = c.r * t;
            return ret;
        }
        public static MyColor operator *(MyColor a, MyColor b)
        {
            MyColor ret = new MyColor();
            ret.r = a.r * b.r;
            ret.g = a.g * b.g;
            ret.b = a.b * b.b;
            return ret;
        }

        public static MyColor operator *(double t, MyColor c)
        {
            return c * t;
        }
        


        public Color ConvertToColor()
        {
            /*
            int r = (int)(this.r * 255);
            int g = (int)(this.g * 255);
            int b = (int)(this.b * 255);
            */
            int r = (int)(this.r);
            int g = (int)(this.g);
            int b = (int)(this.b);
            r = MyStaticMethod.CMID(r, 0, 255);
            g = MyStaticMethod.CMID(g, 0, 255);
            b = MyStaticMethod.CMID(b, 0, 255);

            return Color.FromArgb(r, g, b);
        }
    }

}
