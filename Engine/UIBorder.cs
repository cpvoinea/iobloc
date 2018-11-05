namespace iobloc
{
    /// <summary>
    /// A matrix of box drawing symbols that construct a grid of lines around panels
    /// </summary>
    struct UIBorder
    {
        // box drawing matrix
        private readonly int[,] _grid;
        /// <summary>
        /// Access to box drawing symbol matrix
        /// </summary>
        public int this[int row, int col] => _grid[row, col];
        /// <summary>
        /// Maximum width
        /// </summary>
        /// <value></value>
        public int Width { get; private set; }
        /// <summary>
        /// Maximum height
        /// </summary>
        /// <value></value>
        public int Height { get; private set; }

        /// <summary>
        /// Initialize to a rectangle of width x height with double line borders
        /// </summary>
        /// <param name="width">include start and end vertical lines</param>
        /// <param name="height">includes start and end horizontal lines</param>
        public UIBorder(int width, int height)
        {
            Width = width;
            Height = height;
            _grid = new int[Height, Width];

            AddLines(new[]{
                new UIBorderLine(0, width - 1, 0, false, false), // top
                new UIBorderLine(0, height - 1, 0, true, false), // left
                new UIBorderLine(0, width - 1, height - 1, false, false), //right
                new UIBorderLine(0, height - 1, width - 1, true, false) // down
            });
        }

        /// <summary>
        /// Add extra lines and calculates intersections and symbols
        /// </summary>
        /// <param name="lines">interior lines to be added</param>
        public void AddLines(UIBorderLine[] lines)
        {
            for (int i1 = 0; i1 < lines.Length; i1++)
            {
                var line1 = lines[i1];
                for (int i = line1.From; i <= line1.To; i++)
                    if (line1.IsVertical)
                    {
                        if (_grid[i, line1.Position] == Symbols.None)
                            _grid[i, line1.Position] = line1.IsSingle ? Symbols.SingleVerticalLine : Symbols.VerticalLine;
                    }
                    else
                    {
                        if (_grid[line1.Position, i] == Symbols.None)
                            _grid[line1.Position, i] = line1.IsSingle ? Symbols.SingleHorizontalLine : Symbols.HorizontalLine;
                    }
                // look for intersections
                for (int i2 = i1 + 1; i2 < lines.Length; i2++)
                {
                    var line2 = lines[i2];
                    var s = line1.GetIntersectionSymbol(line2);
                    if (s != Symbols.None)
                    {
                        if (line1.IsVertical)
                            _grid[line2.Position, line1.Position] = s;
                        else
                            _grid[line1.Position, line2.Position] = s;
                    }
                }
            }
        }
    }
}
