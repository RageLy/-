using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 居保缴费客户端
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void check()
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "乡镇编号:";
            label2.Text = "用户名:";
            label3.Text = "密码:";
            button1.Text = "登陆";
            button2.Text = "取消";
            checkBox1.Text = "记住用户名密码";
   
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
