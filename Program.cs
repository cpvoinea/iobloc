using System;
using System.Windows.Forms;

namespace iobloc
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Engine());
        }
    }
}
