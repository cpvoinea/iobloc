namespace iobloc
{
    // Entry point to application, handle initialization and disposal
    static class Engine
    {
        // Summary:
        //      Load settings from default or external file,
        //      Initialize UI
        // Parameters: settingsFilePath: 
        public static void Initialize(string settingsFilePath)
        {
            Serializer.Load(settingsFilePath);
            Renderer.Initialize();
        }

        // Summary:
        //      Display menu game
        //      Handle linking of games (menu->game->animation->menu->etc)
        // Parameters: settingsFilePath: optional external settings file path, if null use default settings
        public static void Start()
        {
            IGame menu = Serializer.GetGame((int)GameType.Menu);
            IGame game = menu;
            while (game != null)
            {
                GameRunner.Run(game);
                if (game is IBaseGame) // base game selected
                    game = (game as IBaseGame).Next; // continue to next game
                else if (game != menu) // return to menu
                    game = menu;
                else // exit
                    game = null;
            }
        }

        // Summary:
        //      Save settings if external file is used,
        //      Restore UI to previous state, in case errors need to be displayed
        public static void Stop()
        {
            Serializer.Save();
            Renderer.Exit();
        }
    }
}
