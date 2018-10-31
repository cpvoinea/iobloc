namespace iobloc
{
    class LevelBoard : BaseBoard
    {
        internal LevelBoard() : base(BoardType.Level) { }

        protected override void InitializeGrid()
        {
            for (int i = 0; i < 16; i++)
                Main[0, i] = 15 - i;

            base.InitializeGrid();
        }

        protected override void ChangeGrid(bool set)
        {
            if (set)
            {
                Main[0, Level] = 15;
                Main.HasChanges = true;
            }
            else
                Main[0, Level] = 15 - Level;
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
