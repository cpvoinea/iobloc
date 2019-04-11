namespace iobloc
{
    class Invaders : BaseGame
    {
        private int CP, CE, CN, AR, AC;
        private int LT => AR * AC;
        private int _ship;
        private int _bulletCol;
        private int _bulletRow;
        private int _skipFrame;
        private bool _shot;
        private bool _movingRight;
        private int _targets = int.MaxValue;

        public Invaders() : base(GameType.Invaders) { }

        protected override void InitializeSettings()
        {
            base.InitializeSettings();
            CP = Serializer.GetColor(GameSettings, Settings.PlayerColor);
            CE = Serializer.GetColor(GameSettings, Settings.EnemyColor);
            CN = Serializer.GetColor(GameSettings, Settings.NeutralColor);
            AR = Serializer.GetInt(GameSettings, "AlienRows");
            AC = Serializer.GetInt(GameSettings, "AlienCols");
        }

        protected override void Initialize()
        {
            if (IsInitialized)
            {
                Main.Clear();
                _shot = false;
            }
            else // don't reset the score when level is reset
                base.Initialize();
            _ship = Width / 2 - 1;
            _bulletCol = Width / 2 - 1;
            _bulletRow = Height - 2;
            _skipFrame = 2;
            _movingRight = true;
            _targets = LT;

            for (int row = 0; row < AR; row++)
                for (int col = 0; col < Width && col < AC * Block; col += Block)
                    for (int i = 0; col + i < Width && i < BlockWidth; i++)
                        Main[row, col + i] = CE;
            Change(true);
        }

        protected override void Change(bool set)
        {
            for (int i = -1; i <= 1; i++)
                Main[Height - 1, _ship + i] = set ? CP : 0;
            Main[_bulletRow, _bulletCol] = set ? CN : 0;
            base.Change(set);
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case UIKey.LeftArrow:
                    if (_ship > 1)
                    {
                        Change(false);
                        _ship--;
                        if (!_shot)
                            _bulletCol--;
                        Change(true);
                    }
                    break;
                case UIKey.RightArrow:
                    if (_ship < Width - 2)
                    {
                        Change(false);
                        _ship++;
                        if (!_shot)
                            _bulletCol++;
                        Change(true);
                    }
                    break;
                case UIKey.UpArrow:
                    _shot = true;
                    break;
            }
        }

        public override void NextFrame()
        {
            for (int i = 0; i < Width; i++)
                if (Main[Height - 2, i] == CE)
                {
                    base.Initialize(); // reset score and level
                    Lose();
                    return;
                }

            Change(false);
            if (_skipFrame <= 0)
            {
                _skipFrame = 2;
                for (int i = 0; i < Height; i++)
                    if (_movingRight && Main[i, Width - 1] > 0 ||
                     !_movingRight && Main[i, 0] > 0)
                    {
                        for (int k = Height - 1; k >= 0; k--)
                            for (int j = 0; j < Width; j++)
                                Main[k, j] = k == 0 ? 0 : Main[k - 1, j];

                        _movingRight = !_movingRight;
                        break;
                    }
                if (_movingRight)
                    for (int i = 0; i < Height; i++)
                        for (int j = Width - 1; j >= 0; j--)
                            Main[i, j] = j == 0 ? 0 : Main[i, j - 1];
                else
                    for (int i = 0; i < Height; i++)
                        for (int j = 0; j < Width; j++)
                            Main[i, j] = j == Width - 1 ? 0 : Main[i, j + 1];
            }

            _skipFrame--;
            if (_shot)
            {
                if (_bulletRow <= 0)
                {
                    _shot = false;
                    _bulletRow = Height - 2;
                    _bulletCol = _ship;
                }
                else
                {
                    _bulletRow--;
                    if (Main[_bulletRow, _bulletCol] == CE)
                    {
                        int c = _bulletCol;
                        do
                            Main[_bulletRow, c++] = 0;
                        while (c < Width && Main[_bulletRow, c] == CE);

                        c = _bulletCol;
                        while (c > 0 && Main[_bulletRow, --c] == CE)
                            Main[_bulletRow, c] = 0;

                        Score++;
                        _targets--;
                        if (_targets == 0)
                        {
                            Level++;
                            Initialize();
                        }
                        else
                        {
                            _shot = false;
                            _bulletRow = Height - 2;
                            _bulletCol = _ship;
                            Main[_bulletRow, _bulletCol] = CN;
                        }
                    }
                }
            }

            Change(true);
        }
    }
}
