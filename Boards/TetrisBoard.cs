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
            _piece = NewPiece();
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
            Change(false);
            switch (key)
            {
                case "UpArrow": Rotate(); break;
                case "LeftArrow": MoveLeft(); break;
                case "RightArrow": MoveRight(); break;
                case "DownArrow": MoveDown(); break;
            }
            Change(true);
        }

        public override void NextFrame()
        {
            Change(false);
            MoveDown();
            Change(true);
        }

        TetrisPiece NewPiece()
        {
            return new TetrisPiece(_random.Next(7) + 1, _random.Next(4));
        }

        bool CanSet(TetrisPiece piece, bool partiallyEntered = true)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (piece.Mask[i, j] > 0)
                    {
                        int gx = piece.X - 1 + i;
                        int gy = piece.Y - 2 + j;
                        if (gx >= Height || gy < 0 || gy >= Width ||
                            (!partiallyEntered && gx < 0) ||
                            (gx >= 0 && Main[gx, gy] > 0))
                            return false;
                    }

            return true;
        }

        void Rotate()
        {
            var p = _piece.Rotate();
            if (CanSet(p))
                _piece = p;
        }

        void MoveLeft()
        {
            var p = _piece.Left();
            if (CanSet(p))
                _piece = p;
        }

        void MoveRight()
        {
            var p = _piece.Right();
            if (CanSet(p))
                _piece = p;
        }

        void MoveDown()
        {
            var p = _piece.Down();
            if (CanSet(p))
                _piece = p;
            else
            {
                if (!CanSet(_piece, false))
                {
                    Stop();
                    return;
                }
                Change(true);

                RemoveRows();
                _piece = NewPiece();
                if (!CanSet(_piece))
                {
                    Stop();
                    return;
                }

                Change(true);
                Main.HasChanges = true;
                return;
            }
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
