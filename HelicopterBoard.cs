using System;

namespace iobloc
{
    /// <summary>
    /// Helicopter game, reuses Endless runner board, changes the Jump mechanic and adds new obstacles from the top of the screen
    /// </summary>
    class HelicopterBoard : RunnerBoard
    {
        /// <summary>
        /// Helicopter vertical speed: +1 if upwards, 0 if hanging in the air, -1 if downwards
        /// </summary>
        int _speed;

        /// <summary>
        /// Obstacles + helicopter
        /// </summary>
        /// <value></value>
        public override int[,] Grid
        {
            get
            {
                var result = _grid.Copy(Height, Width);
                if (_distance < Height)
                    result[_distance, 5] = result[_distance, 6] = Settings.Game.ColorPlayer;
                return result;
            }
        }

        /// <summary>
        /// Helicopter game
        /// </summary>
        /// <returns></returns>
        internal HelicopterBoard() : base()
        {
            _speed = -1; // start by falling downwards
        }

        /// <summary>
        /// Lift action is performed
        /// </summary>
        protected override bool Jump()
        {
            if (_distance <= 1) // helicopter is too close to the sky and will burn
            {
                Clear(Settings.Game.ColorEnemy); // kill animation
                _kill = true;
                return true; // return true to draw
            }

            _distance--; // initial lift
            _speed = 1; // upward movement of inertia

            return true; // return true to draw
        }

        /// <summary>
        /// Air movement
        /// </summary>
        protected override void Move()
        {
            if (_speed == 1) // is moving upwards
            {
                _distance--;
                _speed--;
            }
            else if (_speed == 0) // is hanging
                _speed--;
            else // is falling
                _distance++;
        }

        protected override bool Collides()
        {
            return _distance >= Height // crash to the ground
                || _grid[_distance, 5] > 0 // obstacle collides with tail
                || _grid[_distance, 6] > 0; // obstacle collides with cabin
        }

        /// <summary>
        /// Reset the game and the score
        /// </summary>
        protected override void Restart()
        {
            _distance = 0;
            _score = 0;
            _speed = -1;
            Clear(0);
        }

        /// <summary>
        /// Randomly generate obstacles
        /// </summary>
        protected override void CreateFence()
        {
            int p = _random.Next(4); // 0 = no obstacles, 1 = top only, 2 = bottom only, 3 = both
            if (p == 0)
                return;
            if ((p & 1) > 0) // top obstacle
            {
                int c = _random.Next(5);
                for (int i = Height - 1; i >= Height - 1 - c; i--)
                    _grid[i, Width - 1] = Settings.Game.ColorEnemy;
            }
            if ((p & 2) > 0) // bottom obstacle
            {
                int c = _random.Next(4);
                for (int i = 0; i < c; i++)
                    _grid[i, Width - 1] = Settings.Game.ColorEnemy;
            }
        }

        public override string ToString()
        {
            return "Helicopter";
        }
    }
}