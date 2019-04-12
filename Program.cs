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

            using (var app = Launcher.Launch(RenderType.ImageForm))
            {
                if (app != null)
                    Application.Run(app);
            }
        }
    }
}
