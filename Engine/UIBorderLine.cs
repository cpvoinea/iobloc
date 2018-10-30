namespace iobloc
{
    struct UIBorderLine
    {
        internal int From { get; private set; }
        internal int To { get; private set; }
        internal int Position { get; private set; }
        internal bool IsVertical { get; private set; }
        internal bool IsSingle { get; private set; }

        internal UIBorderLine(int from, int to, int position, bool isVertical, bool isSingle)
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

        internal int GetIntersectionSymbol(UIBorderLine line)
        {
            if (IsVertical == line.IsVertical || Position < line.From || Position > line.To || line.Position < From || line.Position > To)
                return UISymbolType.None;
            if (IsVertical)
            {
                if (Position == line.From)
                {
                    if (line.Position == From)
                        return UISymbolType.UpperLeftCorner;
                    if (line.Position == To)
                        return UISymbolType.LowerLeftCorner;
                    if (IsSingle)
                        return UISymbolType.SingleLeftSplit;
                    return UISymbolType.LeftSplit;
                }
                if (Position == line.To)
                {
                    if (line.Position == From)
                        return UISymbolType.UpperRightCorner;
                    if (line.Position == To)
                        return UISymbolType.LowerRightCorner;
                    if (IsSingle)
                        return UISymbolType.SingleRightSplit;
                    return UISymbolType.RightSplit;
                }
            }
            else
            {
                if (Position == line.From)
                {
                    if (line.Position == From)
                        return UISymbolType.UpperLeftCorner;
                    if (line.Position == To)
                        return UISymbolType.UpperRightCorner;
                    if (IsSingle)
                        return UISymbolType.SingleUppperSplit;
                    return UISymbolType.UpperSplit;
                }
                if (Position == line.To)
                {
                    if (line.Position == From)
                        return UISymbolType.LowerLeftCorner;
                    if (line.Position == To)
                        return UISymbolType.LowerRightCorner;
                    if (IsSingle)
                        return UISymbolType.SingleLowerSplit;
                    return UISymbolType.LowerSplit;
                }
            }

            return UISymbolType.SingleIntersection;
        }

        public override bool Equals(object obj)
        {
            var l = (UIBorderLine)obj;
            return l.From == From && l.To == To && l.Position == Position && l.IsVertical == IsVertical;
        }

        public override int GetHashCode()
        {
            return To * From * Position * (IsVertical ? -1 : 1);
        }
    }
}
