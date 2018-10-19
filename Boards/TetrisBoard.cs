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
            ChangeGrid(true);
        }

        protected override void ChangeGrid(bool set)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (_piece.Mask[i, j] > 0)
                    {
                        int gx = _piece.X - 1 + i;
                        int gy = _piece.Y - 2 + j;
                        if (gx >= 0 && gx < _height && gy >= 0 && gy < _width)
                            _main.Grid[gx, gy] = set ? (int)_piece.Type : 0;
                    }
            if (set)
                _main.HasChanges = true;
        }

        public override void HandleInput(string key)
        {
            ChangeGrid(false);
            switch (key)
            {
                case "UpArrow": Rotate(); break;
                case "LeftArrow": MoveLeft(); break;
                case "RightArrow": MoveRight(); break;
                case "DownArrow": MoveDown(); break;
            }
            ChangeGrid(true);
        }

        public override void NextFrame()
        {
            ChangeGrid(false);
            MoveDown();
            ChangeGrid(true);
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
                        if (gx >= _height || gy < 0 || gy >= _width ||
                            (!partiallyEntered && gx < 0) ||
                            (gx >= 0 && _main.Grid[gx, gy] > 0))
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
                    Win = false;
                    IsRunning = false;
                    return;
                }
                ChangeGrid(true);

                RemoveRows();
                _piece = NewPiece();
                if (!CanSet(_piece))
                {
                    Win = false;
                    IsRunning = false;
                    return;
                }

                ChangeGrid(true);
                _main.HasChanges = true;
                return;
            }
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
