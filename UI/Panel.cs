namespace iobloc
{
    class Panel
    {
        internal char Symbol { get; private set; } = (char)BoxGraphics.BlockFull;
        internal int FromRow { get; private set; }
        internal int FromCol { get; private set; }
        internal int ToRow { get; private set; }
        internal int ToCol { get; private set; }
        internal int[,] Grid { get; set; }
        internal bool HasChanges { get; set; }
        internal int Width { get { return ToCol - FromCol + 1; } }
        internal int Height { get { return ToRow - FromRow + 1; } }

        internal Panel(int fromRow, int fromCol, int toRow, int toCol, char? symbol = null)
        {
            FromRow = fromRow;
            FromCol = fromCol;
            ToRow = toRow;
            ToCol = toCol;
            Grid = new int[Height, Width];
            if (symbol.HasValue)
                Symbol = symbol.Value;
        }
    }
}
