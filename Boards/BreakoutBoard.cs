using System;

namespace iobloc
{
    class BreakoutBoard : BaseBoard
    {
        int CP => BoardSettings.GetColor(Settings.PlayerColor);
        int CE => BoardSettings.GetColor(Settings.EnemyColor);
        int CN => BoardSettings.GetColor(Settings.NeutralColor);
        int BW => BoardSettings.GetInt("BlockWidth");
        int BS => BoardSettings.GetInt("BlockSpace");
        int BR => BoardSettings.GetInt("BlockRows");
        int B => BW + BS;

        int _paddle;
        int _ballCol;
        int _ballRow;
        double _ballX;
        double _ballY;
        double _angle;

        public BreakoutBoard() : base(BoardType.Breakout) { }

        public override void Reset()
        {
            _paddle = Width / 2 - 2;
            _ballX = _ballCol = 2;
            _ballY = _ballRow = BR + 2;
            _angle = 7 * Math.PI / 4;

            for (int row = 0; row < BR; row++)
                for (int col = 0; col < Width; col += B)
                    for (int i = 0; i < BW; i++)
                        Main[row, col + i] = CE;
            Main.HasChanges = true;

            Initialize();
        }

        public override void Change(bool set)
        {
            for (int i = -2; i <= 2; i++)
                Main[Height - 1, _paddle + i] = set ? CP : 0;
            Main[_ballRow, _ballCol] = set ? CN : 0;
            if(set)
                Main.HasChanges = true;
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case "LeftArrow":
                    if (_paddle > 2)
                    {
                        Change(false);
                        _paddle--;
                        Change(true);
                    }
                    break;
                case "RightArrow":
                    if (_paddle < Width - 3)
                    {
                        Change(false);
                        _paddle++;
                        Change(true);
                    }
                    break;
            }
        }

        public override void NextFrame()
        {
            Change(false);
            _ballRow = (int)Math.Round(_ballY);
            _ballCol = (int)Math.Round(_ballX);

            bool lost;
            _angle = NextAngle(out lost);
            if (lost)
            {
                Stop();
                return;
            }
            _ballX += Math.Cos(_angle);
            _ballY -= Math.Sin(_angle);

            Change(true);
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
                    else if (col < 0 || col >= Width)
                        newAngle = Math.PI - newAngle;
                    else if (Main[row, col] == CE)
                    {
                        Break(row, col);
                        newAngle = 2 * Math.PI - newAngle;
                    }
                    else
                        hit = false;
                }
                else
                {
                    if (row >= Height)
                    {
                        lost = true;
                        newAngle = 2 * Math.PI - newAngle;
                    }
                    else if (col < 0 || col >= Width)
                        newAngle = 3 * Math.PI - newAngle;
                    else if (row == Height - 1 && Math.Abs(col - _paddle) <= 2)
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
                    else if (Main[row, col] > 0)
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
                Main[row, x + i] = 0;
            Score++;
        }
    }
}
