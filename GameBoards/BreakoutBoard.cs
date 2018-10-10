using System;

namespace iobloc
{
    /// <summary>
    /// Breakout game
    /// </summary>
    class BreakoutBoard : BaseBoard
    {
        int CP => _settings.All.GetInt("PlayerColor");
        int CE => _settings.All.GetInt("EnemyColor");
        int CN => _settings.All.GetInt("NeutralColor");
        int BW => _settings.All.GetInt("BlockWidth");
        int BS => _settings.All.GetInt("BlockSpace");
        int BR => _settings.All.GetInt("BlockRows");
        int B => BW + BS;
        public override bool Won => Score == (Width / B + 1) * BR;
        /// <summary>
        /// Blocks + pad + ball
        /// </summary>
        public override int[,] Grid
        {
            get
            {
                var result = _grid.Copy(Height, Width);
                for (int i = -2; i <= 2; i++)
                    result[Height - 1, _paddle + i] = CP;
                result[_ballRow, _ballCol] = CN;
                return result;
            }
        }

        /// <summary>
        /// Remaining blocks
        /// </summary>
        readonly int[,] _grid;
        /// <summary>
        /// Pad position from left
        /// </summary>
        int _paddle;
        /// <summary>
        /// Ball position from left
        /// </summary>
        int _ballCol;
        /// <summary>
        /// Ball position from top
        /// </summary>
        int _ballRow;
        /// <summary>
        /// Horizontal grid position
        /// </summary>
        double _ballX;
        /// <summary>
        /// Vertical grid position
        /// </summary>
        double _ballY;
        /// <summary>
        /// Current ball direction angle
        /// </summary>
        double _angle;

        /// <summary>
        /// Breakout game
        /// </summary>
        internal BreakoutBoard() : base(GameOption.Breakout)
        {
            _paddle = Width / 2 - 2;
            _ballX = 5;
            _ballY = BR;
            _angle = 7 * Math.PI / 4;
            _grid = new int[Height, Width];
            for (int row = 0; row < BR; row++) // set rows of blocks
                for (int col = 0; col < Width; col += B)
                    for (int i = 0; i < BW; i++)
                        _grid[row, col + i] = CE;
        }

        /// <summary>
        /// Pad movement
        /// </summary>
        /// <param name="key">direction key</param>
        /// <returns>movement success</returns>
        public override bool Action(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (_paddle > 2)
                    {
                        _paddle--;
                        Clip = new[] { 0, Height - 1, Width, Height };
                        return true;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (_paddle < Width - 3)
                    {
                        _paddle++;
                        Clip = new[] { 0, Height - 1, Width, Height };
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
        public override bool Step()
        {
            if (Won)
                return false;
            bool lost;
            _angle = NextAngle(out lost);
            if (lost)
                return false;
            _ballX += Math.Cos(_angle);
            _ballY -= Math.Sin(_angle);
            Clip = new[] {
                _ballCol < 3 ? 0 : _ballCol - 3,
                 _ballRow < 1 ? 0 : _ballRow - 1,
                 _ballCol > Width - 4 ? Width : _ballCol + 4,
                _ballRow > Height - 2 ? Height : _ballRow + 2 };
            _ballRow = (int)Math.Round(_ballY); ;
            _ballCol = (int)Math.Round(_ballX);

            return true;
        }

        double NextAngle(out bool lost)
        {
            lost = false;

            double newAngle = _angle;
            bool hit;
            do
            {
                hit = true;
                double newX = _ballX + Math.Cos(newAngle);
                double newY = _ballY - Math.Sin(newAngle);
                int row = (int)Math.Round(newY);
                int col = (int)Math.Round(newX);

                if (newAngle < Math.PI) // moving upwards
                {
                    if (row < 0) // hitting ceiling
                        newAngle = 2 * Math.PI - newAngle;
                    else if (col < 0 || col >= Width) // hitting walls
                        newAngle = Math.PI - newAngle;
                    else if (_grid[row, col] > 0) // hitting block
                    {
                        Break(row, col);
                        newAngle = 2 * Math.PI - newAngle;
                    }
                    else
                        hit = false;
                }
                else // moving downwards
                {
                    // vertical check
                    if (row >= Height) // hitting floor
                    {
                        lost = true;
                        newAngle = 2 * Math.PI - newAngle;
                    }
                    else if (col < 0 || col >= Width) // hitting walls
                        newAngle = 3 * Math.PI - newAngle;
                    else if (row == Height - 1 && Math.Abs(col - _paddle) <= 2) // hitting paddle
                    {
                        int p = col - _paddle;
                        double a = 2 * Math.PI - newAngle;
                        switch (p)
                        {
                            case -2: newAngle = 3 * Math.PI / 4; break;
                            case -1: newAngle = Math.Min(a + Math.PI / 4, 3 * Math.PI / 4); break;
                            case 0: newAngle = a; break;
                            case 1: newAngle = Math.Max(a - Math.PI / 4, Math.PI / 4); break;
                            case 2: newAngle = Math.PI / 4; break;
                        }
                    }
                    else if (_grid[row, col] > 0) // hitting block
                    {
                        Break(row, col);
                        newAngle = 2 * Math.PI - newAngle;
                    }
                    else
                        hit = false;
                }
            } while (hit);

            return newAngle;
        }

        void Break(int row, int col)
        {
            int x = (col / B) * B;
            for (int i = 0; i < BW; i++)
                _grid[row, x + i] = 0;
            Score++;
        }
    }
}