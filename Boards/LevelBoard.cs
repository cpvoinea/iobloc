using System;

namespace iobloc
{
    class LevelBoard : SinglePanelBoard
    {
        const int MAX = Config.LEVEL_MAX - 1;
        public override int Score { get { return _level; } }
        int _level;

        internal LevelBoard() : base(Option.Level)
        {
            _level = Config.Level;
            InitializeGrid();
            ChangeGrid(true);
        }

        protected override void InitializeGrid()
        {
            for (int i = 0; i <= MAX; i++)
                _main.Grid[0, i] = MAX - i;
        }

        protected override void ChangeGrid(bool set)
        {
            if (set)
            {
                _main.Grid[0, _level] = MAX;
                _main.HasChanges = true;
            }
            else
                _main.Grid[0, _level] = MAX - _level;
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case "RightArrow":
                    if (_level < MAX)
                    {
                        ChangeGrid(false);
                        _level++;
                        ChangeGrid(true);
                    }
                    break;
                case "LeftArrow":
                    if (_level > 0)
                    {
                        ChangeGrid(false);
                        _level--;
                        ChangeGrid(true);
                    }
                    break;
                case "Enter":
                    Config.Level = _level;
                    IsRunning = false;
                    break;
            }
        }

        public override void NextFrame()
        {
        }
    }
}
