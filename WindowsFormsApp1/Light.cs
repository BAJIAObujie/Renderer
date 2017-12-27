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
    }
}
