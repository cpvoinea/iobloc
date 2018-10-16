using System;

namespace iobloc
{
    class LevelBoard : SinglePanelBoard
    {
        const int MAX = Config.LEVEL_MAX;
        public string Name => "Level";
        public string[] Help => new[] { "<<Easy   Hard>>" };
        public ConsoleKey[] Keys { get; private set; } = new[] { ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.Enter };
        public bool Won => false;
        public int StepInterval => 200;
        public Border Frame { get; private set; } = new Border(MAX + 2, 3);
        public int[] Clip { get; private set; } = new[] { 0, 0, MAX, 1 };
        public int Score => Config.Level;
        public int[,] Grid => _levels;

        readonly int[,] _levels;
        bool _exit = false;

        internal LevelBoard() : base(Option.Level)
        {
            _levels = new int[1, MAX];
            for (int i = 0; i < MAX; i++)
                _levels[0, i] = 15 - i;
            _levels[0, Score] = 15;
        }

        public bool Action(ConsoleKey key)
        {
            int lvl = Config.Level;
            _levels[0, lvl] = 15 - lvl;
            if (key == ConsoleKey.RightArrow && lvl < MAX - 1)
                lvl++;
            else if (key == ConsoleKey.LeftArrow && lvl > 0)
                lvl--;
            else if (key == ConsoleKey.Enter)
                _exit = true;

            Config.Level = lvl;
            _levels[0, lvl] = 15;
            return true;
        }

        public bool Step()
        {
            if (_exit)
                return false;
            return true;
        }
    }
}