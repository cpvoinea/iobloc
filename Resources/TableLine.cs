namespace iobloc
{
    class TableLine
    {
        private Panel Panel { get; set; }
        private int BlockWidth { get; set; }
        private int Block { get; set; }
        private int StartCol { get; set; }
        private int StartRow { get; set; }
        private int Direction { get; set; }
        public int Count { get; private set; }
        public bool IsWhite { get; private set; }
        public bool IsMarked { get; private set; }

        public TableLine(Panel panel, int blockWidth, int block, int col, int row, bool isLower)
        {
            Panel = panel;
            BlockWidth = blockWidth;
            Block = block;
            StartCol = col;
            StartRow = row;
            Direction = isLower ? -1 : 1;
            Count = 0;
            IsWhite = false;
            IsMarked = false;
        }

        public void Initialize()
        {
            Count = 0;
            IsMarked = false;
            for (int i = 1; i < Panel.Height; i++)
                Set(i, 0);
        }

        public void Initialize(int count, bool isWhite, int color)
        {
            Count = count;
            IsWhite = isWhite;
            for (int i = 1; i <= count; i++)
                Set(i, color);
        }

        public void Take(int backColor)
        {
            Set(Count, backColor);
            Count--;
        }

        public void Put(bool isWhite, int color)
        {
            Count++;
            IsWhite = isWhite;
            Set(Count, color);
        }

        public void Mark(bool set)
        {
            IsMarked = set;
        }

        public void Set(int row, int color)
        {
            for (int i = 0; i < BlockWidth; i++)
                Panel[StartRow + row * Direction, StartCol * Block + i] = color;
            Panel.Change();
        }
    }
}
