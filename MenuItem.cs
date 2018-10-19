namespace iobloc
{
    struct MenuItem
    {
        internal Option Option { get; private set; }
        internal string Name { get; private set; }
        internal bool Visible { get; private set; }
        internal int Color { get { return 15 - (int)Option; } }
        internal int? Info { get { return Config.GetHighscore(Option); } }

        internal MenuItem(Option option, bool visible = true)
        {
            Option = option;
            Name = option.ToString();
            Visible = visible;
        }
    }
}
