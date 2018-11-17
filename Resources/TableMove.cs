namespace iobloc
{
    enum MoveType { Skip, Select, Pick, Put }

    struct TableMove
    {
        public MoveType Type { get; private set; }
        public int Line { get; private set; }
        public int Dice { get; private set; }

        public TableMove(MoveType type, int line, int dice)
        {
            Type = type;
            Line = line;
            Dice = dice;
        }
    }
}
