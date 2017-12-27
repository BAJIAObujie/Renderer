using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


/*
using Microsoft.DirectX;          //开发directx需要包含的两个命名空间
using Microsoft.DirectX.Direct3D;
*/


namespace WindowsFormsApp1
{
    public partial class Form1
    {
   
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        protected void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Button_ReadTexture = new System.Windows.Forms.Button();
            this.CheckBox_IsTriangleWireFrame = new System.Windows.Forms.CheckBox();
            this.CheckBox_IsRectWireFrame = new System.Windows.Forms.CheckBox();
            this.CheckBox_IsTexture = new System.Windows.Forms.CheckBox();
            this.CheckBox_IsLighting = new System.Windows.Forms.CheckBox();
            this.EnvironmentLightScroll = new System.Windows.Forms.TrackBar();
            this.CheckBox_IsEye = new System.Windows.Forms.CheckBox();
            this.CheckBox_IsLookat = new System.Windows.Forms.CheckBox();
            this.CheckBox_IsPointLight = new System.Windows.Forms.CheckBox();
            this.Button_CameraMove = new System.Windows.Forms.Button();
            this.EnvironmentLight = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EnvironmentLightScroll)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.BackColor = System.Drawing.Color.Gray;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.pictureBox1.Location = new System.Drawing.Point(184, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(480, 480);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Button_ReadTexture
            // 
            this.Button_ReadTexture.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button_ReadTexture.Location = new System.Drawing.Point(12, 12);
            this.Button_ReadTexture.Name = "Button_ReadTexture";
            this.Button_ReadTexture.Size = new System.Drawing.Size(152, 34);
            this.Button_ReadTexture.TabIndex = 3;
            this.Button_ReadTexture.Text = "ReadTexture";
            this.Button_ReadTexture.UseVisualStyleBackColor = true;
            this.Button_ReadTexture.Click += new System.EventHandler(this.ReadImage_Click);
            // 
            // CheckBox_IsTriangleWireFrame
            // 
            this.CheckBox_IsTriangleWireFrame.AutoSize = true;
            this.CheckBox_IsTriangleWireFrame.Location = new System.Drawing.Point(12, 123);
            this.CheckBox_IsTriangleWireFrame.Name = "CheckBox_IsTriangleWireFrame";
            this.CheckBox_IsTriangleWireFrame.Size = new System.Drawing.Size(138, 16);
            this.CheckBox_IsTriangleWireFrame.TabIndex = 8;
            this.CheckBox_IsTriangleWireFrame.Text = "IsTriangleWireFrame";
            this.CheckBox_IsTriangleWireFrame.UseVisualStyleBackColor = true;
            this.CheckBox_IsTriangleWireFrame.CheckedChanged += new System.EventHandler(this.CheckBox_IsTriangleWireFrame_CheckedChanged);
            // 
            // CheckBox_IsRectWireFrame
            // 
            this.CheckBox_IsRectWireFrame.AutoSize = true;
            this.CheckBox_IsRectWireFrame.Location = new System.Drawing.Point(11, 161);
            this.CheckBox_IsRectWireFrame.Name = "CheckBox_IsRectWireFrame";
            this.CheckBox_IsRectWireFrame.Size = new System.Drawing.Size(114, 16);
            this.CheckBox_IsRectWireFrame.TabIndex = 9;
            this.CheckBox_IsRectWireFrame.Text = "IsRectWireFrame";
            this.CheckBox_IsRectWireFrame.UseVisualStyleBackColor = true;
            this.CheckBox_IsRectWireFrame.CheckedChanged += new System.EventHandler(this.CheckBox_IsRectWireFrame_CheckedChanged);
            // 
            // CheckBox_IsTexture
            // 
            this.CheckBox_IsTexture.AutoSize = true;
            this.CheckBox_IsTexture.Location = new System.Drawing.Point(11, 198);
            this.CheckBox_IsTexture.Name = "CheckBox_IsTexture";
            this.CheckBox_IsTexture.Size = new System.Drawing.Size(78, 16);
            this.CheckBox_IsTexture.TabIndex = 10;
            this.CheckBox_IsTexture.Text = "IsTexture";
            this.CheckBox_IsTexture.UseVisualStyleBackColor = true;
            this.CheckBox_IsTexture.CheckedChanged += new System.EventHandler(this.CheckBox_IsTexture_CheckedChanged);
            // 
            // CheckBox_IsLighting
            // 
            this.CheckBox_IsLighting.AutoSize = true;
            this.CheckBox_IsLighting.Location = new System.Drawing.Point(11, 237);
            this.CheckBox_IsLighting.Name = "CheckBox_IsLighting";
            this.CheckBox_IsLighting.Size = new System.Drawing.Size(114, 16);
            this.CheckBox_IsLighting.TabIndex = 11;
            this.CheckBox_IsLighting.Text = "IsPointLighting";
            this.CheckBox_IsLighting.UseVisualStyleBackColor = true;
            this.CheckBox_IsLighting.CheckedChanged += new System.EventHandler(this.CheckBox_IsLighting_CheckedChanged);
            // 
            // EnvironmentLightScroll
            // 
            this.EnvironmentLightScroll.Location = new System.Drawing.Point(11, 295);
            this.EnvironmentLightScroll.Maximum = 100;
            this.EnvironmentLightScroll.Name = "EnvironmentLightScroll";
            this.EnvironmentLightScroll.Size = new System.Drawing.Size(104, 45);
            this.EnvironmentLightScroll.TabIndex = 12;
            this.EnvironmentLightScroll.Scroll += new System.EventHandler(this.EnvironmentLightScroll_Scroll);
            // 
            // CheckBox_IsEye
            // 
            this.CheckBox_IsEye.AutoSize = true;
            this.CheckBox_IsEye.Location = new System.Drawing.Point(11, 346);
            this.CheckBox_IsEye.Name = "CheckBox_IsEye";
            this.CheckBox_IsEye.Size = new System.Drawing.Size(72, 16);
            this.CheckBox_IsEye.TabIndex = 13;
            this.CheckBox_IsEye.Text = "IsCamera";
            this.CheckBox_IsEye.UseVisualStyleBackColor = true;
            this.CheckBox_IsEye.CheckedChanged += new System.EventHandler(this.CheckBox_IsEye_CheckedChanged);
            // 
            // CheckBox_IsLookat
            // 
            this.CheckBox_IsLookat.AutoSize = true;
            this.CheckBox_IsLookat.Location = new System.Drawing.Point(11, 381);
            this.CheckBox_IsLookat.Name = "CheckBox_IsLookat";
            this.CheckBox_IsLookat.Size = new System.Drawing.Size(60, 16);
            this.CheckBox_IsLookat.TabIndex = 14;
            this.CheckBox_IsLookat.Text = "IsCube";
            this.CheckBox_IsLookat.UseVisualStyleBackColor = true;
            this.CheckBox_IsLookat.CheckedChanged += new System.EventHandler(this.CheckBox_IsLookat_CheckedChanged);
            // 
            // CheckBox_IsPointLight
            // 
            this.CheckBox_IsPointLight.AutoSize = true;
            this.CheckBox_IsPointLight.Location = new System.Drawing.Point(11, 417);
            this.CheckBox_IsPointLight.Name = "CheckBox_IsPointLight";
            this.CheckBox_IsPointLight.Size = new System.Drawing.Size(96, 16);
            this.CheckBox_IsPointLight.TabIndex = 15;
            this.CheckBox_IsPointLight.Text = "IsPointLight";
            this.CheckBox_IsPointLight.UseVisualStyleBackColor = true;
            this.CheckBox_IsPointLight.CheckedChanged += new System.EventHandler(this.CheckBox_IsPointLight_CheckedChanged);
            // 
            // Button_CameraMove
            // 
            this.Button_CameraMove.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button_CameraMove.Location = new System.Drawing.Point(12, 62);
            this.Button_CameraMove.Name = "Button_CameraMove";
            this.Button_CameraMove.Size = new System.Drawing.Size(152, 37);
            this.Button_CameraMove.TabIndex = 7;
            this.Button_CameraMove.Text = "CameraMove";
            this.Button_CameraMove.UseVisualStyleBackColor = true;
            this.Button_CameraMove.Click += new System.EventHandler(this.Button_CameraMove_Click);
            // 
            // EnvironmentLight
            // 
            this.EnvironmentLight.AutoSize = true;
            this.EnvironmentLight.Location = new System.Drawing.Point(14, 280);
            this.EnvironmentLight.Name = "EnvironmentLight";
            this.EnvironmentLight.Size = new System.Drawing.Size(101, 12);
            this.EnvironmentLight.TabIndex = 16;
            this.EnvironmentLight.Text = "EnvironmentLight";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 575);
            this.Controls.Add(this.EnvironmentLight);
            this.Controls.Add(this.CheckBox_IsPointLight);
            this.Controls.Add(this.CheckBox_IsLookat);
            this.Controls.Add(this.CheckBox_IsEye);
            this.Controls.Add(this.EnvironmentLightScroll);
            this.Controls.Add(this.CheckBox_IsLighting);
            this.Controls.Add(this.CheckBox_IsTexture);
            this.Controls.Add(this.CheckBox_IsRectWireFrame);
            this.Controls.Add(this.CheckBox_IsTriangleWireFrame);
            this.Controls.Add(this.Button_CameraMove);
            this.Controls.Add(this.Button_ReadTexture);
            this.Controls.Add(this.pictureBox1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EnvironmentLightScroll)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected PictureBox pictureBox1;
        private Button Button_ReadTexture;
        private CheckBox CheckBox_IsTriangleWireFrame;
        private CheckBox CheckBox_IsRectWireFrame;
        private CheckBox CheckBox_IsTexture;
        private CheckBox CheckBox_IsLighting;
        private TrackBar EnvironmentLightScroll;
        private CheckBox CheckBox_IsEye;
        private CheckBox CheckBox_IsLookat;
        private CheckBox CheckBox_IsPointLight;
        private Button Button_CameraMove;
        private Label EnvironmentLight;
    }
}

