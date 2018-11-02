namespace iobloc
{
    class SokobanBoard : BaseBoard
    {
        int P => BoardSettings.GetColor(Settings.PlayerColor);
        int B => BoardSettings.GetColor("BlockColor");
        int W => BoardSettings.GetColor("WallColor");
        int T => BoardSettings.GetColor("TargetColor");
        int R => BoardSettings.GetColor("TargetBlockColor");
        int H => BoardSettings.GetColor("TargetPlayerColor");
        int BW => BoardSettings.GetInt("BlockWidth");
        int WS => BoardSettings.GetInt("WinScore");

        int _targets = int.MaxValue;
        int _startScore;
        int _row;
        int _col;

        public SokobanBoard() : base(BoardType.Sokoban) { }

        protected override void Initialize()
        {
            base.Initialize();

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
            Main.HasChanges = true;
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
                Initialize();
                return;
            }

            int h = 0;
            int v = 0;
            switch (key)
            {
                case UIKeys.LeftArrow: h = -BW; break;
                case UIKeys.RightArrow: h = BW; break;
                case UIKeys.UpArrow: v = -1; break;
                case UIKeys.DownArrow: v = 1; break;
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
        }
    }
}
