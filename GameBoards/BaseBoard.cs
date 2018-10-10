using System;

namespace iobloc
{
    class BaseBoard : IBoard
    {
        public string[] Help => throw new NotImplementedException();

        public int Score => throw new NotImplementedException();

        public bool Won => throw new NotImplementedException();

        public ConsoleKey[] Keys => throw new NotImplementedException();

        public int StepInterval => throw new NotImplementedException();

        public BoardFrame Frame => throw new NotImplementedException();

        public int[,] Grid => throw new NotImplementedException();

        public int[] Clip => throw new NotImplementedException();

        public bool Action(ConsoleKey key)
        {
            throw new NotImplementedException();
        }

        public bool Step()
        {
            throw new NotImplementedException();
        }
    }
}