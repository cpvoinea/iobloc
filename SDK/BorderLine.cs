namespace iobloc
{
    // Define a line by its position and orientation
    public struct BorderLine
    {
        // Start point is row vertical lines or column for horizontal lines
        internal int From { get; private set; }
        // End point is row vertical lines or column for horizontal lines
        internal int To { get; private set; }
        // Line placement is a row for horizontal lines and a column for vertical lines
        internal int Position { get; private set; }
        // Orientation
        internal bool IsVertical { get; private set; }
        // Type, either single or double
        internal bool IsSingle { get; private set; }

        // Summary:
        //      Component of border
        // Parameters: from: starting position - distance from left if horizontal, distance from top if vertical
        // Parameters: to: ending position (inclusive)
        // Parameters: position: cross position - distance from top if horizontal, distance from left if vertical
        // Parameters: isVertical: vertical (true) or horizontal line
        // Parameters: isSingle: single line (true) or double line
        public BorderLine(int from, int to, int position, bool isVertical, bool isSingle = true)
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

        // Summary:
        //      Depending on line type (single/double) and interection type (cross, T, corner) the intersection symbol is different
        // Parameters: line: line to check against
        internal int GetIntersectionSymbol(BorderLine line)
        {
            if (IsVertical == line.IsVertical // parallel lines
                || Position < line.From || Position > line.To || line.Position < From || line.Position > To) // no intersection
                return Symbol.None;
            if (IsVertical)
            {
                if (Position == line.From)
                {
                    if (line.Position == From)
                        return Symbol.UpperLeftCorner;
                    if (line.Position == To)
                        return Symbol.LowerLeftCorner;
                    if (IsSingle)
                        return Symbol.SingleLeftSplit;
                    return Symbol.LeftSplit;
                }
                if (Position == line.To)
                {
                    if (line.Position == From)
                        return Symbol.UpperRightCorner;
                    if (line.Position == To)
                        return Symbol.LowerRightCorner;
                    if (IsSingle)
                        return Symbol.SingleRightSplit;
                    return Symbol.RightSplit;
                }
            }
            else
            {
                if (Position == line.From)
                {
                    if (line.Position == From)
                        return Symbol.UpperLeftCorner;
                    if (line.Position == To)
                        return Symbol.UpperRightCorner;
                    if (IsSingle)
                        return Symbol.SingleUppperSplit;
                    return Symbol.UpperSplit;
                }
                if (Position == line.To)
                {
                    if (line.Position == From)
                        return Symbol.LowerLeftCorner;
                    if (line.Position == To)
                        return Symbol.LowerRightCorner;
                    if (IsSingle)
                        return Symbol.SingleLowerSplit;
                    return Symbol.LowerSplit;
                }
            }

            return Symbol.SingleIntersection;
        }

        public override bool Equals(object obj)
        {
            var l = (BorderLine)obj;
            return l.From == From && l.To == To && l.Position == Position && l.IsVertical == IsVertical && l.IsSingle == IsSingle;
        }

        public override int GetHashCode()
        {
            return To * From * Position * (IsVertical ? -1 : 1);
        }
    }
}
