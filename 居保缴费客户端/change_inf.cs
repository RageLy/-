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

        #region 功能函数
        private bool data_query()       //按照身份证查询数据
        {
            bool results = false;
            string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
            SqlConnection connection1 = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("Query_Inf", connection1);
            try
            {
                connection1.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.VarChar, 22).Value = textBox2.Text.Trim();
                cmd.Parameters.Add("@MARKS1", SqlDbType.Int, 4);
                cmd.Parameters.Add("@MARKS2", SqlDbType.Int, 4);
                cmd.Parameters.Add("@MARKS3", SqlDbType.NVarChar, 12);
                cmd.Parameters.Add("@SEX", SqlDbType.NVarChar, 12);
                cmd.Parameters.Add("@TESHURENQUN", SqlDbType.NVarChar, 12);
                cmd.Parameters.Add("@YILIAODAIYU", SqlDbType.NVarChar, 12);
                cmd.Parameters.Add("@LEIXING", SqlDbType.NVarChar, 12);
                cmd.Parameters.Add("@BIRTHDAY", SqlDbType.NVarChar, 12);
                cmd.Parameters.Add("@NAME", SqlDbType.NVarChar, 12);
                cmd.Parameters.Add("@HANGHAO", SqlDbType.UniqueIdentifier, 14);


                cmd.Parameters["@HANGHAO"].Direction = ParameterDirection.Output;
                cmd.Parameters["@MARKS1"].Direction = ParameterDirection.Output;
                cmd.Parameters["@MARKS2"].Direction = ParameterDirection.Output;
                cmd.Parameters["@MARKS3"].Direction = ParameterDirection.Output;
                cmd.Parameters["@SEX"].Direction = ParameterDirection.Output;
                cmd.Parameters["@TESHURENQUN"].Direction = ParameterDirection.Output;
                cmd.Parameters["@YILIAODAIYU"].Direction = ParameterDirection.Output;
                cmd.Parameters["@LEIXING"].Direction = ParameterDirection.Output;
                cmd.Parameters["@BIRTHDAY"].Direction = ParameterDirection.Output;
                cmd.Parameters["@NAME"].Direction = ParameterDirection.Output;

                int s = cmd.ExecuteNonQuery();
                string[] res = new string[5];
                string a1 = "";
                res[0] = cmd.Parameters["@MARKS1"].Value.ToString();
                res[1] = cmd.Parameters["@MARKS2"].Value.ToString();
                res[2] = cmd.Parameters["@MARKS3"].Value.ToString();

                if (res[0] == "0")
                {
                    MessageBox.Show("人员库中没有该人员信息,请确认身份证无误\n如果是新参保人员请点击\n菜单中人员信息录入");
                }
                else
                {
                    guid = new Guid(cmd.Parameters["@HANGHAO"].Value.ToString());
                    MessageBox.Show(cmd.Parameters["@NAME"].Value.ToString());
                    textBox1.Text = cmd.Parameters["@NAME"].Value.ToString();
                    textBox3.Text = textBox2.Text.Trim();
                    a1 = cmd.Parameters["@SEX"].Value.ToString();
                    if (a1 == "男")
                    {
                        comboBox1.SelectedIndex = 1;
                    }
                    else
                    {
                        comboBox1.SelectedIndex = 0;
                    }

                    textBox4.Text = cmd.Parameters["@BIRTHDAY"].Value.ToString();

                    a1 = cmd.Parameters["@YILIAODAIYU"].Value.ToString().Trim();
                    if (a1 == "成年人")
                    {
                        comboBox2.SelectedIndex = 0;
                    }
                    else
                    {
                        comboBox2.SelectedIndex = 1;
                    }


                    a1 = cmd.Parameters["@LEIXING"].Value.ToString();
                    if (a1 == "正常续保")
                    {
                        comboBox4.SelectedIndex = 0;
                    }
                    else if (a1 == "新参保")
                    {
                        comboBox4.SelectedIndex = 1;
                    }
                    else
                    {
                        comboBox4.SelectedIndex = 2;
                    }

                    if (res[2] == "正常")
                    {
                        label10.Text = "";
                        button3.Visible = false;
                        button3.Enabled = false;
                    }
                    else if (res[2] == "存在重复号码")
                    {
                        label10.Text = "注意：该身份证存在问题，具体表现为" + res[2] + " 如果信息不符合，点击下一个查看相同身份证的其他人员的信息";
                        button3.Visible = true;
                        button3.Enabled = true;
                    }
                    else
                    {
                        label10.Text = "注意：该身份证存在问题，具体表现为" + res[2];
                        button3.Visible = false;
                        button3.Enabled = false;
                    }
                    results = true;
                }
                
                connection1.Close();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            return results;
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)   //查询按钮
        {
            bool res = data_query();
            if(res==false)
            {
                MessageBox.Show("查询失败");
            }
        }
    }
}
