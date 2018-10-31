namespace iobloc
{
    struct TetrisPiece
    {
        enum PieceType { I = 1, O, T, L, J, S, Z }

        int Orientation { get; set; }
        internal int Type { get; private set; }
        internal int[,] Mask { get; private set; }
        internal int X { get; private set; }
        internal int Y { get; private set; }
        internal int Color { get { return 15 - Type; } }

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

        TetrisPiece(TetrisPiece p)
        {
            Type = p.Type;
            Orientation = p.Orientation;
            Mask = p.Mask;
            X = p.X;
            Y = p.Y;
        }

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

        internal TetrisPiece Rotate()
        {
            var p = new TetrisPiece(this);
            p.Orientation = (p.Orientation + 1) % 4;
            // refresh the mask
            p.Mask = GetMask((PieceType)p.Type, p.Orientation);
            return p;
        }

        internal TetrisPiece Left()
        {
            var p = new TetrisPiece(this);
            p.Y--;
            return p;
        }

        internal TetrisPiece Right()
        {
            var p = new TetrisPiece(this);
            p.Y++;
            return p;
        }

        internal TetrisPiece Down()
        {
            var p = new TetrisPiece(this);
            p.X++;
            return p;
        }
    }
}
