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

            using (var app = Launcher.Form(GameType.Paint2, RenderType.ImageForm))
            {
                if (app != null)
                    Application.Run(app);
            }
        }
    }
}
