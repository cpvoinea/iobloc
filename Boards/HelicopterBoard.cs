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

        internal HelicopterBoard() : base(BoardType.Helicopt) { }

        protected override void InitializeGrid()
        {
            Main.Clear();
            _distance = 0;
            _speed = 0;
            Score = 0;
            IsWinner = null;

            base.InitializeGrid();
        }

        protected override void ChangeGrid(bool set)
        {
            if (_distance >= 0 && _distance < Height)
                Main[_distance, PP] = Main[_distance, PP + 1] = set ? CP : 0;

            base.ChangeGrid(set);
        }

        public override void HandleInput(string key)
        {
            if (IsWinner == false)
                InitializeGrid();
            else
                _speed = 2;
        }

        public override void NextFrame()
        {
            if (IsWinner == false) return;
            Move();
            if (IsWinner == false) return;

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

        void Advance()
        {
            ChangeGrid(false);
            for (int j = 1; j < Width - 1; j++)
                for (int i = 0; i < Height; i++)
                    Main[i, j] = Main[i, j + 1];

            for (int i = 0; i < Height; i++)
                Main[i, Width - 1] = 0;
            CreateObstacles();
            Score++;

            if (!CheckDead())
                ChangeGrid(true);
        }

        bool CheckDead()
        {
            if (_distance < 0 || _distance >= Height ||
                Main[_distance, PP] == CE || Main[_distance, PP + 1] == CE)
            {
                IsWinner = false;
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
