using System;
using System.Text;

namespace iobloc
{
    class Engine : IDisposable
    {
        readonly StringBuilder _log = new StringBuilder();
        readonly Menu _menu;
        Game _currentGame;

        internal Engine(string settingsFilePath)
        {
            Config.Load(settingsFilePath);
            _menu = new Menu(Config.MenuItems);
            UI.Open();
        }

        internal void ShowMenu()
        {
            UI.Clear();
            _menu.Show();
            var option = _menu.WaitOption();
            if (!option.HasValue)
                return;
            Open(option.Value);
        }

        internal void ShowLog()
        {
            UI.Clear();
            UI.Text(_log.ToString());
            UI.InputWait();
        }

        void Open(Option option)
        {
            if (option == Option.Log)
                ShowLog();
            else
                try
                {
                    _log.AppendLine($"Start {option}");
                    Config.Highscore = Config.GetHighscore(option);
                    UI.Clear();

                    _currentGame = new Game(option);
                    _currentGame.Start();

                    Config.UpdateHighscore(option, _currentGame.Score);
                }
                catch (Exception ex)
                {
                    _log.AppendLine($"Error playing {option}: {ex}");
                }
                finally
                {
                    _log.AppendLine($"Quit {option}");
                }

            ShowMenu();
        }

        public void Dispose()
        {
            Config.Save();
            UI.Close();

            if (_currentGame != null)
            {
                _currentGame.Dispose();
                _currentGame = null;
            }

            _log.Clear();
        }
    }
}
