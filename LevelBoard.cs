using System;

namespace iobloc
{
    class LevelBoard : IBoard
    {
        public string[] Help => new[] { "<<Easy   Hard>>" };
        public int Score => Settings.Game.Level;
        public ConsoleKey[] Keys => new[] { ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.Enter };
        public int StepInterval => 50;
        public int Width => Settings.Game.LEVEL_MAX;
        public int Height => 1;
        public bool Won => false;
        public int[] Clip => new[] { 0, 0, Settings.Game.LEVEL_MAX, 1 };
        public int[,] Grid { get { return _levels; } }
        readonly int[,] _levels;
        bool _exit = false;

        internal LevelBoard()
        {
            _levels = new int[1, Width];
            for (int i = 0; i < Width; i++)
                _levels[0, i] = 15 - i;
            _levels[0, Score] = 15;
        }

        public bool Action(ConsoleKey key)
        {
            _levels[0, Score] = 15 - Score;
            if (key == ConsoleKey.RightArrow && Settings.Game.Level < Width - 1)
                Settings.Game.Level++;
            else if (key == ConsoleKey.LeftArrow && Settings.Game.Level > 0)
                Settings.Game.Level--;
            else if (key == ConsoleKey.Enter)
                _exit = true;

            _levels[0, Score] = 15;
            return true;
        }

        public bool Step()
        {
            if (_exit)
                return false;
            return true;
        }

        public override string ToString()
        {
            return "Level";
        }
    }
}