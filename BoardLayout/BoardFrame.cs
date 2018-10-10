namespace iobloc
{
    struct BoardFrame
    {
        internal int Width { get; private set; }
        internal int Height { get; private set; }
        internal int[,] EmptyFrame { get; private set; }

        internal BoardFrame(BoardLine[] lines)
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
            EmptyFrame = new int[Height, Width];

            for (int i1 = 0; i1 < lines.Length; i1++)
            {
                var line1 = lines[i1];
                for (int i = line1.From; i <= line1.To; i++)
                    if (line1.IsVertical)
                    {
                        if (EmptyFrame[i, line1.Position] == BoxGraphics.None)
                            EmptyFrame[i, line1.Position] = line1.IsSingle ? BoxGraphics.SingleVerticalLine : BoxGraphics.VerticalLine;
                    }
                    else
                    {
                        if (EmptyFrame[line1.Position, i] == BoxGraphics.None)
                            EmptyFrame[line1.Position, i] = line1.IsSingle ? BoxGraphics.SingleHorizontalLine : BoxGraphics.HorizontalLine;
                    }
                for (int i2 = i1 + 1; i2 < lines.Length; i2++)
                {
                    var line2 = lines[i2];
                    var s = line1.GetIntersectionSymbol(line2);
                    if (s != BoxGraphics.None)
                    {
                        if (line1.IsVertical)
                            EmptyFrame[line2.Position, line1.Position] = s;
                        else
                            EmptyFrame[line1.Position, line2.Position] = s;
                    }
                }
            }
        }
    }
}