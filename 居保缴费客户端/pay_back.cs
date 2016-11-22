using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace 居保缴费客户端
{
    public partial class pay_back : Form
    {
        #region 界面开始的事件
        public pay_back(string ids)
        {
            InitializeComponent();
            id = ids;
        
        }
        private string id;
        private int marks;
        public int Marks
        {
            get { return this.marks; }
        }
        private void pay_back_Load(object sender, EventArgs e)
        {
            groupBox2.Text = "缴费信息";
            
            
            label1.Text = "姓名";
            label3.Text = "性别";
            label4.Text = "出生日期";
            label5.Text = "身份证号";
            label6.Text = "医疗待遇类别";
            label7.Text = "类型";
            label8.Text = "缴费时间";
            label9.Text = "缴费金额";

            textBox1.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            textBox9.Enabled = false;

            button3.Text = "确认回退";
            button2.Text = "取消";

            bool RES = inf_query();
        }
        #endregion

        #region 按钮事件
        private void button2_Click(object sender, EventArgs e)   //取消按钮
        {
            this.Close();
            marks = 0;
            this.DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)    //确认回退按钮
        {
            bool res = pay_backs();
            
        }
        #endregion

        #region 功能函数
        private bool inf_query()
        {
            bool results = false;
            Guid guid = new Guid(id);
            string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
            SqlConnection connection1 = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("BACK_INF", connection1);
            try
            {
                connection1.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@HANGHAO", SqlDbType.UniqueIdentifier, 14).Value = guid;
                cmd.Parameters.Add("@DATES", SqlDbType.DateTime, 4);
                cmd.Parameters.Add("@BIRTHDAY", SqlDbType.Date, 31);
                cmd.Parameters.Add("@RES", SqlDbType.Int, 4);
                cmd.Parameters.Add("@NAME", SqlDbType.NVarChar, 12);
                cmd.Parameters.Add("@SEX", SqlDbType.NVarChar, 12);
                cmd.Parameters.Add("@YILIAODAIYU", SqlDbType.NVarChar, 12);
                cmd.Parameters.Add("@LEIXING", SqlDbType.NVarChar, 12);
                cmd.Parameters.Add("@MONEY", SqlDbType.Int, 4);
                cmd.Parameters.Add("@ID", SqlDbType.VarChar, 22);

                cmd.Parameters["@RES"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ID"].Direction = ParameterDirection.Output;
                cmd.Parameters["@BIRTHDAY"].Direction = ParameterDirection.Output;
                cmd.Parameters["@NAME"].Direction = ParameterDirection.Output;
                cmd.Parameters["@SEX"].Direction = ParameterDirection.Output;
                cmd.Parameters["@YILIAODAIYU"].Direction = ParameterDirection.Output;
                cmd.Parameters["@LEIXING"].Direction = ParameterDirection.Output;
                cmd.Parameters["@MONEY"].Direction = ParameterDirection.Output;
                cmd.Parameters["@DATES"].Direction = ParameterDirection.Output;

                int s = cmd.ExecuteNonQuery();
                string[] res = new string[5];
                res[0] = cmd.Parameters["@RES"].Value.ToString();
                if (res[0] == "0")
                {
                    MessageBox.Show("信息加载失败");
                    button3.Enabled = false;
                }
                else
                {
                    textBox1.Text = cmd.Parameters["@NAME"].Value.ToString();
                    textBox3.Text = cmd.Parameters["@SEX"].Value.ToString();
                    textBox4.Text = DateTime.Parse(cmd.Parameters["@BIRTHDAY"].Value.ToString()).ToShortDateString();
                    textBox5.Text = cmd.Parameters["@ID"].Value.ToString();
                    textBox6.Text = cmd.Parameters["@YILIAODAIYU"].Value.ToString();
                    textBox7.Text = cmd.Parameters["@LEIXING"].Value.ToString();
                    textBox8.Text = cmd.Parameters["@DATES"].Value.ToString();
                    textBox9.Text = cmd.Parameters["@MONEY"].Value.ToString();

                }
                connection1.Close();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            return results;
        }

        private bool pay_backs()
        {

            bool results = false;
            Guid guid = new Guid(id);
            string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
            SqlConnection connection1 = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("PAY_MONEY_BACK", connection1);
            try
            {
                connection1.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@HANGHAO", SqlDbType.UniqueIdentifier, 14).Value = guid;
                cmd.Parameters.Add("@RES", SqlDbType.Int, 4);
  

                cmd.Parameters["@RES"].Direction = ParameterDirection.Output;


                int s = cmd.ExecuteNonQuery();
                string[] res = new string[5];
                res[0] = cmd.Parameters["@RES"].Value.ToString();
                if (res[0] == "1")
                {
                    MessageBox.Show("该人缴费记录已经回退！");
                    marks = 1;
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("缴费回退失败！错误原因： " + res[0]);
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
