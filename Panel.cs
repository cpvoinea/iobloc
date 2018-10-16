namespace iobloc
{
    struct Panel
    {
        internal int FromRow { get; private set; }
        internal int FromCol { get; private set; }
        internal int ToRow { get; private set; }
        internal int ToCol { get; private set; }
        internal int Width { get; private set; }
        internal int Height { get; private set; }
        internal int[,] Grid { get; private set; }
        internal bool HasChanges { get; set; }

        internal Panel(int fromRow, int fromCol, int toRow, int toCol)
        {
            FromRow = fromRow;
            FromCol = fromCol;
            ToRow = toRow;
            ToCol = toCol;
            Width = ToCol - FromCol + 1;
            Height = ToRow - FromRow + 1;
            Grid = new int[Height, Width];
            HasChanges = false;
        }
    }
}