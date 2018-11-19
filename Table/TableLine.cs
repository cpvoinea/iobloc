namespace iobloc
{
    class TableLine
    {
        private readonly int _bw;
        private readonly int _b;
        private readonly int _startCol;
        private readonly int _startRow;
        private readonly int _direction;
        private int _marking;
        private int _background;
        public Panel Panel { get; private set; }
        public int Count { get; private set; }

        public TableLine(Panel panel, int bw, int b, int col, int row, bool isLower)
        {
            Panel = panel;
            _bw = bw;
            _b = b;
            _startCol = col;
            _startRow = row;
            _direction = isLower ? -1 : 1;
        }

        public void Initialize(int count, int color)
        {
            Count = count;
            for (int i = 1; i <= count; i++)
                Set(i, color);
        }

        public void SetBackground(int background)
        {
            _background = background;
            for (int i = Count + 1; i < Panel.Height; i++)
                Set(i, background);
            Change();
        }

        public void SetMarking(int marking)
        {
            _marking = marking;
            Set(0, marking);
            Change();
        }

        public void Select(int select, int hover)
        {
            Set(0, select == 0 && _marking > 0 ? _marking : select);
            Set(Panel.Height - 1, hover);
            Change();
        }

        public void Take()
        {
            Set(Count, 0);
            Count--;
            Change();
        }

        public void Put(int color)
        {
            Count++;
            Set(Count, color);
            Change();
        }

        private void Set(int row, int val)
        {
            for (int i = 0; i < _bw; i++)
                Panel[_startRow + row * _direction, _startCol * _b + i] =
                row == 0 || val > 0 ? val : _background;
        }

        private void Change()
        {
            Panel.Change();
        }
    }
}
