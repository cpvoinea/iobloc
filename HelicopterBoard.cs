using System;

namespace iobloc
{
    class HelicopterBoard : IBoard
    {
        public string[] Help { get { return Settings.Helicopter.HELP; } }
        public ConsoleKey[] Keys { get { return Settings.Helicopter.KEYS; } }
        public int StepInterval { get { return Settings.Helicopter.INTERVAL; } }
        public int Width { get { return Settings.Helicopter.WIDTH; } }
        public int Height { get { return Settings.Helicopter.HEIGHT; } }
        public int[,] Grid
        {
            get
            {
                var result = _grid.Copy(Height, Width);
                result[_distance, 5] = result[_distance, 6] = 1;
                return result;
            }
        }

        readonly Random _random = new Random();
        readonly int[,] _grid;
        int _distance;
        int _speed;
        bool _skipAdvance;
        bool _kill;

        internal HelicopterBoard()
        {
            _grid = new int[Height, Width];
            _speed = -1;
        }

        public bool Action(ConsoleKey key)
        {
            if (_kill)
            {
                _kill = false;
                Restart();
                return true;
            }

            if (_distance <= 1)
            {
                Clear(4);
                _kill = true;
                return true;
            }

            _distance--;
            _speed = 1;

            return true;
        }

        public bool Step()
        {
            if (_kill)
                return true;

            if (_speed == 1)
            {
                _distance--;
                _speed--;
            }
            else if (_speed == 0)
                _speed--;
            else if (_distance < Height - 1)
                _distance++;
            else
            {
                _kill = true;
                Clear(4);
                return true;
            }

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

        void Restart()
        {
            _distance = 0;
            _speed = -1;
            Clear(0);
        }

        void Clear(int v)
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    _grid[i, j] = v;
        }

        bool Collides()
        {
            return _grid[_distance, 5] > 0 || _grid[_distance, 6] > 0;
        }

        void Advance()
        {
            for (int j = 1; j < Width - 1; j++)
                for (int i = 0; i < Height; i++)
                    _grid[i, j] = _grid[i, j + 1];
            var fence = CreateFence();
            for (int i = 0; i < Height; i++)
                _grid[i, Width - 1] = fence[i];
        }

        int[] CreateFence()
        {
            int[] result = new int[Height];
            int p = _random.Next(4);
            if (p == 0)
                return result;
            if ((p & 1) > 0)
            {
                int c = _random.Next(5);
                for (int i = Height - 1; i >= Height - 1 - c; i--)
                    result[i] = 4;
            }
            if ((p & 2) > 0)
            {
                int c = _random.Next(4);
                for (int i = 0; i < c; i++)
                    result[i] = 4;
            }
            return result;
        }
    }
}