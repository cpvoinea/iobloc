using System;

namespace iobloc
{
    class TetrisBoard : SinglePanelBoard
    {
        readonly Random _random = new Random();
        TetrisPiece _piece;

        internal TetrisBoard() : base(Option.Tetris)
        {
            _piece = NewPiece();
            SetPiece();
        }

        public override void HandleInput(string key)
        {
            switch (key)
            {
                case "UpArrow": Rotate(); break;
                case "LeftArrow": MoveLeft(); break;
                case "RightArrow": MoveRight(); break;
                case "DownArrow": MoveDown(); break;
            }
        }

        public override void NextFrame()
        {
            MoveDown();
        }

        bool CheckGridPiece(TetrisPiece piece, bool partiallyEntered, bool? set = null)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                    if (piece.Mask[i, j] > 0)
                    {
                        int gx = piece.X - 1 + i;
                        int gy = piece.Y - 2 + j;
                        if (gx >= _height || gy < 0 || gy >= _width ||
                            (!partiallyEntered && gx < 0) ||
                            (partiallyEntered && gx >= 0 && _main.Grid[gx, gy] > 0))
                            return false;
                        if (gx >= 0 && set.HasValue)
                        {
                            if (set.Value)
                                _main.Grid[gx, gy] = (int)piece.Type;
                            else
                                _main.Grid[gx, gy] = 0;
                        }
                    }
            }

            return true;
        }

        TetrisPiece NewPiece()
        {
            return new TetrisPiece(_random.Next(7) + 1, _random.Next(4));
        }

        bool Collides(TetrisPiece piece)
        {
            return !CheckGridPiece(piece, true, false);
        }

        void SetPiece()
        {
            CheckGridPiece(_piece, true, true);
        }

        void UnsetPiece()
        {
            CheckGridPiece(_piece, true, false);
        }

        void Rotate()
        {
            var p = _piece.Rotate();
            if (Collides(p))
                return;

            UnsetPiece();
            _piece = p;
            SetPiece();
            _main.HasChanges = true;
        }

        void MoveLeft()
        {
            var p = _piece.Left();
            if (Collides(p))
                return;

            UnsetPiece();
            _piece = p;
            SetPiece();
            _main.HasChanges = true;
        }

        void MoveRight()
        {
            var p = _piece.Right();
            if (Collides(p))
                return;

            UnsetPiece();
            _piece = p;
            SetPiece();
            _main.HasChanges = true;
        }

        void MoveDown()
        {
            var p = _piece.Down();
            if (Collides(p))
            {
                if (!CheckGridPiece(_piece, false, true))
                {
                    IsRunning = false;
                    return;
                }

                RemoveRows();
                _piece = NewPiece();
                if (!CheckGridPiece(_piece, true, true))
                {
                    IsRunning = false;
                    return;
                }

                _main.HasChanges = true;
                return;
            }

            UnsetPiece();
            _piece = p;
            SetPiece();
            _main.HasChanges = true;
        }

        void RemoveRows()
        {
            int series = 0;
            for (int i = _height - 1; i >= 0; i--)
            {
                bool line = true;
                int j = 0;
                while (line && j < _width)
                    line &= _main.Grid[i, j++] > 0;
                if (line)
                {
                    for (int k = i; k >= 0; k--)
                        for (int l = 0; l < _width; l++)
                            _main.Grid[k, l] = k == 0 ? 0 : _main.Grid[k - 1, l];
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
