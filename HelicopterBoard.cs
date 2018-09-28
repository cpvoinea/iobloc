using System;

namespace iobloc
{
    class HelicopterBoard : RunnerBoard
    {
        public override int[,] Grid
        {
            get
            {
                var result = _grid.Copy(Height, Width);
                if (_distance < Height)
                    result[_distance, 5] = result[_distance, 6] = 1;
                return result;
            }
        }

        int _speed;

        internal HelicopterBoard() : base()
        {
            _speed = -1;
        }

        protected override bool Jump()
        {
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

        protected override void Move()
        {
            if (_speed == 1)
            {
                _distance--;
                _speed--;
            }
            else if (_speed == 0)
                _speed--;
            else
                _distance++;
        }

        protected override bool Collides()
        {
            return _distance >= Height || _grid[_distance, 5] > 0 || _grid[_distance, 6] > 0;
        }

        protected override void Restart()
        {
            _distance = 0;
            _score = 0;
            _speed = -1;
            Clear(0);
        }

        protected override void CreateFence()
        {
            int p = _random.Next(4);
            if (p == 0)
                return;
            if ((p & 1) > 0)
            {
                int c = _random.Next(5);
                for (int i = Height - 1; i >= Height - 1 - c; i--)
                    _grid[i, Width - 1] = 4;
            }
            if ((p & 2) > 0)
            {
                int c = _random.Next(4);
                for (int i = 0; i < c; i++)
                    _grid[i, Width - 1] = 4;
            }
        }
    }
}