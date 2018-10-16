using System;

namespace iobloc
{
    class InvadersBoard : SinglePanelBoard
    {
        int CP => (int)_config.GetColor("PlayerColor");
        int CE => (int)_config.GetColor("EnemyColor");
        int CN => (int)_config.GetColor("NeutralColor");
        int AW => _config.GetInt("AlienWidth");
        int AS => _config.GetInt("AlienSpace");
        int AR => _config.GetInt("AlienRows");
        int AC => _config.GetInt("AlienCols");
        int BS => _config.GetInt("BulletSpeed");
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
            for (int row = 0; row < AR; row++)
                for (int col = 0; col < _width && col < AC * A; col += A)
                    for (int i = 0; col + i < _width && i < AW; i++)
                        _mainPanel.Grid[row, col + i] = CE;
            _ship = _width / 2 - 1;
            _bulletCol = _width / 2 - 1;
            _bulletRow = _height - 2;

            for (int i = -1; i <= 1; i++)
                _mainPanel.Grid[_height - 1, _ship + i] = CP;
            _mainPanel.Grid[_bulletRow, _bulletCol] = CN;
        }

        public override void HandleInput(int key)
        {
            switch ((ConsoleKey)key)
            {
                case ConsoleKey.LeftArrow:
                    if (_ship > 1)
                    {
                        _mainPanel.Grid[_height - 1, _ship + 1] = 0;
                        _ship--;
                        _mainPanel.Grid[_height - 1, _ship - 1] = CP;
                        if (!_shot)
                        {
                            _mainPanel.Grid[_bulletRow, _bulletCol] = 0;
                            _bulletCol--;
                            _mainPanel.Grid[_bulletRow, _bulletCol] = CN;
                        }
                        _mainPanel.HasChanges = true;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (_ship < _width - 2)
                    {
                        _mainPanel.Grid[_height - 1, _ship - 1] = 0;
                        _ship++;
                        _mainPanel.Grid[_height - 1, _ship + 1] = CP;
                        if (!_shot)
                        {
                            _mainPanel.Grid[_bulletRow, _bulletCol] = 0;
                            _bulletCol++;
                            _mainPanel.Grid[_bulletRow, _bulletCol] = CN;
                        }
                        _mainPanel.HasChanges = true;
                    }
                    break;
                case ConsoleKey.Spacebar:
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

            // lost
            for (int i = 0; i < _width; i++)
                if (_mainPanel.Grid[_height - 1, i] > 0)
                {
                    IsRunning = false;
                    return;
                }

            if (_skipFrame <= 0)
            {
                _skipFrame = BS;
                // animate invaders
                for (int i = 0; i < _height; i++)
                    if (_movingRight && _mainPanel.Grid[i, _width - 1] > 0 ||
                     !_movingRight && _mainPanel.Grid[i, 0] > 0) // switch sides
                    {
                        for (int k = _height - 1; k >= 0; k--)
                            for (int j = 0; j < _width; j++)
                                _mainPanel.Grid[k, j] = k == 0 ? 0 : _mainPanel.Grid[k - 1, j];

                        _movingRight = !_movingRight;
                        break;
                    }
                if (_movingRight)
                    for (int i = 0; i < _height; i++)
                        for (int j = _width - 1; j >= 0; j--)
                            _mainPanel.Grid[i, j] = j == 0 ? 0 : _mainPanel.Grid[i, j - 1];
                else
                    for (int i = 0; i < _height; i++)
                        for (int j = 0; j < _width; j++)
                            _mainPanel.Grid[i, j] = j == _width - 1 ? 0 : _mainPanel.Grid[i, j + 1];
            }

            _skipFrame--;
            if (_shot)
            {
                // animate bullet
                if (_bulletRow <= 0)
                {
                    _mainPanel.Grid[_bulletRow, _bulletCol] = 0;
                    _shot = false;
                    _bulletRow = _height - 2;
                    _bulletCol = _ship;
                }
                else
                {
                    _bulletRow--;
                    // hit
                    if (_mainPanel.Grid[_bulletRow, _bulletCol] > 0)
                    {
                        int c = _bulletCol;
                        do
                            _mainPanel.Grid[_bulletRow, _bulletCol++] = 0;
                        while (_bulletCol < _width && _mainPanel.Grid[_bulletRow, _bulletCol] > 0);
                        while (c > 0 && _mainPanel.Grid[_bulletRow, --c] > 0)
                            _mainPanel.Grid[_bulletRow, c] = 0;

                        Score++;
                        _shot = false;
                        _bulletRow = _height - 2;
                        _bulletCol = _ship;
                    }
                }
            }

            _mainPanel.HasChanges = true;
        }
    }
}