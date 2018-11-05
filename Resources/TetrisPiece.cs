namespace iobloc
{
    /// <summary>
    /// Tetris piece struct with rotation maps for each piece
    /// </summary>
    struct TetrisPiece
    {
        // basic tetrominos
        private enum PieceType { I = 1, O, T, L, J, S, Z }
        // orientation and type are used to determine piece footprint (mask)
        private int Orientation { get; set; }

        /// <summary>
        /// Tetromino type
        /// </summary>
        /// <value></value>
        public int Type { get; private set; }
        /// <summary>
        /// Piece footprint as a 4x4 matrix
        /// </summary>
        /// <value></value>
        public int[,] Mask { get; private set; }
        /// <summary>
        /// Mask distance to top (can be negativ on piece entry)
        /// </summary>
        /// <value></value>
        public int X { get; private set; }
        /// <summary>
        /// Mask distance to left
        /// </summary>
        /// <value></value>
        public int Y { get; private set; }
        /// <summary>
        /// Color depends on type
        /// </summary>
        public int Color => 15 - Type;

        /// <summary>
        /// Construct a piece which can only move or rotate
        /// </summary>
        /// <param name="type"></param>
        /// <param name="orientation"></param>
        public TetrisPiece(int type, int orientation)
        {
            Type = type;
            Orientation = orientation;
            // mask is set once and changed only on rotations
            Mask = GetMask((PieceType)type, orientation);
            // start position is top-center of board
            X = -1;
            Y = 5;
        }

        private TetrisPiece(TetrisPiece p)
        {
            Type = p.Type;
            Orientation = p.Orientation;
            Mask = p.Mask;
            X = p.X;
            Y = p.Y;
        }

        /// <summary>
        /// Decide on a mask from type and rotation
        /// </summary>
        /// <param name="type"></param>
        /// <param name="orientation"></param>
        /// <returns></returns>
        private static int[,] GetMask(PieceType type, int orientation)
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
        /// Change orientation (and mask)
        /// </summary>
        /// <returns></returns>
        public TetrisPiece Rotate()
        {
            var p = new TetrisPiece(this);
            p.Orientation = (p.Orientation + 1) % 4;
            p.Mask = GetMask((PieceType)p.Type, p.Orientation);
            return p;
        }

        /// <summary>
        ///  Strafe left
        /// </summary>
        /// <returns></returns>
        public TetrisPiece Left()
        {
            var p = new TetrisPiece(this);
            p.Y--;
            return p;
        }

        /// <summary>
        /// Strafe right
        /// </summary>
        /// <returns></returns>
        public TetrisPiece Right()
        {
            var p = new TetrisPiece(this);
            p.Y++;
            return p;
        }

        /// <summary>
        /// Strafe down
        /// </summary>
        /// <returns></returns>
        public TetrisPiece Down()
        {
            var p = new TetrisPiece(this);
            p.X++;
            return p;
        }
    }
}
