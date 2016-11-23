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
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Threading;


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
        System.Data.DataTable final_dt1;
        private string[] user;
        private string[] values = new string[5];
        int[] marks = new int[10];                              //标记某些参数，防止反复执行
        Thread thread = new Thread(new ThreadStart(tests));
        private void sum_inf_Load(object sender, EventArgs e)
        {
            label1.Text = "地区选择";
            label2.Text = "是否缴费";
            label3.Text = "人员类型";

            checkBox1.Text = "包含重残-低保人员";
            dataGridView1.RowHeadersVisible = false;
            progressBar1.Visible = false;
            label4.Text = "";

            button1.Text = "查询";
            button2.Text = "导出";
            button2.Enabled = false;
            button3.Text = "取消";
            button4.Text = "显示项目";

            groupBox1.Text = "查询条件";
            groupBox2.Text = "查询结果";

            comboBox2.Items.Add("全部");
            comboBox2.Items.Add("已缴费");
            comboBox2.Items.Add("未缴费");
            comboBox3.Items.Add("全部");
            comboBox3.Items.Add("正常续保");
            comboBox3.Items.Add("新增人员");
           // comboBox3.Items.Add("中断库人员");

            user = ConfigurationManager.AppSettings["user"].Split(',');
     
            fill_region();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;

            checkedListBox1.Visible = false;
            checkedListBox1.CheckOnClick = true;
            checkBox2.Visible = false;
            checkBox2.Text = "全选";
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
                else if (user[3] == "3")
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

        private bool inf_query(string a)     //信息查询
        {
            bool results = false;
            string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
            SqlConnection connection1 = new SqlConnection(con);
            try
            {
                connection1.Open();
                string sqlstring1 = create_sql(a);
                // MessageBox.Show(sqlstring1);
                SqlCommand mycom = new SqlCommand(sqlstring1, connection1);
                SqlDataAdapter myadp = new SqlDataAdapter();
                myadp.SelectCommand = mycom;
                DataSet mydt = new DataSet();
                myadp.Fill(mydt, "table1");
                //mydt.Tables[0].Columns[0].ColumnName = "姓名";
                //mydt.Tables[0].Columns[1].ColumnName = "身份证号码";
                //mydt.Tables[0].Columns[2].ColumnName = "是否缴费";
                //mydt.Tables[0].Columns[3].ColumnName = "缴费金额";
                //mydt.Tables[0].Columns[4].ColumnName = "缴费时间";
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

        private string create_sql(string a)    //生成sql语句
        {
            string sqlstring1 = "SELECT "+a+" FROM 人员基础信息 ";

            if (comboBox1.Text == "全部")
            {
                sqlstring1 = sqlstring1 + "WHERE 单位编号!='1'";
            }
            else
            {
                foreach (DataRow dr in final_dt.Rows)
                {
                    if (comboBox1.Text == dr[1].ToString().Trim())
                    {
                        sqlstring1 = sqlstring1 + "WHERE 单位编号='" + dr[2].ToString().Substring(0, 7) + "0" + dr[2].ToString().Substring(7, 5) + "'";
                        //   MessageBox.Show(sqlstring1);
                        break;
                    }
                }
            }

            if (checkBox1.Checked == true)
            {

            }
            else
            {
                sqlstring1 = sqlstring1 + "AND 特殊人群='非特殊人群'";
            }

            if (comboBox2.Text=="全部")
            {
                
            }
            else if(comboBox2.Text == "已缴费")
            {
                sqlstring1 = sqlstring1 + "AND 是否缴费='是'";
            }
            else
            {
                sqlstring1 = sqlstring1 + "AND 是否缴费='否'";
            }

            if (comboBox3.Text == "全部")
            {

            }
            else if (comboBox3.Text == "新增人员")
            {
                sqlstring1 = sqlstring1 + "AND 类型='新增人员'";
            }
            else
            {
                sqlstring1 = sqlstring1 + "AND 类型!='新增人员'";
            }
            return sqlstring1;
        }

        private string[] Items()
        {
            string con = ConfigurationManager.ConnectionStrings["connection1"].ConnectionString;
            string[] names = new string[20];
            System.Data.DataTable final_dt3;
            SqlConnection connection1 = new SqlConnection(con);
            try
            {
                connection1.Open();
                string sqlstring1 = "Select Name FROM SysColumns Where id=Object_Id('[人员基础信息]')";
                // MessageBox.Show(sqlstring1);
                SqlCommand mycom = new SqlCommand(sqlstring1, connection1);
                SqlDataAdapter myadp = new SqlDataAdapter();
                myadp.SelectCommand = mycom;
                DataSet mydt = new DataSet();
                myadp.Fill(mydt, "table1");
                final_dt3 = mydt.Tables[0];
                connection1.Close();
                int j = final_dt3.Rows.Count;
                for (int i = 0; i < j; i++)
                {
                    checkedListBox1.Items.Add(final_dt3.Rows[i][0].ToString());
                    names[i] = final_dt3.Rows[i][0].ToString();
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
          
            return names;
        }     //获取表的列名

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
                label4.Text = "正在导出第" + i + "条数据";
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
            label4.Text = "";
            H_max = -1;
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)     //查询按钮
        {
            bool ress = false;
            if (marks[0] == 2)                                                //隐藏勾选框
            {
                checkedListBox1.Visible = false;
                checkBox2.Visible = false;
                System.Drawing.Point p = dataGridView1.Location;
                System.Drawing.Size s = dataGridView1.Size;
                p.X = p.X - 140;
                s.Width = s.Width + 140;
                dataGridView1.Location = p;
                dataGridView1.Size = s;
                marks[0]--;
            }
            else
            {

            }
            int count = checkedListBox1.Items.Count;                      //将勾选框中勾选的值传入到查询字符串中
            int count1 = 0;
            string[] values1 = new string[count + 1];
            for (int i = 0; i < count; i++)
            {
                if (checkedListBox1.GetItemChecked(i) == true)
                {
                    values1[count1] = checkedListBox1.Items[i].ToString();
                    count1++;
                }
            }
            if (count1 == count)
            {
                values1[count] = "*";
                test();
                ress = inf_query(values1[count]);
            }
            else if (count1 == 0)
            {
                test();
                MessageBox.Show("至少选择一项");
            }
            else
            {

                for (int i = 0; i < count1; i++)
                {
                    if (i == (count1 - 1))
                        values1[count] = values1[count] + "[" + values1[i] + "]";
                    else
                        values1[count] = values1[count] + "[" + values1[i] + "],";
                }
                test();
             //   MessageBox.Show(values1[count]);
                ress = inf_query(values1[count]);
            }
            if(ress==true)
            {
                button2.Enabled = true;
            }
            else
            {
                MessageBox.Show("查询失败");
                button2.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)    //导出按钮
        {
            SaveFileDialog sfd = new SaveFileDialog();
            string name = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
            sfd.FileName = name + "-缴费记录";
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

        private void button3_Click(object sender, EventArgs e)    //取消按钮
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (marks[0] == 0)
            {
                string[] names = Items();
                System.Drawing.Point p = dataGridView1.Location;
                System.Drawing.Size s = dataGridView1.Size;
                p.X = p.X + 140;
                s.Width = s.Width - 140;
                dataGridView1.Location = p;
                dataGridView1.Size = s;
                checkedListBox1.Visible = true;
                checkBox2.Visible = true;
                marks[0] = marks[0] + 2;
            }
            else if (marks[0] == 1)
            {
                System.Drawing.Point p = dataGridView1.Location;
                System.Drawing.Size s = dataGridView1.Size;
                p.X = p.X + 140;
                s.Width = s.Width - 140;
                dataGridView1.Location = p;
                dataGridView1.Size = s;
                checkedListBox1.Visible = true;
                checkBox2.Visible = true;
                marks[0]++;
            }
        }  //查询条目选项

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    checkedListBox1.SetItemChecked(i, true);
            }
            else
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    checkedListBox1.SetItemChecked(i, false);
            }
        }
    }
}
