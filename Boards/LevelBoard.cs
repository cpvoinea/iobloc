using System;

namespace iobloc
{
    class LevelBoard : SinglePanelBoard
    {
        const int MAX = Config.LEVEL_MAX;
        public override int Score => Config.Level;
        int _level;

        internal LevelBoard() : base(Option.Level)
        {
            _level = Score;
            InitializeGrid();
            ChangeGrid(true);
        }

        protected override void InitializeGrid()
        {
            for (int i = 0; i < MAX; i++)
                _main.Grid[0, i] = 15 - i;
        }

        protected override void ChangeGrid(bool set)
        {
            if (set)
            {
                _main.Grid[0, _level] = 15;
                _main.HasChanges = true;
            }
            else
                _main.Grid[0, _level] = 15 - _level;
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case "RightArrow":
                    if (_level < MAX - 1)
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
                    Score = _level;
                    IsRunning = false;
                    break;
            }
        }

        public override void NextFrame()
        {
        }
    }
}