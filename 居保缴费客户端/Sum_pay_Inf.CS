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
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Threading;
using System.IO;

namespace 居保缴费客户端
{
    public partial class Sum_pay_Inf : Form
    {
        #region 程序开启时响应的事件
        public Sum_pay_Inf()
        {
            InitializeComponent();
        }
        DataTable final_dt;
        DataTable final_dt1;
        string[] user;
        Thread thread = new Thread(new ThreadStart(tests));
        private string[] values = new string[5];
        private void Sum_pay_Inf_Load(object sender, EventArgs e)
        {
            chushihua();
        }
        private void chushihua()
        {
            label1.Text = "地区";
            label2.Text = "时间段";
            label3.Text = "金额";
            label4.Text = "—";
            label5.Text = "";
            label6.Text = "身份证号码";

            progressBar1.Visible = false;
            dataGridView1.RowHeadersVisible = false;

            button1.Text = "查询";
            button2.Text = "导出";
            button3.Text = "取消";

            groupBox1.Text = "查询条件";
            groupBox2.Text = "查询结果";

            comboBox2.Items.Add("全部");
            comboBox2.Items.Add("150");
            comboBox2.Items.Add("100");

            string s = DateTime.Now.ToShortDateString();
            textBox1.Text = "2016/11/20";
            textBox2.Text = s;

            user = ConfigurationManager.AppSettings["user"].Split(',');

            fill_region();

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }
        #endregion

        #region 按钮事件
        private void button3_Click(object sender, EventArgs e)   //取消按钮
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)   //查询按钮
        {
            bool res = checks();
            if (res == true)
            {
                res = inf_query();
                if (res == false)
                {
                    MessageBox.Show("查询失败");
                }
            }
            else
            {
                MessageBox.Show("输入不符合规范");
            }
        }

        private void button2_Click(object sender, EventArgs e)    //导出按钮
        {
            SaveFileDialog sfd = new SaveFileDialog();
            string name = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
            sfd.FileName = comboBox1.Text + "-"+ name + "-缴费记录";
            sfd.Filter = "(*.xls)|*.xls"; //删选、设定文件显示类型
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;

                try
                {
                    if (path != "")
                    {
                        values[0] = path;
                        if (thread.IsAlive)
                            thread.Abort();
                        thread = new Thread(new ThreadStart(threads));
                        thread.IsBackground = true;
                        thread.Start();

                    }
                }
                catch (Exception es)
                {
                    MessageBox.Show("导出失败" + es.ToString());
                }
            }
            else
            {

            }
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
                if (user[3] == "2")
                {
                    foreach (DataRow row2 in final_dt.Rows)
                    {
                        if (row2[2].ToString() == user[0])
                            comboBox1.Items.Add(row2[1].ToString());
                    }
                }
                else if ((user[3] == "3"))
                {
                    comboBox1.Items.Add("全部");
                    foreach (DataRow row2 in final_dt.Rows)
                    {
                        comboBox1.Items.Add(row2[1].ToString());
                    }
                }
                button2.Enabled = true;
                connection1.Close();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }

        private string create_sql()    //生成sql语句
        {
            string sqlstring1 = "SELECT P.单位名称,P.姓名,P.公民身份号码,Q.DATE,Q.MONEY,Q.operater,Q.EXPLAIN  FROM 人员基础信息 P INNER JOIN PAY_REC Q ON P.行号=Q.行号";

            if (comboBox1.Text == "全部")
            {
                sqlstring1 = sqlstring1 + " WHERE Q.operater!='1'";
            }
            else
            {
                foreach (DataRow dr in final_dt.Rows)
                {
                    if (comboBox1.Text == dr[1].ToString().Trim())
                    {
                        sqlstring1 = sqlstring1 + " WHERE Q.operater='" + dr[2].ToString().Trim()+"'";
                        //   MessageBox.Show(sqlstring1);
                        break;
                    }
                }
            }
            sqlstring1 = sqlstring1 + "AND Q.date>'"+textBox1.Text.Trim()+ "' AND  Q.date<'" + textBox2.Text.Trim() + "'";


            if (comboBox2.Text == "全部")
            {

            }
            else if (comboBox2.Text == "100")
            {
                sqlstring1 = sqlstring1 + "AND Q.money='100'";
            }
            else
            {
                sqlstring1 = sqlstring1 + "AND Q.money='150'";
            }

            if(textBox3.Text.Length!=18 && textBox3.Text.Length != 15)
            {

            }
            else
            {
                sqlstring1 = sqlstring1 + "AND P.公民身份号码='"+textBox3.Text.Trim()+"'";
            }
            return sqlstring1;
        }

        private bool inf_query()     //信息查询
        {
            bool results = false;
            string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
            SqlConnection connection1 = new SqlConnection(con);
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
                mydt.Tables[0].Columns[0].ColumnName = "地区";
                mydt.Tables[0].Columns[1].ColumnName = "姓名";
                mydt.Tables[0].Columns[2].ColumnName = "身份证号码";
                mydt.Tables[0].Columns[3].ColumnName = "缴费时间";
                mydt.Tables[0].Columns[4].ColumnName = "缴费金额";
                mydt.Tables[0].Columns[5].ColumnName = "缴费地区";
                mydt.Tables[0].Columns[6].ColumnName = "备注";
                final_dt1 = mydt.Tables[0];
                dataGridView1.DataSource = final_dt1;
                connection1.Close();
                results = true;
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            return results;
        }

        private bool checks()       //检测输入的数据
        {
            bool res = false;
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                res = false;
            }
            else
            {
                try
                {
                    DateTime dt1 = DateTime.Parse(textBox1.Text.Trim());
                    DateTime dt2 = DateTime.Parse(textBox2.Text.Trim());
                    int i = DateTime.Compare(dt1, dt2);
                    if (i >= 0)
                    {

                    }
                    else
                    {
                        res = true;
                    }
                }
                catch
                {
                    res = false;
                }
            }
            return res;
        }

        public void test()
        {
            //final_dt1.Clear();
            //dataGridView1.Columns.Clear();
            //dataGridView1.DataSource = final_dt1;
            //dataGridView1.Refresh();
        }            //清空数据

        public void threads()
        {
            try
            {
                int a = 15;
                TableToExcelForXLS(final_dt1, values[0], ref a);
                if (a == -1)
                {
                    MessageBox.Show("导出成功");
                }
                else
                {
                    MessageBox.Show("导出失败");
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }

        public static void tests()
        {

        }

        private delegate void SetLabHandle(int i);
        private void SetLab(int i)
        {
            if (label4.InvokeRequired)
            {
                Invoke(new SetLabHandle(SetLab), new object[] { i });
            }
            else
            {
                label5.Text = "正在导出第" + i + "条数据";
            }
        }

        private delegate void SetProHandle(int i);
        private void SetPro(int i)
        {
            if (progressBar1.InvokeRequired)
            {
                Invoke(new SetLabHandle(SetPro), new object[] { i });
            }
            else
            {
                progressBar1.Value = i;
            }
        }

        public void TableToExcelForXLS(System.Data.DataTable dt, string file, ref int H_max)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet = hssfworkbook.CreateSheet("Test");
            progressBar1.Minimum = 0;
            progressBar1.Maximum = dt.Rows.Count;
            progressBar1.Visible = true;

            //表头
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (i > H_max)
                    break;
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(dt.Columns[i].ColumnName);
            }

            //数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j > H_max)
                        break;
                    SetLab(i);
                    SetPro(i);
                    ICell cell = row1.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                }
            }

            //转为字节数组
            MemoryStream stream = new MemoryStream();
            hssfworkbook.Write(stream);
            var buf = stream.ToArray();

            //保存为Excel文件
            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
            }
            progressBar1.Visible = false;
            progressBar1.Value = 0;
            label5.Text = "";
            H_max = -1;
        }   //导出数据到excel
        #endregion
    }
}
