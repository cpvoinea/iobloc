namespace iobloc
{
    struct BorderLine
    {
        internal int From { get; private set; }
        internal int To { get; private set; }
        internal int Position { get; private set; }
        internal bool IsVertical { get; private set; }
        internal bool IsSingle { get; private set; }

        internal BorderLine(int from, int to, int position, bool isVertical, bool isSingle)
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

        internal int GetIntersectionSymbol(BorderLine line)
        {
            if (IsVertical == line.IsVertical || Position < line.From || Position > line.To || line.Position < From || line.Position > To)
                return BoxGraphics.None;
            if (IsVertical)
            {
                if (Position == line.From)
                {
                    if (line.Position == From)
                        return BoxGraphics.UpperLeftCorner;
                    if (line.Position == To)
                        return BoxGraphics.LowerLeftCorner;
                    if (IsSingle)
                        return BoxGraphics.SingleLeftSplit;
                    return BoxGraphics.LeftSplit;
                }
                if (Position == line.To)
                {
                    if (line.Position == From)
                        return BoxGraphics.UpperRightCorner;
                    if (line.Position == To)
                        return BoxGraphics.LowerRightCorner;
                    if (IsSingle)
                        return BoxGraphics.SingleRightSplit;
                    return BoxGraphics.RightSplit;
                }
            }
            else
            {
                if (Position == line.From)
                {
                    if (line.Position == From)
                        return BoxGraphics.UpperLeftCorner;
                    if (line.Position == To)
                        return BoxGraphics.UpperRightCorner;
                    if (IsSingle)
                        return BoxGraphics.SingleUppperSplit;
                    return BoxGraphics.UpperSplit;
                }
                if (Position == line.To)
                {
                    if (line.Position == From)
                        return BoxGraphics.LowerLeftCorner;
                    if (line.Position == To)
                        return BoxGraphics.LowerRightCorner;
                    if (IsSingle)
                        return BoxGraphics.SingleLowerSplit;
                    return BoxGraphics.LowerSplit;
                }
            }

            return BoxGraphics.SingleIntersection;
        }

        public override bool Equals(object obj)
        {
            var l = (BorderLine)obj;
            return l.From == From && l.To == To && l.Position == Position && l.IsVertical == IsVertical;
        }
 
        public override int GetHashCode()
        {
            return To * From * Position * (IsVertical ? -1 : 1);
        }
   }
}