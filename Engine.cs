using System;
using System.Text;

namespace iobloc
{
    class Engine : IDisposable
    {
        readonly StringBuilder _log = new StringBuilder();
        readonly Menu _menu;
        bool _saveSettings;

        internal Engine(string settingsFilePath)
        {
            if (!string.IsNullOrEmpty(settingsFilePath))
            {
                Config.Load(settingsFilePath);
                _saveSettings = true;
            }
            else
                Config.Load();
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
            {
                Game game = null;
                try
                {
                    _log.AppendLine($"Start {option}");
                    UI.Clear();

                    game = new Game(option);
                    game.Start();
                    _log.AppendLine($"{game.EndedMessage} {option}");
                }
                catch (Exception ex)
                {
                    _log.AppendLine($"Error playing {option}: {ex}");
                }
                finally
                {
                    if (game != null)
                        game.Dispose();
                }
            }

            ShowMenu();
        }

        public void Dispose()
        {
            Config.Save(_saveSettings);
            UI.Close();
            _log.Clear();
        }
    }
}
