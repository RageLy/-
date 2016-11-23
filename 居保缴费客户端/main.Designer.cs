namespace 居保缴费客户端
{
    partial class main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.缴费信息录入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.缴费信息更改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.缴费记录查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.信息统计ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.基本信息修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改密码ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.缴费信息录入ToolStripMenuItem,
            this.缴费信息更改ToolStripMenuItem,
            this.缴费记录查询ToolStripMenuItem,
            this.信息统计ToolStripMenuItem,
            this.基本信息修改ToolStripMenuItem,
            this.修改密码ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(801, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 缴费信息录入ToolStripMenuItem
            // 
            this.缴费信息录入ToolStripMenuItem.Name = "缴费信息录入ToolStripMenuItem";
            this.缴费信息录入ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.缴费信息录入ToolStripMenuItem.Text = "缴费信息录入";
            this.缴费信息录入ToolStripMenuItem.Click += new System.EventHandler(this.缴费信息录入ToolStripMenuItem_Click);
            // 
            // 缴费信息更改ToolStripMenuItem
            // 
            this.缴费信息更改ToolStripMenuItem.Name = "缴费信息更改ToolStripMenuItem";
            this.缴费信息更改ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.缴费信息更改ToolStripMenuItem.Text = "人员信息录入";
            this.缴费信息更改ToolStripMenuItem.Click += new System.EventHandler(this.缴费信息更改ToolStripMenuItem_Click);
            // 
            // 缴费记录查询ToolStripMenuItem
            // 
            this.缴费记录查询ToolStripMenuItem.Name = "缴费记录查询ToolStripMenuItem";
            this.缴费记录查询ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.缴费记录查询ToolStripMenuItem.Text = "缴费记录查询";
            this.缴费记录查询ToolStripMenuItem.Click += new System.EventHandler(this.缴费记录查询ToolStripMenuItem_Click);
            // 
            // 信息统计ToolStripMenuItem
            // 
            this.信息统计ToolStripMenuItem.Name = "信息统计ToolStripMenuItem";
            this.信息统计ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.信息统计ToolStripMenuItem.Text = "信息统计";
            this.信息统计ToolStripMenuItem.Click += new System.EventHandler(this.信息统计ToolStripMenuItem_Click);
            // 
            // 基本信息修改ToolStripMenuItem
            // 
            this.基本信息修改ToolStripMenuItem.Name = "基本信息修改ToolStripMenuItem";
            this.基本信息修改ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.基本信息修改ToolStripMenuItem.Text = "基本信息修改";
            this.基本信息修改ToolStripMenuItem.Click += new System.EventHandler(this.基本信息修改ToolStripMenuItem_Click);
            // 
            // 修改密码ToolStripMenuItem
            // 
            this.修改密码ToolStripMenuItem.Name = "修改密码ToolStripMenuItem";
            this.修改密码ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.修改密码ToolStripMenuItem.Text = "修改密码";
            this.修改密码ToolStripMenuItem.Click += new System.EventHandler(this.修改密码ToolStripMenuItem_Click);
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 553);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "主界面";
            this.Load += new System.EventHandler(this.main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 缴费信息录入ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 缴费信息更改ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 缴费记录查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 信息统计ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 基本信息修改ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改密码ToolStripMenuItem;
    }
}