namespace iobloc
{
    /// <summary>
    /// Define a line by its position and orientation
    /// </summary>
    struct UIBorderLine
    {
        /// <summary>
        /// Start point is row vertical lines or column for horizontal lines
        /// </summary>
        /// <value></value>
        public int From { get; private set; }
        /// <summary>
        /// End point is row vertical lines or column for horizontal lines
        /// </summary>
        /// <value></value>
        public int To { get; private set; }
        /// <summary>
        /// Line placement is a row for horizontal lines and a column for vertical lines
        /// </summary>
        /// <value></value>
        public int Position { get; private set; }
        /// <summary>
        /// Orientation
        /// </summary>
        /// <value></value>
        public bool IsVertical { get; private set; }
        /// <summary>
        /// Type, either single or double
        /// </summary>
        /// <value></value>
        public bool IsSingle { get; private set; }

        public UIBorderLine(int from, int to, int position, bool isVertical, bool isSingle)
        {
            if (from < to)
            {
                From = from;
                To = to;
            }
            else
            {
                From = to;
                To = from;
            }
            Position = position;
            IsVertical = isVertical;
            IsSingle = isSingle;
        }

        /// <summary>
        /// Depending on line type (single/double) and interection type (cross, T, corner) the intersection symbol is different
        /// </summary>
        /// <param name="line">line to check against</param>
        /// <returns>symbol code</returns>
        public int GetIntersectionSymbol(UIBorderLine line)
        {
            if (IsVertical == line.IsVertical // parallel lines
                || Position < line.From || Position > line.To || line.Position < From || line.Position > To) // no intersection
                return Symbols.None;
            if (IsVertical)
            {
                if (Position == line.From)
                {
                    if (line.Position == From)
                        return Symbols.UpperLeftCorner;
                    if (line.Position == To)
                        return Symbols.LowerLeftCorner;
                    if (IsSingle)
                        return Symbols.SingleLeftSplit;
                    return Symbols.LeftSplit;
                }
                if (Position == line.To)
                {
                    if (line.Position == From)
                        return Symbols.UpperRightCorner;
                    if (line.Position == To)
                        return Symbols.LowerRightCorner;
                    if (IsSingle)
                        return Symbols.SingleRightSplit;
                    return Symbols.RightSplit;
                }
            }
            else
            {
                if (Position == line.From)
                {
                    if (line.Position == From)
                        return Symbols.UpperLeftCorner;
                    if (line.Position == To)
                        return Symbols.UpperRightCorner;
                    if (IsSingle)
                        return Symbols.SingleUppperSplit;
                    return Symbols.UpperSplit;
                }
                if (Position == line.To)
                {
                    if (line.Position == From)
                        return Symbols.LowerLeftCorner;
                    if (line.Position == To)
                        return Symbols.LowerRightCorner;
                    if (IsSingle)
                        return Symbols.SingleLowerSplit;
                    return Symbols.LowerSplit;
                }
            }

            return Symbols.SingleIntersection;
        }

        public override bool Equals(object obj)
        {
            var l = (UIBorderLine)obj;
            return l.From == From && l.To == To && l.Position == Position && l.IsVertical == IsVertical && l.IsSingle == IsSingle;
        }

        public override int GetHashCode()
        {
            return To * From * Position * (IsVertical ? -1 : 1);
        }
    }
}
