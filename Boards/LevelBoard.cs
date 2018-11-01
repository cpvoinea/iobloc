namespace iobloc
{
    class LevelBoard : BaseBoard
    {
        public LevelBoard() : base(BoardType.Level) { }

        protected override void InitializeMain()
        {
            Level = Serializer.Level;
            for (int i = 0; i < 16; i++)
                Main[0, i] = 15 - i;
            ChangeMain(true);
        }

        protected override void ChangeMain(bool set)
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
                        ChangeMain(false);
                        Level++;
                        ChangeMain(true);
                    }
                    break;
                case "LeftArrow":
                    if (Level > 0)
                    {
                        ChangeMain(false);
                        Level--;
                        ChangeMain(true);
                    }
                    break;
                case "Enter":
                    Serializer.Level = Level;
                    Stop();
                    break;
            }
        }
    }
}
