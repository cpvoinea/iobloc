namespace iobloc
{
    class TableLine
    {
        int PW => TableModel.PW;
        int PC => TableModel.PC;
        int EC => TableModel.EC;
        int NC => TableModel.NC;

        private UIPanel _panel;
        private int _col;
        private int _row;
        private int _dir = 1;

        public bool? IsWhite { get; private set; }
        public int Count { get; private set; }
        public bool IsSelected { get; private set; }

        public TableLine(UIPanel panel, int col, int row = 0, bool isLower = false)
        {
            _panel = panel;
            _col = col;
            _row = row;
            if (isLower)
                _dir = -1;
        }

        public void Clear(int val = 0)
        {
            for (int i = 1; i < _panel.Height; i++)
                for (int j = 0; j < PW; j++)
                    _panel[i, _col * PW + j] = val;
            if (val == 0)
            {
                Count = 0;
                IsWhite = null;
            }
            _panel.Change(true);
        }

        public void Set(int count, bool isWhite)
        {
            for (int i = 1; i <= count; i++)
                for (int j = 0; j < PW; j++)
                    _panel[_row + i * _dir, _col * PW + j] = isWhite ? PC : EC;
            _panel.Change(true);

            Count = count;
            if (count > 0)
                IsWhite = isWhite;
            else
                IsWhite = null;
        }

        public void Select(bool set = false)
        {
            for (int j = 0; j < PW; j++)
                _panel[_row, _col * PW + j] = set ? NC : 0;
            _panel.Change(true);

            IsSelected = set;
        }

        public void Pick()
        {
            for (int j = 0; j < PW; j++)
                _panel[_row + (Count + 1) * _dir, _col * PW + j] = 0;
            _panel.Change(true);

            Count--;
            if (Count == 0)
                IsWhite = null;
        }

        public void Put(bool isWhite)
        {
            for (int j = 0; j < PW; j++)
                _panel[_row + (Count + 2) * _dir, _col * PW + j] = 0;
            _panel.Change(true);

            Count++;
            IsWhite = isWhite;
        }
    }
}
