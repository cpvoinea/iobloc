namespace iobloc
{
    public struct PaneCell
    {
        public int Color { get; set; }
        public bool IsCursor { get; set; }
        public CellShape Shape { get; set; }
        public char Char { get; set; }

        public PaneCell(int color, bool isCursor = false, CellShape shape = CellShape.Block, char ch = '\0')
        {
            Color = color;
            IsCursor = isCursor;
            Shape = shape;
            Char = ch;
        }
    }
}
