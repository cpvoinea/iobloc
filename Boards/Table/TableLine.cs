namespace iobloc
{
    class TableLine
    {
        private readonly int _startCol;
        private readonly int _startRow;
        private readonly int _direction;
        private int _picked;
        private bool _highlight;

        public UIPanel Panel { get; private set; }
        public bool IsPlayerWhite { get; private set; }
        public int Count { get; private set; }

        public TableLine(UIPanel panel, int col, int row = 0, bool isLower = false)
        {
            Panel = panel;
            _startCol = col;
            _startRow = row;
            _direction = isLower ? -1 : 1;
        }

        public void Clear()
        {
            for (int i = 0; i < Panel.Height; i++)
                Set(i, 0);
            Count = 0;
            _picked = 0;
        }

        public void ClearSelection()
        {
            Set(0, 0);
        }

        public void Initialize(int count, bool isPlayerWhite)
        {
            var c = isPlayerWhite ? TableBoard.CP : TableBoard.CE;
            for (int i = 1; i < Panel.Height; i++)
                Set(i, i <= count ? c : 0);

            Count = count;
            IsPlayerWhite = isPlayerWhite;
        }

        public void Select(bool set, bool hl = false)
        {
            if (set)
            {
                Set(0, hl ? TableBoard.CH : TableBoard.CN);
                if (hl)
                    _highlight = true;
            }
            else
            {
                Set(0, _highlight && !hl ? TableBoard.CH : 0);
            }
            Change();
        }

        public void Pick()
        {
            Take();
            _picked++;
            Set(Panel.Height - _picked, IsPlayerWhite ? TableBoard.CP : TableBoard.CE);
            Change();
        }

        public void Unpick()
        {
            Set(Panel.Height - _picked, 0);
            _picked--;
            Change();
        }

        public void Put(bool isPlayerWhite)
        {
            Count++;
            Set(Count, isPlayerWhite ? TableBoard.CP : TableBoard.CE);
            IsPlayerWhite = isPlayerWhite;
            Change();
        }

        public void Take()
        {
            Set(Count, 0);
            Count--;
            Change();
        }

        private void Set(int row, int val)
        {
            for (int i = 0; i < TableBoard.BW; i++)
                Panel[_startRow + row * _direction, _startCol * TableBoard.BW + i] = val;
        }

        private void Change()
        {
            Panel.Change(true);
        }
    }
}
