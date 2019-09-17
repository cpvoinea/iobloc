namespace iobloc
{
    public struct PaneCell
    {
        public int Color { get; set; }
        public bool IsCursor { get; set; }
        public PaneCell(int color, bool isCursor = false)
        {
            Color = color;
            IsCursor = isCursor;
        }
    }
}
