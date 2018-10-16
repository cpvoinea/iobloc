using System;

namespace iobloc
{
    class Engine : IDisposable
    {
        readonly Config _config;
        readonly UI _ui;
        readonly Menu _menu;
        Game _currentGame = null;

        internal Engine(string configFilePath)
        {
            _config = new Config(configFilePath);
            _ui = new UI(_config);
            _menu = new Menu(_config, _ui);
            _menu.OnItemSelected += MenuItemSelected;
            _menu.OnExit += MenuExit;
        }

        internal void ShowMenu()
        {
            _menu.Show();
        }

        void MenuItemSelected(MenuItem item)
        {
            _currentGame = new Game(_config, _ui, item.Option);
            _currentGame.OnExit += GameExit;
            _currentGame.Start();
        }

        void MenuExit()
        {
            _config.Save();
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
            _ui.Dispose();
            _config.Dispose();
        }
    }
}
