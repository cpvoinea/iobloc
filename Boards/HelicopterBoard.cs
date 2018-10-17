namespace iobloc
{
    class HelicopterBoard : RunnerBoard
    {
        int CP => _settings.GetColor("PlayerColor");
        int CE => _settings.GetColor("EnemyColor");

        int _speed;

        internal HelicopterBoard() : base(Option.Helicopter)
        {
            _speed = -1; // start by falling downwards
        }

        protected override void Set(bool set)
        {
            if (_distance < _height)
                _main.Grid[_distance, 5] = _main.Grid[_distance, 6] = CP;
        }

        protected override bool Jump()
        {
            if (_distance <= 1) // helicopter is too close to the sky and will burn
            {
                Clear(CE); // kill animation
                _kill = true;
                return true; // return true to draw
            }

            Set(false);
            _distance--; // initial lift
            Set(true);
            _main.HasChanges = true;
            _speed = 1; // upward movement of inertia

            return true; // return true to draw
        }

        protected override void Move()
        {
            if (_speed == 1) // is moving upwards
            {
                Set(false);
                _distance--;
                Set(true);
                _main.HasChanges = true;
                _speed--;
            }
            else if (_speed == 0) // is hanging
                _speed--;
            else // is falling
            {
                Set(false);
                _distance++;
                Set(true);
                _main.HasChanges = true;
            }
        }

        protected override bool Collides()
        {
            return _distance >= _height // crash to the ground
                || _main.Grid[_distance, 5] > 0 // obstacle collides with tail
                || _main.Grid[_distance, 6] > 0; // obstacle collides with cabin
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
                    _main.Grid[i, _width - 1] = CE;
            }
            if ((p & 2) > 0) // bottom obstacle
            {
                int c = _random.Next(_height - 4 - up);
                for (int i = _height - 1; i > _height - 1 - c; i--)
                    _main.Grid[i, _width - 1] = CE;
            }
        }
    }
}
