namespace iobloc
{
    struct MenuItem
    {
        internal int Code { get; private set; }
        internal string Name { get; private set; }
        internal bool Visible { get; private set; }
        internal int Color { get { return 15 - Code; } }

        internal MenuItem(Option option, bool visible = true)
        {
            Code = (int)option;
            Name = option.ToString();
            Visible = visible;
        }
    }
}
