namespace iobloc
{
    struct PaintCell
    {
        public int Color { get; set; }
        public PaintShape Shape { get; set; }
        public bool IsSelected { get; set; }

        public void Select(bool selected = true)
        {
            IsSelected = selected;
        }
    }
}
