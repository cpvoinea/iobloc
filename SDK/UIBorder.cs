using System.Collections.Generic;

namespace iobloc
{
    /// <summary>
    /// A matrix of box drawing UISymbol that construct a grid of lines around panels
    /// </summary>
    public struct UIBorder
    {
        // box drawing matrix
        private readonly int[,] _grid;
        private readonly List<UIBorderLine> _lines;
        /// <summary>
        /// Access to box drawing symbol matrix
        /// </summary>
        internal int this[int row, int col] => _grid[row, col];
        /// <summary>
        /// Maximum width
        /// </summary>
        /// <value></value>
        internal int Width { get; private set; }
        /// <summary>
        /// Maximum height
        /// </summary>
        /// <value></value>
        internal int Height { get; private set; }

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
            _lines = new List<UIBorderLine>();

            AddLines(new[]{
                new UIBorderLine(0, width - 1, 0, false, false), // top
                new UIBorderLine(0, height - 1, 0, true, false), // left
                new UIBorderLine(0, width - 1, height - 1, false, false), //right
                new UIBorderLine(0, height - 1, width - 1, true, false) // down
            });
        }

        /// <summary>
        /// Add extra lines and calculates intersections and UISymbol
        /// </summary>
        /// <param name="lines">interior lines to be added</param>
        public void AddLines(UIBorderLine[] lines)
        {
            _lines.AddRange(lines);
            for (int i1 = 0; i1 < _lines.Count; i1++)
            {
                var line1 = _lines[i1];
                for (int i = line1.From; i <= line1.To; i++)
                    if (line1.IsVertical)
                    {
                        if (_grid[i, line1.Position] == UISymbol.None)
                            _grid[i, line1.Position] = line1.IsSingle ? UISymbol.SingleVerticalLine : UISymbol.VerticalLine;
                    }
                    else
                    {
                        if (_grid[line1.Position, i] == UISymbol.None)
                            _grid[line1.Position, i] = line1.IsSingle ? UISymbol.SingleHorizontalLine : UISymbol.HorizontalLine;
                    }
                // look for intersections
                for (int i2 = i1 + 1; i2 < _lines.Count; i2++)
                {
                    var line2 = _lines[i2];
                    var s = line1.GetIntersectionSymbol(line2);
                    if (s != UISymbol.None)
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
