using System;

namespace iobloc
{
    class TetrisBoard : BaseBoard
    {
        readonly Random _random = new Random();
        TetrisPiece _piece;

        public TetrisBoard() : base(BoardType.Tetris) { }

        public override void Initialize()
        {
            base.Initialize();
            if (IsInitialized)
            {
                Main.Clear();
                SetScore(0);
            }
            _piece = NewPiece();
            Change(true);
        }

        public override void Change(bool set)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (_piece.Mask[i, j] > 0)
                    {
                        int gx = _piece.X - 1 + i;
                        int gy = _piece.Y - 2 + j;
                        if (gx >= 0 && gx < Height && gy >= 0 && gy < Width)
                            Main[gx, gy] = set ? (int)_piece.Color : 0;
                    }
            if (set)
                Main.HasChanges = true;
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case UIKeys.LeftArrow: MoveLeft(); break;
                case UIKeys.RightArrow: MoveRight(); break;
                case UIKeys.DownArrow: NextFrame(); break;
                case UIKeys.UpArrow: Rotate(); break;
            }
        }

        public override void NextFrame()
        {
            var p = _piece.Down();
            Change(false);
            if (CanSet(p))
            {
                _piece = p;
                Change(true);
            }
            else
            {
                if (!CanSet(_piece, false))
                {
                    Change(true);
                    Lose();
                }
                else
                {
                    Change(true);
                    RemoveRows();
                    _piece = NewPiece();
                    if (!CanSet(_piece))
                    {
                        Change(true);
                        Lose();
                    }
                    else
                        Change(true);
                }
            }
        }

        bool CanSet(TetrisPiece piece, bool partiallyEntered = true)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (piece.Mask[i, j] > 0)
                    {
                        int gx = piece.X - 1 + i;
                        int gy = piece.Y - 2 + j;
                        if (!(gx < Height && gy >= 0 && gy < Width) ||
                            (gx < 0 && !partiallyEntered) ||
                            (gx >= 0 && Main[gx, gy] > 0))
                            return false;
                    }

            return true;
        }

        TetrisPiece NewPiece()
        {
            return new TetrisPiece(_random.Next(7) + 1, _random.Next(4));
        }

        void Rotate()
        {
            var p = _piece.Rotate();
            Change(false);
            if (CanSet(p))
                _piece = p;
            Change(true);
        }

        void MoveLeft()
        {
            var p = _piece.Left();
            Change(false);
            if (CanSet(p))
                _piece = p;
            Change(true);
        }

        void MoveRight()
        {
            var p = _piece.Right();
            Change(false);
            if (CanSet(p))
                _piece = p;
            Change(true);
        }

        void RemoveRows()
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
