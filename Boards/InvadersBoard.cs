namespace iobloc
{
    class InvadersBoard : BaseBoard
    {
        int CP => BoardSettings.GetColor(Settings.PlayerColor);
        int CE => BoardSettings.GetColor(Settings.EnemyColor);
        int CN => BoardSettings.GetColor(Settings.NeutralColor);
        int AW => BoardSettings.GetInt("AlienWidth");
        int AS => BoardSettings.GetInt("AlienSpace");
        int AR => BoardSettings.GetInt("AlienRows");
        int AC => BoardSettings.GetInt("AlienCols");
        int BS => BoardSettings.GetInt("BulletSpeed");
        int A => AW + AS;

        int _ship;
        int _bulletCol;
        int _bulletRow;
        int _skipFrame;
        bool _shot;
        bool _movingRight;

        public InvadersBoard() : base(BoardType.Invaders) { }

        public override void Initialize()
        {
            base.Initialize();

            _skipFrame = BS;
            _ship = Width / 2 - 1;
            _bulletCol = Width / 2 - 1;
            _bulletRow = Height - 2;
            _movingRight = true;

            for (int row = 0; row < AR; row++)
                for (int col = 0; col < Width && col < AC * A; col += A)
                    for (int i = 0; col + i < Width && i < AW; i++)
                        Main[row, col + i] = CE;
            Main.HasChanges = true;
        }

        public override void Reset()
        {
            base.Reset();
            Main.Clear();
            Initialize();
        }

        public override void Change(bool set)
        {
            for (int i = -1; i <= 1; i++)
                Main[Height - 1, _ship + i] = set ? CP : 0;
            Main[_bulletRow, _bulletCol] = set ? CN : 0;
            if(set)
                Main.HasChanges= true;
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case "LeftArrow":
                    if (_ship > 1)
                    {
                        Change(false);
                        _ship--;
                        if (!_shot)
                            _bulletCol--;
                        Change(true);
                    }
                    break;
                case "RightArrow":
                    if (_ship < Width - 2)
                    {
                        Change(false);
                        _ship++;
                        if (!_shot)
                            _bulletCol++;
                        Change(true);
                    }
                    break;
                case "UpArrow":
                    _shot = true;
                    break;
            }
        }

        public override void NextFrame()
        {
            for (int i = 0; i < Width; i++)
                if (Main[Height - 2, i] == CE)
                {
                    Stop();
                    return;
                }

            Change(false);
            if (_skipFrame <= 0)
            {
                _skipFrame = BS;
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

                        _shot = false;
                        _bulletRow = Height - 2;
                        _bulletCol = _ship;
                        Main[_bulletRow, _bulletCol] = CN;
                    }
                }
            }

            Change(true);
        }
    }
}
