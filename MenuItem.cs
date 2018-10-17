namespace iobloc
{
    struct MenuItem
    {
        internal Option Option { get; private set; }
        internal string Name { get; private set; }
        internal bool Visible { get; set; }
        internal int Color { get { return 15 - (int)Option; } }
        internal string Info
        {
            get
            {
                if (!Highscores.Exists(Option))
                    return string.Empty;
                return Highscores.Get(Option).ToString();
            }
        }

        internal MenuItem(Option option, string name)
        {
            Option = option;
            Name = name;
            Visible = true;
        }
    }
}
