using System;
using System.Windows.Forms;
using Avalonia;
using Avalonia.Logging.Serilog;

namespace iobloc
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            using (var app = Launcher.Launch(RenderType.Avalonia, GameType.Labirint))
            {
                if (app is Form)
                    System.Windows.Forms.Application.Run(app as Form);
                else
                    BuildAvaloniaApp().Start(AppMain, args);
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<AvaloniaApp>().UsePlatformDetect().LogToDebug();

        // Your application's entry point. Here you can initialize your MVVM framework, DI
        // container, etc.
        private static void AppMain(Avalonia.Application app, string[] args)
        {
            app.Run(new AvaloniaWindow());
        }
    }
}
