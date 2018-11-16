namespace iobloc
{
    enum GameState { Running = 0, Ended = 1 }
    enum PlayerSide { White = 0, Black = 1 }
    enum ActionType { Pick, Unpick, Put }
    enum LineType
    {
        Line1 = 0, Line2 = 1, Line3 = 2, Line4 = 3, Line5 = 4, Line6 = 5, Line7 = 6, Line8 = 7, Line9 = 8, Line10 = 9, Line11 = 10, Line12 = 11,
        Line13 = 12, Line14 = 13, Line15 = 14, Line16 = 15, Line17 = 16, Line18 = 17, Line19 = 18, Line20 = 19, Line21 = 20, Line22 = 21, Line23 = 22, Line24 = 23,
        Taken = 24, Out = 26
    }

    struct Action
    {
        public ActionType Type { get; private set; }
        public LineType Line { get; private set; }

        public Action(ActionType type, LineType line)
        {
            Type = type;
            Line = line;
        }
    }
}
