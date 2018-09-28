using System;

namespace iobloc
{
    class RunnerBoard : IBoard
    {
        public string[] Help { get { return Settings.Runner.HELP; } }
        public ConsoleKey[] Keys { get { return Settings.Runner.KEYS; } }
        public int StepInterval { get { return Settings.Runner.INTERVAL; } }
        public int Width { get { return Settings.Runner.WIDTH; } }
        public int Height { get { return Settings.Runner.HEIGHT; } }
        public virtual int[,] Grid
        {
            get
            {
                var result = _grid.Copy(Height, Width);
                int h = Height - 1 - _distance;
                result[h, 1] = result[h - 1, 1] = 1;
                return result;
            }
        }
        public int Score { get { return _highscore; } }

        protected readonly Random _random = new Random();
        protected readonly int[,] _grid;
        protected int _distance;
        protected int _highscore;
        protected int _score;
        protected bool _skipAdvance;
        protected bool _kill;

        int _hang;
        bool _upwards;
        bool _doubleJump;

        protected internal RunnerBoard()
        {
            _grid = new int[Height, Width];
        }

        public bool Action(ConsoleKey key)
        {
            if (_kill)
            {
                _kill = false;
                Restart();
                return true;
            }

            return Jump();
        }

        public bool Step()
        {
            if (_kill)
                return true;

            Move();

            _skipAdvance = !_skipAdvance;
            if (!_skipAdvance)
            {
                Advance();
                if (Collides())
                {
                    Clear(4);
                    _kill = true;
                }
            }
            
            return true;
        }

        protected virtual bool Jump()
        {
            if (_distance == 0)
            {
                _upwards = true;
                return true;
            }

            if (_distance > 0 && !_doubleJump)
            {
                _doubleJump = true;
                _upwards = true;
                return true;
            }

            return false;
        }

        protected virtual void Move()
        {
            int max = _doubleJump ? 3 : 2;
            if (_upwards && _distance < max)
                _distance++;
            else
            {
                _upwards = false;
                if (_distance == max && _hang < max)
                    _hang++;
                else
                {
                    _hang = 0;

                    if (_distance > 0)
                        _distance--;
                    else
                        _doubleJump = false;
                }
            }
        }

        protected virtual bool Collides()
        {
            int fence = 0;
            int x = Height - 1;
            while (_grid[x--, 1] > 0)
                fence++;
            return _distance < fence;
        }

        protected virtual void Restart()
        {
            _distance = 0;
            _score = 0;
            _hang = 0;
            _upwards = false;
            _doubleJump = false;
            Clear(0);
        }

        protected virtual void CreateFence()
        {
            bool hasSpace = true;
            int y = Width - 4;
            while (hasSpace && y >= Width - 12)
                hasSpace &= _grid[Height - 1, y--] == 0;
            if (!hasSpace)
                return;
            int fence = _random.Next(3);
            for (int i = 0; i < 3; i++)
                _grid[Height - 1 - i, Width - 2] = i < fence ? 4 : 0;
        }

        protected void Clear(int v)
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    _grid[i, j] = v;
        }

        void Advance()
        {
            _score++;
            if (_score > _highscore)
                _highscore = _score;

            for (int j = 1; j < Width - 1; j++)
                for (int i = 0; i < Height; i++)
                    _grid[i, j] = _grid[i, j + 1];
            CreateFence();
        }
    }
}