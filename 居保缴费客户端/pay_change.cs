using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 居保缴费客户端
{
    public partial class pay_change : Form
    {
        public pay_change()
        {
            InitializeComponent();
        }

        private void pay_change_Load(object sender, EventArgs e)
        {
            groupBox1.Text = "已缴费人员查询";
            groupBox2.Text = "查询结果";
        }
    }
}
