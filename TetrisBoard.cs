using System;

namespace iobloc
{
    class TetrisBoard : IBoard
    {
        public string[] Help { get { return Settings.Tetris.HELP; } }
        public ConsoleKey[] Keys { get { return Settings.Tetris.KEYS; } }
        public int StepInterval { get { return Settings.Tetris.INTERVAL; } }
        public int Width { get { return Settings.Tetris.WIDTH; } }
        public int Height { get { return Settings.Tetris.HEIGHT; } }
        public int[,] Grid
        {
            get
            {
                var result = _grid.Copy(Height, Width);
                CheckGridPiece(result, _piece, true, true);
                return result;
            }
        }
        public int Score { get { return _score; } }

        readonly Random _random = new Random();
        readonly int[,] _grid;
        TetrisPiece _piece;
        int _score;

        internal TetrisBoard()
        {
            _piece = NewPiece();
            _grid = new int[Height, Width];
        }

        public bool Action(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow: return Rotate();
                case ConsoleKey.LeftArrow: return MoveLeft();
                case ConsoleKey.RightArrow: return MoveRight();
                case ConsoleKey.DownArrow: return MoveDown();
                default: return false;
            }
        }

        public bool Step()
        {
            return MoveDown() || Next();
        }

        bool CheckGridPiece(int[,] grid, TetrisPiece piece, bool partiallyEntered, bool set)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                    if (piece.Mask[i, j] > 0)
                    {
                        int gx = piece.X - 1 + i;
                        int gy = piece.Y - 2 + j;
                        if (gx >= Height || gy < 0 || gy >= Width ||
                            (!partiallyEntered && gx < 0) ||
                            (partiallyEntered && gx >= 0 && grid[gx, gy] > 0))
                            return false;
                        if (set && gx >= 0)
                            grid[gx, gy] = (int)piece.Type;
                    }
            }

            return true;
        }

        TetrisPiece NewPiece()
        {
            return new TetrisPiece(_random.Next(7), _random.Next(4));
        }

        bool Collides(TetrisPiece piece)
        {
            return !CheckGridPiece(_grid, piece, true, false);
        }

        bool Rotate()
        {
            var p = _piece.Rotate();
            if (Collides(p))
                return false;
            _piece = p;
            return true;
        }

        bool MoveLeft()
        {
            var p = _piece.Left();
            if (Collides(p))
                return false;
            _piece = p;
            return true;
        }

        bool MoveRight()
        {
            var p = _piece.Right();
            if (Collides(p))
                return false;
            _piece = p;
            return true;
        }

        bool MoveDown()
        {
            var p = _piece.Down();
            if (Collides(p))
                return false;
            _piece = p;
            return true;
        }

        bool Next()
        {
            if (!CheckGridPiece(_grid, _piece, false, true))
                return false;
            RemoveRows();
            _piece = NewPiece();
            if (!CheckGridPiece(_grid, _piece, true, false))
                return false;
            return true;
        }

        void RemoveRows()
        {
            int series = 0;
            for (int i = Height - 1; i >= 0; i--)
            {
                bool line = true;
                int j = 0;
                while (line && j < Width)
                    line &= _grid[i, j++] > 0;
                if (line)
                {
                    for (int k = i; k >= 0; k--)
                        for (int l = 0; l < Width; l++)
                            _grid[k, l] = k == 0 ? 0 : _grid[k - 1, l];
                    i++;
                    series++;
                }
            }
            if (series == 4)
                _score += 10;
            else if (series == 3)
                _score += 6;
            else if (series == 2)
                _score += 3;
            else if (series == 1)
                _score += 1;
        }
    }
}