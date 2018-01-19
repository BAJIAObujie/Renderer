using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Camera
    {
        public int Width;
        public int Height;

        public Vector eye;
        public Vector lookat;
        public double Aspect;
        public double Fovy;

        public Camera()
        {
            this.Width = 720;
            this.Height = 480;

            //this.eye = new Vector(150, 0, 0, 1); //摄像机位置
            this.eye = new Vector(150, 150, 150, 1); //摄像机位置
            //this.lookat = new Vector(0, 100, 0, 1); //默认看向的世界坐标
            this.lookat = new Vector(0, 0, 0, 1); //默认看向的世界坐标
            this.Aspect = (double)this.Width / this.Height;
            this.Fovy = 3.1415926 * 0.5;
        }


    }
}
