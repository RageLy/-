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
using Microsoft.Office.Interop.Excel;

namespace 居保缴费客户端
{
    public partial class sum_inf : Form
    {
        #region 界面开启时的事件
        public sum_inf()
        {
            InitializeComponent();
        }
        System.Data.DataTable final_dt;
        private void sum_inf_Load(object sender, EventArgs e)
        {
            label1.Text = "地区选择";
            label2.Text = "是否缴费";
            label3.Text = "人员类型";

            checkBox1.Text = "包含重残-低保人员";

            button1.Text = "查询";
            button2.Text = "导出";
            button3.Text = "取消";
            button4.Text = "显示项目";

            groupBox1.Text = "查询条件";
            groupBox2.Text = "查询结果";

            comboBox2.Items.Add("全部");
            comboBox2.Items.Add("已缴费");
            comboBox2.Items.Add("未缴费");
            comboBox3.Items.Add("全部");
            comboBox3.Items.Add("正常续保");
            comboBox3.Items.Add("新参保");
            comboBox3.Items.Add("中断库人员");
     
            fill_region();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
        }
        #endregion

        #region 功能函数
        private void fill_region()   //填充地区信息
        {

            string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
            SqlConnection connection1 = new SqlConnection(con);
            try
            {
                connection1.Open();
                string sqlstring1 = "SELECT DISTINCT(name),explain,regin_code FROM user_inf";
                // MessageBox.Show(sqlstring1);
                SqlCommand mycom = new SqlCommand(sqlstring1, connection1);
                SqlDataAdapter myadp = new SqlDataAdapter();
                myadp.SelectCommand = mycom;
                DataSet mydt = new DataSet();
                myadp.Fill(mydt, "table1");
                final_dt = mydt.Tables[0];
                comboBox1.Items.Add("全部");
                foreach (DataRow row2 in final_dt.Rows)
                {
                    comboBox1.Items.Add(row2[1].ToString()) ;
                }
                button2.Enabled = true;
                connection1.Close();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }

        private bool inf_query()     //信息查询
        {
            bool results = false;
            string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
            SqlConnection connection1 = new SqlConnection(con);
            //SqlCommand cmd = new SqlCommand("ID_QUERY", connection1);
            try
            {
                connection1.Open();
                string sqlstring1 = create_sql();
                // MessageBox.Show(sqlstring1);
                SqlCommand mycom = new SqlCommand(sqlstring1, connection1);
                SqlDataAdapter myadp = new SqlDataAdapter();
                myadp.SelectCommand = mycom;
                DataSet mydt = new DataSet();
                myadp.Fill(mydt, "table1");
                mydt.Tables[0].Columns[0].ColumnName = "姓名";
                mydt.Tables[0].Columns[1].ColumnName = "身份证号码";
                mydt.Tables[0].Columns[2].ColumnName = "是否缴费";
                mydt.Tables[0].Columns[3].ColumnName = "缴费金额";
                mydt.Tables[0].Columns[4].ColumnName = "缴费时间";
                final_dt = mydt.Tables[0];
                int r1 = 0, r2 = 0;
                foreach (DataRow row2 in final_dt.Rows)
                {
                    r1++;
                    r2 = r2 + int.Parse(row2[3].ToString());
                }
                DataRow dr1 = final_dt.NewRow();
                dr1["姓名"] = "合计人数、金额";
                dr1["身份证号码"] = r1 + "人";
                dr1["缴费金额"] = r2;
                final_dt.Rows.Add(dr1);
                dataGridView1.DataSource = final_dt;
                button2.Enabled = true;
                connection1.Close();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            return results;
        }

        private string create_sql()    //生成sql语句
        {
            string sqlstring1 = "SELECT 个人编号,,,,,,,,";

            return sqlstring1;
        }
        #endregion
    }
}
