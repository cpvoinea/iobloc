using System;

namespace iobloc
{
    class InvadersBoard : SinglePanelBoard
    {
        int CP => _settings.GetColor("PlayerColor");
        int CE => _settings.GetColor("EnemyColor");
        int CN => _settings.GetColor("NeutralColor");
        int AW => _settings.GetInt("AlienWidth");
        int AS => _settings.GetInt("AlienSpace");
        int AR => _settings.GetInt("AlienRows");
        int AC => _settings.GetInt("AlienCols");
        int BS => _settings.GetInt("BulletSpeed");
        int A => AW + AS;

        public override bool Won => Score == AR * AC;

        int _ship;
        int _bulletCol;
        int _bulletRow;
        int _skipFrame;
        bool _shot = false;
        bool _movingRight = true;

        internal InvadersBoard() : base(Option.Invaders)
        {
            _skipFrame = BS;
            _ship = _width / 2 - 1;
            _bulletCol = _width / 2 - 1;
            _bulletRow = _height - 2;

            InitializeGrid();
            ChangeGrid(true);
        }

        protected override void InitializeGrid()
        {
            for (int row = 0; row < AR; row++)
                for (int col = 0; col < _width && col < AC * A; col += A)
                    for (int i = 0; col + i < _width && i < AW; i++)
                        _main.Grid[row, col + i] = CE;
        }

        protected override void ChangeGrid(bool set)
        {
            for (int i = -1; i <= 1; i++)
                _main.Grid[_height - 1, _ship + i] = set ? CP : 0;
            _main.Grid[_bulletRow, _bulletCol] = set ? CN : 0;
            if (set)
                _main.HasChanges = true;
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
                    if (_ship < _width - 2)
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
            if (Won)
            {
                IsRunning = false;
                return;
            }

            for (int i = 0; i < _width; i++)
                if (_main.Grid[_height - 1, i] == CE)
                {
                    IsRunning = false;
                    return;
                }

            ChangeGrid(false);
            if (_skipFrame <= 0)
            {
                _skipFrame = BS;
                for (int i = 0; i < _height; i++)
                    if (_movingRight && _main.Grid[i, _width - 1] > 0 ||
                     !_movingRight && _main.Grid[i, 0] > 0)
                    {
                        for (int k = _height - 1; k >= 0; k--)
                            for (int j = 0; j < _width; j++)
                                _main.Grid[k, j] = k == 0 ? 0 : _main.Grid[k - 1, j];

                        _movingRight = !_movingRight;
                        break;
                    }
                if (_movingRight)
                    for (int i = 0; i < _height; i++)
                        for (int j = _width - 1; j >= 0; j--)
                            _main.Grid[i, j] = j == 0 ? 0 : _main.Grid[i, j - 1];
                else
                    for (int i = 0; i < _height; i++)
                        for (int j = 0; j < _width; j++)
                            _main.Grid[i, j] = j == _width - 1 ? 0 : _main.Grid[i, j + 1];
            }

            _skipFrame--;
            if (_shot)
            {
                if (_bulletRow <= 0)
                {
                    _shot = false;
                    _bulletRow = _height - 2;
                    _bulletCol = _ship;
                }
                else
                {
                    _bulletRow--;
                    if (_main.Grid[_bulletRow, _bulletCol] == CE)
                    {
                        int c = _bulletCol;
                        do
                            _main.Grid[_bulletRow, c++] = 0;
                        while (c < _width && _main.Grid[_bulletRow, c] == CE);

                        c = _bulletCol;
                        while (c > 0 && _main.Grid[_bulletRow, --c] == CE)
                            _main.Grid[_bulletRow, c] = 0;

                        Score++;

                        _shot = false;
                        _bulletRow = _height - 2;
                        _bulletCol = _ship;
                        _main.Grid[_bulletRow, _bulletCol] = CN;
                    }
                }
            }

            ChangeGrid(true);
        }
    }
}
