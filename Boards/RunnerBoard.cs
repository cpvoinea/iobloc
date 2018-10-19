using System;

namespace iobloc
{
    class RunnerBoard : SinglePanelBoard
    {
        int CP => _settings.GetColor("PlayerColor");
        int CE => _settings.GetColor("EnemyColor");
        int FS => _settings.GetInt("FenceSpace");

        readonly Random _random = new Random();
        int _distance;
        bool _skipAdvance;
        int _hang;
        bool _upwards;
        bool _doubleJump;

        internal RunnerBoard() : base(Option.Runner)
        {
            ChangeGrid(true);
        }

        protected override void ChangeGrid(bool set)
        {
            int h = _height - 1 - _distance;
            _main.Grid[h, 1] = _main.Grid[h - 1, 1] = set ? CP : 0;
            if (set)
                _main.HasChanges = true;
        }

        public override void HandleInput(string key)
        {
            if (Win == false)
            {
                Win = null;
                Restart();
            }
            else
            {
                if (_distance == 0)
                    _upwards = true;
                else if (_distance > 0 && !_doubleJump)
                {
                    _doubleJump = true;
                    _upwards = true;
                }
            }
        }

        public override void NextFrame()
        {
            if (Win == false) return;
            Move();
            if (Win == false) return;

            _skipAdvance = !_skipAdvance;
            if (_skipAdvance)
                Advance();
        }

        void Move()
        {
            int max = _doubleJump ? 3 : 2;
            if (_upwards && _distance < max)
            {
                ChangeGrid(false);
                _distance++;
                ChangeGrid(true);
            }
            else
            {
                _upwards = false;
                if (_distance == max && _hang < max)
                    _hang++;
                else
                {
                    _hang = 0;

                    if (_distance > 0)
                    {
                        ChangeGrid(false);
                        _distance--;
                        if (!CheckDead())
                            ChangeGrid(true);
                    }
                    else
                        _doubleJump = false;
                }
            }
        }

        void Advance()
        {
            ChangeGrid(false);
            for (int j = 1; j < _width - 1; j++)
                for (int i = 0; i < _height; i++)
                    _main.Grid[i, j] = _main.Grid[i, j + 1];

            for (int i = 0; i < _height; i++)
                _main.Grid[i, _width - 1] = 0;
            CreateFence();

            if (_main.Grid[_height - 1, 1] == CE)
                Score++;

            if (!CheckDead())
                ChangeGrid(true);
        }

        bool CheckDead()
        {
            int h = _height - 1 - _distance;
            if (_main.Grid[h, 1] == CE || _main.Grid[h - 1, 1] == CE)
            {
                Win = false;
                Clear(CE);
                return true;
            }

            return false;
        }

        void Restart()
        {
            _distance = 0;
            _hang = 0;
            _upwards = false;
            _doubleJump = false;
            Score = 0;
            Clear(0);
        }

        void CreateFence()
        {
            bool hasSpace = true;
            int y = _width - 4;
            while (hasSpace && y >= 0 && y >= _width - FS)
                hasSpace &= _main.Grid[_height - 1, y--] == 0;
            if (!hasSpace)
                return;
            int fence = _random.Next(3);
            for (int i = 0; i < 3; i++)
                _main.Grid[_height - 1 - i, _width - 2] = i < fence ? CE : 0;
        }

        void Clear(int v)
        {
            for (int i = 0; i < _height; i++)
                for (int j = 0; j < _width; j++)
                    _main.Grid[i, j] = v;
            ChangeGrid(true);
        }
    }
}
