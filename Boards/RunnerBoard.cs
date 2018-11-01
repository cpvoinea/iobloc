using System;

namespace iobloc
{
    class RunnerBoard : BaseBoard
    {
        int CP => Settings.GetColor("PlayerColor");
        int CE => Settings.GetColor("EnemyColor");
        int FS => Settings.GetInt("FenceSpace");

        readonly Random _random = new Random();
        int _distance;
        bool _skipAdvance;
        int _hang;
        bool _upwards;
        bool _doubleJump;
        bool _restart;

        internal RunnerBoard() : base(BoardType.Runner) { }

        protected override void Restart()
        {
            Main.Clear();
            _distance = 0;
            _hang = 0;
            _upwards = false;
            _doubleJump = false;
            _restart = false;
            Score = 0;
        }

        protected override void Change(bool set)
        {
            int h = Height - 1 - _distance;
            Main[h, 1] = Main[h - 1, 1] = set ? CP : 0;
            if(set)
                Main.HasChanges = true;
        }

        public override void HandleInput(string key)
        {
            if (_restart)
                Restart();
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
            if (_restart) return;
            Move();
            if (_restart) return;

            _skipAdvance = !_skipAdvance;
            if (_skipAdvance)
                Advance();
        }

        void Move()
        {
            int max = _doubleJump ? 3 : 2;
            if (_upwards && _distance < max)
            {
                Change(false);
                _distance++;
                Change(true);
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
                        Change(false);
                        _distance--;
                        if (!CheckDead())
                            Change(true);
                    }
                    else
                        _doubleJump = false;
                }
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
            CreateFence();

            if (Main[Height - 1, 1] == CE)
                Score++;

            if (!CheckDead())
                Change(true);
        }

        bool CheckDead()
        {
            int h = Height - 1 - _distance;
            if (Main[h, 1] == CE || Main[h - 1, 1] == CE)
            {
                _restart = true;
                Main.Clear(CE);
                return true;
            }

            return false;
        }

        void CreateFence()
        {
            bool hasSpace = true;
            int y = Width - 4;
            while (hasSpace && y >= 0 && y >= Width - FS)
                hasSpace &= Main[Height - 1, y--] == 0;
            if (!hasSpace)
                return;
            int fence = _random.Next(3);
            for (int i = 0; i < 3; i++)
                Main[Height - 1 - i, Width - 2] = i < fence ? CE : 0;
        }
    }
}
