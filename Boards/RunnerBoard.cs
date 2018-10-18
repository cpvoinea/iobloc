using System;

namespace iobloc
{
    class RunnerBoard : SinglePanelBoard
    {
        int CP => _settings.GetColor("PlayerColor");
        int CE => _settings.GetColor("EnemyColor");
        int JS => _settings.GetInt("JumpSpace");

        public override int Score => _highscore;

        readonly Random _random = new Random();
        int _distance;
        int _score;
        int _highscore;
        bool _skipAdvance;
        bool _dead;
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
            if (_dead)
            {
                _dead = false;
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
            if (_dead) return;
            Move();
            if (_dead) return;

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

            if (_main.Grid[1, _height - 1] == CE)
            {
                _score++;
                if (_score > _highscore)
                    _highscore = _score;
            }

            if (!CheckDead())
                ChangeGrid(true);
        }

        bool CheckDead()
        {
            int h = _height - 1 - _distance;
            _dead = _main.Grid[h, 1] == CE || _main.Grid[h - 1, 1] == CE;
            if (_dead)
                Clear(CE);

            return _dead;
        }

        void Restart()
        {
            _distance = 0;
            _score = 0;
            _hang = 0;
            _upwards = false;
            _doubleJump = false;
            Clear(0);
        }

        void CreateFence()
        {
            bool hasSpace = true;
            int y = _width - 4;
            while (hasSpace && y >= 0 && y >= _width - JS)
                hasSpace &= _main.Grid[_height - 1, y--] == 0;
            if (!hasSpace) // no room for new fence
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
