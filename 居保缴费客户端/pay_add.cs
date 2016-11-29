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
        private string[] values = new string[10];       //
        static private string aaa = "";
        Guid guid;                                    //记录当前缴费人员的guid
        private void pay_add_Load(object sender, EventArgs e)
        {
            groupBox1.Text = "信息查询";
            groupBox2.Text = "查询结果";
            label2.Text = "身份证号码";
            button1.Text = "查询";
            button2.Text = "确认缴费";
            button3.Text = "下一个";
            button3.Visible = false;
            button3.Enabled = false;
            checkBox1.Text = "外来或者暂住人员";

            label1.Text = "姓名";
            label3.Text = "性别";
            label4.Text = "出生日期";
            label5.Text = "身份证号";
            label6.Text = "医疗待遇类别";
            label7.Text = "类型";
            label8.Text = "特殊人群";
            label9.Text = "缴费金额";
            label10.Text = "";
            label11.Text = "备注";

            textBox1.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            textBox9.Enabled = false;


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
            if (textBox2.Text.Trim().Length != 15 && textBox2.Text.Trim().Length != 18)
            {
                MessageBox.Show("身份证号码长度有误");
                button2.Enabled = false;
            }
            else
            {
                bool res = data_query();
                if(res==true)
                {
                    button2.Enabled = true;
                }
                else
                {
                    button2.Enabled = false;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)       //下一个按钮
        {
            if (textBox2.Text.Trim().Length != 16 && textBox2.Text.Trim().Length != 18)
            {
                MessageBox.Show("身份证号码长度有误");
                button2.Enabled = false;
            }
            else
            {
                bool res = data_query_next();
                if (res == true)
                {
                    button2.Enabled = true;
                }
                else
                {
                    button2.Enabled = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)      //确认缴费按钮
        {
            bool res = update_data();
            if (res == true)
            {
                textBox2.Text = "";
                checkBox1.Checked = false;
                button2.Enabled = false;
            }
            else
            {

            }
        }

        #region 数据的查询和修改
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
                res[0] = cmd.Parameters["@MARKS1"].Value.ToString();
                res[1] = cmd.Parameters["@MARKS2"].Value.ToString();
                res[2] = cmd.Parameters["@MARKS3"].Value.ToString();
                
                if (res[0] == "0")
                {
                    MessageBox.Show("人员库中没有该人员信息,请确认身份证无误\n如果是新参保人员请点击\n菜单中人员信息录入");
                }
                else if (res[0] == "1")
                {
                    MessageBox.Show("该人员已经缴费！请不要重复缴费");
                }
                else
                {
                    guid = new Guid(cmd.Parameters["@HANGHAO"].Value.ToString());
                    textBox1.Text = cmd.Parameters["@NAME"].Value.ToString();
                    textBox3.Text = cmd.Parameters["@SEX"].Value.ToString();
                    textBox4.Text = cmd.Parameters["@BIRTHDAY"].Value.ToString();
                    textBox5.Text = textBox2.Text;
                    textBox6.Text = cmd.Parameters["@YILIAODAIYU"].Value.ToString();
                    textBox7.Text = cmd.Parameters["@LEIXING"].Value.ToString();
                    if (res[1] == "1")
                    {
                        textBox8.Text = "困难老人";
                        textBox9.Text = "100";
                    }
                    else if(res[1]=="2")
                    {
                        textBox8.Text = "非特殊人群";
                        textBox9.Text = "150";
                    }
                    else if(res[1]=="3")
                    {
                        textBox8.Text = "低保人群";
                        textBox9.Text = "0";
                    }
                    else
                    {
                        textBox8.Text = "重残人群";
                        textBox9.Text = "0";
                    }

                    if (res[2] == "正常")
                    {
                        label10.Text = "";
                        button3.Visible = false;
                        button3.Enabled = false;
                    }
                    else if(res[2]=="存在重复号码")
                    {
                        label10.Text = "注意：该身份证存在问题，具体表现为" + res[2]+" 如果信息不符合，点击下一个查看相同身份证的其他人员的信息";
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
                    aaa = textBox9.Text;
                }
                connection1.Close();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            return results;
        }

        private bool data_query_next()        //查询身份证号码重复的人员
        {
            bool results = false;
            string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
            SqlConnection connection1 = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("Query_Next_Inf", connection1);
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
                cmd.Parameters.Add("@HANGHAO1", SqlDbType.UniqueIdentifier, 14).Value= guid;
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
                res[0] = cmd.Parameters["@MARKS1"].Value.ToString();
                res[1] = cmd.Parameters["@MARKS2"].Value.ToString();
                res[2] = cmd.Parameters["@MARKS3"].Value.ToString();
                //values[0] = cmd.Parameters["@HANGHAO"].Value.ToString();
                guid = new Guid(cmd.Parameters["@HANGHAO"].Value.ToString());
                if (res[0] == "0")
                {
                    MessageBox.Show("人员库中没有该人员信息,请确认身份证无误\n如果是新参保人员请点击\n菜单中新增人员信息 ");
                }
                else if (res[0] == "1")
                {
                    MessageBox.Show("该人员已经缴费！请不要重复缴费");
                }
                else
                {
                    textBox1.Text = cmd.Parameters["@NAME"].Value.ToString();
                    textBox3.Text = cmd.Parameters["@SEX"].Value.ToString();
                    textBox4.Text = cmd.Parameters["@BIRTHDAY"].Value.ToString();
                    textBox5.Text = textBox2.Text;
                    textBox6.Text = cmd.Parameters["@YILIAODAIYU"].Value.ToString();
                    textBox7.Text = cmd.Parameters["@LEIXING"].Value.ToString();
                    if (res[1] == "1")
                    {
                        textBox8.Text = "困难老人";
                        textBox9.Text = "100";
                    }
                    else if (res[1] == "2")
                    {
                        textBox8.Text = "非特殊人群";
                        textBox9.Text = "150";
                    }
                    else if (res[1] == "3")
                    {
                        textBox8.Text = "低保人群";
                        textBox9.Text = "0";
                    }
                    else
                    {
                        textBox8.Text = "重残人群";
                        textBox9.Text = "0";
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
                    aaa = textBox9.Text;
                }
                connection1.Close();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            return results;
        }

        private bool update_data()            //上传缴费记录
        {
            bool results = false;
            string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
            SqlConnection connection1 = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("PAY_MONEY", connection1);
            try
            {
                connection1.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@GUID", SqlDbType.UniqueIdentifier, 14).Value = guid;
                cmd.Parameters.Add("@DATE", SqlDbType.DateTime, 4).Value=DateTime.Now;
                cmd.Parameters.Add("@MONEY", SqlDbType.Int, 4).Value=int.Parse(textBox9.Text);
                cmd.Parameters.Add("@REGIN_CODE", SqlDbType.NVarChar, 100).Value=user[0]+richTextBox1.Text.Trim();
                cmd.Parameters.Add("@RES", SqlDbType.Int, 4);



                cmd.Parameters["@RES"].Direction = ParameterDirection.Output;


                int s = cmd.ExecuteNonQuery();
                string[] res = new string[5];
                res[0] = cmd.Parameters["@RES"].Value.ToString();
                if (res[0] == "0")
                {
                    MessageBox.Show("该人员已缴费");
                }
                else if (res[0] == "1")
                {
                    MessageBox.Show("缴费成功");
                    results = true;
                }
                else if(res[0]=="2")
                {
                    MessageBox.Show("缴费人员不存在");
                }
                else
                {
                    MessageBox.Show("缴费失败");
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
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {          
            if (checkBox1.Checked==true)
            {
                textBox9.Text = "150";
            }
            else
            {
                textBox9.Text = aaa;
            }
        }
    }
}
