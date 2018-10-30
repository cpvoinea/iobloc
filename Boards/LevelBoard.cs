using System;

namespace iobloc
{
    class LevelBoard : BaseBoard
    {
        internal LevelBoard() : base(BoardType.Level)
        {
            InitializeGrid();
        }

        protected override void InitializeGrid()
        {
            for (int i = 0; i < 16; i++)
                _main[0, i] = 15 - i;
            ChangeGrid(true);
        }

        protected override void ChangeGrid(bool set)
        {
            if (set)
            {
                _main[0, Level] = 15;
                _main.HasChanges = true;
            }
            else
                _main[0, Level] = 15 - Level;
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case "RightArrow":
                    if (Level < 15)
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
                    Serializer.Level = Level;
                    IsRunning = false;
                    break;
            }
        }
    }
}
