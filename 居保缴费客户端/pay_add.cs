using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace 居保缴费客户端
{
    public partial class pay_add : Form
    {
        public pay_add()
        {
            InitializeComponent();
        }

        private void pay_add_Load(object sender, EventArgs e)
        {
            groupBox1.Text = "信息查询";
            groupBox2.Text = "结果展示";

        }
    }
}
