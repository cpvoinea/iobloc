namespace iobloc
{
    struct Position
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public Position(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public override bool Equals(object obj)
        {
            Position p = (Position)obj;
            return p.Row == Row && p.Col == Col;
        }

        public override int GetHashCode()
        {
            return Col + Row * 100;
        }
    }
}
