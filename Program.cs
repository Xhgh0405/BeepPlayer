using System;
using System.Windows.Forms;

namespace HW4_BeepPlayer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmBeepPlayer());
        }
    }
}
