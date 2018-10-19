using System;

namespace iobloc
{
    class SokobanBoard : SinglePanelBoard
    {
        int P => (int)_settings.GetColor("PlayerColor");
        int B => (int)_settings.GetColor("BlockColor");
        int W => (int)_settings.GetColor("WallColor");
        int T => (int)_settings.GetColor("TargetColor");
        int R => (int)_settings.GetColor("TargetBlockColor");
        int H => (int)_settings.GetColor("TargetPlayerColor");
        int BW => (int)_settings.GetInt("BlockWidth");
        int WS => (int)_settings.GetInt("WinScore");
        int L
        {
            get { return Config.Level; }
            set { Config.Level = value; }
        }

        public override bool Won => _won;

        bool _won;
        int _startScore = 0;
        int _targets = 100;
        int _row;
        int _col;

        internal SokobanBoard() : base(Option.Sokoban)
        {
            if (L > SokobanLevels.Count)
                L = SokobanLevels.Count - 1;
            InitializeLevel();
        }

        public override void HandleInput(string key)
        {
            if (key == "R")
            {
                InitializeLevel();
                Score = _startScore;
            }
            else
            {
                int h = 0;
                int v = 0;
                switch (key)
                {
                    case "LeftArrow": h = -BW; break;
                    case "RightArrow": h = BW; break;
                    case "UpArrow": v = -1; break;
                    case "DownArrow": v = 1; break;
                }

                if (_row + v < 0 || _row + v >= _height || _col + h < 0 || _col + h >= _width)
                    return;
                int next = _main.Grid[_row + v, _col + h];
                if (next == W)
                    return;
                _main.HasChanges = true;

                if (next == 0 || next == T)
                {
                    SetBlock(_row, _col, _main.Grid[_row, _col] == H ? T : 0);
                    _row += v;
                    _col += h;
                    SetBlock(_row, _col, _main.Grid[_row, _col] == T ? H : P);
                    Score--;
                    return;
                }
                if (next == B || next == R)
                {
                    if (_row + 2 * v < 0 || _row + 2 * v >= _height || _col + 2 * h < 0 || _col + 2 * h >= _width)
                        return;
                    int second = _main.Grid[_row + 2 * v, _col + 2 * h];
                    if (second == W || second == B || second == R)
                        return;
                    if (second == 0 || second == T)
                    {
                        SetBlock(_row, _col, _main.Grid[_row, _col] == H ? T : 0);
                        _row += v;
                        _col += h;
                        if (_main.Grid[_row, _col] == R)
                        {
                            _targets++;
                            SetBlock(_row, _col, H);
                        }
                        else
                            SetBlock(_row, _col, P);
                        Score--;
                        if (_main.Grid[_row + v, _col + h] == T)
                        {
                            SetBlock(_row + v, _col + h, R);
                            _targets--;
                            if (_targets == 0)
                            {
                                Score += WS;
                                _won = true;
                                L++;
                            }
                            return;
                        }
                        else
                        {
                            SetBlock(_row + v, _col + h, B);
                            return;
                        }
                    }
                }
            }
        }

        public override void NextFrame()
        {
            if (Won)
            {
                if (L >= SokobanLevels.Count)
                {
                    IsRunning = false;
                    return;
                }
                _won = false;
                InitializeLevel();
                _startScore = Score;
            }
        }

        void InitializeLevel()
        {
            var board = SokobanLevels.Get(L);
            _targets = 0;
            for (int i = 0; i < _height && i < 6; i++)
                for (int j = 0; j < _width && j / BW < 4; j += BW)
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
            _main.HasChanges = true;
        }

        void SetBlock(int row, int col, int val)
        {
            for (int i = col; i < col + BW; i++)
                _main.Grid[row, i] = val;
        }
    }
}
