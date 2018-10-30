namespace iobloc
{
    class UIPanel
    {
        readonly int[,] _grid;

        internal int this[int row, int col] { get { return _grid[row, col]; } set { _grid[row, col] = value; } }
        internal bool HasChanges { get; set; }
        internal bool IsText { get; set; }
        internal string[] Text { get; private set; }
        internal char Symbol { get; private set; }
        internal int FromRow { get; private set; }
        internal int FromCol { get; private set; }
        internal int ToRow { get; private set; }
        internal int ToCol { get; private set; }
        internal int Width { get { return ToCol - FromCol + 1; } }
        internal int Height { get { return ToRow - FromRow + 1; } }

        internal UIPanel(int fromRow, int fromCol, int toRow, int toCol, char symbol = (char)UISymbolType.BlockFull)
        {
            Symbol = symbol;
            FromRow = fromRow;
            FromCol = fromCol;
            ToRow = toRow;
            ToCol = toCol;
            _grid = new int[Height, Width];
        }

        internal void SetText(string[] text)
        {
            Text = text;
            IsText = true;
        }
    }
}
