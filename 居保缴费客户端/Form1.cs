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

namespace 居保缴费客户端
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private string[] user;
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
            checkBox1.Text = "记住密码";
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
            if (textBox1.Text.Length != 8)
            {
                MessageBox.Show("地区编码错误");
                textBox1.Text = "";
            }
            else if (textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("用户名或密码为空");
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
                        temp = temp + textBox3.Text;
                        SetValue("user", temp);
                        //  MessageBox.Show("保存成功");
                    }
                    else
                    {
                        string temp = "";
                        temp = temp + textBox1.Text + ",";
                        temp = temp + textBox2.Text + ",";
                        SetValue("user", temp);
                    }
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    
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
            ConfigurationManager.RefreshSection("appSetting");
        }

        private bool login()        //登陆函数
        {
            bool results = false;
            string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
            int i = 0;
            SqlConnection connection1 = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("LOGIN_USER", connection1);
            try
            {
                connection1.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@NAME", SqlDbType.NVarChar, 12).Value = textBox2.Text.Trim();
                cmd.Parameters.Add("@PASSWORD", SqlDbType.NVarChar, 12).Value = textBox3.Text.Trim(); ;
                cmd.Parameters.Add("@REGIN_CODE", SqlDbType.NVarChar, 12).Value = textBox1.Text.Trim(); ;
                cmd.Parameters.Add("@RES", SqlDbType.Int, 4);

                cmd.Parameters["@RES"].Direction = ParameterDirection.Output;

                int s = cmd.ExecuteNonQuery();
                string res = cmd.Parameters["@RES"].Value.ToString();
                if (res == "0")
                {
                    MessageBox.Show("登陆失败");
                }
                else if (res == "1")
                {
                    //MessageBox.Show("登陆成功");
                    results = true;
                }
                else
                {
                    MessageBox.Show("该用户被禁用");
                }
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
