using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class ScanLine
    {
        public Vertex leftvertex, step;
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
}
