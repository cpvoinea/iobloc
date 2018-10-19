using System;

namespace iobloc
{
    class HelicopterBoard : SinglePanelBoard
    {
        int CP => _settings.GetColor("PlayerColor");
        int CE => _settings.GetColor("EnemyColor");
        int PP => _settings.GetInt("PlayerPosition");
        int OS => _settings.GetInt("ObstacleSpace");

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
                _main.Grid[_distance, PP] = _main.Grid[_distance, PP + 1] = set ? CP : 0;
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
            bool collides = _main.Grid[_distance, PP] > 0 || _main.Grid[_distance, PP + 1] > 0;
            ChangeGrid(true);

            if (_skipAdvance)
            {
                _score++;
                if (_score > _highscore)
                    _highscore = _score;
            }
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
                _main.Grid[_distance, PP] == CE || _main.Grid[_distance, PP + 1] == CE;
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
            bool hasSpace = true;
            int y = _width - 2;
            while (hasSpace && y >= 0 && y >= _width - OS)
                hasSpace &= (_main.Grid[_height - 1, y] == 0 && _main.Grid[0, y--] == 0);
            if (!hasSpace)
                return;

            int p = _random.Next(4);
            if (p == 0)
                return;
            int fence = 0;
            if ((p & 1) > 0)
            {
                fence = _random.Next(_height - 3);
                for (int i = _height - 1; i > _height - 1 - fence; i--)
                    _main.Grid[i, _width - 1] = CE;
            }
            if ((p & 2) > 0)
            {
                int ceil = _random.Next(_height - 3 - fence);
                for (int i = 0; i < ceil; i++)
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
