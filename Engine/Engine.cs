namespace iobloc
{
    static class Engine
    {
        public static void Start(string settingsFilePath)
        {
            Serializer.Load(settingsFilePath);
            UIPainter.Initialize();

            IBaseBoard board = Serializer.GetBoard((int)BoardType.Menu);
            while (board != null)
            {
                BoardRunner.Run(board);
                board = board.Next;
            }
        }

        public static void Stop()
        {
            Serializer.Save();
            UIPainter.Exit();
        }
    }
}
