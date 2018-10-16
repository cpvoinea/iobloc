using System;

namespace iobloc
{
    class SokobanBoard : SinglePanelBoard
    {
        int P => (int)_config.GetColor("PlayerColor");
        int B => (int)_config.GetColor("BlockColor");
        int W => (int)_config.GetColor("WallColor");
        int T => (int)_config.GetColor("TargetColor");
        int R => (int)_config.GetColor("TargetBlockColor");
        int H => (int)_config.GetColor("TargetPlayerColor");
        int BW => (int)_config.GetInt("BlockWidth");
        int WS => (int)_config.GetInt("WinScore");
        int L
        {
            get { return Config.Level; }
            set { Config.Level = value; }
        }
        public override bool Won { get; set; }
        public override int[,] Grid { get { return _grid; } }

        readonly int[,] _grid;
        int _startScore = 0;
        int _targets = 100;
        int _row;
        int _col;

        internal SokobanBoard() : base(Option.Sokoban)
        {
            _grid = new int[Height, Width];
            if (L > SokobanLevels.Count)
                L = SokobanLevels.Count - 1;
            InitializeLevel();
        }

        void SetBlock(int row, int col, int val)
        {
            for (int i = col; i < col + BW; i++)
                Grid[row, i] = val;
        }

        void InitializeLevel()
        {
            var board = SokobanLevels.Get(L);
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
        }

        public override bool Action(ConsoleKey key)
        {
            if (key == ConsoleKey.R)
            {
                InitializeLevel();
                Score = _startScore;
                return true;
            }
            else
            {
                int h = 0;
                int v = 0;
                switch (key)
                {
                    case ConsoleKey.LeftArrow: h = -BW; break;
                    case ConsoleKey.RightArrow: h = BW; break;
                    case ConsoleKey.UpArrow: v = -1; break;
                    case ConsoleKey.DownArrow: v = 1; break;
                }
                if (_row + v < 0 || _row + v >= Height || _col + h < 0 || _col + h >= Width)
                    return false;
                int next = Grid[_row + v, _col + h];
                if (next == W)
                    return false;
                if (next == 0 || next == T)
                {
                    SetBlock(_row, _col, Grid[_row, _col] == H ? T : 0);
                    _row += v;
                    _col += h;
                    SetBlock(_row, _col, Grid[_row, _col] == T ? H : P);
                    Score--;
                    return true;
                }
                if (next == B || next == R)
                {
                    if (_row + 2 * v < 0 || _row + 2 * v >= Height || _col + 2 * h < 0 || _col + 2 * h >= Width)
                        return false;
                    int second = Grid[_row + 2 * v, _col + 2 * h];
                    if (second == W || second == B || second == R)
                        return false;
                    if (second == 0 || second == T)
                    {
                        SetBlock(_row, _col, Grid[_row, _col] == H ? T : 0);
                        _row += v;
                        _col += h;
                        if (Grid[_row, _col] == R)
                        {
                            _targets++;
                            SetBlock(_row, _col, H);
                        }
                        else
                            SetBlock(_row, _col, P);
                        Score--;
                        if (Grid[_row + v, _col + h] == T)
                        {
                            SetBlock(_row + v, _col + h, R);
                            _targets--;
                            if (_targets == 0)
                            {
                                Score += WS;
                                Won = true;
                                L++;
                            }
                            return true;
                        }
                        else
                        {
                            SetBlock(_row + v, _col + h, B);
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public override bool Step()
        {
            if (Won)
            {
                if (L >= SokobanLevels.Count)
                    return false;
                Won = false;
                InitializeLevel();
                _startScore = Score;
                return true;
            }
            else
                return true;
        }
    }
}