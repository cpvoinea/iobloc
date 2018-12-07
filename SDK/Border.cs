using System.Collections.Generic;

namespace iobloc
{
    // A matrix of box drawing Symbol that construct a grid of lines around panes
    public struct Border
    {
        // box drawing matrix
        private readonly int[,] _grid;
        private readonly List<BorderLine> _lines;
        // Access to box drawing symbol matrix
        internal int this[int row, int col] => _grid[row, col];
        // Maximum width
        internal int Width { get; private set; }
        // Maximum height
        internal int Height { get; private set; }

        // Summary:
        //      Initialize to a rectangle of width x height with double line borders
        // Parameters: width: include start and end vertical lines
        // Parameters: height: includes start and end horizontal lines
        public Border(int width, int height)
        {
            Width = width;
            Height = height;
            _grid = new int[Height, Width];
            _lines = new List<BorderLine>();

            AddLines(new[]{
                new BorderLine(0, width - 1, 0, false, false), // top
                new BorderLine(0, height - 1, 0, true, false), // left
                new BorderLine(0, width - 1, height - 1, false, false), //right
                new BorderLine(0, height - 1, width - 1, true, false) // down
            });
        }

        // Summary:
        //      Add extra lines and calculates intersections and Symbol
        // Parameters: lines: interior lines to be added
        public void AddLines(BorderLine[] lines)
        {
            _lines.AddRange(lines);
            for (int i1 = 0; i1 < _lines.Count; i1++)
            {
                var line1 = _lines[i1];
                for (int i = line1.From; i <= line1.To; i++)
                    if (line1.IsVertical)
                    {
                        if (_grid[i, line1.Position] == Symbol.None)
                            _grid[i, line1.Position] = line1.IsSingle ? Symbol.SingleVerticalLine : Symbol.VerticalLine;
                    }
                    else
                    {
                        if (_grid[line1.Position, i] == Symbol.None)
                            _grid[line1.Position, i] = line1.IsSingle ? Symbol.SingleHorizontalLine : Symbol.HorizontalLine;
                    }
                // look for intersections
                for (int i2 = i1 + 1; i2 < _lines.Count; i2++)
                {
                    var line2 = _lines[i2];
                    var s = line1.GetIntersectionSymbol(line2);
                    if (s != Symbol.None)
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
