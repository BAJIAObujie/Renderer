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
        Device MyDevice;
        //------------Form构造函数---------
        public Form1()
        {
            InitializeComponent();
            MyDevice = new Device();
            this.pictureBox1.Image = MyDevice.GetBigMap_UpdateMatNBitmap();
        }
        /*
        private void UpdateMyPictureBox()
        {
            if (IsRectWireFrame) DrawAllRect(mesh.Vertices, mesh.Faces); //矩形线框模式
            else DrawAllRect(mesh.Vertices, mesh.TriangleFaces); //三角形线框以及普通模式
            
        }
        */
        //-----------------UI控件逻辑-------------------
        private void ReadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.ShowDialog();
            this.MyDevice.TexturePath = o.FileName;
            this.pictureBox1.Image = this.MyDevice.GetBigMap_UpdateMatrix();
        }
        private void Button_CameraMove_Click(object sender, EventArgs e)
        {
            //mesh.Move(new Vector(-mesh.CubeCenter.x, -mesh.CubeCenter.y, -mesh.CubeCenter.z,1));
            MyStaticMethod.RotateYAroundPoint(this.MyDevice.mesh, new Vector(0, 0, 0, 1), 0.1);
            //mesh.Move(mesh.CubeCenter);
            this.pictureBox1.Image = this.MyDevice.GetBigMap_UpdateMatNBitmap();
        }

        private void CheckBox_IsTriangleWireFrame_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_IsTriangleWireFrame.Checked == true) this.MyDevice.IsTriangleWireFrame = true;
            else this.MyDevice.IsTriangleWireFrame = false;
            this.pictureBox1.Image = this.MyDevice.GetBigMap_UpdateMatrix();
        }

        private void CheckBox_IsRectWireFrame_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_IsRectWireFrame.Checked == true) this.MyDevice.IsRectWireFrame = true;
            else this.MyDevice.IsRectWireFrame = false;
            this.pictureBox1.Image = this.MyDevice.GetBigMap_UpdateMatrix();
        }

        private void CheckBox_IsTexture_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_IsTexture.Checked == true) this.MyDevice.IsTexture = true;
            else this.MyDevice.IsTexture = false;
            this.pictureBox1.Image = this.MyDevice.GetBigMap_UpdateMatrix();
        }

        private void CheckBox_IsLighting_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_IsLighting.Checked == true) this.MyDevice.IsPointLighting = true;
            else this.MyDevice.IsPointLighting = false;
            this.pictureBox1.Image = this.MyDevice.GetBigMap_UpdateMatrix();
        }

        private void EnvironmentLightScroll_Scroll(object sender, EventArgs e)
        {
            MyStaticMethod.EnvironmentLight.r = ((double)EnvironmentLightScroll.Value) / 10;
            MyStaticMethod.EnvironmentLight.g = ((double)EnvironmentLightScroll.Value) / 10;
            MyStaticMethod.EnvironmentLight.b = ((double)EnvironmentLightScroll.Value) / 10;
            this.pictureBox1.Image = this.MyDevice.GetBigMap_UpdateMatrix();
        }

        private void CheckBox_IsEye_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_IsEye.Checked == true)
            {
                MyDevice.IsEye = true;

                MyDevice.IsPointLight = false;
                CheckBox_IsPointLight.Checked = false;
                MyDevice.IsCube = false;
                CheckBox_IsLookat.Checked = false;
            }
            else
            {
                MyDevice.IsEye = false;
            }
                
        }

        private void CheckBox_IsPointLight_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_IsPointLight.Checked == true) {
                MyDevice.IsPointLight = true;

                MyDevice.IsEye = false;
                CheckBox_IsEye.Checked = false;
                MyDevice.IsCube = false;
                CheckBox_IsLookat.Checked = false;
                
            } 
            else MyDevice.IsPointLight = false;
        }

        private void CheckBox_IsLookat_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_IsLookat.Checked == true)
            {
                MyDevice.IsCube = true;

                MyDevice.IsPointLight = false;
                CheckBox_IsPointLight.Checked = false;
                MyDevice.IsEye = false;
                CheckBox_IsEye.Checked = false;
            }
                
            else MyDevice.IsCube = false;
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
            if (MyDevice.IsEye) this.MyDevice.MyCamera.eye += new Vector(0, 25, 0, 1);
            else if (MyDevice.IsCube) MyDevice.mesh.Move(new Vector(0, 25, 0, 1));  
            else if (MyDevice.IsPointLight)   MyStaticMethod.PointLight.LightPos += new Vector(0, 25, 0, 1);
            this.pictureBox1.Image = this.MyDevice.GetBigMap_UpdateMatNBitmap();
        }
        private void MoveEvent_Down()
        {
            if (MyDevice.IsEye) this.MyDevice.MyCamera.eye -= new Vector(0, 25, 0, 1);
            else if (MyDevice.IsCube) MyDevice.mesh.Move(new Vector(0, -25, 0, 1));
            else if (MyDevice.IsPointLight) MyStaticMethod.PointLight.LightPos -= new Vector(0, 25, 0, 1);
            this.pictureBox1.Image = this.MyDevice.GetBigMap_UpdateMatNBitmap();

        }
        private void MoveEvent_Left()
        {
            if (MyDevice.IsEye) this.MyDevice.MyCamera.eye += new Vector(0, 0, -25, 1);
            else if (MyDevice.IsCube) MyDevice.mesh.Move(new Vector(0, 0, -25, 1));
            else if (MyDevice.IsPointLight) MyStaticMethod.PointLight.LightPos += new Vector(0, 0, -25, 1);
            this.pictureBox1.Image = this.MyDevice.GetBigMap_UpdateMatNBitmap();
        }
        private void MoveEvent_Right()
        {
            if (MyDevice.IsEye) this.MyDevice.MyCamera.eye += new Vector(0, 0, 25, 1);
            else if (MyDevice.IsCube) MyDevice.mesh.Move(new Vector(0, 0, 25, 1));
            else if (MyDevice.IsPointLight) MyStaticMethod.PointLight.LightPos += new Vector(0, 0, 25, 1);
            this.pictureBox1.Image = this.MyDevice.GetBigMap_UpdateMatNBitmap();
        }


    }
}
