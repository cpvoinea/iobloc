using System;

namespace iobloc
{
    class HelicopterBoard : SinglePanelBoard
    {
        int CP => _settings.GetColor("PlayerColor");
        int CE => _settings.GetColor("EnemyColor");

        public override int Score => _highscore;

        readonly Random _random = new Random();
        int _speed;
        int _distance;
        int _score;
        int _highscore;
        bool _skipAdvance;
        bool _dead;

        internal HelicopterBoard() : base(Option.Helicopter)
        {
            ChangeGrid(true);
        }

        protected override void ChangeGrid(bool set)
        {
            if (_distance >= 0 && _distance < _height)
            {
                _main.Grid[_distance, 5] = _main.Grid[_distance, 6] = set ? CP : 0;
                if (set)
                    _main.HasChanges = true;
            }
        }

        public override void HandleInput(string key)
        {
            if (_dead)
            {
                _dead = false;
                Restart();
            }
            else
                _speed = 2;
        }

        public override void NextFrame()
        {
            if (_dead) return;
            Move();
            if (_dead) return;

            _skipAdvance = !_skipAdvance;
            if (!_skipAdvance)
                Advance();
        }

        void Move()
        {
            if (_speed >= 0)
            {
                if (_speed > 0)
                {
                    ChangeGrid(false);
                    _distance--;
                    if (!CheckDead())
                        ChangeGrid(true);
                }

                _speed--;
            }
            else
            {
                ChangeGrid(false);
                _distance++;
                if (!CheckDead())
                    ChangeGrid(true);
            }
        }

        bool AdvanceCollides()
        {
            ChangeGrid(false);
            Advance();
            bool collides = _main.Grid[_distance, 5] > 0 || _main.Grid[_distance, 6] > 0;
            ChangeGrid(true);

            _score++;
            if (_score > _highscore)
                _highscore = _score;
            return collides;
        }

        void Advance()
        {
            ChangeGrid(false);
            for (int j = 1; j < _width - 1; j++)
                for (int i = 0; i < _height; i++)
                    _main.Grid[i, j] = _main.Grid[i, j + 1];

            for (int i = 0; i < _height; i++)
                _main.Grid[i, _width - 1] = 0;
            CreateObstacles();

            _score++;
            if (_score > _highscore)
                _highscore = _score;
            if (!CheckDead())
                ChangeGrid(true);
        }

        bool CheckDead()
        {
            _dead = _distance < 0 || _distance >= _height ||
                _main.Grid[_distance, 5] == CE || _main.Grid[_distance, 6] == CE;
            if (_dead)
                Clear(CE);

            return _dead;
        }

        void Restart()
        {
            _distance = 0;
            _score = 0;
            _speed = 0;
            Clear(0);
        }

        void CreateObstacles()
        {
            int p = _random.Next(4);
            if (p == 0)
                return;
            int up = 0;
            if ((p & 1) > 0)
            {
                up = _random.Next(4);
                for (int i = 0; i < up; i++)
                    _main.Grid[i, _width - 1] = CE;
            }
            if ((p & 2) > 0)
            {
                int c = _random.Next(_height - 4 - up);
                for (int i = _height - 1; i > _height - 1 - c; i--)
                    _main.Grid[i, _width - 1] = CE;
            }
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
