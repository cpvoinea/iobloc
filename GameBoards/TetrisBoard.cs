using System;

namespace iobloc
{
    /// <summary>
    /// Tetris game
    /// </summary>
    class TetrisBoard : IBoard
    {
        #region Settings
        public string[] Help => Settings.Tetris.HELP;
        public ConsoleKey[] Keys => Settings.Tetris.KEYS;
        public int Width => Settings.Tetris.WIDTH;
        public int Height => Settings.Tetris.HEIGHT;
        public bool Won => false;
        #endregion

        /// <summary>
        /// Current score
        /// </summary>
        int _score;
        /// <summary>
        /// Used for next piece
        /// </summary>
        readonly Random _random = new Random();
        /// <summary>
        /// Fixed pieces
        /// </summary>
        readonly int[,] _grid;
        /// <summary>
        /// Current piece
        /// </summary>
        TetrisPiece _piece;

        public int StepInterval { get { return Settings.Game.LevelInterval * Settings.Tetris.INTERVALS; } }

        /// <summary>
        /// Fixed pieces + current piece
        /// </summary>
        public int[,] Grid
        {
            get
            {
                var result = _grid.Copy(Height, Width);
                CheckGridPiece(result, _piece, true, true);
                return result;
            }
        }

        /// <summary>
        /// Current score
        /// </summary>
        public int Score { get { return _score; } }

        public int[] Clip { get { return new[] { 0, 0, Width, Height }; } }

        /// <summary>
        /// Tetris game
        /// </summary>
        internal TetrisBoard()
        {
            _piece = NewPiece();
            _grid = new int[Height, Width];
        }

        /// <summary>
        /// Perform apropriate action
        /// </summary>
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

        /// <summary>
        /// Move current piece down or bring in a new piece if bottom was reached
        /// </summary>
        /// <returns>false if game over</returns>
        public bool Step()
        {
            return MoveDown()   // try to move down, if not able then
                || Next();      // try to bring in a new piece
        }

        /// <summary>
        /// Helper method to check for collisions and set grid
        /// </summary>
        /// <param name="grid">fixed pieces grid</param>
        /// <param name="piece">mobile piece</param>
        /// <param name="partiallyEntered">ignore collision if mobile piece is partially outside the upper bounds of the grid</param>
        /// <param name="set">permanently fix the mobile piece to the grid</param>
        /// <returns>true if collission was detected (keeping account of partiallyEntered option)</returns>
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

        /// <summary>
        /// Randomly generate a new piece
        /// </summary>
        /// <returns>new piece</returns>
        TetrisPiece NewPiece()
        {
            return new TetrisPiece(_random.Next(7) + 1, _random.Next(4));
        }

        /// <summary>
        /// Check if this piece is colliding with the fixed grid
        /// </summary>
        /// <param name="piece">piece to check</param>
        /// <returns>true if piece overlaps the fixed pieces</returns>
        bool Collides(TetrisPiece piece)
        {
            return !CheckGridPiece(_grid, piece, true, false);
        }

        /// <summary>
        /// Try to rotate piece in place
        /// </summary>
        /// <returns>true if rotation was successful, false if collision detected</returns>
        bool Rotate()
        {
            var p = _piece.Rotate();
            if (Collides(p))
                return false;
            _piece = p;
            return true;
        }

        /// <summary>
        /// Try to shieft piece to left
        /// </summary>
        /// <returns>false if collision detected</returns>
        bool MoveLeft()
        {
            var p = _piece.Left();
            if (Collides(p))
                return false;
            _piece = p;
            return true;
        }

        /// <summary>
        /// Try to shieft piece to right
        /// </summary>
        /// <returns>false if collision detected</returns>
        bool MoveRight()
        {
            var p = _piece.Right();
            if (Collides(p))
                return false;
            _piece = p;
            return true;
        }

        /// <summary>
        /// Try to shieft piece down
        /// </summary>
        /// <returns>false if collision detected</returns>
        bool MoveDown()
        {
            var p = _piece.Down();
            if (Collides(p))
                return false;
            _piece = p;
            return true;
        }

        /// <summary>
        /// If there is currently an overlap of mobile and fixed pieces, the game is over.
        /// Otherwise remove completed lines and try to insert a new piece.
        /// </summary>
        /// <returns>false if game over</returns>
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

        /// <summary>
        /// Remove completed lines and add to score
        /// </summary>
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

        public override string ToString()
        {
            return "Tetris";
        }
    }
}