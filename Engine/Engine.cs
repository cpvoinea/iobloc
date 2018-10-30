using System;
using System.Text;

namespace iobloc
{
    class Engine : IDisposable
    {
        readonly StringBuilder _log = new StringBuilder();

        internal Engine(string settingsFilePath)
        {
            Serializer.LoadSettings(settingsFilePath);
            Serializer.LoadHighscores();
            UIPainter.Initialize();
        }

        internal void ShowMenu()
        {
            var runner = new BoardRunner(new MenuBoard(), _log);
            runner.Run();
        }

        internal void ShowLog()
        {
            var runner = new BoardRunner(new LogBoard(), _log);
            runner.Run();
        }

        public void Dispose()
        {
            Serializer.SaveSettings();
            Serializer.SaveHighscores();
            UIPainter.Exit();
        }
    }
}
