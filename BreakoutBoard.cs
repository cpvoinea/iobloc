using System;

namespace iobloc
{
    class BreakoutBoard : IBoard
    {
        public string[] Help { get { return Settings.Breakout.HELP; } }
        public ConsoleKey[] Keys { get { return Settings.Breakout.KEYS; } }
        public int StepInterval { get { return Settings.Breakout.INTERVAL; } }
        public int Width { get { return Settings.Breakout.WIDTH; } }
        public int Height { get { return Settings.Breakout.HEIGHT; } }
        public int[,] Grid
        {
            get
            {
                var result = _grid.Copy(Height, Width);
                result[Height - 1, _pad - 1] = result[Height - 1, _pad] = result[Height - 1, _pad + 1] = 1;
                result[_ballRow, _ballCol] = 7;
                return result;
            }
        }
        public int Score { get { return _score; } }

        readonly int[,] _grid;
        int _pad = 5;
        int _ballCol = 0;
        int _ballRow = 5;
        double _ballX = 0;
        double _ballY = 5;
        double _angle = 7 * Math.PI / 4;
        int _score;

        internal BreakoutBoard()
        {
            _grid = new int[Height, Width];
            for (int row = 0; row < 5; row++)
                for (int col = 0; col < Width; col++)
                    _grid[row, col] = 4;
        }

        public bool Action(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (_pad > 1)
                    {
                        _pad--;
                        return true;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (_pad < Width - 2)
                    {
                        _pad++;
                        return true;
                    }
                    break;
            }

            return false;
        }

        public bool Step()
        {
            if (_ballRow >= Height - 1)
                return false;

            if (_ballRow == Height - 2 && Math.Abs(_ballCol - _pad) <= 2 && _angle > Math.PI)
            {
                int x = _ballCol - _pad;
                switch (x)
                {
                    case -2: _angle = 3 * Math.PI / 4; break;
                    case -1: _angle = Math.Min(9 * Math.PI / 4 - _angle, 3 * Math.PI / 4); break;
                    case 0: _angle = 2 * Math.PI - _angle; break;
                    case 1: _angle = Math.Max(7 * Math.PI / 4 - _angle, Math.PI / 4); break;
                    case 2: _angle = Math.PI / 4; break;
                }
            }
            else if (_ballCol == 0 && (_angle > Math.PI / 2 && _angle < 3 * Math.PI / 2) ||
                _ballCol == Width - 1 && (_angle < Math.PI / 2 || _angle > 3 * Math.PI / 2))
                _angle = Math.PI - _angle;
            else if (_ballRow == 0 && (_angle > 0 && _angle < Math.PI))
                _angle = 2 * Math.PI - _angle;

            _ballX = _ballX + Math.Cos(_angle);
            _ballY = _ballY - Math.Sin(_angle);
            _ballRow = (int)Math.Round(_ballY);
            _ballCol = (int)Math.Round(_ballX);
            if (_grid[_ballRow, _ballCol] > 0)
            {
                int x = _ballCol / 3;
                x *= 3;
                _grid[_ballRow, x] = _grid[_ballRow, x + 1] = _grid[_ballRow, x + 2] = 0;
                _angle = 2 * Math.PI - _angle;
                _score++;
            }
            
            return true;
        }
    }
}