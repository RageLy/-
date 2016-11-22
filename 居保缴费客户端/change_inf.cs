using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace 居保缴费客户端
{
    public partial class change_inf : Form
    {
        #region 界面开启时的事件
        public change_inf()
        {
            InitializeComponent();
        }

        private string[] steps = new string[10];
        private string[] r_steps = new string[10];
        private string[] user;
        private string[] values = new string[10];       //
        Guid guid;                                    //记录当前缴费人员的guid

        private void change_inf_Load(object sender, EventArgs e)
        {
            groupBox1.Text = "信息查询";
            groupBox2.Text = "查询结果";
            label2.Text = "身份证号码";
            button1.Text = "查询";
            button2.Text = "确认修改";
            button3.Text = "下一个";
            button3.Visible = false;
            button3.Enabled = false;

            label1.Text = "性别";
            label2.Text = "身份证号码";
            label3.Text = "姓名";
            label4.Text = "身份证号码";
            label5.Text = "出生日期";
            label6.Text = "社会保障卡号";
            label7.Text = "医疗待遇类别";
            label8.Text = "特殊群体";
            label9.Text = "类型";
            label10.Text = "";

            comboBox1.Items.Add("女");
            comboBox1.Items.Add("男");
            comboBox2.Items.Add("成年人");
            comboBox2.Items.Add("未成年人");
            comboBox3.Items.Add("非特殊人群");
            comboBox3.Items.Add("困难老人");
            comboBox3.Items.Add("低保");
            comboBox3.Items.Add("重度残疾");
            comboBox4.Items.Add("正常续保");
            comboBox4.Items.Add("新参保");
            comboBox4.Items.Add("中断库");

            steps[0] = "数据库连接中..."; r_steps[0] = "数据库连接中...";
            steps[1] = "数据库连接成功"; r_steps[1] = "数据库连接失败";
            steps[2] = "数据查询中..."; r_steps[2] = "数据查询中...";
            steps[3] = "数据查询成功"; r_steps[3] = "数据查询失败";
            steps[4] = "数据计算中..."; r_steps[4] = "数据计算中...";
            steps[5] = "数据计算成功"; r_steps[5] = "数据计算失败";
            steps[6] = "数据上传中..."; r_steps[6] = "数据上传中...";
            steps[7] = "数据上传成功"; r_steps[7] = "数据上传失败";

            user = ConfigurationManager.AppSettings["user"].Split(',');
        }
        #endregion
    }
}
