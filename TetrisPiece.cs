namespace iobloc
{
    struct TetrisPiece
    {
        enum PieceType { I = 1, O, T, L, J, S, Z }

        int Rotation { get; set; }
        internal int Type { get; private set; }
        internal int[,] Mask { get; private set; }
        internal int X { get; private set; }
        internal int Y { get; private set; }

        internal TetrisPiece(int type, int rotation)
        {
            Type = type;
            Rotation = rotation;
            Mask = GetMask((PieceType)type, rotation);
            X = -1;
            Y = 5;
        }

        TetrisPiece(TetrisPiece p)
        {
            Type = p.Type;
            Rotation = p.Rotation;
            Mask = p.Mask;
            X = p.X;
            Y = p.Y;
        }

        static int[,] GetMask(PieceType type, int rotation)
        {
            switch (type)
            {
                case PieceType.I:
                    switch (rotation % 2)
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
                    switch (rotation % 4)
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
                    switch (rotation % 4)
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
                    switch (rotation % 4)
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
                    switch (rotation % 2)
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
                    switch (rotation % 2)
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
            p.Rotation = (p.Rotation + 1) % 4;
            p.Mask = GetMask((PieceType)p.Type, p.Rotation);
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