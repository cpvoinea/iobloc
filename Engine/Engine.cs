namespace iobloc
{
    /// <summary>
    /// Entry point to application, handle initialization and disposal
    /// </summary>
    static class Engine
    {
        /// <summary>
        /// Load settings from default or external file,
        /// Initialize UI
        /// </summary>
        /// <param name="settingsFilePath"></param>
        public static void Initialize(string settingsFilePath)
        {
            Serializer.Load(settingsFilePath);
            UIPainter.Initialize();
        }

        /// <summary>
        /// Display menu board
        /// Handle linking of boards (menu->game->animation->menu->etc)
        /// </summary>
        /// <param name="settingsFilePath">optional external settings file path, if null use default settings</param>
        public static void Start()
        {
            IBoard menu = Serializer.GetBoard((int)BoardType.Menu);
            IBoard board = menu;
            while (board != null)
            {
                BoardRunner.Run(board);
                if (board is IBaseBoard) // base board selected
                    board = (board as IBaseBoard).Next; // continue to next board
                else if (board != menu) // return to menu
                    board = menu;
                else // exit
                    board = null;
            }
        }

        /// <summary>
        /// Save settings if external file is used,
        /// Restore UI to previous state, in case errors need to be displayed
        /// </summary>
        public static void Stop()
        {
            Serializer.Save();
            UIPainter.Exit();
        }
    }
}
