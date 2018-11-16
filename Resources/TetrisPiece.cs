namespace iobloc
{
    // Tetris piece struct with rotation maps for each piece
    struct TetrisPiece
    {
        // basic tetrominos
        private enum PieceType { I = 1, O, T, L, J, S, Z }
        // orientation and type are used to determine piece footprint (mask)
        private int Orientation { get; set; }

        // Tetromino type
        public int Type { get; private set; }
        // Piece footprint as a 4x4 matrix
        public int[,] Mask { get; private set; }
        // Mask distance to top (can be negativ on piece entry)
        public int X { get; private set; }
        // Mask distance to left
        public int Y { get; private set; }
        // Color depends on type
        public int Color => 15 - Type;

        // Summary:
        //      Construct a piece which can only move or rotate
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

        // Summary:
        //      Decide on a mask from type and rotation
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

        // Summary:
        //      Change orientation (and mask)
        public TetrisPiece Rotate()
        {
            var p = new TetrisPiece(this);
            p.Orientation = (p.Orientation + 1) % 4;
            p.Mask = GetMask((PieceType)p.Type, p.Orientation);
            return p;
        }

        // Summary:
        //      Strafe left
        public TetrisPiece Left()
        {
            var p = new TetrisPiece(this);
            p.Y--;
            return p;
        }

        // Summary:
        //      Strafe right
        public TetrisPiece Right()
        {
            var p = new TetrisPiece(this);
            p.Y++;
            return p;
        }

        // Summary:
        //      Strafe down
        public TetrisPiece Down()
        {
            var p = new TetrisPiece(this);
            p.X++;
            return p;
        }
    }
}
