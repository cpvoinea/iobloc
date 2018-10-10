using System;

namespace iobloc
{
    class InvadersBoard : BaseBoard
    {
        const int A = Settings.Invaders.ALIEN_WIDTH + Settings.Invaders.ALIEN_SPACE;
        public override bool Won => Score == Settings.Invaders.ALIEN_ROWS * Settings.Invaders.ALIEN_COLS;
        public override int[,] Grid
        {
            get
            {
                var result = _grid.Copy(Height, Width);
                for (int i = -1; i <= 1; i++)
                    result[Height - 1, _ship + i] = Settings.Invaders.COLOR_PLAYER;
                result[_bulletRow, _bulletCol] = Settings.Invaders.COLOR_NEUTRAL;
                return result;
            }
        }

        readonly int[,] _grid;
        int _ship;
        int _bulletCol;
        int _bulletRow;
        int _skipFrame = Settings.Invaders.BULLET_SPEED;
        bool _shot = false;
        bool _movingRight = true;

        internal InvadersBoard() : base(GameOption.Invaders)
        {
            _grid = new int[Height, Width];
            for (int row = 0; row < Settings.Invaders.ALIEN_ROWS; row++)
                for (int col = 0; col < Settings.Invaders.ALIEN_COLS * A; col += A)
                    for (int i = 0; i < Settings.Invaders.ALIEN_WIDTH; i++)
                        _grid[row, col + i] = Settings.Invaders.COLOR_ENEMY;
            _ship = Width / 2 - 1;
            _bulletCol = Width / 2 - 1;
            _bulletRow = Height - 2;
        }

        public override bool Action(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (_ship > 1)
                    {
                        _ship--;
                        if (!_shot)
                            _bulletCol--;
                        Clip = new[] { 0, Height - 2, Width, Height };
                        return true;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (_ship < Width - 2)
                    {
                        _ship++;
                        if (!_shot)
                            _bulletCol++;
                        Clip = new[] { 0, Height - 1, Width, Height };
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

        public override bool Step()
        {
            if (Won)
                return false;

            // lost
            for (int i = 0; i < Width; i++)
                if (_grid[Height - 1, i] > 0)
                    return false;

            if (_skipFrame <= 0)
            {
                _skipFrame = Settings.Invaders.BULLET_SPEED;
                // animate invaders
                for (int i = 0; i < Height; i++)
                    if (_movingRight && _grid[i, Width - 1] > 0 ||
                     !_movingRight && _grid[i, 0] > 0) // switch sides
                    {
                        for (int k = Height - 1; k >= 0; k--)
                            for (int j = 0; j < Width; j++)
                                _grid[k, j] = k == 0 ? 0 : _grid[k - 1, j];

                        _movingRight = !_movingRight;
                        break;
                    }
                if (_movingRight)
                    for (int i = 0; i < Height; i++)
                        for (int j = Width - 1; j >= 0; j--)
                            _grid[i, j] = j == 0 ? 0 : _grid[i, j - 1];
                else
                    for (int i = 0; i < Height; i++)
                        for (int j = 0; j < Width; j++)
                            _grid[i, j] = j == Width - 1 ? 0 : _grid[i, j + 1];
            }

            _skipFrame--;
            if (_shot)
            {
                // animate bullet
                if (_bulletRow <= 0)
                {
                    _grid[_bulletRow, _bulletCol] = 0;
                    _shot = false;
                    _bulletRow = Height - 2;
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
                        while (_bulletCol < Width && _grid[_bulletRow, _bulletCol] > 0);
                        while (c > 0 && _grid[_bulletRow, --c] > 0)
                            _grid[_bulletRow, c] = 0;

                        Score++;
                        _shot = false;
                        _bulletRow = Height - 2;
                        _bulletCol = _ship;
                    }
                }
            }

            Clip = new[] { 0, 0, Width, Height };
            return true;
        }
    }
}