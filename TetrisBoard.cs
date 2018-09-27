using System;

namespace iobloc
{
    class TetrisBoard : IBoard
    {
        internal enum MoveType { Rotate, MoveLeft, MoveRight, MoveDown, Next }

        const int WIDTH = 10;
        const int HEIGHT = 20;

        readonly Random _random = new Random();
        readonly int[,] _grid = new int[HEIGHT, WIDTH];
        Piece _piece;

        internal TetrisBoard()
        {
            _piece = NewPiece();
        }

        public int Width { get { return WIDTH; } }
        public int Height { get { return HEIGHT; } }

        public int[,] Grid
        {
            get
            {
                int[,] result = CopyGrid();
                CheckGridPiece(result, _piece, true, true);
                return result;
            }
        }

        public bool Move(object action)
        {
            switch ((MoveType)action)
            {
                case MoveType.Rotate: return Rotate();
                case MoveType.MoveLeft: return MoveLeft();
                case MoveType.MoveRight: return MoveRight();
                case MoveType.MoveDown: return MoveDown();
                case MoveType.Next: return Next();
                default: return false;
            }
        }

        Piece NewPiece()
        {
            return new Piece((PieceType)(_random.Next(7) + 1), _random.Next(4));
        }

        int[,] CopyGrid()
        {
            int[,] result = new int[HEIGHT, WIDTH];
            for (int i = 0; i < HEIGHT; i++)
                for (int j = 0; j < WIDTH; j++)
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
                        if (gx >= HEIGHT || gy < 0 || gy >= WIDTH ||
                            (!partiallyEntered && gx < 0) ||
                            (partiallyEntered && gx >= 0 && grid[gx, gy] > 0))
                            return false;
                        if (set && gx >= 0)
                            grid[gx, gy] = (int)piece.Type;
                    }
            }

            return true;
        }

        bool Collides(Piece piece)
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
            for (int i = HEIGHT - 1; i >= 0; i--)
            {
                bool line = true;
                int j = 0;
                while (line && j < WIDTH)
                    line &= _grid[i, j++] > 0;
                if (line)
                {
                    for (int k = i; k >= 0; k--)
                        for (int l = 0; l < WIDTH; l++)
                            _grid[k, l] = k == 0 ? 0 : _grid[k - 1, l];
                    i++;
                }
            }
        }
    }
}