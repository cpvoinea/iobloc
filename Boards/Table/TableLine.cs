namespace iobloc
{
    struct TableLine
    {
        private readonly int _startCol;
        private readonly int _startRow;
        private readonly int _direction;
        private readonly bool? _isDark;
        private bool _isHighlight;
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
            _isHighlight = false;
            Count = 0;
            IsWhite = null;

            Panel = panel;
            for (int i = 1; i < panel.Height; i++)
                Set(i, Background);
        }

        public void Initialize(int count, bool isWhite)
        {
            Count = count;
            IsWhite = isWhite;
            for (int i = 1; i <= count; i++)
                Set(i, isWhite);
        }

        public void ClearSelect()
        {
            Set(0);
            _isHighlight = false;
            Change();
        }

        public void Select(bool set, bool highlight = false)
        {
            if (set)
            {
                Set(0, highlight ? Table.CH : Table.CN);
                if (highlight)
                    _isHighlight = true;
            }
            else
                Set(0, _isHighlight && !highlight ? Table.CH : 0);
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

        private void Set(int row, bool? isWhite = null)
        {
            int c = isWhite.HasValue ? (isWhite.Value ? Table.CP : Table.CE) : Background;
            Set(row, c);
        }

        private void Set(int row, int val)
        {
            for (int i = 0; i < Table.BW; i++)
                Panel[_startRow + row * _direction, _startCol * Table.B + i] = val;
        }

        private void Change()
        {
            Panel.Change();
        }
    }
}
