using System;

namespace iobloc
{
    class Breakout : BaseGame
    {
        private int CP, CE, CN, BR;
        private int LT => (Width + BlockSpace) / Block * BR;
        private int _paddle;
        private int _ballCol;
        private int _ballRow;
        private double _ballX;
        private double _ballY;
        private double _angle;
        private int _targets = int.MaxValue;

        public Breakout() : base(GameType.Breakout) { }

        protected override void InitializeSettings()
        {
            base.InitializeSettings();
            CP = Serializer.GetColor(GameSettings, Settings.PlayerColor);
            CE = Serializer.GetColor(GameSettings, Settings.EnemyColor);
            CN = Serializer.GetColor(GameSettings, Settings.NeutralColor);
            BR = Serializer.GetInt(GameSettings, "BlockRows");
        }

        protected override void Initialize()
        {
            if (IsInitialized)
                Main.Clear();
            else // don't reset the score when level is reset
                base.Initialize();
            _paddle = Width / 2 - 2;
            _ballX = _ballCol = 2;
            _ballY = _ballRow = BR + 2;
            _angle = 7 * Math.PI / 4;
            for (int row = 0; row < BR; row++)
                for (int col = 0; col < Width; col += Block)
                    for (int i = 0; i < BlockWidth; i++)
                        Main[row, col + i] = new PaneCell(CE);
            _targets = LT;
            Change(true);
        }

        protected override void Change(bool set)
        {
            for (int i = -2; i <= 2; i++)
                Main[Height - 1, _paddle + i] = new PaneCell(set ? CP : 0);
            if (set && _ballRow >= Height - 1)
            {
                base.Initialize(); // reset score and level
                Lose();
            }
            else
            {
                Main[_ballRow, _ballCol] = new PaneCell(set ? CN : 0);
                base.Change(set);
            }
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case UIKey.LeftArrow:
                    if (_paddle > 2)
                    {
                        Change(false);
                        _paddle--;
                        Change(true);
                    }
                    break;
                case UIKey.RightArrow:
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
                    else if (Main[row, col].Color == CE)
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
                    else if (Main[row, col].Color > 0)
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
            int x = (col / Block) * Block;
            for (int i = 0; i < BlockWidth; i++)
                Main[row, x + i] = new PaneCell(0);
            Score++;
            _targets--;
            if (_targets == 0)
            {
                Level++;
                Initialize();
            }
        }
    }
}
