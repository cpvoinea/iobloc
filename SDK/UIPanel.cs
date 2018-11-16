namespace iobloc
{
    // A matrix of values representing a rectangle component that needs to be drawn.
    // There are 2 ways to draw a panel: as lines of text or as a color matrix
    public class UIPanel
    {
        // panel color matrix
        private readonly int[,] _grid;
        // Access to color matrix
        public int this[int row, int col] { get { return _grid[row, col]; } set { _grid[row, col] = value; } }
        // Character to be drawn in different colors as configured in grid matrix
        internal char Symbol { get; private set; }
        // Distance from top where panel begins
        internal int FromRow { get; private set; }
        // Distance from left where panel begins
        internal int FromCol { get; private set; }
        // Number of columns in panel
        public int Width { get; private set; }
        // Number of rows in panel
        public int Height { get; private set; }
        // Lines of text to be displayed in text mode
        internal string[] Text { get; private set; }
        // Indicates if text mode (true) or color mode (false)
        internal bool IsTextMode { get; private set; }
        // If true, panel has changed and should be drawn again
        internal bool HasChanges { get; private set; }

        // Summary:
        //      Initialize panel
        // Param: fromRow: Distance from top where panel begins
        // Param: fromCol: Distance from left where panel begins
        // Param: toRow: Distance from top where panel ends, including last row/param>
        // Param: toCol: Distance from left where panel ends, including last column
        // Param: symbol: Character to be drawn in different colors as configured in grid matrix, defaults to full block
        public UIPanel(int fromRow, int fromCol, int toRow, int toCol, char symbol = (char)UISymbol.BlockFull)
        {
            Symbol = symbol;
            FromRow = fromRow;
            FromCol = fromCol;
            Width = toCol - fromCol + 1;
            Height = toRow - fromRow + 1;
            HasChanges = true;
            _grid = new int[Height, Width];
        }

        // Summary:
        //      Set color matrix to same value
        // Param: val: value defaults to 0
        public void Clear(int val = 0)
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    this[i, j] = val;
        }

        // Summary:
        //      Set lines of text for text mode, enters text mode and marks changes if required
        // Param: textLines: text
        // Param: setTextMode: switch to text mode
        public void SetText(string[] textLines, bool setTextMode = true)
        {
            Text = textLines;
            if (setTextMode)
                IsTextMode = true;
            if (IsTextMode)
                HasChanges = true;
        }

        // Summary:
        //      Change first line of text, does not enter change mode but marks changes if already in text mode
        public void SetText(string text)
        {
            if (Text == null || Text.Length != 1)
                Text = new[] { text };
            else
                Text[0] = text;
            IsTextMode = true;
            HasChanges = true;
        }

        // Summary:
        //      Toggle between text mode and matrix mode and marks changes
        public void SwitchMode()
        {
            IsTextMode = !IsTextMode;
            HasChanges = true;
        }

        // Summary:
        //      Mark the panel with changes or not
        public void Change(bool hasChanges = true)
        {
            HasChanges = hasChanges;
        }
    }
}
