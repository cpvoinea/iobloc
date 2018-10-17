using System;

namespace iobloc
{
    class Engine : IDisposable
    {
        readonly Menu _menu;
        Game _currentGame;

        internal Engine(string configFilePath)
        {
            Config.Load(configFilePath);
            _menu = new Menu(Config.MenuItems);
            _menu.OnItemSelected += MenuItemSelected;
            UI.Open();
        }

        internal void ShowMenu()
        {
            UI.Clear();
            _menu.Show();
        }

        void MenuItemSelected(Option option)
        {
            _currentGame = new Game(option);
            _currentGame.OnExit += GameExit;
            UI.Clear();
            _currentGame.Start();
        }

        void GameExit(IBoard board)
        {
            if (_currentGame != null)
            {
                _currentGame.OnExit -= GameExit;
                _currentGame.Dispose();
                _currentGame = null;
            }

            ShowMenu();
        }

        public void Dispose()
        {
            _menu.OnItemSelected -= MenuItemSelected;
            _menu.Dispose();
            Config.Save();
            UI.Close();
        }
    }
}
