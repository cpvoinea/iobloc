namespace iobloc
{
    // A matrix of values representing a rectangle component that needs to be drawn.
    // There are 2 ways to draw a pane: as lines of text or as a color matrix
    public class Pane<T> where T : struct
    {
        // pane color matrix
        private readonly T[,] _grid;
        // Access to color matrix
        public T this[int row, int col] { get { return _grid[row, col]; } set { _grid[row, col] = value; } }
        // Character to be drawn in different colors as configured in grid matrix
        public char BlockChar { get; private set; }
        // Distance from top where pane begins
        public int FromRow { get; private set; }
        // Distance from left where pane begins
        public int FromCol { get; private set; }
        // Number of columns in pane
        public int Width { get; private set; }
        // Number of rows in pane
        public int Height { get; private set; }
        // Lines of text to be displayed in text mode
        public string[] Text { get; private set; }
        // Indicates if text mode (true) or color mode (false)
        public bool IsTextMode { get; private set; }
        // If true, pane has changed and should be drawn again
        public bool HasChanges { get; private set; }
        public Area? Area { get; set; }

        // Summary:
        //      Initialize pane
        // Parameters: fromRow: Distance from top where pane begins
        // Parameters: fromCol: Distance from left where pane begins
        // Parameters: toRow: Distance from top where pane ends, including last row/param>
        // Parameters: toCol: Distance from left where pane ends, including last column
        // Parameters: symbol: Character to be drawn in different colors as configured in grid matrix, defaults to full block
        public Pane(int fromRow, int fromCol, int toRow, int toCol, char blockChar = (char)Symbol.BlockFull)
        {
            BlockChar = blockChar;
            FromRow = fromRow;
            FromCol = fromCol;
            Width = toCol - fromCol + 1;
            Height = toRow - fromRow + 1;
            HasChanges = true;
            _grid = new T[Height, Width];
        }

        // Summary:
        //      Set color matrix to same value
        // Parameters: val: value defaults to 0
        public void Clear(T val = default)
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    this[i, j] = val;
        }

        // Summary:
        //      Set lines of text for text mode, enters text mode and marks changes if required
        // Parameters: textLines: text
        // Parameters: setTextMode: switch to text mode
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
            SetText(text.Split(','));
        }

        // Summary:
        //      Toggle between text mode and matrix mode and marks changes
        public void SwitchMode()
        {
            IsTextMode = !IsTextMode;
            HasChanges = true;
        }

        // Summary:
        //      Mark the pane with changes or not
        public void Change(bool hasChanges = true)
        {
            HasChanges = hasChanges;
        }
    }
}
