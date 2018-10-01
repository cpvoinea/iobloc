namespace iobloc
{
    struct TetrisPiece
    {
        /// <summary>
        /// 1-based list of piece types, each int value has a color corespondent in ConsoleColor
        /// </summary>
        enum PieceType { I = 1, O, T, L, J, S, Z }

        /// <summary>
        /// Current orientation
        /// </summary>
        int Orientation { get; set; }
        internal int Type { get; private set; }
        /// <summary>
        /// A 4x4 mask of piece
        /// </summary>
        internal int[,] Mask { get; private set; }
        /// <summary>
        /// Distance from top
        /// </summary>
        internal int X { get; private set; }
        /// <summary>
        /// Distance from left
        /// </summary>
        internal int Y { get; private set; }

        /// <summary>
        /// Initialize piece
        /// </summary>
        /// <param name="type">only 1-7 values allowed</param>
        /// <param name="orientation">only 0-3 values allowed</param>
        internal TetrisPiece(int type, int orientation)
        {
            Type = type;
            Orientation = orientation;
            // mask is set once and changed only on rotations
            Mask = GetMask((PieceType)type, orientation);
            // start position is top-center of board
            X = -1;
            Y = 5;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        TetrisPiece(TetrisPiece p)
        {
            Type = p.Type;
            Orientation = p.Orientation;
            Mask = p.Mask;
            X = p.X;
            Y = p.Y;
        }

        /// <summary>
        /// Get piece template mask
        /// </summary>
        /// <returns>a 4x4 array</returns>
        static int[,] GetMask(PieceType type, int orientation)
        {
            switch (type)
            {
                case PieceType.I:
                    switch (orientation % 2)
                    {
                        case 0:
                            return new[,]{
                                {0,0,1,0},
                                {0,0,1,0},
                                {0,0,1,0},
                                {0,0,1,0}
                            };
                        case 1:
                            return new[,]{
                                {0,0,0,0},
                                {1,1,1,1},
                                {0,0,0,0},
                                {0,0,0,0}
                            };
                    }
                    break;
                case PieceType.O:
                    return new[,]{
                        {0,0,0,0},
                        {0,1,1,0},
                        {0,1,1,0},
                        {0,0,0,0}
                    };
                case PieceType.T:
                    switch (orientation % 4)
                    {
                        case 0:
                            return new[,]{
                                {0,0,0,0},
                                {0,1,1,1},
                                {0,0,1,0},
                                {0,0,0,0}
                            };
                        case 1:
                            return new[,]{
                                {0,0,1,0},
                                {0,1,1,0},
                                {0,0,1,0},
                                {0,0,0,0}
                            };
                        case 2:
                            return new[,]{
                                {0,0,1,0},
                                {0,1,1,1},
                                {0,0,0,0},
                                {0,0,0,0}
                            };
                        case 3:
                            return new[,]{
                                {0,0,1,0},
                                {0,0,1,1},
                                {0,0,1,0},
                                {0,0,0,0}
                            };
                        default: break;
                    }
                    break;
                case PieceType.L:
                    switch (orientation % 4)
                    {
                        case 0:
                            return new[,]{
                                {0,0,1,0},
                                {0,0,1,0},
                                {0,0,1,1},
                                {0,0,0,0}
                            };
                        case 1:
                            return new[,]{
                                {0,0,0,0},
                                {0,1,1,1},
                                {0,1,0,0},
                                {0,0,0,0}
                            };
                        case 2:
                            return new[,]{
                                {0,1,1,0},
                                {0,0,1,0},
                                {0,0,1,0},
                                {0,0,0,0}
                            };
                        case 3:
                            return new[,]{
                                {0,0,0,1},
                                {0,1,1,1},
                                {0,0,0,0},
                                {0,0,0,0}
                            };
                        default: break;
                    }
                    break;
                case PieceType.J:
                    switch (orientation % 4)
                    {
                        case 0:
                            return new[,]{
                                {0,0,1,0},
                                {0,0,1,0},
                                {0,1,1,0},
                                {0,0,0,0}
                            };
                        case 1:
                            return new[,]{
                                {0,1,0,0},
                                {0,1,1,1},
                                {0,0,0,0},
                                {0,0,0,0}
                            };
                        case 2:
                            return new[,]{
                                {0,0,1,1},
                                {0,0,1,0},
                                {0,0,1,0},
                                {0,0,0,0}
                            };
                        case 3:
                            return new[,]{
                                {0,0,0,0},
                                {0,1,1,1},
                                {0,0,0,1},
                                {0,0,0,0}
                            };
                        default: break;
                    }
                    break;
                case PieceType.S:
                    switch (orientation % 2)
                    {
                        case 0:
                            return new[,]{
                                {0,0,0,0},
                                {0,0,1,1},
                                {0,1,1,0},
                                {0,0,0,0}
                            };
                        case 1:
                            return new[,]{
                                {0,0,1,0},
                                {0,0,1,1},
                                {0,0,0,1},
                                {0,0,0,0}
                            };
                        default: break;
                    }
                    break;
                case PieceType.Z:
                    switch (orientation % 2)
                    {
                        case 0:
                            return new[,]{
                                {0,0,0,0},
                                {0,1,1,0},
                                {0,0,1,1},
                                {0,0,0,0}
                            };
                        case 1:
                            return new[,]{
                                {0,0,1,0},
                                {0,1,1,0},
                                {0,1,0,0},
                                {0,0,0,0}
                            };
                        default: break;
                    }
                    break;
            }
            return null;
        }

        /// <summary>
        /// Get a rotated piece
        /// </summary>
        /// <returns>a new piece</returns>
        internal TetrisPiece Rotate()
        {
            var p = new TetrisPiece(this);
            p.Orientation = (p.Orientation + 1) % 4;
            // refresh the mask
            p.Mask = GetMask((PieceType)p.Type, p.Orientation);
            return p;
        }

        /// <summary>
        /// Get a shifted piece
        /// </summary>
        /// <returns>a new piece</returns>
        internal TetrisPiece Left()
        {
            var p = new TetrisPiece(this);
            p.Y--;
            return p;
        }

        /// <summary>
        /// Get a shifted piece
        /// </summary>
        /// <returns>a new piece</returns>
        internal TetrisPiece Right()
        {
            var p = new TetrisPiece(this);
            p.Y++;
            return p;
        }

        /// <summary>
        /// Get a shifted piece
        /// </summary>
        /// <returns>a new piece</returns>
        internal TetrisPiece Down()
        {
            var p = new TetrisPiece(this);
            p.X++;
            return p;
        }
    }
}