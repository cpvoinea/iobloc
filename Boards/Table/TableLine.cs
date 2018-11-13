namespace iobloc
{
    class TableLine
    {
        int BW => TableBoard.BW;
        int B => TableBoard.B;
        int CP => TableBoard.CP;
        int CE => TableBoard.CE;
        int CN => TableBoard.CN;
        int CH => TableBoard.CH;
        private readonly int _startCol;
        private readonly int _startRow;
        private readonly int _direction;
        private int _picked;
        private bool _isHighlight;

        public UIPanel Panel { get; private set; }
        public PlayerSide? Player { get; private set; }
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
            Player = null;
            Count = 0;
            _picked = 0;
            _isHighlight = false;
            Change();
        }

        public void ClearSelection()
        {
            Set(0, 0);
            _isHighlight = false;
            Change();
        }

        public bool CanPut(PlayerSide player)
        {
            return Count <= 1 || Player == player;
        }

        public bool HasAny(PlayerSide player)
        {
            return Player == player && Count > 0;
        }

        public void Initialize(int count, PlayerSide player)
        {
            var c = player == PlayerSide.White ? CP : CE;
            for (int i = 1; i < Panel.Height; i++)
                Set(i, i <= count ? c : 0);

            Player = player;
            Count = count;
            _picked = 0;
            _isHighlight = false;
        }

        public void Select(bool set, bool highlight = false)
        {
            if (set)
            {
                Set(0, highlight ? CH : CN);
                if (highlight)
                    _isHighlight = true;
            }
            else
            {
                Set(0, _isHighlight && !highlight ? CH : 0);
            }
            Change();
        }

        public void Pick()
        {
            if (!Player.HasValue)
                return;
            int c = Player == PlayerSide.White ? CP : CE;
            Take();
            _picked++;
            Set(Count + _picked, c);
            Change();
        }

        public void Take()
        {
            Set(Count, 0);
            Count--;
            if (Count == 0)
                Player = null;
            Change();
        }

        public void Unpick()
        {
            if (_picked == 0)
                return;
            Set(Count + _picked, 0);
            _picked--;
            Change();
        }

        public void Put(PlayerSide player)
        {
            int c = player == PlayerSide.White ? CP : CE;
            Count++;
            Set(Count, c);
            Player = player;
            Change();
        }

        private void Set(int row, int val)
        {
            for (int i = 0; i < BW; i++)
                Panel[_startRow + row * _direction, _startCol * B + i] = val;
        }

        private void Change()
        {
            Panel.HasChanges = true;
        }
    }
}
