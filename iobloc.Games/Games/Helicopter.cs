namespace iobloc
{
    public class Helicopter : BaseGame
    {
        private int CP, CE, OS;
        private readonly System.Random _random = new System.Random();
        private int _speed;
        private int _distance;
        private bool _skipAdvance;
        private bool _lost;

        public Helicopter() : base(GameType.Helicopt) { }

        protected override void InitializeSettings()
        {
            base.InitializeSettings();
            CP = Serializer.GetColor(GameSettings, Settings.PlayerColor);
            CE = Serializer.GetColor(GameSettings, Settings.EnemyColor);
            OS = Serializer.GetInt(GameSettings, "ObstacleSpace");
        }

        protected override void Initialize()
        {
            base.Initialize();
            if (IsInitialized)
            {
                Main.Clear();
                _speed = 0;
                _distance = 0;
                _skipAdvance = false;
                _lost = false;
            }
            Change(true);
        }

        protected override void Change(bool set)
        {
            _lost = _distance >= Height;
            for (int i = 0; !_lost && i < BlockWidth; i++)
                _lost |= Main[_distance, i + 1].Color == CE;
            if (set && _lost)
                Main.Clear(new PaneCell(CE));
            else
                for (int i = 0; i < BlockWidth; i++)
                    Main[_distance, i + 1] = new PaneCell(set ? CP : 0);
            base.Change(set);
        }

        public override void HandleInput(string key)
        {
            if (_lost)
                Lose(false);
            else
                _speed = 2;
        }

        public override void NextFrame()
        {
            if (_lost) return;
            Move();
            if (_lost) return;

            _skipAdvance = !_skipAdvance;
            if (!_skipAdvance)
                Advance();
        }

        private void Move()
        {
            if (_speed >= 0)
            {
                if (_speed > 0)
                {
                    if (_distance > 0)
                    {
                        Change(false);
                        _distance--;
                        Change(true);
                    }
                    else
                        _speed--;
                }
                _speed--;
            }
            else
            {
                Change(false);
                _distance++;
                Change(true);
            }
        }

        private void Advance()
        {
            Change(false);
            for (int j = 1; j < Width - 1; j++)
                for (int i = 0; i < Height; i++)
                    Main[i, j] = Main[i, j + 1];

            for (int i = 0; i < Height; i++)
                Main[i, Width - 1] = new PaneCell(0);
            CreateObstacles();
            Score++;

            Change(true);
        }

        private void CreateObstacles()
        {
            bool hasSpace = true;
            int y = Width - 2;
            while (hasSpace && y >= 0 && y >= Width - OS)
                hasSpace &= (Main[Height - 1, y].Color == 0 && Main[0, y--].Color == 0);
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
                    Main[i, Width - 1] = new PaneCell(CE);
            }
            if ((p & 2) > 0)
            {
                int ceil = _random.Next(Height - 3 - fence);
                for (int i = 0; i < ceil; i++)
                    Main[i, Width - 1] = new PaneCell(CE);
            }
        }
    }
}
