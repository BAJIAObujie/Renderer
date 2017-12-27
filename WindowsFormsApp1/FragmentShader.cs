using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    struct FragmentShader
    {
        public Vector WorldNormal;
        public Vector WorldPos;
        public Vector eye;
        
        public FragmentShader(Vector WorldPos, Vector WorldNormal, Vector eye)
        {
            this.WorldNormal = new Vector(WorldNormal);
            this.WorldPos = new Vector(WorldPos);
            this.eye = new Vector(eye);
            
        }
    }
}
