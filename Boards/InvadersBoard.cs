namespace iobloc
{
    class InvadersBoard : BaseBoard
    {
        int CP => Settings.GetColor("PlayerColor");
        int CE => Settings.GetColor("EnemyColor");
        int CN => Settings.GetColor("NeutralColor");
        int AW => Settings.GetInt("AlienWidth");
        int AS => Settings.GetInt("AlienSpace");
        int AR => Settings.GetInt("AlienRows");
        int AC => Settings.GetInt("AlienCols");
        int BS => Settings.GetInt("BulletSpeed");
        int A => AW + AS;
        int WinScore => AR * AC;

        int _targets;
        int _ship;
        int _bulletCol;
        int _bulletRow;
        int _skipFrame;
        bool _shot;
        bool _movingRight;

        internal InvadersBoard() : base(BoardType.Invaders) { }

        protected override void InitializeGrid()
        {
            if (_ship > 0)
                ChangeGrid(false);
            _skipFrame = BS;
            _ship = Width / 2 - 1;
            _bulletCol = Width / 2 - 1;
            _bulletRow = Height - 2;
            _movingRight = true;
            _targets = WinScore;

            for (int row = 0; row < AR; row++)
                for (int col = 0; col < Width && col < AC * A; col += A)
                    for (int i = 0; col + i < Width && i < AW; i++)
                        Main[row, col + i] = CE;

            base.InitializeGrid();
        }

        protected override void ChangeGrid(bool set)
        {
            for (int i = -1; i <= 1; i++)
                Main[Height - 1, _ship + i] = set ? CP : 0;
            Main[_bulletRow, _bulletCol] = set ? CN : 0;

            base.ChangeGrid(set);
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case "LeftArrow":
                    if (_ship > 1)
                    {
                        ChangeGrid(false);
                        _ship--;
                        if (!_shot)
                            _bulletCol--;
                        ChangeGrid(true);
                    }
                    break;
                case "RightArrow":
                    if (_ship < Width - 2)
                    {
                        ChangeGrid(false);
                        _ship++;
                        if (!_shot)
                            _bulletCol++;
                        ChangeGrid(true);
                    }
                    break;
                case "UpArrow":
                    _shot = true;
                    break;
            }
        }

        public override void NextFrame()
        {
            if (_targets == 0)
            {
                Level++;
                if (Level > 15)
                {
                    IsWinner = true;
                    IsRunning = false;
                }
                else
                {
                    ChangeGrid(false);
                    InitializeGrid();
                }
                return;
            }

            for (int i = 0; i < Width; i++)
                if (Main[Height - 2, i] == CE)
                {
                    IsWinner = false;
                    IsRunning = false;
                    return;
                }

            ChangeGrid(false);
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
                        _targets--;

                        _shot = false;
                        _bulletRow = Height - 2;
                        _bulletCol = _ship;
                        Main[_bulletRow, _bulletCol] = CN;
                    }
                }
            }

            ChangeGrid(true);
        }
    }
}
