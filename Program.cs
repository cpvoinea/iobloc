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

            using (Form app = new PaintRenderer())
            {
                if (app != null)
                    Application.Run(app);
            }
        }
    }
}
