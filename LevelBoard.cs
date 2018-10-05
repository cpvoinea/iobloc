using System;

namespace iobloc
{
    class LevelBoard : IBoard
    {
        public string[] Help => new[] { "<Easy  Hard>" };
        public int Score => Settings.Game.Level;
        public ConsoleKey[] Keys => new[] { ConsoleKey.LeftArrow, ConsoleKey.RightArrow };
        public int StepInterval => 1000;
        public int Width => 12;
        public int Height => 1;
        public int[] Clip => new[] { 0, 0, 12, 1 };
        readonly int[,] _levels = new[,] { { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 } };
        public int[,] Grid { get { return _levels; } }

        internal LevelBoard()
        {
            _levels[0, Score] = 15;
        }

        public bool Action(ConsoleKey key)
        {
            _levels[0, Score] = Score;
            if (key == ConsoleKey.RightArrow && Settings.Game.Level < 11)
                Settings.Game.Level++;
            else if (key == ConsoleKey.LeftArrow && Settings.Game.Level > 0)
                Settings.Game.Level--;
            _levels[0, Score] = 15;
            return true;
        }

        public bool Step()
        {
            return true;
        }
    }
}