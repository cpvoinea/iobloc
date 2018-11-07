namespace iobloc
{
    class TableLine
    {
        private int _startCol;
        private int _startRow;
        private int _direction;
        private int _picked;

        public UIPanel Panel { get; private set; }
        public bool IsPlayerWhite { get; private set; }
        public int Count { get; private set; }

        private void Set(int row, int val)
        {
            for (int i = 0; i < TableBoard.BW; i++)
                Panel[_startRow + row * _direction, _startCol + i] = val;
        }

        public TableLine(UIPanel panel, int col, int row = 0, bool isLower = false)
        {
            Panel = panel;
            _startCol = col;
            _startRow = row;
            _direction = isLower ? -1 : 1;
        }

        public void Clear()
        {
            for (int i = 1; i < Panel.Height; i++)
                Set(i, 0);
            Count = 0;
        }

        public void Set(int count, bool isPlayerWhite)
        {
            var c = isPlayerWhite ? TableBoard.CP : TableBoard.CE;
            for (int i = 1; i <= Panel.Height; i++)
                Set(i, i <= count ? c : 0);

            Count = count;
            IsPlayerWhite = isPlayerWhite;
        }

        public void Select(bool set, bool highlight = false)
        {
            var c = highlight ? TableBoard.CH : TableBoard.CN;
            Set(0, set ? c : 0);
            if (set) Change();
        }

        public void Pick()
        {
            _picked++;
            Set(Panel.Height - _picked, IsPlayerWhite);
            Take();
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

        private void Change()
        {
            Panel.Change(true);
        }
    }
}
