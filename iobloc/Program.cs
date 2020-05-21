using System;
using System.Windows.Forms;

namespace iobloc.UI.Forms
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Launcher.Launch(RenderType.NativeConsole, GameType.Table);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(Launcher.Launch(RenderType.PanelForm, GameType.Table));
        }
    }
}
