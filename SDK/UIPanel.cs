namespace iobloc
{
    /// <summary>
    /// A matrix of values representing a rectangle component that needs to be drawn.
    /// There are 2 ways to draw a panel: as lines of text or as a color matrix
    /// </summary>
    public class UIPanel
    {
        // panel color matrix
        private readonly int[,] _grid;
        /// <summary>
        /// Access to color matrix
        /// </summary>
        /// <value></value>
        public int this[int row, int col] { get { return _grid[row, col]; } set { _grid[row, col] = value; } }
        /// <summary>
        /// Character to be drawn in different colors as configured in grid matrix
        /// </summary>
        /// <value></value>
        internal char Symbol { get; private set; }
        /// <summary>
        /// Distance from top where panel begins
        /// </summary>
        /// <value></value>
        internal int FromRow { get; private set; }
        /// <summary>
        /// Distance from left where panel begins
        /// </summary>
        /// <value></value>
        internal int FromCol { get; private set; }
        /// <summary>
        /// Number of columns in panel
        /// </summary>
        /// <value></value>
        public int Width { get; private set; }
        /// <summary>
        /// Number of rows in panel
        /// </summary>
        /// <value></value>
        public int Height { get; private set; }
        /// <summary>
        /// Lines of text to be displayed in text mode
        /// </summary>
        /// <value></value>
        public string[] Text { get; set; }
        /// <summary>
        /// Indicates if text mode (true) or color mode (false)
        /// </summary>
        /// <value></value>
        public bool IsText { get; set; }
        /// <summary>
        /// If true, panel has changed and should be drawn again
        /// </summary>
        /// <value></value>
        public bool HasChanges { get; set; }

        /// <summary>
        /// Initialize panel
        /// </summary>
        /// <param name="fromRow">Distance from top where panel begins</param>
        /// <param name="fromCol">Distance from left where panel begins</param>
        /// <param name="toRow">Distance from top where panel ends, including last row/param>
        /// <param name="toCol">Distance from left where panel ends, including last column</param>
        /// <param name="textLength">Set to a value > 0 to enter text mode and initialize text lines array to this length</param>
        /// <param name="symbol">Character to be drawn in different colors as configured in grid matrix, defaults to full block</param>
        /// <returns></returns>
        public UIPanel(int fromRow, int fromCol, int toRow, int toCol, int textLength = 0, char symbol = (char)UISymbol.BlockFull)
        {
            Symbol = symbol;
            FromRow = fromRow;
            FromCol = fromCol;
            Width = toCol - fromCol + 1;
            Height = toRow - fromRow + 1;
            Text = new string[textLength];
            IsText = textLength > 0;
            HasChanges = false;
            _grid = new int[Height, Width];
        }

        /// <summary>
        /// Set color matrix to same value
        /// </summary>
        /// <param name="val">value defaults to 0</param>
        public void Clear(int val = 0)
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    this[i, j] = val;
        }
    }
}
