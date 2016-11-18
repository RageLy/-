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
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        #region 功能函数
        public bool ShowChildrenForm(string p_ChildrenFormText)    //防止打开多个窗口
        {
            int i;
            //依次检测当前窗体的子窗体
            for (i = 0; i < this.MdiChildren.Length; i++)
            {
                //判断当前子窗体的Text属性值是否与传入的字符串值相同
                if (this.MdiChildren[i].Name == p_ChildrenFormText)
                {
                    //如果值相同则表示此子窗体为想要调用的子窗体，激活此子窗体并返回true值
                    this.MdiChildren[i].WindowState = FormWindowState.Maximized;
                    return true;
                }
            }
            //如果没有相同的值则表示要调用的子窗体还没有被打开，返回false值
            return false;
        }


        #endregion

        private void 缴费信息录入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ShowChildrenForm("pay_add"))
            {
                pay_add f2 = new pay_add();
                f2.MdiParent = this;
                f2.WindowState = FormWindowState.Maximized;
                f2.Show();
            }
            else
            {

            }
        }
        

        private void 缴费信息更改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ShowChildrenForm("pay_add"))
            {
                pay_change f2 = new pay_change();
                f2.MdiParent = this;
                f2.WindowState = FormWindowState.Maximized;
                f2.Show();
            }
            else
            {

            }
        }

        private void main_Load(object sender, EventArgs e)
        {
            this.IsMdiContainer = true;
        }
    }
}
