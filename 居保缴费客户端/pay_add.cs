using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Configuration;

namespace 居保缴费客户端
{
    public partial class pay_add : Form
    {
        public pay_add()
        {
            InitializeComponent();
        }
        private string[] steps=new string[10];
        private string[] r_steps = new string[10];
        private string[] user;
        private void pay_add_Load(object sender, EventArgs e)
        {
            groupBox1.Text = "信息查询";
            groupBox2.Text = "查询结果";
            label2.Text = "身份证号码";
            button1.Text = "查询";
            button2.Text = "确认";

            label1.Text = "姓名";
            label3.Text = "性别";
            label4.Text = "出生日期";
            label5.Text = "身份证号";
            label6.Text = "医疗待遇类别";
            label7.Text = "类型";
            label8.Text = "特殊人群";
            label9.Text = "缴费金额";


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

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim().Length != 16 && textBox2.Text.Trim().Length != 18)
            {
                MessageBox.Show("身份证号码长度有误");
            }
            else
            {
                string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
                int i = 0;
                SqlConnection connection1 = new SqlConnection(con);
                SqlCommand cmd = new SqlCommand("Query_Inf", connection1);
                try
                {
                    connection1.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.VarChar, 22).Value = textBox2.Text.Trim();
                    cmd.Parameters.Add("@MARKS1", SqlDbType.Int, 4);
                    cmd.Parameters.Add("@SEX",SqlDbType.NVarChar , 12);
                    cmd.Parameters.Add("@TESHURENQUN", SqlDbType.NVarChar, 12);
                    cmd.Parameters.Add("@YILIAODAIYU", SqlDbType.NVarChar, 12);
                    cmd.Parameters.Add("@LEIXING", SqlDbType.NVarChar, 12);
                    cmd.Parameters.Add("@BIRTHDAY", SqlDbType.NVarChar, 12);
                    cmd.Parameters.Add("@NAME", SqlDbType.NVarChar, 12);


                    cmd.Parameters["@MARKS1"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@SEX"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@TESHURENQUN"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@YILIAODAIYU"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@LEIXING"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@BIRTHDAY"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@NAME"].Direction = ParameterDirection.Output;

                    int s = cmd.ExecuteNonQuery();
                    string res = cmd.Parameters["@MARKS1"].Value.ToString();
                    if(res == "0")
                    {
                        MessageBox.Show("人员库中没有该人员信息,请确认身份证无误\n如果是新参保人员请点击\n菜单中新增人员信息 ");
                    }
                    else if(res=="1")
                    {
                        MessageBox.Show("该人员已经缴费！请不要重复缴费");
                    }
                    else
                    {
                        textBox1.Text= cmd.Parameters["@NAME"].Value.ToString();
                        textBox3.Text = cmd.Parameters["@SEX"].Value.ToString();
                        textBox4.Text = cmd.Parameters["@BIRTHDAY"].Value.ToString();
                        textBox5.Text = textBox2.Text;
                        textBox6.Text = cmd.Parameters["@YILIAODAIYU"].Value.ToString();
                        textBox7.Text = cmd.Parameters["@LEIXING"].Value.ToString();

                    }
                }
                catch(Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
            }
        }
    }
}
