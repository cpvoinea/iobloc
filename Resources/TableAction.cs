namespace iobloc
{
    enum ActionType { Skip, Select, Take, Put }

    struct TableAction
    {
        public ActionType Type { get; private set; }
        public int Line { get; private set; }
        public int Dice { get; private set; }

        public TableAction(ActionType type, int line, int dice)
        {
            Type = type;
            Line = line;
            Dice = dice;
        }
    }
}
