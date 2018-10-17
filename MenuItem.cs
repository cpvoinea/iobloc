namespace iobloc
{
    struct MenuItem
    {
        internal Option Option { get; private set; }
        internal string Name { get; private set; }
        internal bool Visible { get; set; }
        internal int Color { get { return 15 - (int)Option; } }
        internal int? Info { get { return Config.GetHighscore(Option); } }

        internal MenuItem(Option option, string name)
        {
            Option = option;
            Name = name;
            Visible = true;
        }
    }
}
