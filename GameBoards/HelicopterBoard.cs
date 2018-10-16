using System;

namespace iobloc
{
    class HelicopterBoard : SinglePanelBoard
    {
        int CP => (int)_config.GetColor("PlayerColor");
        int CE => (int)_config.GetColor("EnemyColor");
        public override int[,] Grid
        {
            get
            {
                var result = _grid.Copy(Height, Width);
                if (_distance < Height)
                    result[_distance, 5] = result[_distance, 6] = CP;
                return result;
            }
        }

        int _speed;

        internal HelicopterBoard() : base(Option.Helicopter)
        {
            _speed = -1; // start by falling downwards
        }

        protected override bool Jump()
        {
            if (_distance <= 1) // helicopter is too close to the sky and will burn
            {
                Clear(CE); // kill animation
                _kill = true;
                return true; // return true to draw
            }

            _distance--; // initial lift
            _speed = 1; // upward movement of inertia

            return true; // return true to draw
        }

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

        protected override void Restart()
        {
            _distance = 0;
            _score = 0;
            _speed = -1;
            Clear(0);
        }

        protected override void CreateFence()
        {
            int p = _random.Next(4); // 0 = no obstacles, 1 = top only, 2 = bottom only, 3 = both
            if (p == 0)
                return;
            int up = 0;
            if ((p & 1) > 0) // top obstacle
            {
                up = _random.Next(4);
                for (int i = 0; i < up; i++)
                    _grid[i, Width - 1] = CE;
            }
            if ((p & 2) > 0) // bottom obstacle
            {
                int c = _random.Next(Height - 4 - up);
                for (int i = Height - 1; i > Height - 1 - c; i--)
                    _grid[i, Width - 1] = CE;
            }
        }
    }
}