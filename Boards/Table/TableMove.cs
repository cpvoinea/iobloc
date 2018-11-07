namespace iobloc
{
    struct TableMove
    {
        public int From { get; private set; }
        public int To { get; private set; }

        public TableMove(int from, int to)
        {
            From = from;
            To = to;
        }
    }
}
