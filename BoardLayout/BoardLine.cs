namespace iobloc
{
    struct BoardLine
    {
        internal int From { get; private set; }
        internal int To { get; private set; }
        internal int Position { get; private set; }
        internal bool IsVertical { get; private set; }
        internal bool IsSingle { get; private set; }

        internal BoardLine(int from, int to, int position, bool isVertical, bool isSingle)
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

        internal BoardSymbol GetIntersection(BoardLine line)
        {
            if (IsVertical == line.IsVertical || Position < line.From || Position > line.To || line.Position < From || line.Position > To)
                return BoardSymbol.None;
            if (IsVertical)
            {
                if (Position == line.From)
                {
                    if (line.Position == From)
                        return BoardSymbol.UpperLeftCorner;
                    if (line.Position == To)
                        return BoardSymbol.LowerLeftCorner;
                    if (IsSingle)
                        return BoardSymbol.SingleLeftSplit;
                    return BoardSymbol.LeftSplit;
                }
                if (Position == line.To)
                {
                    if (line.Position == From)
                        return BoardSymbol.UpperRightCorner;
                    if (line.Position == To)
                        return BoardSymbol.LowerRightCorner;
                    if (IsSingle)
                        return BoardSymbol.SingleRightSplit;
                    return BoardSymbol.RightSplit;
                }
            }
            else
            {
                if (Position == line.From)
                {
                    if (line.Position == From)
                        return BoardSymbol.UpperLeftCorner;
                    if (line.Position == To)
                        return BoardSymbol.UpperRightCorner;
                    if (IsSingle)
                        return BoardSymbol.SingleUppperSplit;
                    return BoardSymbol.UpperSplit;
                }
                if (Position == line.To)
                {
                    if (line.Position == From)
                        return BoardSymbol.LowerLeftCorner;
                    if (line.Position == To)
                        return BoardSymbol.LowerRightCorner;
                    if (IsSingle)
                        return BoardSymbol.SingleLowerSplit;
                    return BoardSymbol.LowerSplit;
                }
            }

            return BoardSymbol.SingleIntersection;
        }

        public override bool Equals(object obj)
        {
            var l = (BoardLine)obj;
            return l.From == From && l.To == To && l.Position == Position && l.IsVertical == IsVertical;
        }
 
        public override int GetHashCode()
        {
            return To * From * Position * (IsVertical ? -1 : 1);
        }
   }
}