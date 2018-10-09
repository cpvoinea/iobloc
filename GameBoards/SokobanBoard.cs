using System;

namespace iobloc
{
    class SokobanBoard : IBoard
    {
        #region Settings
        public string[] Help => Settings.Sokoban.HELP;
        public ConsoleKey[] Keys => Settings.Sokoban.KEYS;
        public int StepInterval => 1;
        public int Width => Settings.Sokoban.WIDTH;
        public int Height => Settings.Sokoban.HEIGHT;
        public int[] Clip => new[] { 0, 0, Settings.Sokoban.WIDTH, Settings.Sokoban.HEIGHT };

        const int BW = Settings.Sokoban.BLOCK_WIDTH;
        const int P = Settings.Sokoban.MARK_PLAYER;
        const int B = Settings.Sokoban.MARK_BLOCK;
        const int W = Settings.Sokoban.MARK_WALL;
        const int T = Settings.Sokoban.MARK_TARGET;
        const int R = Settings.Sokoban.MARK_TARGET_BLOCK;
        const int H = Settings.Sokoban.MARK_TARGET_PLAYER;
        readonly int[][,] Levels = new[]
        {
            new[,] {
                {W, W, W, W},
                {W, P, 0, W},
                {W, B, 0, W},
                {W, 0, B, W},
                {W, R, 0, W},
                {W, T, T, W}
            }
        };
        #endregion

        readonly int[,] _grid = new int[Settings.Sokoban.HEIGHT, Settings.Sokoban.WIDTH];
        int _score = 0;
        bool _won = false;
        int _level = 0;
        int _targets = 100;
        int _row;
        int _col;

        public int[,] Grid { get { return _grid; } }
        public int Score { get { return _score; } }
        public bool Won { get { return _won; } }
        internal SokobanBoard()
        {
            _level = Settings.Game.Level;
            if (_level > Levels.Length)
                _level = Levels.Length - 1;
            InitializeLevel();
        }

        public bool Step()
        {
            if (_won)
            {
                if (_level >= Levels.Length)
                    return false;
                InitializeLevel();
                return true;
            }
            else
                return true;
        }

        void SetBlock(int row, int col, int val)
        {
            for (int i = col; i < col + BW; i++)
                _grid[row, i] = val;
        }

        void InitializeLevel()
        {
            var board = Levels[_level];
            _targets = 0;
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j += BW)
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

        public bool Action(ConsoleKey key)
        {
            if (key == ConsoleKey.R)
            {
                InitializeLevel();
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
                int next = _grid[_row + v, _col + h];
                if (next == W)
                    return false;
                if (next == 0 || next == T)
                {
                    SetBlock(_row, _col, _grid[_row, _col] == H ? T : 0);
                    _row += v;
                    _col += h;
                    SetBlock(_row, _col, _grid[_row, _col] == T ? H : P);
                    _score--;
                    return true;
                }
                if (next == B || next == R)
                {
                    if (_row + 2 * v < 0 || _row + 2 * v >= Height || _col + 2 * h < 0 || _col + 2 * h >= Width)
                        return false;
                    int second = _grid[_row + 2 * v, _col + 2 * h];
                    if (second == W || second == B || second == R)
                        return false;
                    if (second == 0 || second == T)
                    {
                        SetBlock(_row, _col, _grid[_row, _col] == H ? T : 0);
                        _row += v;
                        _col += h;
                        if (_grid[_row, _col] == R)
                        {
                            _targets++;
                            SetBlock(_row, _col, H);
                        }
                        else
                            SetBlock(_row, _col, P);
                        _score--;
                        if (_grid[_row + v, _col + h] == T)
                        {
                            SetBlock(_row + v, _col + h, R);
                            _targets--;
                            if (_targets == 0)
                            {
                                _score += Settings.Sokoban.LEVEL_SCORE;
                                _won = true;
                                _level++;
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

        public override string ToString()
        {
            return "Sokoban";
        }
    }
}