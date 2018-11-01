namespace iobloc
{
    class UIPanel
    {
        readonly int[,] _grid;

        public int this[int row, int col] { get { return _grid[row, col]; } set { _grid[row, col] = value; } }
        public bool HasChanges { get; set; }
        public string[] Text { get; set; }
        public bool IsText { get; set; }
        public char Symbol { get; private set; }
        public int FromRow { get; private set; }
        public int FromCol { get; private set; }
        public int ToRow { get; private set; }
        public int ToCol { get; private set; }
        public int Width => ToCol - FromCol + 1;
        public int Height => ToRow - FromRow + 1;

        public UIPanel(int fromRow, int fromCol, int toRow, int toCol, int textLength = 0, char symbol = (char)UISymbolType.BlockFull)
        {
            Symbol = symbol;
            FromRow = fromRow;
            FromCol = fromCol;
            ToRow = toRow;
            ToCol = toCol;
            if (textLength > 0)
            {
                Text = new string[textLength];
                IsText = true;
            }
            _grid = new int[Height, Width];
        }

        public void Clear(int val = 0)
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    this[i, j] = val;
            HasChanges = true;
        }
    }
}
