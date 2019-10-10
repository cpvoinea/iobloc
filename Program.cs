using System;
using System.Windows.Forms;

namespace iobloc
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var app = Launcher.Launch(RenderType.PanelForm, GameType.Labirint))
            {
                if (app is Form)
                    Application.Run(app as Form);
            }
        }
    }
}
