using System;

namespace iobloc
{
    class HelicopterBoard : BaseBoard
    {
        int CP => Settings.GetColor("PlayerColor");
        int CE => Settings.GetColor("EnemyColor");
        int PP => Settings.GetInt("PlayerPosition");
        int OS => Settings.GetInt("ObstacleSpace");

        readonly Random _random = new Random();
        int _speed;
        int _distance;
        bool _skipAdvance;
        bool _restart;

        internal HelicopterBoard() : base(BoardType.Helicopt) { }

        protected override void Restart()
        {
            Main.Clear();
            _distance = 0;
            _speed = 0;
            Initialize();
        }

        protected override void Change(bool set)
        {
            if (_distance >= 0 && _distance < Height)
                Main[_distance, PP] = Main[_distance, PP + 1] = set ? CP : 0;
            if(set)
                Main.HasChanges = true;
        }

        public override void HandleInput(string key)
        {
            if (_restart)
                Restart();
            else
                _speed = 2;
        }

        public override void NextFrame()
        {
            if (_restart) return;
            Move();
            if (_restart) return;

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
                    Change(false);
                    _distance--;
                    if (!CheckDead())
                        Change(true);
                }

                _speed--;
            }
            else
            {
                Change(false);
                _distance++;
                if (!CheckDead())
                    Change(true);
            }
        }

        void Advance()
        {
            Change(false);
            for (int j = 1; j < Width - 1; j++)
                for (int i = 0; i < Height; i++)
                    Main[i, j] = Main[i, j + 1];

            for (int i = 0; i < Height; i++)
                Main[i, Width - 1] = 0;
            CreateObstacles();
            Score++;

            if (!CheckDead())
                Change(true);
        }

        bool CheckDead()
        {
            if (_distance < 0 || _distance >= Height ||
                Main[_distance, PP] == CE || Main[_distance, PP + 1] == CE)
            {
                _restart = true;
                Main.Clear(CE);
                return true;
            }

            return false;
        }

        void CreateObstacles()
        {
            bool hasSpace = true;
            int y = Width - 2;
            while (hasSpace && y >= 0 && y >= Width - OS)
                hasSpace &= (Main[Height - 1, y] == 0 && Main[0, y--] == 0);
            if (!hasSpace)
                return;

            int p = _random.Next(4);
            if (p == 0)
                return;
            int fence = 0;
            if ((p & 1) > 0)
            {
                fence = _random.Next(Height - 3);
                for (int i = Height - 1; i > Height - 1 - fence; i--)
                    Main[i, Width - 1] = CE;
            }
            if ((p & 2) > 0)
            {
                int ceil = _random.Next(Height - 3 - fence);
                for (int i = 0; i < ceil; i++)
                    Main[i, Width - 1] = CE;
            }
        }
    }
}
