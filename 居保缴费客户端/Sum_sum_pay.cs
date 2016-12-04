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
using System.Threading;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace 居保缴费客户端
{
    public partial class Sum_sum_pay : Form
    {
        #region 初始加载的函数
        DataTable final_dt1;
        public Sum_sum_pay()
        {
            InitializeComponent();
        }

        private void Sum_sum_pay_Load(object sender, EventArgs e)
        {
            loads(1);
        }

        private void loads(int i)
        {
            if (i == 1)
            {
                groupBox1.Text = "查询条件";
                groupBox2.Text = "查询结果";
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.AutoGenerateColumns = false;

                label2.Text = "起始事件";
                label3.Text = "结束时间";

                button1.Text = "查询";
                button2.Text = "导出";
                button3.Text = "取消";

                label1.Text = "提示：开始缴费的时间为11月22号，如果要计算截止到目前的缴费人数，截止时间要在今天的时间上加1天";

                bool res = query_date();
                if(res==true)
                {
                    button2.Enabled = true;
                }
                else
                {
                    MessageBox.Show("报表查询失败");
                    button2.Enabled = false;
                }
            }
        }

        #endregion

        #region 按钮事件
        private void button1_Click(object sender, EventArgs e)   //查询按钮
        {
            bool res = datecheck();
            if (res == true)
            {
                res = cal_date();
                if (res == true)
                {
                    res = query_date();
                    if (res == true)
                    {

                    }
                    else
                    {
                        MessageBox.Show("报表查询失败");
                    }
                }
                else
                {
                    MessageBox.Show("报表生成失败");
                }
            }
            else
            {
                MessageBox.Show("输入错误");
            }
        }

        private void button2_Click(object sender, EventArgs e)  //导出按钮
        {
            SaveFileDialog sfd = new SaveFileDialog();
            string name = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
            sfd.FileName = name + "-统计数据";
            sfd.Filter = "(*.xls)|*.xls"; //删选、设定文件显示类型
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;

                try
                {
                    int a = 10;
                    TableToExcelForXLS(final_dt1, path, ref a);
                    if (a == -1)
                    {
                        MessageBox.Show("导出成功");
                    }
                    else
                    {
                        MessageBox.Show("导出失败");
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

        private void button3_Click(object sender, EventArgs e)   //取消按钮
        {
            this.Close();
        }
        #endregion

        #region 功能函数
        private bool cal_date()   //按照选择的时间生成统计报表
        {
            bool results = false;
            string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
            SqlConnection connection1 = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand("SUM_SUM_LOAD", connection1);
            try
            {
                connection1.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RES", SqlDbType.Int, 8);
                cmd.Parameters.Add("@DATE1", SqlDbType.DateTime2, 33).Value = DateTime.Parse(textBox1.Text.Trim().ToString());
                cmd.Parameters.Add("@DATE2", SqlDbType.DateTime2, 33).Value = DateTime.Parse(textBox2.Text.Trim().ToString());


                cmd.Parameters["@RES"].Direction = ParameterDirection.Output;


                int s = cmd.ExecuteNonQuery();
                string[] res = new string[5];
                res[0] = cmd.Parameters["@RES"].Value.ToString();
                if (res[0] == "1")
                {
                    results = true;
                }
                else
                {
                    results = false;
                }
                connection1.Close();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            return results;
        }

        private bool query_date()
        {
            bool results = false;
            string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
            SqlConnection connection1 = new SqlConnection(con);
            try
            {
                connection1.Open();
                string sqlstring1 = "SELECT * FROM sum_inf";
                // MessageBox.Show(sqlstring1);
                SqlCommand mycom = new SqlCommand(sqlstring1, connection1);
                SqlDataAdapter myadp = new SqlDataAdapter();
                myadp.SelectCommand = mycom;
                DataSet mydt = new DataSet();
                myadp.Fill(mydt, "table1");
                mydt.Tables[0].Columns[0].ColumnName = "单位编号";
                mydt.Tables[0].Columns[1].ColumnName = "单位名称";
                mydt.Tables[0].Columns[4].ColumnName = "去年缴费人数";
                mydt.Tables[0].Columns[5].ColumnName = "今年已缴费人数";
                mydt.Tables[0].Columns[6].ColumnName = "今年续保人数";
                mydt.Tables[0].Columns[7].ColumnName = "今年新增人数";
                mydt.Tables[0].Columns[8].ColumnName = "今年新增人数_未缴费";
                mydt.Tables[0].Columns[2].ColumnName = "开始时间";
                mydt.Tables[0].Columns[3].ColumnName = "结束时间";
                final_dt1 = mydt.Tables[0];
                int[] r = new int[6];
                r[5] = 0;
                DateTime[] DTS = new DateTime[2];
                foreach (DataRow row2 in final_dt1.Rows)
                {
                    r[0] = r[0] + int.Parse(row2[4].ToString());
                    r[1] = r[1] + int.Parse(row2[5].ToString());
                    r[2] = r[2] + int.Parse(row2[6].ToString());
                    r[3] = r[3] + int.Parse(row2[7].ToString());
                    r[4] = r[4] + int.Parse(row2[8].ToString());
                    if (r[5] == 0)
                    {
                        DTS[0] = DateTime.Parse(row2[2].ToString());
                        DTS[1] = DateTime.Parse(row2[3].ToString());
                    }
                    r[5]++;
                }
                DataRow dr1 = final_dt1.NewRow();
                dr1["单位名称"] = "秭归县";
                dr1["去年缴费人数"] = r[0];
                dr1["今年已缴费人数"] = r[1];
                dr1["今年续保人数"] = r[2];
                dr1["今年新增人数"] = r[3];
                dr1["今年新增人数_未缴费"] = r[4];
                dr1["开始时间"] = DTS[0];
                dr1["结束时间"] = DTS[1];
                final_dt1.Rows.Add(dr1);

                textBox1.Text = DTS[0].ToShortDateString();
                textBox2.Text = DTS[1].ToShortDateString();

                dataGridView1.DataSource = final_dt1;
                button2.Enabled = true;
                connection1.Close();
                results = true;
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
            return results;
        }

        private bool datecheck()
        {
            bool res = false;
            try
            {
                DateTime d1 = DateTime.Parse(textBox1.Text.ToString());
                DateTime d2 = DateTime.Parse(textBox2.Text.ToString());
                int i = DateTime.Compare(d1, d2);
                if(i>0)
                {
                    res = false;
                }
                else
                {
                    res = true;
                }
            }
            catch
            {

            }
            return res;
        }

        public void TableToExcelForXLS(System.Data.DataTable dt, string file, ref int H_max)  //导出数据
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet = hssfworkbook.CreateSheet("Test");


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


            H_max = -1;
        }
        #endregion
    }
}
