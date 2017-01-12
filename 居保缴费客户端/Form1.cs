using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using 验证码;

namespace 居保缴费客户端
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private string[] user;
        private int temps = 0;           //记录输错的次数
        private string codes = "";      //验证字符串
        private Bitmap maps;            //验证码
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
            label4.Text = "验证码";
            label4.Visible = false;
            textBox4.Enabled = false;
            textBox4.Visible = false;
            checkBox1.Text = "记住密码";
            checkBox1.Checked = true;
            user = ConfigurationManager.AppSettings["user"].Split(',');
            textBox1.Text = user[0];
            textBox2.Text = user[1];
            textBox3.Text = user[2];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 8|| textBox1.Text.Length>13)
            {
                MessageBox.Show("地区编码错误");
                textBox1.Text = "";
                temps++;
            }
            else if (textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("用户名或密码为空");
                temps++;
            }
            else
            {
                if (temps >= 3)
                {
                    if (textBox4.Text == codes)
                    {
                        bool res = login();
                        if (res == true)
                        {
                            if (checkBox1.Checked == true)
                            {
                                string temp = "";
                                temp = temp + textBox1.Text + ",";
                                temp = temp + textBox2.Text + ",";
                                temp = temp + textBox3.Text + ",";
                                temp = temp + user[3];
                                SetValue("user", temp);
                                //  MessageBox.Show("保存成功");
                            }
                            else
                            {
                                string temp = "";
                                temp = temp + textBox1.Text + ",";
                                temp = temp + textBox2.Text + ",";
                                temp = temp + "," + user[3];
                                SetValue("user", temp);
                            }
                            this.DialogResult = DialogResult.OK;
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        MessageBox.Show("验证码输入错误");
                    }
                }
                else

                {
                    bool res = login();
                    if (res == true)
                    {
                        if (checkBox1.Checked == true)
                        {
                            string temp = "";
                            temp = temp + textBox1.Text + ",";
                            temp = temp + textBox2.Text + ",";
                            temp = temp + textBox3.Text + ",";
                            temp = temp + user[3];
                            SetValue("user", temp);
                            //  MessageBox.Show("保存成功");
                        }
                        else
                        {
                            string temp = "";
                            temp = temp + textBox1.Text + ",";
                            temp = temp + textBox2.Text + ",";
                            temp = temp + "," + user[3];
                            SetValue("user", temp);
                        }
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        temps++;
                    }
                }
            }

            if(temps==3)
            {
                Class1 c1 = new Class1();
                bool res = c1.UpdateVerifyCode(ref codes, ref maps);
                if(res==true)
                {
                    pictureBox1.Image = maps;
                    textBox4.Visible = true;
                    textBox4.Enabled = true;
                    label4.Visible = true;
                }
                else
                {
                    MessageBox.Show("出现错误！程序即将关闭");
                }
            }

        }

        #region
        public static void SetValue(string AppKey, string AppValue)    //修改appconfig的值
        {
            System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
            xDoc.Load(System.Windows.Forms.Application.ExecutablePath + ".config");

            System.Xml.XmlNode xNode;
            System.Xml.XmlElement xElem1;
            System.Xml.XmlElement xElem2;
            xNode = xDoc.SelectSingleNode("//appSettings");

            xElem1 = (System.Xml.XmlElement)xNode.SelectSingleNode("//add[@key='" + AppKey + "']");
            if (xElem1 != null) xElem1.SetAttribute("value", AppValue);
            else
            {
                xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("key", AppKey);
                xElem2.SetAttribute("value", AppValue);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(System.Windows.Forms.Application.ExecutablePath + ".config");
            ConfigurationManager.RefreshSection("appSettings");
        }

        private bool login()        //登陆函数
        {
            bool results = false;
            string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
            SqlConnection connection1 = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("LOGIN_USER", connection1);
            try
            {
                connection1.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@NAME", SqlDbType.NVarChar, 12).Value = textBox2.Text.Trim();
                cmd.Parameters.Add("@PASSWORD", SqlDbType.NVarChar, 12).Value = textBox3.Text.Trim(); ;
                cmd.Parameters.Add("@REGIN_CODE", SqlDbType.VarChar, 22).Value = textBox1.Text.Trim(); ;
                cmd.Parameters.Add("@RES", SqlDbType.Int, 4);

                cmd.Parameters["@RES"].Direction = ParameterDirection.Output;

                int s = cmd.ExecuteNonQuery();
                string res = cmd.Parameters["@RES"].Value.ToString();
                if (res == "0")
                {
                    MessageBox.Show("登陆失败");
                }
                else if (res == "-1")
                {
                    MessageBox.Show("该用户被禁用");
                }
                else
                {
                    //MessageBox.Show("登陆成功");
                    user[3] = res;
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
    }
}
