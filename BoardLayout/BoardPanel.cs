namespace iobloc
{
    struct BoardPanel
    {
        internal int FromRow { get; private set; }
        internal int FromCol { get; private set; }
        internal int ToRow { get; private set; }
        internal int ToCol { get; private set; }

        internal BoardPanel(int fromRow, int fromCol, int toRow, int toCol)
        {
            FromRow = fromRow;
            FromCol = fromCol;
            ToRow = toRow;
            ToCol = toCol;
        }
    }
}