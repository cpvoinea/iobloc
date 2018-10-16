using System;

namespace iobloc
{
    class BreakoutBoard : SinglePanelBoard
    {
        int CP => (int)_config.GetColor("PlayerColor");
        int CE => (int)_config.GetColor("EnemyColor");
        int CN => (int)_config.GetColor("NeutralColor");
        int BW => _config.GetInt("BlockWidth");
        int BS => _config.GetInt("BlockSpace");
        int BR => _config.GetInt("BlockRows");
        int B => BW + BS;

        int _paddle;
        int _ballCol;
        int _ballRow;
        double _ballX;
        double _ballY;
        double _angle;

        public override bool Won => Score == (_width / B + 1) * BR;

        internal BreakoutBoard() : base(Option.Breakout)
        {
            _paddle = _width / 2 - 2;
            _ballX = 5;
            _ballY = BR;
            _angle = 7 * Math.PI / 4;

            for (int row = 0; row < BR; row++) // set rows of blocks
                for (int col = 0; col < _width; col += B)
                    for (int i = 0; i < BW; i++)
                        _mainPanel.Grid[row, col + i] = CE;
            for (int i = -2; i <= 2; i++)
                _mainPanel.Grid[_height - 1, _paddle + i] = CP;
            _mainPanel.Grid[_ballRow, _ballCol] = CN;
        }

        public override void HandleInput(int key)
        {
            switch ((ConsoleKey)key)
            {
                case ConsoleKey.LeftArrow:
                    if (_paddle > 2)
                    {
                        _mainPanel.Grid[_height - 1, _paddle + 2] = 0;
                        _paddle--;
                        _mainPanel.Grid[_height - 1, _paddle - 2] = CP;
                        _mainPanel.HasChanges = true;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (_paddle < _width - 3)
                    {
                        _mainPanel.Grid[_height - 1, _paddle - 2] = 0;
                        _paddle++;
                        _mainPanel.Grid[_height - 1, _paddle + 2] = 0;
                        _mainPanel.HasChanges = true;
                    }
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

            bool lost;
            _angle = NextAngle(out lost);
            if (lost)
            {
                IsRunning = false;
                return;
            }
            _ballX += Math.Cos(_angle);
            _ballY -= Math.Sin(_angle);

            _mainPanel.Grid[_ballRow, _ballCol] = 0;
            _ballRow = (int)Math.Round(_ballY);
            _ballCol = (int)Math.Round(_ballX);
            _mainPanel.Grid[_ballRow, _ballCol] = CN;
            _mainPanel.HasChanges = true;
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

                if (newAngle < Math.PI) // moving upwards
                {
                    if (row < 0) // hitting ceiling
                        newAngle = 2 * Math.PI - newAngle;
                    else if (col < 0 || col >= _width) // hitting walls
                        newAngle = Math.PI - newAngle;
                    else if (_mainPanel.Grid[row, col] > 0) // hitting block
                    {
                        Break(row, col);
                        newAngle = 2 * Math.PI - newAngle;
                    }
                    else
                        hit = false;
                }
                else // moving downwards
                {
                    // vertical check
                    if (row >= _height) // hitting floor
                    {
                        lost = true;
                        newAngle = 2 * Math.PI - newAngle;
                    }
                    else if (col < 0 || col >= _width) // hitting walls
                        newAngle = 3 * Math.PI - newAngle;
                    else if (row == _height - 1 && Math.Abs(col - _paddle) <= 2) // hitting paddle
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
                    else if (_mainPanel.Grid[row, col] > 0) // hitting block
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
                _mainPanel.Grid[row, x + i] = 0;
            Score++;
        }
    }
}
