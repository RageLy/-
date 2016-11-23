using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Threading;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;


namespace 居保缴费客户端
{
    public partial class pay_rec : Form
    {
        #region  界面开启时的事件
        public pay_rec()
        {
            InitializeComponent();
        }
        private string[] user;
        System.Data.DataTable final_dt = new System.Data.DataTable();
        private void pay_rec_Load(object sender, EventArgs e)
        {
            groupBox1.Text = "查询条件";
            groupBox2.Text = "查询结果";

            label1.Text = "日期区间";
            label2.Text = "";

            progressBar1.Visible = false;

            comboBox1.Items.Add("今天");
            comboBox1.Items.Add("近一周");
            comboBox1.Items.Add("近一月");
            comboBox1.Items.Add("所有");
            comboBox1.SelectedIndex = 0;
            //      string s = create_sql();

            button1.Text = "取消";
            button2.Text = "导出";
            button3.Text = "查询";
            button2.Enabled = false;

            ConfigurationManager.RefreshSection("appSetting");
            user = ConfigurationManager.AppSettings["user"].Split(',');
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoGenerateColumns = false;
        }
        #endregion

        #region 功能函数
        private void inf_query()     //查询信息
        {

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
                mydt.Tables[0].Columns[5].ColumnName = "备注";
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
        }

        private string create_sql()    //根据选择的日期生成sql语句
        {
            DateTime dt1 = DateTime.Now;
            string dt_1 = date_mange.Get_LongDate(dt1);       //防止电脑的日期格式有问题
            DateTime dt2 = new DateTime();
            string sqlstring1 = "SELECT P.姓名,P.公民身份号码,P.是否缴费,Q.money,Q.date,Q.explain,Q.行号 FROM 人员基础信息 P INNER JOIN pay_rec Q ON P.行号=Q.行号 WHERE Q.operater='" + user[0] + "' AND Q.date ";
            if (comboBox1.SelectedIndex == 0)
            {
                dt2 = DateTime.Parse(date_mange.Get_ShortDate(dt1) + " 00:00:00");
                string s = date_mange.Get_ShortDate(dt1) + " 00:00:00";
                sqlstring1 = sqlstring1 + "BETWEEN '" + s + "' AND '" + dt_1 + "'";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                string s = calcul_date(7) + " 00:00:00";
                sqlstring1 = sqlstring1 + "BETWEEN '" + s + "' AND '" + dt_1 + "'";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                string s = calcul_date(30) + " 00:00:00";
                sqlstring1 = sqlstring1 + "BETWEEN '" + s + "' AND '" + dt_1 + "'";
            }
            else
            {
                string s = "1900/1/1" + " 00:00:00";
                sqlstring1 = sqlstring1 + "BETWEEN '" + s + "' AND '" + dt_1 + "'";
            }
            sqlstring1 = sqlstring1 + " ORDER BY  Q.date";
            return sqlstring1;
        }

        private string calcul_date(int s)    //计算间隔特定时间的日期
        {
            int years, month, day;
            years = int.Parse(DateTime.Now.Year.ToString());
            month = int.Parse(DateTime.Now.Month.ToString());
            day = int.Parse(DateTime.Now.Day.ToString());
            if (s <= 30)
            {
                if (day < s)
                {
                    if (years == 1)
                    {
                        years = years - 1;
                        month = 12;
                        day = day + 30;
                    }
                    else
                    {
                        month = month - 1;
                        day = day + 30;
                    }
                    day = day - s;
                }
                else
                {
                    day = day - s;
                }
            }
            else
            {
                int r1 = s % 30;
                int r2 = s / 30;
                if (month < r2)
                {
                    years = years - 1;
                    month = month + 12;
                    month = month - r2;
                }
                else
                {
                    month = month - r2;
                }

                if (day < r1)
                {
                    if (years == 1)
                    {
                        years = years - 1;
                        month = 12;
                        day = day + 30;
                    }
                    else
                    {
                        month = month - 1;
                        day = day + 30;
                    }
                    day = day - s;
                }
                else
                {
                    day = day - r1;
                }

            }
            return years + "/" + month + "/" + day;
        }

        private string[] value1 = new string[5];

        public void threads()
        {
            try
            {
                int a = 5;
                TableToExcelForXLS(final_dt, value1[0], ref a);
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

        //public void DataTabletoExcel(System.Data.DataTable tmpDataTable, string strFileName)    //将DataTable中的数据导出到excel

        //{
        //    if (tmpDataTable == null)

        //        return;

        //    int rowNum = tmpDataTable.Rows.Count;

        //    int columnNum = tmpDataTable.Columns.Count;
        //    progressBar1.Visible = true;
        //    progressBar1.Minimum = 0;
        //    progressBar1.Maximum = rowNum;
        //    progressBar1.Value = 0;


        //    int rowIndex = 1;

        //    int columnIndex = 0;

        //    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
        //    Microsoft.Office.Interop.Excel._Workbook xBk = null;
        //    Microsoft.Office.Interop.Excel._Worksheet xSt = null;
        //    Microsoft.Office.Interop.Excel.Range rng = null;
        //    xBk = xlApp.Workbooks.Add();
        //    xSt = (Microsoft.Office.Interop.Excel._Worksheet)xBk.Worksheets[1];


        //    xlApp.DefaultFilePath = "";

        //    xlApp.DisplayAlerts = true;

        //    xlApp.SheetsInNewWorkbook = 1;



        //    //将DataTable的列名导入Excel表第一行

        //    foreach (DataColumn dc in tmpDataTable.Columns)

        //    {

        //        columnIndex++;
        //        if (columnIndex == 6)          //只导出前五列的数据
        //            break;

        //        xSt.Cells[rowIndex, columnIndex] = dc.ColumnName;

        //    }

        //    //将DataTable中的数据导入Excel中


        //    rng = xSt.get_Range("B1", "F" + rowNum + 1);
        //    xSt.Cells.Select();

        //    //rng.MergeCells = true;
        //    rng.NumberFormatLocal = "@";
        //    // rng.Font.Color = 121212;

        //    //EntireColumn.NumberFormat = "@";


        //    for (int i = 0; i < rowNum; i++)

        //    {

        //        rowIndex++;

        //        columnIndex = 0;

        //        for (int j = 0; j < 5; j++)

        //        {

        //            columnIndex++;



        //            xSt.Cells[rowIndex, columnIndex] = tmpDataTable.Rows[i][j].ToString();
        //            //rng = xSt.get_Range(xSt.Cells[i, j], xSt.Cells[i, j]);
        //            //rng.EntireColumn.ColumnWidth = 20;
        //            ////    rg.columns.autofit();
        //            //rng.NumberFormatLocal = "@";
        //            SetLab(i);
        //            SetPro(i);

        //        }

        //    }






        //    //xlBook.SaveCopyAs(HttpUtility.UrlDecode(strFileName, System.Text.Encoding.UTF8));

        //    xBk.SaveCopyAs(strFileName);
        //    xBk.Close(false, null, null);
        //    xlApp.Quit();

        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(xBk);
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(xSt);
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);

        //    xBk = null;
        //    xSt = null;
        //    xlApp = null;
        //    GC.Collect();
        //    progressBar1.Visible = false;
        //    progressBar1.Value = 0;
        //    label2.Text = "";

        //}

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
            label2.Text = "";
            H_max = -1;
        }

        private delegate void SetLabHandle(int i);
        private void SetLab(int i)
        {
            if (label2.InvokeRequired)
            {
                Invoke(new SetLabHandle(SetLab), new object[] { i });
            }
            else
            {
                label2.Text = "正在导出第" + i + "条数据";
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

        #endregion

        #region 控件事件
        private void button3_Click(object sender, EventArgs e)    //查询按钮
        {
            inf_query();
        }

        Thread thread = new Thread(new ThreadStart(tests));
        private void button2_Click(object sender, EventArgs e)    //导出按钮
        {
            SaveFileDialog sfd = new SaveFileDialog();
            string name = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
            sfd.FileName =name +"-缴费记录";
            sfd.Filter = "(*.xls)|*.xls"; //删选、设定文件显示类型
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;

                try
                {
                    if (path != "")
                    {
                        value1[0] = path;
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

        private void button1_Click(object sender, EventArgs e)   //取消按钮
        {
            this.Close();
        }
        #endregion

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = dataGridView1.CurrentRow.Index;
            try
            {
                string NUM = final_dt.Rows[index][6].ToString();
                pay_back f1 = new pay_back(NUM);
                f1.ShowDialog();
                if (f1.DialogResult == DialogResult.OK)
                {
                    int s = f1.Marks;
                    if(s==0)
                    {
                        MessageBox.Show("取消回退");
                    }
                    else
                    {
                        MessageBox.Show("回退成功");
                        inf_query();
                    }
                }
            }
            catch
            {
                MessageBox.Show("无效选项");
            }
        }
    }
}
