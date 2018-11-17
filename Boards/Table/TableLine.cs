namespace iobloc
{
    struct TableLine
    {
        private readonly int _startCol;
        private readonly int _startRow;
        private readonly int _direction;
        private readonly bool? _isDark;
        private bool _setHighlight;
        private bool _setBackground;
        private int Background => _isDark.HasValue ? (_isDark.Value ? Table.CD : Table.CL) : 0;
        public Panel Panel { get; private set; }
        public int Count { get; private set; }
        public bool? IsWhite { get; private set; }

        public TableLine(Panel panel, int col, int row, bool isLower, bool? isDark = null)
        {
            _startCol = col;
            _startRow = row;
            _direction = isLower ? -1 : 1;
            _isDark = isDark;
            _setHighlight = false;
            _setBackground = false;
            Count = 0;
            IsWhite = null;

            Panel = panel;
            Initialize(0, null);
        }

        public void Initialize(int count, bool? isWhite)
        {
            Count = count;
            IsWhite = isWhite;
            for (int i = 1; i < Panel.Height; i++)
                if (i <= count)
                    Set(i, isWhite);
                else
                    Set(i);
        }

        public void SetBackground(bool set)
        {
            _setBackground = set;
            Initialize(Count, IsWhite);
            Change();
        }

        public void SetHighlight(bool set)
        {
            _setHighlight = set;
            Set(0, set ? Table.CH : 0);
            Change();
        }

        public void SetCursor(bool set)
        {
            Set(0, set ? Table.CN : (_setHighlight ? Table.CH : 0));
            Change();
        }

        public void Pick()
        {
            Set(Panel.Height - 1, IsWhite);
            Count--;
            if (Count == 0)
                IsWhite = null;
            Change();
        }

        public void Unpick()
        {
            Set(Panel.Height - 1);
            Change();
        }

        public void Put(bool isWhite)
        {
            Count++;
            Set(Count, isWhite);
            IsWhite = isWhite;
            Change();
        }

        private void Set(int row, bool? isWhite)
        {
            int c = isWhite.HasValue ? (isWhite.Value ? Table.CP : Table.CE) : 0;
            Set(row, c);
        }

        private void Set(int row, int val = 0)
        {
            for (int i = 0; i < Table.BW; i++)
                Panel[_startRow + row * _direction, _startCol * Table.B + i] =
                row == 0 || val > 0 ? val : (_setBackground ? Background : 0);
        }

        private void Change()
        {
            Panel.Change();
        }
    }
}
