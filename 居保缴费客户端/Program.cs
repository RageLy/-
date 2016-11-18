using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 居保缴费客户端
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 f1 = new Form1();
            //main ma = new main();
            //string[] s = new string[2];
            //User_Inf u1 = new User_Inf(s);
            //u1.noisesEvent += ma.Main_State;
            //u1.noise();
            f1.ShowDialog();
            if (f1.DialogResult == DialogResult.OK)
            {
                Application.Run(new main());
            }
            else
            {
                return;
            }
        }
    }
}
