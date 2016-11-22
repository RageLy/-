using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace 居保缴费客户端
{
    public partial class change_password : Form
    {
        public change_password()
        {
            InitializeComponent();
        }
        private int marks;
        public int Marks
        {
            get { return this.marks; }
        }

        private void change_password_Load(object sender, EventArgs e)
        {
            label1.Text = "原密码：";
            label2.Text = "新密码：";
            label3.Text = "新密码：";
            label4.Text = "用户名：";

            button1.Text = "确认修改";
            button2.Text = "取消修改";

            string[] user = ConfigurationManager.AppSettings["user"].Split(',');
            textBox4.Text = user[1];
            textBox4.Enabled = false;
        }

        #region 功能函数
        private bool change_pass()
        {
            bool results = false;
            string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
            SqlConnection connection1 = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("CHANGE_PASSWORD", connection1);
            try
            {
                connection1.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@USER", SqlDbType.VarChar,22).Value = textBox4.Text.Trim();
                cmd.Parameters.Add("@PW1", SqlDbType.VarChar, 22).Value = textBox1.Text.Trim();
                cmd.Parameters.Add("@PW2", SqlDbType.VarChar, 22).Value = textBox2.Text.Trim();
                cmd.Parameters.Add("@RES", SqlDbType.Int, 4);



                cmd.Parameters["@RES"].Direction = ParameterDirection.Output;


                int s = cmd.ExecuteNonQuery();
                string[] res = new string[5];
                res[0] = cmd.Parameters["@RES"].Value.ToString();
                if (res[0] == "1")
                {
                    MessageBox.Show("修改成功");
                    results = true;
                }
                else
                {
                    MessageBox.Show("修改失败，原因代码 "+res[0]);
                }
                connection1.Close();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            return results;
        }

        private bool check_input()
        {
            bool res = false;
            if (textBox1.Text==""|| textBox2.Text == "" || textBox3.Text == "" )
            {
                MessageBox.Show("输入禁止为空！");
            }
            else if(textBox2.Text.Trim()!=textBox3.Text.Trim())
            {
                MessageBox.Show("新密码两次输入不一致！");
            }
            else if (textBox2.Text.Trim() == textBox1.Text.Trim())
            {
                MessageBox.Show("原密码与新密码不能相同！");
            }
            else
            {
                res = true;
            }

            return res;
        }
        #endregion

        #region 按钮事件
        private void button2_Click(object sender, EventArgs e)   //取消按钮
        {
            marks = 2;
            this.DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)   //确认修改按钮
        {
            bool res = check_input();
            if(res==true)
            {
                res = change_pass();
                if(res==true)
                {
                    marks = 1;
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    marks = 0;
                    this.DialogResult = DialogResult.OK;
                }
            }
        }
        #endregion
    }
}
