using System;

namespace iobloc
{
    /// <summary>
    /// Breakout game
    /// </summary>
    class BreakoutBoard : IBoard
    {
        const int W = Settings.Breakout.WIDTH;
        const int H = Settings.Breakout.HEIGHT;
        const int B = Settings.Breakout.BLOCK_WIDTH + Settings.Breakout.BLOCK_SPACE;
        public string[] Help => Settings.Breakout.HELP;
        public ConsoleKey[] Keys => Settings.Breakout.KEYS;
        public bool Won => Score == (W / B + 1) * Settings.Breakout.BLOCK_ROWS;
        public int StepInterval { get; private set; } = Settings.Game.LevelInterval * Settings.Breakout.INTERVALS;
        public BoardFrame Frame { get; private set; } = new BoardFrame(W + 2, H + 2);
        public int[] Clip { get; private set; } = new[] { 0, 0, W, H };
        public int Score { get; private set; }
        /// <summary>
        /// Blocks + pad + ball
        /// </summary>
        public int[,] Grid
        {
            get
            {
                var result = _grid.Copy(H, W);
                for (int i = -2; i <= 2; i++)
                    result[H - 1, _paddle + i] = Settings.Game.COLOR_PLAYER;
                result[_ballRow, _ballCol] = Settings.Game.COLOR_NEUTRAL;
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
        int _paddle = Settings.Breakout.WIDTH / 2 - 2;
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
        double _ballY = Settings.Breakout.BLOCK_ROWS;
        /// <summary>
        /// Current ball direction angle
        /// </summary>
        double _angle = 7 * Math.PI / 4;

        /// <summary>
        /// Breakout game
        /// </summary>
        internal BreakoutBoard()
        {
            _grid = new int[H, W];
            for (int row = 0; row < Settings.Breakout.BLOCK_ROWS; row++) // set rows of blocks
                for (int col = 0; col < W; col += B)
                    for (int i = 0; i < Settings.Breakout.BLOCK_WIDTH; i++)
                        _grid[row, col + i] = Settings.Game.COLOR_ENEMY;
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
                    if (_paddle > 2)
                    {
                        _paddle--;
                        Clip = new[] { 0, H - 1, W, H };
                        return true;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (_paddle < W - 3)
                    {
                        _paddle++;
                        Clip = new[] { 0, H - 1, W, H };
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
                 _ballCol > W - 4 ? W : _ballCol + 4,
                _ballRow > H - 2 ? H : _ballRow + 2 };
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
                    else if (col < 0 || col >= W) // hitting walls
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
                    if (row >= H) // hitting floor
                    {
                        lost = true;
                        newAngle = 2 * Math.PI - newAngle;
                    }
                    else if (col < 0 || col >= W) // hitting walls
                        newAngle = 3 * Math.PI - newAngle;
                    else if (row == H - 1 && Math.Abs(col - _paddle) <= 2) // hitting paddle
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
            for (int i = 0; i < Settings.Breakout.BLOCK_WIDTH; i++)
                _grid[row, x + i] = 0;
            Score++;
        }

        public override string ToString()
        {
            return "Breakout";
        }
    }
}