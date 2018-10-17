namespace iobloc
{
    class Panel
    {
        internal int FromRow { get; private set; }
        internal int FromCol { get; private set; }
        internal int ToRow { get; private set; }
        internal int ToCol { get; private set; }
        internal int[,] Grid { get; private set; }
        internal bool HasChanges { get; set; }
        internal int Width { get{return ToCol - FromCol + 1;} }
        internal int Height { get{return ToRow - FromRow + 1;} }

        internal Panel(int fromRow, int fromCol, int toRow, int toCol)
        {
            FromRow = fromRow;
            FromCol = fromCol;
            ToRow = toRow;
            ToCol = toCol;
            Grid = new int[Height, Width];
        }
    }
}
