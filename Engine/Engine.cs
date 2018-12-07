using System;

namespace iobloc
{
    // Entry point to application, handle initialization and disposal
    class Engine : IDisposable
    {
        private readonly string _settingsFilePath;
        private readonly IRenderer _renderer;

        public Engine(string settingsFilePath, bool runInForm)
        {
            _settingsFilePath = settingsFilePath;
            _renderer = runInForm ? (IRenderer)new FormRenderer() : new ConsoleRenderer();
            Serializer.Load(_settingsFilePath);
        }

        // Summary:
        //      Display menu game
        //      Handle linking of games (menu->game->animation->menu->etc)
        // Parameters: settingsFilePath: optional external settings file path, if null use default settings
        public void Start()
        {
            IGame menu = Serializer.GetGame((int)GameType.Menu);
            IGame game = menu;
            while (game != null)
            {
                var runner = new GameRunner(_renderer, game);
                runner.Run();

                if (game is IBaseGame) // base game selected
                    game = (game as IBaseGame).Next; // continue to next game
                else if (game != menu) // return to menu
                    game = menu;
                else // exit
                    game = null;
            }
        }

        public void Dispose()
        {
            Serializer.Save(_settingsFilePath);
            _renderer.Dispose();
        }
    }
}
