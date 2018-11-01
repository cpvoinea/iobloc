namespace iobloc
{
    class LevelBoard : BaseBoard
    {
        internal LevelBoard() : base(BoardType.Level) { }

        protected override void Initialize()
        {
            Level = Serializer.Level;
            for (int i = 0; i < 16; i++)
                Main[0, i] = 15 - i;
            Change(true);
        }

        protected override void Change(bool set)
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
                        Change(false);
                        Level++;
                        Change(true);
                    }
                    break;
                case "LeftArrow":
                    if (Level > 0)
                    {
                        Change(false);
                        Level--;
                        Change(true);
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
