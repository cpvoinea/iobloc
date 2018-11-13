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
        internal string[] Text { get; private set; }
        /// <summary>
        /// Indicates if text mode (true) or color mode (false)
        /// </summary>
        /// <value></value>
        internal bool IsTextMode { get; private set; }
        /// <summary>
        /// If true, panel has changed and should be drawn again
        /// </summary>
        /// <value></value>
        internal bool HasChanges { get; private set; }

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
        public UIPanel(int fromRow, int fromCol, int toRow, int toCol, bool isTextMode = false, char symbol = (char)UISymbol.BlockFull)
        {
            Symbol = symbol;
            FromRow = fromRow;
            FromCol = fromCol;
            Width = toCol - fromCol + 1;
            Height = toRow - fromRow + 1;
            IsTextMode = isTextMode;
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

        /// <summary>
        /// Set lines of text for text mode
        /// </summary>
        /// <param name="textLines">text</param>
        /// <param name="setTextMode">switch to text mode</param>
        public void SetText(string[] textLines, bool setTextMode = true)
        {
            Text = textLines;
            if (setTextMode)
            {
                IsTextMode = true;
                HasChanges = true;
            }
        }

        /// <summary>
        /// Change first line of text
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            if (Text == null || Text.Length != 1)
                Text = new[] { text };
            else
                Text[0] = text;
            if (IsTextMode)
                HasChanges = true;
        }

        /// <summary>
        /// Toggle between text mode and matrix mode
        /// </summary>
        public void SwitchMode()
        {
            IsTextMode = !IsTextMode;
            HasChanges = true;
        }

        /// <summary>
        /// Mark the panel with changes or not
        /// </summary>
        /// <param name="hasChanges"></param>
        public void Change(bool hasChanges = true)
        {
            HasChanges = hasChanges;
        }
    }
}
