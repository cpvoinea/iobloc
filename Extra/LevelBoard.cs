using System;

namespace iobloc
{
    class LevelBoard : SinglePanelBoard
    {
        int MAX => Config.LEVEL_MAX;

        internal LevelBoard() : base(Option.Level)
        {
            InitializeGrid();
        }

        protected override void InitializeGrid()
        {
            for (int i = 0; i < MAX; i++)
                _main.Grid[0, i] = 15 - i;
            ChangeGrid(true);
        }

        protected override void ChangeGrid(bool set)
        {
            if (set)
            {
                _main.Grid[0, Level] = 15;
                _main.HasChanges = true;
            }
            else
                _main.Grid[0, Level] = 15 - Level;
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case "RightArrow":
                    if (Level < MAX - 1)
                    {
                        ChangeGrid(false);
                        Level++;
                        ChangeGrid(true);
                    }
                    break;
                case "LeftArrow":
                    if (Level > 0)
                    {
                        ChangeGrid(false);
                        Level--;
                        ChangeGrid(true);
                    }
                    break;
                case "Enter":
                    Config.Level = Level;
                    IsRunning = false;
                    break;
            }
        }

        public override void NextFrame()
        {
        }
    }
}
