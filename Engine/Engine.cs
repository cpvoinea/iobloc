namespace iobloc
{
    static class Engine
    {
        public static void Start(string settingsFilePath)
        {
            Serializer.LoadSettings(settingsFilePath);
            Serializer.LoadHighscores();
            UIPainter.Initialize();

            IBaseBoard board = Serializer.GetBoard(BoardType.Menu);
            while (board != null)
            {
                BoardRunner.Run(board);
                board = board.Next;
            }
        }

        public static void Stop()
        {
            Serializer.SaveSettings();
            Serializer.SaveHighscores();
            UIPainter.Exit();
        }
    }
}
