using System;

namespace iobloc
{
    /// <summary>
    /// Breakout game
    /// </summary>
    class BreakoutBoard : IBoard
    {
        #region Settings
        public string[] Help { get { return Settings.Breakout.HELP; } }
        public ConsoleKey[] Keys { get { return Settings.Breakout.KEYS; } }
        public int StepInterval { get { return Settings.Breakout.INTERVAL; } }
        public int Width { get { return Settings.Breakout.WIDTH; } }
        public int Height { get { return Settings.Breakout.HEIGHT; } }
        #endregion

        /// <summary>
        /// Current score is number of broken blocks
        /// </summary>
        int _score;
        /// <summary>
        /// Remaining blocks
        /// </summary>
        readonly int[,] _grid;
        /// <summary>
        /// Pad position from left
        /// </summary>
        int _pad = 5;
        /// <summary>
        /// Ball position from left
        /// </summary>
        int _ballCol = 0;
        /// <summary>
        /// Ball position from top
        /// </summary>
        int _ballRow = 5;
        /// <summary>
        /// Horizontal grid position
        /// </summary>
        double _ballX = 0;
        /// <summary>
        /// Vertical grid position
        /// </summary>
        double _ballY = 5;
        /// <summary>
        /// Current ball direction angle
        /// </summary>
        double _angle = 7 * Math.PI / 4;

        /// <summary>
        /// Current score is number of broken blocks
        /// </summary>
        public int Score { get { return _score; } }

        /// <summary>
        /// Blocks + pad + ball
        /// </summary>
        public int[,] Grid
        {
            get
            {
                var result = _grid.Copy(Height, Width);
                result[Height - 1, _pad - 1] = result[Height - 1, _pad] = result[Height - 1, _pad + 1] = Settings.Game.ColorPlayer;
                result[_ballRow, _ballCol] = Settings.Game.ColorNeutral;
                return result;
            }
        }

        /// <summary>
        /// Breakout game
        /// </summary>
        internal BreakoutBoard()
        {
            _grid = new int[Height, Width];
            for (int row = 0; row < 5; row++) // set 5 rows of blocks
                for (int col = 0; col < Width; col++)
                    _grid[row, col] = Settings.Game.ColorEnemy;
        }

        /// <summary>
        /// Pad movement
        /// </summary>
        /// <param name="key">direction key</param>
        /// <returns>movement success</returns>
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

        /// <summary>
        /// Ball movement
        /// </summary>
        /// <returns>false if ball is missed by pad and game is over</returns>
        public bool Step()
        {
            if (_ballRow >= Height - 1) // game over
                return false;

            // ball hits pad
            if (_ballRow == Height - 2 && Math.Abs(_ballCol - _pad) <= 2 && _angle > Math.PI)
            {
                int x = _ballCol - _pad; // impact location
                switch (x)
                {
                    case -2: _angle = 3 * Math.PI / 4; break;
                    case -1: _angle = Math.Min(9 * Math.PI / 4 - _angle, 3 * Math.PI / 4); break;
                    case 0: _angle = 2 * Math.PI - _angle; break;
                    case 1: _angle = Math.Max(7 * Math.PI / 4 - _angle, Math.PI / 4); break;
                    case 2: _angle = Math.PI / 4; break;
                }
            }
            else if (_ballCol == 0 && (_angle > Math.PI / 2 && _angle < 3 * Math.PI / 2) || // left wall is hit
                _ballCol == Width - 1 && (_angle < Math.PI / 2 || _angle > 3 * Math.PI / 2)) // right wall is hit
                _angle = Math.PI - _angle;
            else if (_ballRow == 0 && (_angle > 0 && _angle < Math.PI)) // ceiling is hit
                _angle = 2 * Math.PI - _angle;

            // move the ball
            _ballX = _ballX + Math.Cos(_angle);
            _ballY = _ballY - Math.Sin(_angle);
            _ballRow = (int)Math.Round(_ballY);
            _ballCol = (int)Math.Round(_ballX);
            if (_grid[_ballRow, _ballCol] > 0) // a block is hit
            {
                int x = (_ballCol / 3) * 3; // block start column
                _grid[_ballRow, x] = _grid[_ballRow, x + 1] = _grid[_ballRow, x + 2] = 0; // remove block
                _angle = 2 * Math.PI - _angle; // ricochet angle
                _score++; // increase score
            }
            
            return true;
        }

        public override string ToString()
        {
            return "Breakout";
        }
    }
}