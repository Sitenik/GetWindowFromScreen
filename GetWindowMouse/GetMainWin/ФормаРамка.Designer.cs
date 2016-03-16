namespace GetMainWin
{
    partial class ФормаРамка
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pTop = new System.Windows.Forms.Panel();
            this.pRight = new System.Windows.Forms.Panel();
            this.pLeft = new System.Windows.Forms.Panel();
            this.pBottom = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pTop
            // 
            this.pTop.BackColor = System.Drawing.Color.Red;
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(424, 4);
            this.pTop.TabIndex = 0;
            // 
            // pRight
            // 
            this.pRight.BackColor = System.Drawing.Color.Red;
            this.pRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pRight.Location = new System.Drawing.Point(420, 4);
            this.pRight.Name = "pRight";
            this.pRight.Size = new System.Drawing.Size(4, 343);
            this.pRight.TabIndex = 1;
            // 
            // pLeft
            // 
            this.pLeft.BackColor = System.Drawing.Color.Red;
            this.pLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pLeft.Location = new System.Drawing.Point(0, 4);
            this.pLeft.Name = "pLeft";
            this.pLeft.Size = new System.Drawing.Size(4, 343);
            this.pLeft.TabIndex = 2;
            // 
            // pBottom
            // 
            this.pBottom.BackColor = System.Drawing.Color.Red;
            this.pBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pBottom.Location = new System.Drawing.Point(4, 343);
            this.pBottom.Name = "pBottom";
            this.pBottom.Size = new System.Drawing.Size(416, 4);
            this.pBottom.TabIndex = 3;
            // 
            // ФормаРамка
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 347);
            this.ControlBox = false;
            this.Controls.Add(this.pBottom);
            this.Controls.Add(this.pLeft);
            this.Controls.Add(this.pRight);
            this.Controls.Add(this.pTop);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ФормаРамка";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "F0H13A34";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ФормаРамка_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ФормаРамка_MouseClick);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Panel pRight;
        private System.Windows.Forms.Panel pLeft;
        private System.Windows.Forms.Panel pBottom;
    }
}