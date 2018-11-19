using System;

namespace iobloc
{
    class Runner : BaseGame
    {
        private int CP, CE, FS;
        private readonly Random _random = new Random();
        private int _distance;
        private bool _skipAdvance;
        private int _hang;
        private bool _upwards;
        private bool _doubleJump;
        private bool _lost;

        public Runner() : base(GameType.Runner) { }

        protected override void InitializeSettings()
        {
            base.InitializeSettings();
            CP = GameSettings.GetColor(Settings.PlayerColor);
            CE = GameSettings.GetColor(Settings.EnemyColor);
            FS = GameSettings.GetInt("FenceSpace");
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (IsInitialized)
            {
                Main.Clear();
                _distance = 0;
                _skipAdvance = false;
                _hang = 0;
                _upwards = false;
                _doubleJump = false;
                _lost = false;
            }
            Change(true);
        }

        protected override void Change(bool set)
        {
            int h = Height - 1 - _distance;
            if (set && (Main[h, 1] == CE || Main[h - 1, 1] == CE))
            {
                Main.Clear(CE);
                _lost = true;
            }
            else
                Main[h, 1] = Main[h - 1, 1] = set ? CP : 0;
            base.Change(set);
        }

        public override void HandleInput(string key)
        {
            if (_lost)
            {
                Lose(false);
                return;
            }

            if (_distance == 0)
                _upwards = true;
            else if (_distance > 0 && !_doubleJump)
            {
                _doubleJump = true;
                _upwards = true;
            }
        }

        public override void NextFrame()
        {
            if (_lost) return;
            Move();
            if (_lost) return;

            _skipAdvance = !_skipAdvance;
            if (_skipAdvance)
                Advance();
        }

        private void Move()
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
                        Change(true);
                    }
                    else
                        _doubleJump = false;
                }
            }
        }

        private void Advance()
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
            Change(true);
        }

        private void CreateFence()
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
