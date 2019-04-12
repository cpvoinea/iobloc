namespace iobloc
{
    class PaintPane : Pane<PaintCell>
    {
        public PaintPane(int rows, int cols) : base(0, 0, rows - 1, cols - 1) { }
    }
}
