namespace iobloc
{
    // Define a line by its position and orientation
    public struct UIBorderLine
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
        // Param: from: starting position - distance from left if horizontal, distance from top if vertical
        // Param: to: ending position (inclusive)
        // Param: position: cross position - distance from top if horizontal, distance from left if vertical
        // Param: isVertical: vertical (true) or horizontal line
        // Param: isSingle: single line (true) or double line
        public UIBorderLine(int from, int to, int position, bool isVertical, bool isSingle = true)
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
        // Param: line: line to check against
        internal int GetIntersectionSymbol(UIBorderLine line)
        {
            if (IsVertical == line.IsVertical // parallel lines
                || Position < line.From || Position > line.To || line.Position < From || line.Position > To) // no intersection
                return UISymbol.None;
            if (IsVertical)
            {
                if (Position == line.From)
                {
                    if (line.Position == From)
                        return UISymbol.UpperLeftCorner;
                    if (line.Position == To)
                        return UISymbol.LowerLeftCorner;
                    if (IsSingle)
                        return UISymbol.SingleLeftSplit;
                    return UISymbol.LeftSplit;
                }
                if (Position == line.To)
                {
                    if (line.Position == From)
                        return UISymbol.UpperRightCorner;
                    if (line.Position == To)
                        return UISymbol.LowerRightCorner;
                    if (IsSingle)
                        return UISymbol.SingleRightSplit;
                    return UISymbol.RightSplit;
                }
            }
            else
            {
                if (Position == line.From)
                {
                    if (line.Position == From)
                        return UISymbol.UpperLeftCorner;
                    if (line.Position == To)
                        return UISymbol.UpperRightCorner;
                    if (IsSingle)
                        return UISymbol.SingleUppperSplit;
                    return UISymbol.UpperSplit;
                }
                if (Position == line.To)
                {
                    if (line.Position == From)
                        return UISymbol.LowerLeftCorner;
                    if (line.Position == To)
                        return UISymbol.LowerRightCorner;
                    if (IsSingle)
                        return UISymbol.SingleLowerSplit;
                    return UISymbol.LowerSplit;
                }
            }

            return UISymbol.SingleIntersection;
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
