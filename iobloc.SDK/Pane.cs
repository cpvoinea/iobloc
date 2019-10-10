namespace iobloc.SDK
{
    public class Pane
    {
        private readonly PaneCell[,] _grid;
        public PaneCell this[int row, int col] { get { return _grid[row, col]; } set { _grid[row, col] = value; } }
        internal int FromRow { get; private set; }
        internal int FromCol { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        internal bool HasChanges { get; private set; }

        public Pane(int fromRow, int fromCol, int toRow, int toCol)
        {
            FromRow = fromRow;
            FromCol = fromCol;
            Width = toCol - fromCol + 1;
            Height = toRow - fromRow + 1;
            HasChanges = true;
            _grid = new PaneCell[Height, Width];
        }

        public void Clear(PaneCell val = default)
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    this[i, j] = val;
        }

        public void Change(bool hasChanges = true)
        {
            HasChanges = hasChanges;
        }
    }
}
