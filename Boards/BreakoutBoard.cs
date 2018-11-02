using System;

namespace iobloc
{
    class BreakoutBoard : BaseBoard
    {
        private int CP, CE, CN, BW, BS, BR;
        private int B => BW + BS;
        private int _paddle;
        private int _ballCol;
        private int _ballRow;
        private double _ballX;
        private double _ballY;
        private double _angle;

        public BreakoutBoard() : base(BoardType.Breakout) { }

        protected override void InitializeSettings()
        {
            base.InitializeSettings();
            CP = BoardSettings.GetColor(Settings.PlayerColor);
            CE = BoardSettings.GetColor(Settings.EnemyColor);
            CN = BoardSettings.GetColor(Settings.NeutralColor);
            BW = BoardSettings.GetInt("BlockWidth");
            BS = BoardSettings.GetInt("BlockSpace");
            BR = BoardSettings.GetInt("BlockRows");
        }

        protected override void Initialize()
        {
            if (IsInitialized)
                Main.Clear();
            else
                base.Initialize();
            _paddle = Width / 2 - 2;
            _ballX = _ballCol = 2;
            _ballY = _ballRow = BR + 2;
            _angle = 7 * Math.PI / 4;
            for (int row = 0; row < BR; row++)
                for (int col = 0; col < Width; col += B)
                    for (int i = 0; i < BW; i++)
                        Main[row, col + i] = CE;
            Change(true);
        }

        protected override void Change(bool set)
        {
            for (int i = -2; i <= 2; i++)
                Main[Height - 1, _paddle + i] = set ? CP : 0;
            if (_ballRow >= Height - 1)
                Lose();
            else
            {
                Main[_ballRow, _ballCol] = set ? CN : 0;
                base.Change(set);
            }
        }

        protected override void SetLevel(int level)
        {
            base.SetLevel(level);
            Initialize();
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case UIKeys.LeftArrow:
                    if (_paddle > 2)
                    {
                        Change(false);
                        _paddle--;
                        Change(true);
                    }
                    break;
                case UIKeys.RightArrow:
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

            _angle = NextAngle();
            _ballX += Math.Cos(_angle);
            _ballY -= Math.Sin(_angle);
            Change(true);
        }

        private double NextAngle()
        {
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
                        newAngle = 2 * Math.PI - newAngle;
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

        private void Break(int row, int col)
        {
            int x = (col / B) * B;
            for (int i = 0; i < BW; i++)
                Main[row, x + i] = 0;
            Score++;
        }
    }
}
