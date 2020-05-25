namespace iobloc
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        //[System.STAThread]
        static void Main()
        {
            new ConsoleRenderer().Run(new Walker());

            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(Launcher.Launch(RenderType.Console));
        }
    }
}
