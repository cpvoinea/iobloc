namespace iobloc
{
    class SokobanBoard : BaseBoard
    {
        int P, B, W, T, R, H, WS;
        int _targets = int.MaxValue;
        int _startScore;
        int _row;
        int _col;

        public SokobanBoard() : base(BoardType.Sokoban) { }

        protected override void InitializeSettings()
        {
            base.InitializeSettings();
            P = BoardSettings.GetColor(Settings.PlayerColor);
            B = BoardSettings.GetColor("BlockColor");
            W = BoardSettings.GetColor("WallColor");
            T = BoardSettings.GetColor("TargetColor");
            R = BoardSettings.GetColor("TargetBlockColor");
            H = BoardSettings.GetColor("TargetPlayerColor");
            WS = BoardSettings.GetInt("WinScore");
        }

        // Summary:
        //      Overriden to initialize board level and score on re-start
        public override void Start()
        {
            base.Start();
            Level = Serializer.MasterLevel;
            _startScore = 0;
            ResetLevel();
        }

        public override void HandleInput(string key)
        {
            if (key == "R")
            {
                ResetLevel();
                return;
            }

            int h = 0;
            int v = 0;
            switch (key)
            {
                case UIKey.LeftArrow: h = -BlockWidth; break;
                case UIKey.RightArrow: h = BlockWidth; break;
                case UIKey.UpArrow: v = -1; break;
                case UIKey.DownArrow: v = 1; break;
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
                base.Change(true);
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
                        if (_targets == 0)
                            NextLevel();

                    }
                    else
                        SetBlock(_row + v, _col + h, B);

                    Score--;
                    base.Change(true);
                }
            }
        }

        void ResetLevel()
        {
            var board = SokobanLevels.Get(Level);
            _targets = 0;
            for (int i = 0; i < Height && i < 6; i++)
                for (int j = 0; j < Width && j / BlockWidth < 4; j += BlockWidth)
                {
                    int v = board[i, j / BlockWidth];
                    SetBlock(i, j, v);
                    if (v == P)
                    {
                        _row = i;
                        _col = j;
                    }
                    else if (v == T)
                        _targets++;
                }

            base.Change(true);
            Score = _startScore;
        }

        void NextLevel()
        {
            Score += WS;
            if (Level == SokobanLevels.Count - 1) // no more levels
                Win(true); // exit to win animation
            else
            {
                Level++;
                _startScore = Score; // keep score for restart
                ResetLevel();
            }
        }

        void SetBlock(int row, int col, int val)
        {
            for (int i = 0; i < BlockWidth; i++)
                Main[row, col + i] = val;
        }
    }
}
