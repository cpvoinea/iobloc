namespace iobloc
{
    class SokobanBoard : BaseBoard
    {
        int P => (int)Settings.GetColor("PlayerColor");
        int B => (int)Settings.GetColor("BlockColor");
        int W => (int)Settings.GetColor("WallColor");
        int T => (int)Settings.GetColor("TargetColor");
        int R => (int)Settings.GetColor("TargetBlockColor");
        int H => (int)Settings.GetColor("TargetPlayerColor");
        int BW => (int)Settings.GetInt("BlockWidth");
        int WS => (int)Settings.GetInt("WinScore");

        int _targets = int.MaxValue;
        int _startScore;
        int _row;
        int _col;

        internal SokobanBoard() : base(BoardType.Sokoban) { }

        protected override void InitializeGrid()
        {
            if(_targets < int.MaxValue)
            {
                _startScore = 0;
                Level = Serializer.Level;
            }

            var board = SokobanLevels.Get(Level);
            _targets = 0;
            for (int i = 0; i < Height && i < 6; i++)
                for (int j = 0; j < Width && j / BW < 4; j += BW)
                {
                    int v = board[i, j / BW];
                    SetBlock(i, j, v);
                    if (v == P)
                    {
                        _row = i;
                        _col = j;
                    }
                    else if (v == T)
                        _targets++;
                }

            Score = _startScore;

            base.InitializeGrid();
        }

        void SetBlock(int row, int col, int val)
        {
            for (int i = col; i < col + BW; i++)
                Main[row, i] = val;
        }

        public override void HandleInput(string key)
        {
            if (key == "R")
            {
                InitializeGrid();
                return;
            }

            int h = 0;
            int v = 0;
            switch (key)
            {
                case "LeftArrow": h = -BW; break;
                case "RightArrow": h = BW; break;
                case "UpArrow": v = -1; break;
                case "DownArrow": v = 1; break;
            }

            if (_row + v < 0 || _row + v >= Height || _col + h < 0 || _col + h >= Width)
                return;
            int next = Main[_row + v, _col + h];
            if (next == W)
                return;

            if (next == 0 || next == T)
            {
                SetBlock(_row, _col, Main[_row, _col] == H ? T : 0);
                _row += v;
                _col += h;
                SetBlock(_row, _col, Main[_row, _col] == T ? H : P);

                Score--;
                Main.HasChanges = true;
            }
            else if (next == B || next == R)
            {
                if (_row + 2 * v < 0 || _row + 2 * v >= Height || _col + 2 * h < 0 || _col + 2 * h >= Width)
                    return;
                int second = Main[_row + 2 * v, _col + 2 * h];
                if (second == W || second == B || second == R)
                    return;

                if (second == 0 || second == T)
                {
                    SetBlock(_row, _col, Main[_row, _col] == H ? T : 0);
                    _row += v;
                    _col += h;
                    if (Main[_row, _col] == R)
                    {
                        _targets++;
                        SetBlock(_row, _col, H);
                    }
                    else
                        SetBlock(_row, _col, P);

                    if (Main[_row + v, _col + h] == T)
                    {
                        SetBlock(_row + v, _col + h, R);
                        _targets--;
                    }
                    else
                        SetBlock(_row + v, _col + h, B);

                    Score--;
                    Main.HasChanges = true;
                }
            }
        }

        public override void NextFrame()
        {
            if (_targets > 0)
                return;

            Score += WS;
            Level++;
            if (IsRunning)
            {
                _startScore = Score;
                InitializeGrid();
            }
        }
    }
}
