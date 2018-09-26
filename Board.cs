using System;

namespace iobloc
{
    class Board
    {
        readonly Random _random = new Random();
        readonly int[,] _grid = new int[20, 10];
        Piece _piece;

        internal Board()
        {
            _piece = NewPiece();
        }

        Piece NewPiece()
        {
            return new Piece((PieceType)(_random.Next(7) + 1), _random.Next(4));
        }

        int[,] CopyGrid()
        {
            int[,] result = new int[20, 10];
            for (int i = 0; i < 20; i++)
                for (int j = 0; j < 10; j++)
                    result[i, j] = _grid[i, j];
            return result;
        }

        static bool CheckGridPiece(int[,] grid, Piece piece, bool partiallyEntered, bool set)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                    if (piece.Mask[i, j] > 0)
                    {
                        int gx = piece.X - 1 + i;
                        int gy = piece.Y - 2 + j;
                        if (gx >= 20 || gy < 0 || gy >= 10 ||
                            (!partiallyEntered && gx < 0) ||
                            (partiallyEntered && gx >= 0 && grid[gx, gy] > 0))
                            return false;
                        if (set && gx >= 0)
                            grid[gx, gy] = (int)piece.Type;
                    }
            }

            return true;
        }

        internal int[,] GetGridWithPiece()
        {
            int[,] result = CopyGrid();
            CheckGridPiece(result, _piece, true, true);
            return result;
        }

        bool Collides(Piece piece)
        {
            return !CheckGridPiece(_grid, piece, true, false);
        }

        internal bool Rotate()
        {
            var p = _piece.Rotate();
            if (Collides(p))
                return false;
            _piece = p;
            return true;
        }

        internal bool MoveLeft()
        {
            var p = _piece.Left();
            if (Collides(p))
                return false;
            _piece = p;
            return true;
        }

        internal bool MoveRight()
        {
            var p = _piece.Right();
            if (Collides(p))
                return false;
            _piece = p;
            return true;
        }

        internal bool MoveDown()
        {
            var p = _piece.Down();
            if (Collides(p))
                return false;
            _piece = p;
            return true;
        }

        internal bool Next()
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
            for (int i = 19; i >= 0; i--)
            {
                bool line = true;
                int j = 0;
                while (line && j < 10)
                    line &= _grid[i, j++] > 0;
                if (line)
                {
                    for (int k = i; k >= 0; k--)
                        for (int l = 0; l < 10; l++)
                            _grid[k, l] = k == 0 ? 0 : _grid[k - 1, l];
                    i++;
                }
            }
        }

        void Print()
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 10; j++)
                    Console.Write(_grid[i, j]);
                Console.WriteLine();
            }
        }
    }
}