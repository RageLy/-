using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;

namespace 居保缴费客户端
{
    public partial class pay_change : Form
    {
        #region  界面开启时的事件
        public pay_change()
        {
            InitializeComponent();
        }
        private string[] user;
        private void pay_change_Load(object sender, EventArgs e)
        {
            groupBox1.Text = "已缴费人员查询";
            groupBox2.Text = "查询结果";

            label1.Text = "性别";
            label2.Text = "身份证号码";
            label3.Text = "姓名";
            label4.Text = "身份证号码";
            label5.Text = "出生日期";
            label6.Text = "社会保障卡号";
            label7.Text = "医疗待遇类别";
            label8.Text = "特殊群体";
            label9.Text = "类型";

            textBox3.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;

            comboBox1.Items.Add("女");
            comboBox1.Items.Add("男");
            comboBox2.Items.Add("成年人");
            comboBox2.Items.Add("未成年人");

            button1.Text = "查询";
            button2.Text = "确认添加";

            groupBox2.Enabled = false;
            user = ConfigurationManager.AppSettings["user"].Split(',');
        }
        #endregion

        #region 功能函数
        private void auto_fill(string id)     //根据身份证号码自动生成人员基础信息
        {
            if(id.Length==15)
            {
                textBox3.Text = textBox2.Text;
                textBox7.Text = "非特殊群体";
                textBox8.Text = "新增人员";
                int v1 = int.Parse(id.Substring(13, 1));
                if(v1%2==0)
                {
                    comboBox1.SelectedIndex = 0;
                }
                else
                {
                    comboBox1.SelectedIndex = 1;
                }
                string dates = "19"+id.Substring(6, 2) + "-" + id.Substring(8, 2) + "-" + id.Substring(10,2);
                try
                {
                    DateTime dt = DateTime.Parse(dates);
                    DateTime dt1 = DateTime.Parse("1998-12-1");
                    textBox4.Text = dates;
                    int v2 = DateTime.Compare(dt, dt1);
                    if(v2<0)
                    {
                        comboBox2.SelectedIndex = 0;
                    }
                    else
                    {
                        comboBox2.SelectedIndex = 1;
                    }

                    groupBox2.Enabled = true;
                }
                catch
                {

                }
            }
            else if(id.Length == 18)
            {
                textBox3.Text = textBox2.Text;
                textBox7.Text = "非特殊群体";
                textBox8.Text = "新增人员";
                int v1 = int.Parse(id.Substring(16, 1));
                if (v1 % 2 == 0)
                {
                    comboBox1.SelectedIndex = 0;
                }
                else
                {
                    comboBox1.SelectedIndex = 1;
                }
                string dates =id.Substring(6, 4) + "-" + id.Substring(10, 2) + "-" + id.Substring(12, 2);
                try
                {
                    DateTime dt = DateTime.Parse(dates);
                    DateTime dt1 = DateTime.Parse("1998-12-1");
                    textBox4.Text = dates;
                    int v2 = DateTime.Compare(dt, dt1);
                    if (v2 < 0)
                    {
                        comboBox2.SelectedIndex = 0;
                    }
                    else
                    {
                        comboBox2.SelectedIndex = 1;
                    }

                    groupBox2.Enabled = true;
                }
                catch
                {

                }
            }
            else
            {

            }
        }

        private bool inf_check()              //上传数据时检查数据格式
        {
            bool results = false;
            try
            {
                if(textBox1.Text==""||textBox5.Text.Trim().Length>15)
                {
                    MessageBox.Show("姓名或者社保卡号输入错误");
                }
                else
                {
                    DateTime dt = DateTime.Parse(textBox4.Text.Trim());
                    results = true;
                }
            }
            catch
            {

            }
            return results;
        }

        private bool query_id()    //检查输入的身份证在数据库中是否存在
        {
            bool results = false;
            string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
            SqlConnection connection1 = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("ID_QUERY", connection1);
            try
            {
                connection1.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.VarChar, 22).Value = textBox2.Text.Trim();
                cmd.Parameters.Add("@RES", SqlDbType.Int, 4);



                cmd.Parameters["@RES"].Direction = ParameterDirection.Output;


                int s = cmd.ExecuteNonQuery();
                string[] res = new string[5];
                res[0] = cmd.Parameters["@RES"].Value.ToString();
                if (res[0] == "0")
                {
                    auto_fill(textBox2.Text.Trim());
                    results = true;
                }
                else
                {
                    MessageBox.Show("身份证已存在");
                }
                connection1.Close();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            return results;
        }

        private bool add_inf()     //向数据库中增加人员信息
        {
            bool results = false;
            string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
            SqlConnection connection1 = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("ADD_INF", connection1);
            try
            {
                connection1.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.VarChar, 22).Value = textBox3.Text.Trim();
                cmd.Parameters.Add("@RES", SqlDbType.Int, 4);
                cmd.Parameters.Add("@NAME", SqlDbType.NVarChar, 12).Value = textBox1.Text.Trim();
                cmd.Parameters.Add("@SEX", SqlDbType.NVarChar, 12).Value = comboBox1.Text.Trim();
                cmd.Parameters.Add("@REGIN_CODE", SqlDbType.VarChar, 22).Value = user[0];
                cmd.Parameters.Add("@BIRTHDAY", SqlDbType.Date, 22).Value = DateTime.Parse(textBox4.Text.Trim());
                cmd.Parameters.Add("@SHEBAOKAHAO", SqlDbType.VarChar, 22).Value = textBox5.Text.Trim();
                cmd.Parameters.Add("@YILIAODAIYU", SqlDbType.NVarChar, 12).Value = comboBox2.Text.Trim();

                cmd.Parameters["@RES"].Direction = ParameterDirection.Output;


                int s = cmd.ExecuteNonQuery();
                string[] res = new string[5];
                res[0] = cmd.Parameters["@RES"].Value.ToString();
                if (res[0] == "1")
                {
                    MessageBox.Show("添加成功，在缴费信息录入中为其缴费");
                    results = true;
                }
                else
                {
                    MessageBox.Show("添加失败，在缴费信息录入中为其缴费");
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

        #region 按钮的响应事件
        private void button1_Click(object sender, EventArgs e)      //查询按钮
        {
            if (textBox2.Text.Trim().Length == 15 || textBox2.Text.Trim().Length == 18)
            {
                bool res = query_id();
                if (res == true)
                {

                }
                else
                {
                    groupBox2.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("身份证长度错误");
            }
        }

        private void button2_Click(object sender, EventArgs e)      //添加按钮
        {

            bool res = inf_check();
            if(res==true)
            {
                res = add_inf();
                if (res == true)
                {
                    groupBox2.Enabled = false;
                }
                else
                {

                }
            }
            else
            {
                
            }
        }
        #endregion
    }
}
