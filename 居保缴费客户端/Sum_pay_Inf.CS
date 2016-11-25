using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 居保缴费客户端
{
    public partial class Sum_pay_Inf : Form
    {
        public Sum_pay_Inf()
        {
            InitializeComponent();
        }

        private void Sum_pay_Inf_Load(object sender, EventArgs e)
        {
            chushihua();
        }
        private void chushihua()
        {
            label1.Text = "地区";
            label2.Text = "时间段";
            label3.Text = "金额";
            label4.Text = "—";
            label5.Text = "";

            progressBar1.Visible = false;

            button1.Text = "查询";
            button1.Text = "导出";
            button1.Text = "取消";

            groupBox1.Text = "查询条件";
            groupBox2.Text = "查询结果";

            comboBox2.Items.Add("全部");
            comboBox2.Items.Add("150");
            comboBox2.Items.Add("100");

            string s = DateTime.Now.ToShortDateString();
            textBox1.Text = "2016/11/20";
            textBox2.Text = s;
        }

        #region 按钮事件
        private void button3_Click(object sender, EventArgs e)   //取消按钮
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)   //查询按钮
        {

        }

        private void button2_Click(object sender, EventArgs e)    //导出按钮
        {

        }
        #endregion

        #region 功能函数

        #endregion
    }
}
