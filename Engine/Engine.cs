namespace iobloc
{
    static class Engine
    {
        internal static void Start(string settingsFilePath)
        {
            Serializer.LoadSettings(settingsFilePath);
            Serializer.LoadHighscores();
            UIPainter.Initialize();

            IBoard board = Serializer.GetBoard(BoardType.Menu);
            while (board != null)
            {
                BoardRunner.Run(board);
                board = board.Next;
            }
        }

        internal static void Stop()
        {
            Serializer.SaveSettings();
            Serializer.SaveHighscores();
            UIPainter.Exit();
        }
    }
}
