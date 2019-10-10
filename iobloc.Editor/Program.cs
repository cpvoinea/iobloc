using iobloc.Games;
using iobloc.SDK;
using System;
using System.Windows.Forms;

namespace iobloc.Editor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormRenderer(new Sokoban()));
        }
    }
}
