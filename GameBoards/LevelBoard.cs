using System;

namespace iobloc
{
    class LevelBoard : IBoard
    {
        const int MAX = Settings.Game.LEVEL_MAX;
        public string[] Help => new[] { "<<Easy   Hard>>" };
        public ConsoleKey[] Keys { get; private set; } = new[] { ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.Enter };
        public bool Won => false;
        public int StepInterval => 500;
        public BoardFrame Frame { get; private set; } = new BoardFrame(MAX + 2, 3);
        public int[] Clip { get; private set; } = new[] { 0, 0, MAX, 1 };
        public int Score => Settings.Game.Level;
        public int[,] Grid => _levels;

        readonly int[,] _levels;
        bool _exit = false;

        internal LevelBoard()
        {
            _levels = new int[1, MAX];
            for (int i = 0; i < MAX; i++)
                _levels[0, i] = 15 - i;
            _levels[0, Score] = 15;
        }

        public bool Action(ConsoleKey key)
        {
            _levels[0, Score] = 15 - Score;
            if (key == ConsoleKey.RightArrow && Settings.Game.Level < MAX - 1)
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