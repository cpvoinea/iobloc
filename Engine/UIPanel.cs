namespace iobloc
{
    class UIPanel
    {
        private readonly int[,] _grid;
        public int this[int row, int col] { get { return _grid[row, col]; } set { _grid[row, col] = value; } }
        public char Symbol { get; private set; }
        public int FromRow { get; private set; }
        public int FromCol { get; private set; }
        public int ToRow { get; private set; }
        public int ToCol { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string[] Text { get; set; }
        public bool IsText { get; set; }
        public bool HasChanges { get; set; }

        public UIPanel(int fromRow, int fromCol, int toRow, int toCol, int textLength = 0, char symbol = (char)Symbols.BlockFull)
        {
            Symbol = symbol;
            FromRow = fromRow;
            FromCol = fromCol;
            ToRow = toRow;
            ToCol = toCol;
            Width = toCol - fromCol + 1;
            Height = toRow - fromRow + 1;
            Text = new string[textLength];
            IsText = textLength > 0;
            HasChanges = IsText;
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
