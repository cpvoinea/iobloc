namespace iobloc
{
    struct UIBorder
    {
        readonly int[,] _grid;

        internal int this[int row, int col] { get { return _grid[row, col]; } }
        internal int Width { get; private set; }
        internal int Height { get; private set; }

        internal UIBorder(int width, int height)
        {
            Width = width;
            Height = height;
            _grid = new int[Height, Width];

            AddLines(new[]{
                new UIBorderLine(0, width - 1, 0, false, false),
                new UIBorderLine(0, height - 1, 0, true, false),
                new UIBorderLine(0, width - 1, height - 1, false, false),
                new UIBorderLine(0, height - 1, width - 1, true, false)
            });
        }

        internal UIBorder(UIBorderLine[] lines)
        {
            int maxWidth = 0;
            int maxHeight = 0;
            foreach (var l in lines)
            {
                if (l.IsVertical)
                {
                    if (l.To > maxHeight)
                        maxHeight = l.To;
                    if (l.Position > maxWidth)
                        maxWidth = l.Position;
                }
                else
                {
                    if (l.To > maxWidth)
                        maxWidth = l.To;
                    if (l.Position > maxHeight)
                        maxHeight = l.Position;
                }
            }

            Width = maxWidth + 1;
            Height = maxHeight + 1;
            _grid = new int[Height, Width];

            AddLines(lines);
        }

        internal void AddLines(UIBorderLine[] lines)
        {
            for (int i1 = 0; i1 < lines.Length; i1++)
            {
                var line1 = lines[i1];
                for (int i = line1.From; i <= line1.To; i++)
                    if (line1.IsVertical)
                    {
                        if (_grid[i, line1.Position] == UISymbolType.None)
                            _grid[i, line1.Position] = line1.IsSingle ? UISymbolType.SingleVerticalLine : UISymbolType.VerticalLine;
                    }
                    else
                    {
                        if (_grid[line1.Position, i] == UISymbolType.None)
                            _grid[line1.Position, i] = line1.IsSingle ? UISymbolType.SingleHorizontalLine : UISymbolType.HorizontalLine;
                    }
                for (int i2 = i1 + 1; i2 < lines.Length; i2++)
                {
                    var line2 = lines[i2];
                    var s = line1.GetIntersectionSymbol(line2);
                    if (s != UISymbolType.None)
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
