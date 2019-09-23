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

            using (Form app = Launcher.Launch(RenderType.ImageForm, GameType.Labirint))
            {
                if (app != null)
                    Application.Run(app);
            }
        }
    }
}
