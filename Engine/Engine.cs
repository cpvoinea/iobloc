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
            IBaseBoard board = Serializer.GetBoard((int)BoardType.Menu);
            while (board != null)
            {
                BoardRunner.Run(board);
                // each IBaseBoard has a link to the next one until exit is called
                board = board.Next;
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
