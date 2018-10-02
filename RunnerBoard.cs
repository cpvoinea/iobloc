using System;

namespace iobloc
{
    /// <summary>
    /// Endless runner game
    /// </summary>
    class RunnerBoard : IBoard
    {
        #region Settings
        public string[] Help { get { return Settings.Runner.HELP; } }
        public ConsoleKey[] Keys { get { return Settings.Runner.KEYS; } }
        public int StepInterval { get { return Settings.Runner.INTERVAL; } }
        public int Width { get { return Settings.Runner.WIDTH; } }
        public int Height { get { return Settings.Runner.HEIGHT; } }
        #endregion

        /// <summary>
        /// Current score
        /// </summary>
        protected int _score;
        /// <summary>
        /// Used for generating fences
        /// </summary>
        protected readonly Random _random = new Random();
        /// <summary>
        /// Current fences
        /// </summary>
        protected readonly int[,] _grid;
        /// <summary>
        /// Current jump distance
        /// </summary>
        protected int _distance;
        /// <summary>
        /// Max score
        /// </summary>
        protected int _highscore;
        /// <summary>
        /// Check half-frame, only move fences once every 2 frames, so the jump is faster than the speed
        /// </summary>
        protected bool _skipAdvance;
        /// <summary>
        /// Collision detected
        /// </summary>
        protected bool _kill;
        /// <summary>
        /// Hang frame counter, starts from 2, ends at 0
        /// </summary>
        int _hang;
        /// <summary>
        /// Is moving upwards
        /// </summary>
        bool _upwards;
        /// <summary>
        /// Double jump was called during current jump
        /// </summary>
        bool _doubleJump;

        /// <summary>
        /// Highest score
        /// </summary>
        public int Score { get { return _highscore; } }

        /// <summary>
        /// Fences + jumper
        /// </summary>
        public virtual int[,] Grid
        {
            get
            {
                var result = _grid.Copy(Height, Width);
                int h = Height - 1 - _distance;
                result[h, 1] = result[h - 1, 1] = Settings.Game.ColorPlayer;
                return result;
            }
        }

        /// <summary>
        /// Endless runner game
        /// </summary>
        protected internal RunnerBoard()
        {
            _grid = new int[Height, Width];
        }

        /// <summary>
        /// Jump action is performed
        /// </summary>
        public bool Action(ConsoleKey key)
        {
            if (_kill) // restart on collision
            {
                _kill = false;
                Restart();
                return true;
            }

            return Jump();
        }

        /// <summary>
        /// Move fences and check for collision
        /// </summary>
        public bool Step()
        {
            if (_kill)
                return true;

            Move(); // air move

            _skipAdvance = !_skipAdvance; // skip a frame before moving fences
            if (!_skipAdvance)
            {
                Advance(); // move fences (player advances)
                if (Collides()) // check if player collides with a fence
                {
                    Clear(Settings.Game.ColorEnemy); // kill animation
                    _kill = true;
                }
            }

            return true;
        }

        /// <summary>
        /// Jump or double jump
        /// </summary>
        protected virtual bool Jump()
        {
            if (_distance == 0) // is on ground level
            {
                _upwards = true; // start moving up
                return true;
            }

            if (_distance > 0 && !_doubleJump) // is in the air and double jump is available
            {
                _doubleJump = true; // do a double jump
                _upwards = true; // start moving up again
                return true;
            }

            // if not on ground and no double jump available, no action is needed
            // air movement is perfomed in half-frame step
            return false;
        }

        /// <summary>
        /// Air movement
        /// </summary>
        protected virtual void Move()
        {
            int max = _doubleJump ? 3 : 2; // jump height limit
            if (_upwards && _distance < max) // move upwards
                _distance++;
            else
            {
                _upwards = false; // upward movement is done
                if (_distance == max && _hang < max) // hang in the air
                    _hang++;
                else
                {
                    _hang = 0; // hanging is done

                    if (_distance > 0) // move downwards
                        _distance--;
                    else
                        _doubleJump = false; // landed, double jump is available again
                }
            }
        }

        /// <summary>
        /// Check if fence is hit
        /// </summary>
        /// <returns>true if fence is hit</returns>
        protected virtual bool Collides()
        {
            int fence = 0; // fence height
            int x = Height - 1;
            while (_grid[x--, 1] > 0)
                fence++;
            return _distance < fence; // collides if fence is higher than jump distance
        }

        /// <summary>
        /// Reset the game and the score
        /// </summary>
        protected virtual void Restart()
        {
            _distance = 0;
            _score = 0;
            _hang = 0;
            _upwards = false;
            _doubleJump = false;
            Clear(0);
        }

        /// <summary>
        /// Randomly generates a new fence at the end of the grid
        /// </summary>
        protected virtual void CreateFence()
        {
            bool hasSpace = true; // fences should not be to close together; check if there is room for new fence
            int y = Width - 4;
            while (hasSpace && y >= Width - 12)
                hasSpace &= _grid[Height - 1, y--] == 0;
            if (!hasSpace) // no room for new fence
                return;
            int fence = _random.Next(3); // random height, including 0
            for (int i = 0; i < 3; i++)
                _grid[Height - 1 - i, Width - 2] = i < fence ? Settings.Game.ColorEnemy : 0; // set fence to grid
        }

        /// <summary>
        /// Fill grid with color
        /// </summary>
        /// <param name="v">color value</param>
        protected void Clear(int v)
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    _grid[i, j] = v;
        }

        /// <summary>
        /// Move fence to the left to simulate player advance
        /// </summary>
        void Advance()
        {
            _score++; // each step is a score
            if (_score > _highscore) // only keep the highscore
                _highscore = _score;

            for (int j = 1; j < Width - 1; j++) // shift grid to left
                for (int i = 0; i < Height; i++)
                    _grid[i, j] = _grid[i, j + 1];
            for (int i = 0; i < Height; i++)
                _grid[i, Width - 1] = 0;
            CreateFence();
        }

        public override string ToString()
        {
            return "Endless Runner";
        }
    }
}