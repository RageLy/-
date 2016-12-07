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
            textBox6.Enabled = false;

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
            label11.Text = "是否缴费";

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
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;

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
            try
            {
                connection1.Open();
                string sqlstring1 = "SELECT * FROM 人员基础信息  WHERE 公民身份号码='"+textBox2.Text.Trim()+"'";
                // MessageBox.Show(sqlstring1);
                SqlCommand mycom = new SqlCommand(sqlstring1, connection1);
                SqlDataReader dr = mycom.ExecuteReader();
           
                results = true;
                if (dr.Read())
                {
                    string[] RESS = new string[15];
                    guid =new Guid(dr[0].ToString());
                    RESS[0] = dr[0].ToString();                  //  行号
                    RESS[1] = dr[1].ToString();                   //个人编号
                    RESS[2] = dr[2].ToString();                   //单位编号
                    RESS[3] = dr[3].ToString();                   //单位名称
                    RESS[4] = dr[4].ToString();                   //身份证号码
                    RESS[5] = dr[5].ToString();                   //姓名
                    RESS[6] = dr[6].ToString();                   //特殊人群
                    RESS[7] = dr[7].ToString();                   //性别
                    RESS[8] = dr[8].ToString();                   //出生日期
                    RESS[9] = dr[9].ToString();                   //社会保障卡号
                    RESS[10] = dr[10].ToString();                   //医疗待遇
                    RESS[11] = dr[11].ToString();                   //类型
                    RESS[12] = dr[12].ToString();                   //是否缴费
                    RESS[13] = dr[13].ToString();                   //身份证问题
                    textBox1.Text = RESS[5];
                    textBox3.Text = RESS[4];
                    textBox4.Text = DateTime.Parse(RESS[8]).ToShortDateString();
                    textBox5.Text = RESS[9];
                    textBox6.Text = RESS[12];
                    label10.Text = "身份证问题：   "+RESS[13];         
                    if (RESS[7] == "男")
                    {
                        comboBox1.SelectedIndex = 1;
                    }
                    else
                    {
                        comboBox1.SelectedIndex = 0;
                    }

                    if (RESS[10] == "成年人")
                    {
                        comboBox2.SelectedIndex = 0;
                    }
                    else
                    {
                        comboBox2.SelectedIndex = 1;
                    }


                    if (RESS[6] == "非特殊人群")
                    {
                        comboBox3.SelectedIndex = 0;
                    }
                    else if (RESS[6] == "困难老人")
                    {
                        comboBox3.SelectedIndex = 1;
                    }
                    else if (RESS[6] == "低保人员")
                    {
                        comboBox3.SelectedIndex = 2;
                    }
                    else
                    {
                        comboBox3.SelectedIndex = 3;
                    }

                    if (RESS[11] == "正常续保")
                    {
                        comboBox4.SelectedIndex = 0;
                    }
                    else if (RESS[11] == "新参保")
                    {
                        comboBox4.SelectedIndex = 1;
                    }
                    else
                    {
                        comboBox4.SelectedIndex = 2;
                    }

                }
                else
                {
                    MessageBox.Show("该人员不存在");
                }

                connection1.Close();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            return results;
        }

        private bool data_change()        //按照查询的行号修改这个人的基本信息
        {
            bool results = false;
            string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
            SqlConnection connection1 = new SqlConnection(con);
            try
            {
                connection1.Open();
                string sqlstring1 = "UPDATE 人员基础信息 SET 公民身份号码='"+textBox3.Text.Trim()+"',姓名='"+ textBox1.Text.Trim()
                    + "',性别='" + comboBox1.Text + "',出生日期='" + textBox4.Text.Trim() + "',社会保障卡卡号='" + textBox5.Text.Trim() 
                    + "',医疗待遇类别='" + comboBox2.Text + "',特殊人群='" + comboBox3.Text + "',类型='" + comboBox4.Text
                    + "'  WHERE 行号='" + guid + "'";
                MessageBox.Show(sqlstring1);
                SqlCommand mycom = new SqlCommand(sqlstring1, connection1);
                int s = mycom.ExecuteNonQuery();
                if(s==0)
                {
                    MessageBox.Show("修改失败");
                }
                results = true;
              
              
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            finally
            {
                connection1.Close();
            }
            return results;
        }

        private bool date_check()         //检查输入的值是否符合规范
        {
            bool results = false;

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
        #endregion

        private void button1_Click(object sender, EventArgs e)   //查询按钮
        {
            bool res = data_query();
            if(res==false)
            {
                MessageBox.Show("查询失败");
            }
        }

        private void button2_Click(object sender, EventArgs e)     //修改个人的用户信息
        {
            bool res = data_change();
            if (res == false)
            {
                MessageBox.Show("修改失败");
            }
        }
    }
}
