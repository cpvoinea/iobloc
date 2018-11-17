using System;

namespace iobloc
{
    class Tetris : BaseGame
    {
        private readonly Random _random = new Random();
        private TetrisPiece _piece;

        public Tetris() : base(GameType.Tetris) { }

        protected override void Initialize()
        {
            base.Initialize();
            if (IsInitialized)
                Main.Clear();
            _piece = NewPiece();
            Change(true);
        }

        protected override void Change(bool set)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (_piece.Mask[i, j] > 0)
                    {
                        int gx = _piece.X - 1 + i;
                        int gy = (_piece.Y - 2 + j) * BlockWidth;
                        if (gx >= 0)
                            for (int k = 0; k < BlockWidth; k++)
                                Main[gx, gy + k] = set ? (int)_piece.Color : 0;
                    }
            base.Change(set);
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case UIKey.LeftArrow: MoveLeft(); break;
                case UIKey.RightArrow: MoveRight(); break;
                case UIKey.DownArrow: NextFrame(); break; // accelerate to next frame
                case UIKey.UpArrow: Rotate(); break;
            }
        }

        public override void NextFrame()
        {
            var p = _piece.Down();
            Change(false);
            if (CanSet(p))
            {
                _piece = p;
                Change(true); // move down
            }
            else
            {
                if (!CanSet(_piece, false)) // is outside the box
                {
                    Change(true);
                    Lose();
                }
                else
                {
                    Change(true); // set piece
                    RemoveRows();
                    _piece = NewPiece();
                    if (!CanSet(_piece)) // cannot enter new piece
                    {
                        Change(true);
                        Lose();
                    }
                    else
                        Change(true);
                }
            }
        }

        private bool CanSet(TetrisPiece piece, bool ignoreTop = true)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (piece.Mask[i, j] > 0)
                    {
                        int gx = piece.X - 1 + i;
                        int gy = (piece.Y - 2 + j) * BlockWidth;
                        bool free = gy >= 0 && gy < Width && gx < Height &&
                            (gx < 0 && ignoreTop || gx >= 0 && Main[gx, gy] == 0);
                        if (!free)
                            return false;
                    }

            return true;
        }

        private TetrisPiece NewPiece()
        {
            return new TetrisPiece(_random.Next(7) + 1, _random.Next(4));
        }

        private void Rotate()
        {
            var p = _piece.Rotate();
            Change(false);
            if (CanSet(p))
                _piece = p;
            Change(true);
        }

        private void MoveLeft()
        {
            var p = _piece.Left();
            Change(false);
            if (CanSet(p))
                _piece = p;
            Change(true);
        }

        private void MoveRight()
        {
            var p = _piece.Right();
            Change(false);
            if (CanSet(p))
                _piece = p;
            Change(true);
        }

        private void RemoveRows()
        {
            int series = 0;
            for (int i = Height - 1; i >= 0; i--)
            {
                bool line = true;
                int j = 0;
                while (line && j < Width)
                    line &= Main[i, j++] > 0;
                if (line)
                {
                    for (int k = i; k >= 0; k--)
                        for (int l = 0; l < Width; l++)
                            Main[k, l] = k == 0 ? 0 : Main[k - 1, l];
                    i++;
                    series++;
                }
            }
            if (series == 4)
                Score += 10;
            else if (series == 3)
                Score += 6;
            else if (series == 2)
                Score += 3;
            else if (series == 1)
                Score += 1;
        }
    }
}
