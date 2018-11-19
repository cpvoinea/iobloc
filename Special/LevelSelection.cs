namespace iobloc
{
    // Display a list of colors representing levels to select the MasterLevel
    class LevelSelection : BaseGame
    {
        public LevelSelection() : base(GameType.Level) { }

        // Summary:
        //      A line of colors representing levels
        protected override void Initialize()
        {
            for (int i = 0; i < 16; i++)
                Main[0, i] = 15 - i;
            Level = Serializer.MasterLevel;
        }

        // Summary:
        //      Mark selection cursor
        protected override void Change(bool set)
        {
            if (set)
            {
                Main[0, Level] = 15;
                base.Change(true);
            }
            else
                Main[0, Level] = 15 - Level;
        }

        // Summary:
        //      Move cursor left-right, select level
        public override void HandleInput(string key)
        {
            switch (key)
            {
                case UIKey.RightArrow:
                    if (Level < 15)
                    {
                        Change(false);
                        Level++;
                        Change(true);
                    }
                    break;
                case UIKey.LeftArrow:
                    if (Level > 0)
                    {
                        Change(false);
                        Level--;
                        Change(true);
                    }
                    break;
                case UIKey.Enter:
                    Serializer.MasterLevel = Level;
                    Stop();
                    break;
            }
        }
    }
}
