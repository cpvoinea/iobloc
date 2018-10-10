using System;

namespace iobloc
{
    class InvadersBoard : IBoard
    {
        const int W = Settings.Invaders.WIDTH;
        const int H = Settings.Invaders.HEIGHT;
        const int A = Settings.Invaders.ALIEN_WIDTH + Settings.Invaders.ALIEN_SPACE;
        public string[] Help => Settings.Invaders.HELP;
        public ConsoleKey[] Keys => Settings.Invaders.KEYS;
        public bool Won => Score == Settings.Invaders.ALIEN_ROWS * Settings.Invaders.ALIEN_COLS;
        public int StepInterval { get; private set; } = Settings.Game.LevelInterval * Settings.Invaders.INTERVALS;
        public BoardFrame Frame { get; private set; } = new BoardFrame(W + 2, H + 2);
        public int[] Clip { get; private set; } = new[] { 0, 0, W, H };
        public int Score { get; private set; }
        public int[,] Grid
        {
            get
            {
                var result = _grid.Copy(H, W);
                for (int i = -1; i <= 1; i++)
                    result[H - 1, _ship + i] = Settings.Game.COLOR_PLAYER;
                result[_bulletRow, _bulletCol] = Settings.Game.COLOR_NEUTRAL;
                return result;
            }
        }

        readonly int[,] _grid;
        int _ship = W / 2 - 1;
        int _bulletCol = Settings.Invaders.WIDTH / 2 - 1;
        int _bulletRow = H - 2;
        int _skipFrame = Settings.Invaders.BULLET_SPEED;
        bool _shot = false;
        bool _movingRight = true;

        internal InvadersBoard()
        {
            _grid = new int[H, W];
            for (int row = 0; row < Settings.Invaders.ALIEN_ROWS; row++)
                for (int col = 0; col < Settings.Invaders.ALIEN_COLS * A; col += A)
                    for (int i = 0; i < Settings.Invaders.ALIEN_WIDTH; i++)
                        _grid[row, col + i] = Settings.Game.COLOR_ENEMY;
        }

        public bool Action(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (_ship > 1)
                    {
                        _ship--;
                        if (!_shot)
                            _bulletCol--;
                        Clip = new[] { 0, H - 2, W, H };
                        return true;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (_ship < W - 2)
                    {
                        _ship++;
                        if (!_shot)
                            _bulletCol++;
                        Clip = new[] { 0, H - 1, W, H };
                        return true;
                    }
                    break;
                case ConsoleKey.Spacebar:
                    {
                        _shot = true;
                        return false;
                    }
            }

            return false;
        }

        public bool Step()
        {
            if (Won)
                return false;

            // lost
            for (int i = 0; i < W; i++)
                if (_grid[H - 1, i] > 0)
                    return false;

            if (_skipFrame <= 0)
            {
                _skipFrame = Settings.Invaders.BULLET_SPEED;
                // animate invaders
                for (int i = 0; i < H; i++)
                    if (_movingRight && _grid[i, W - 1] > 0 ||
                     !_movingRight && _grid[i, 0] > 0) // switch sides
                    {
                        for (int k = H - 1; k >= 0; k--)
                            for (int j = 0; j < W; j++)
                                _grid[k, j] = k == 0 ? 0 : _grid[k - 1, j];

                        _movingRight = !_movingRight;
                        break;
                    }
                if (_movingRight)
                    for (int i = 0; i < H; i++)
                        for (int j = W - 1; j >= 0; j--)
                            _grid[i, j] = j == 0 ? 0 : _grid[i, j - 1];
                else
                    for (int i = 0; i < H; i++)
                        for (int j = 0; j < W; j++)
                            _grid[i, j] = j == W - 1 ? 0 : _grid[i, j + 1];
            }

            _skipFrame--;
            if (_shot)
            {
                // animate bullet
                if (_bulletRow <= 0)
                {
                    _grid[_bulletRow, _bulletCol] = 0;
                    _shot = false;
                    _bulletRow = H - 2;
                    _bulletCol = _ship;
                }
                else
                {
                    _bulletRow--;
                    // hit
                    if (_grid[_bulletRow, _bulletCol] > 0)
                    {
                        int c = _bulletCol;
                        do
                            _grid[_bulletRow, _bulletCol++] = 0;
                        while (_bulletCol < W && _grid[_bulletRow, _bulletCol] > 0);
                        while (c > 0 && _grid[_bulletRow, --c] > 0)
                            _grid[_bulletRow, c] = 0;

                        Score++;
                        _shot = false;
                        _bulletRow = H - 2;
                        _bulletCol = _ship;
                    }
                }
            }

            Clip = new[] { 0, 0, W, H };
            return true;
        }

        public override string ToString()
        {
            return "Invaders";
        }
    }
}