using System;

namespace iobloc
{
    class BreakoutBoard : SinglePanelBoard
    {
        int CP => _settings.GetColor("PlayerColor");
        int CE => _settings.GetColor("EnemyColor");
        int CN => _settings.GetColor("NeutralColor");
        int BW => _settings.GetInt("BlockWidth");
        int BS => _settings.GetInt("BlockSpace");
        int BR => _settings.GetInt("BlockRows");
        int B => BW + BS;
        int WinScore => (_width + BS) / B * BR;
        int _targets;
        int _paddle;
        int _ballCol;
        int _ballRow;
        double _ballX;
        double _ballY;
        double _angle;

        internal BreakoutBoard() : base(Option.Breakout)
        {
            InitializeGrid();
        }

        protected override void InitializeGrid()
        {
            _paddle = _width / 2 - 2;
            _ballX = _ballCol = 2;
            _ballY = _ballRow = BR + 2;
            _angle = 7 * Math.PI / 4;
            _targets = WinScore;

            for (int row = 0; row < BR; row++)
                for (int col = 0; col < _width; col += B)
                    for (int i = 0; i < BW; i++)
                        _main.Grid[row, col + i] = CE;

            ChangeGrid(true);
        }

        protected override void ChangeGrid(bool set)
        {
            for (int i = -2; i <= 2; i++)
                _main.Grid[_height - 1, _paddle + i] = set ? CP : 0;
            _main.Grid[_ballRow, _ballCol] = set ? CN : 0;
            if (set)
                _main.HasChanges = true;
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case "LeftArrow":
                    if (_paddle > 2)
                    {
                        ChangeGrid(false);
                        _paddle--;
                        ChangeGrid(true);
                    }
                    break;
                case "RightArrow":
                    if (_paddle < _width - 3)
                    {
                        ChangeGrid(false);
                        _paddle++;
                        ChangeGrid(true);
                    }
                    break;
            }
        }

        public override void NextFrame()
        {
            if (_targets == 0)
            {
                Level++;
                if (Level >= Config.LEVEL_MAX)
                {
                    Win = true;
                    IsRunning = false;
                }
                else
                {
                    ChangeGrid(false);
                    InitializeGrid();
                }
                return;
            }

            ChangeGrid(false);
            _ballRow = (int)Math.Round(_ballY);
            _ballCol = (int)Math.Round(_ballX);

            bool lost;
            _angle = NextAngle(out lost);
            if (lost)
            {
                Win = false;
                IsRunning = false;
                return;
            }
            _ballX += Math.Cos(_angle);
            _ballY -= Math.Sin(_angle);

            ChangeGrid(true);
        }

        double NextAngle(out bool lost)
        {
            lost = false;

            double newAngle = _angle;
            bool hit;
            do
            {
                hit = true;
                double newX = _ballX + Math.Cos(newAngle);
                double newY = _ballY - Math.Sin(newAngle);
                int row = (int)Math.Round(newY);
                int col = (int)Math.Round(newX);

                if (newAngle < Math.PI)
                {
                    if (row < 0)
                        newAngle = 2 * Math.PI - newAngle;
                    else if (col < 0 || col >= _width)
                        newAngle = Math.PI - newAngle;
                    else if (_main.Grid[row, col] == CE)
                    {
                        Break(row, col);
                        newAngle = 2 * Math.PI - newAngle;
                    }
                    else
                        hit = false;
                }
                else
                {
                    if (row >= _height)
                    {
                        lost = true;
                        newAngle = 2 * Math.PI - newAngle;
                    }
                    else if (col < 0 || col >= _width)
                        newAngle = 3 * Math.PI - newAngle;
                    else if (row == _height - 1 && Math.Abs(col - _paddle) <= 2)
                    {
                        int p = col - _paddle;
                        double a = 2 * Math.PI - newAngle;
                        switch (p)
                        {
                            case -2: newAngle = 3 * Math.PI / 4; break;
                            case -1: newAngle = Math.Min(a + Math.PI / 4, 3 * Math.PI / 4); break;
                            case 0: newAngle = a; break;
                            case 1: newAngle = Math.Max(a - Math.PI / 4, Math.PI / 4); break;
                            case 2: newAngle = Math.PI / 4; break;
                        }
                    }
                    else if (_main.Grid[row, col] > 0)
                    {
                        Break(row, col);
                        newAngle = 2 * Math.PI - newAngle;
                    }
                    else
                        hit = false;
                }
            } while (hit);

            return newAngle;
        }

        void Break(int row, int col)
        {
            int x = (col / B) * B;
            for (int i = 0; i < BW; i++)
                _main.Grid[row, x + i] = 0;
            Score++;
            _targets--;
        }
    }
}
